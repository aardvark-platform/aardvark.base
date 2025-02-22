﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
#endif

using Aardbase = Aardvark.Base;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    [Flags]
    public enum DirFlags
    {
        None = 0x00,
        NegativeX = 0x01,
        PositiveX = 0x02,
        NegativeY = 0x04,
        PositiveY = 0x08,
        NegativeZ = 0x10,
        PositiveZ = 0x20,
        NegativeW = 0x40,
        PositiveW = 0x80,

        X = NegativeX | PositiveX,
        Y = NegativeY | PositiveY,
        Z = NegativeZ | PositiveZ,
    };
    //# Action comma = () => Out(", ");
    //# Action add = () => Out(" + ");
    //# Action addqcommaspace = () => Out(" + \", \" ");
    //# Action addbetween = () => Out(" + between ");
    //# Action andand = () => Out(" && ");
    //# Action oror = () => Out(" || ");
    //# Action xor = () => Out(" ^ ");
    //# Action el = () => Out("else ");
    //# string f0 = Meta.VecFields[0], f1 = Meta.VecFields[1];
    //# string f2 = Meta.VecFields[2], f3 = Meta.VecFields[3];
    //# var fdtypes = new[] { Meta.FloatType, Meta.DoubleType };
    //# var allfields = Meta.VecFields;
    //#
    //# Func<Meta.SimpleType, Meta.SimpleType, bool> iscolormapped =
    //#     (t1, t2) => (t1 != t2) && !(t1.IsReal && t2.IsReal);
    //#
    //# Func<Meta.SimpleType, Meta.SimpleType, bool> coltovecsupported =
    //#     (cft, vft) => (!cft.IsReal || vft.IsReal) && (cft != Meta.UIntType || vft != Meta.IntType);
    //#
    //# var colmaxvalmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "255" },
    //#         { Meta.UShortType, "2^16 - 1" },
    //#         { Meta.UIntType, "2^32 - 1" },
    //#         { Meta.FloatType, "1" },
    //#         { Meta.DoubleType, "1" },
    //#     };

    //# foreach (var vt in Meta.VecTypes) {
    //#     int d = vt.Len;
    //#     var ft = vt.FieldType;
    //#     var unsigned = Meta.UnsignedTypes.Contains(ft);
    //#     var ct = Meta.ComputationTypeOf(ft);
    //#     var ht = Meta.HighPrecisionTypeOf(ft);
    //#     var vct = Meta.VecTypeOf(d, ct);
    //#     var vtype = vt.Name;
    //#     var fcaps = ft.Caps;
    //#     var ftype = ft.Name;
    //#     var fchar = ft.Char;
    //#     var ctype = ct.Name;
    //#     var htype = ht.Name;
    //#     var vctype = vct.Name;
    //#     var vitype = Meta.VecTypeOf(d, Meta.IntType).Name;
    //#     var v2type = Meta.VecTypeOf(2, ft).Name;
    //#     var v3type = Meta.VecTypeOf(3, ft).Name;
    //#     var v4type = Meta.VecTypeOf(4, ft).Name;
    //#     var mtype = unsigned ? "UnknownMatrixType" : Meta.MatTypeOf(d, d, ft).Name;
    //#     var fields = vt.Fields;
    //#     var args = fields.ToLower();
    //#     var getptr = "&" + fields[0];
    #region __vtype__

    [DataContract]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __vtype__ : IVector<double>, ISize__d____fchar__, IFormattable, IEquatable<__vtype__>
    {
        //# fields.ForEach(f => {
        [DataMember]
        public __ftype__ __f__;
        //# });

        #region Constructors

        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var ftype1 = ft1.Name;
        /// <summary>
        /// Creates a new vector from given <see cref="__ftype1__"/> elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(/*# args.ForEach(a => { */__ftype1__ __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f, a) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */__a__;
            //# });
        }

        /// <summary>
        /// Creates a new vector by assigning the given <see cref="__ftype1__"/> to all elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(__ftype1__ v)
        {
            //# fields.ForEach(args, (f, a) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */v;
            //# });
        }

        /// <summary>
        /// Creates a new vector from the given array.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(__ftype1__[] a)
        {
            //# fields.ForEach((f, i) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */a[__i__];
            //# });
        }

        /// <summary>
        /// Creates a new vector from given array, starting at specified index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(__ftype1__[] a, int start)
        {
            //# fields.ForEach((f, i) => {
            __f__ = /*# if (ft != ft1) {*/(__ftype__)/*# } */a[start + __i__];
            //# });
        }

        //# if (d > 2) { for (int i = 1; i < d; i++) { var j = d - i;
        //# var fsttype = (i > 1) ? Meta.VecTypeOf(i, ft1) : ft1;
        //# var sndtype = (j > 1) ? Meta.VecTypeOf(j, ft1) : ft1;
        /// <summary>
        /// Creates a new vector from the given <see cref="__fsttype.Name__"/> and <see cref="__sndtype.Name__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(__fsttype.Name__ a, __sndtype.Name__ b)
        {
            //# if (i == 1) {
            __f0__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/a;
            //# } else { for (int k = 0; k < i; k++) { var fk = Meta.VecFields[k];
            __fk__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/a.__fk__;
            //# } }
            //# if (j == 1) { var fj = Meta.VecFields[i];
            __fj__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/b;
            //# } else { for (int k = 0; k < j; k++) { var fj = Meta.VecFields[i + k]; var fk = Meta.VecFields[k];
            __fj__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/b.__fk__;
            //# } }
        }

        //# } }
        //# if (d > 3) { for (int i = 0; i < 3; i++) {
        //# var t0 = (i == 0) ? Meta.VecTypeOf(2, ft1).Name : ft1.Name;
        //# var t1 = (i == 1) ? Meta.VecTypeOf(2, ft1).Name : ft1.Name;
        //# var t2 = (i == 2) ? Meta.VecTypeOf(2, ft1).Name : ft1.Name;
        /// <summary>
        /// Creates a new vector from the given <see cref="__t0__"/>, <see cref="__t1__"/>, and <see cref="__t2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(__t0__ a, __t1__ b, __t2__ c)
        {
            //# if (i != 0) {
            __f0__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/a;
            //# } else { for (int k = 0; k < 2; k++) { var fk = Meta.VecFields[k];
            __fk__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/a.__fk__;
            //# } }
            //# if (i != 1) { var fj = Meta.VecFields[(i < 1) ? 2 : 1];
            __fj__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/b;
            //# } else { for (int k = 0; k < 2; k++) { var fj = Meta.VecFields[((i < 1) ? 2 : 1) + k]; var fk = Meta.VecFields[k];
            __fj__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/b.__fk__;
            //# } }
            //# if (i != 2) {
            __f3__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/c;
            //# } else { for (int k = 0; k < 2; k++) { var fj = Meta.VecFields[2 + k]; var fk = Meta.VecFields[k];
            __fj__ = /*# if (ft != ft1) {*/(__ftype__)/*# }*/c.__fk__;
            //# } }
        }

        //# } }
        //# }
        /// <summary>
        /// Creates a vector from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(Func<int, __ftype__> index_fun)
        {
            //# fields.ForEach((f, fi) => {
            __f__ = index_fun(__fi__);
            //# });
        }

        /// <summary>
        /// Creates a vector from a general vector implementing the IVector&lt;__ftype__&gt; interface.
        /// The caller has to verify that the dimension of <paramref name="v"/> is at least __d__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(IVector<double> v)
            : this(/*# d.ForEach(i => {*/v[__i__]/*# }, comma); */)
        { }

        //# foreach (var vt1 in Meta.VecTypes) {
        //# var vtype1 = vt1.Name;
        //# var d1 = vt1.Len;
        //# var ft1 = vt1.FieldType;
        //# var cast = (ft != ft1) ? "(" + ftype + ")" : "";
        //# var missingfields = allfields.Skip(d1).Take(d - d1);
        //# var mfcount = missingfields.Count();
        //# var ignoredfields = allfields.Skip(d).Take(d1 - d);
        //# var ifcount = ignoredfields.Count();
        /// <summary>
        /// Creates a vector from another vector of type <see cref="__vtype1__"/>.
        //# if (mfcount > 0) { var isare = (mfcount > 1) ? "are" : "is";
        /// /*# missingfields.ForEach(f => {*/__f__/*# }, comma);*/ __isare__ set to zero.
        //# }
        //# if (ifcount > 0) { var isare = (ifcount > 1) ? "are" : "is";
        /// /*# ignoredfields.ForEach(f => {*/v.__f__/*# }, comma);*/ __isare__ ignored.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(__vtype1__ v)
        {
            //# fields.ForEach((f, i) => {
            //# if (i < d1) {
            __f__ = __cast__v.__f__;
            //# } else {
            __f__ = 0;
            //# }
            //# });
        }

        //# }
        //# if (d == 3 || d == 4) {
        //#     foreach (var t1 in Meta.ColorTypes) { var ft1 = t1.FieldType;
        //#         if (coltovecsupported(ft1, ft)) {
        //#             var type1 = t1.Name;
        //#             var ftype1 = ft1.Name;
        //#             var convert = ft != ft1 ? "("+ ftype +")" : "";
        //#             var maxval = colmaxvalmap[ft1];
        /// <summary>
        /// Creates a vector from a <see cref="__type1__"/> color.
        //# if (iscolormapped(ft, ft1)) {
        /// The values are not mapped from the <see cref="__type1__"/> color range.
        //# }
        //# if (d == 4 && !t1.HasAlpha) {
        /// W is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vtype__(__type1__ c)
        {
            //# t1.Channels.ForEach(fields, (c, f) => {
            __f__ = __convert__(c.__c__);
            //# });
            //# if (d == 4) {
            //#     if (t1.HasAlpha) {
            W = __convert__(c.A);
            //#     } else {
            W = __convert____t1.MaxValue__;
            //#     }
            //# }
        }

        //#     } }
        //# }
        #endregion

        #region Conversions

        //# foreach (var vt1 in Meta.VecTypes) {
        //#     var vtype1 = vt1.Name;
        //#     if (vt != vt1) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __vtype__(__vtype1__ v)
            => new __vtype__(v);

        //# } }
        //# foreach (var vt1 in Meta.VecTypes) {
        //#     var ft1 = vt1.FieldType;
        //#     var vtype1 = vt1.Name;
        //#     var d1 = vt1.Len;
        //#     if (vt != vt1 && (ft != ft1 || d < d1)) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __vtype1__ To__vtype1__() => (__vtype1__)this;

        //# } }
        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var ftype1 = ft1.Name;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __ftype1__[](__vtype__ v)
            => new __ftype1__[] { /*# fields.ForEach(f => {
                if (ft != ft1) { */(__ftype1__)/*# } */v.__f__/*# }, comma); */ };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __vtype__(__ftype1__[] v)
            => new __vtype__(/*# d.ForEach(fi => {
                if (ft != ft1) { */(__ftype__)/*# } */v[__fi__]/*# }, comma); */);

        //# }
        //# if (d == 3 || d == 4) {
        //#     foreach (var t1 in Meta.ColorTypes) { var ft1 = t1.FieldType;
        //#         if (coltovecsupported(ft1, ft)) {
        //#             var type1 = t1.Name;
        //#             var ftype1 = ft1.Name;
        //#             var convert = ft != ft1 ? "("+ ftype +")" : "";
        //#             var maxval = colmaxvalmap[ft1];
        /// <summary>
        /// Converts the given <see cref="__type1__"/> color to a <see cref="__vtype__"/> vector.
        //# if (iscolormapped(ft, ft1)) {
        /// The values are not mapped from the <see cref="__type1__"/> color range.
        //# }
        //# if (d == 4 && !t1.HasAlpha) {
        /// W is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __vtype__(__type1__ v)
            => new __vtype__(v);

        /// <summary>
        /// Converts the given <see cref="__vtype__"/> vector to a <see cref="__type1__"/> color.
        //# if (iscolormapped(ft, ft1)) {
        /// The values are not mapped to the <see cref="__type1__"/> color range.
        //# }
        //# if (d == 3 && t1.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __type1__ To__type1__() => (__type1__)this;

        //#     } }
        //# }
        //# if (ft.IsReal) {
        //#     var ichar = ft == Meta.DoubleType ? "l" : "i";
        //#     var icast = ft == Meta.DoubleType ? "long" : "int";
        //#     foreach (var floor in new[] { "Floor", "Ceiling" }) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly V__d____ichar__ To__floor__V__d____ichar__()
            => new V__d____ichar__(/*# vt.Fields.ForEach(f => { */(__icast__)Fun.__floor__(__f__)/*# }, comma); */);

        //#   }
        //# }
        //# if (d >= 3 && ft.IsReal) {
        //#     foreach (var ft1 in Meta.VecFieldTypes) {
        //#         var vt1 = Meta.VecTypeOf(d-1, ft1);
        //#         var vtype1 = vt1.Name;
        //#         var ftype1 = ft1.Name;
        //#         var lastF = fields.Last();
        /// <summary>
        /// Creates the inhomogenized Version by dividing the first elements by the last element.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __vtype1__ To__vtype1__Inhomo()
        {
            var div = 1 / __lastF__;
            return new __vtype1__(/*# vt1.Fields.ForEach(f => { */__f__ * div/*# }, comma); */);
        }

        //#     }
        //# }
        //# if (d <= 3 && ft.IsReal) {
        //#     foreach (var ft1 in Meta.VecFieldTypes) {
        //#         var vt1 = Meta.VecTypeOf(d+1, ft1);
        //#         var vtype1 = vt1.Name;
        //#         var ftype1 = ft1.Name;
        /// <summary>
        /// Creates the homogenized Version by adding an additional element 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __vtype1__ To__vtype1__Homo()
            => new __vtype1__(/*# fields.ForEach(f => { */__f__/*# }, comma); */, 1);

        //#     }
        //# }
        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var vtype1 = vt1.Name;
        //#     var ftype1 = ft1.Name;
        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __vtype1__ Copy(Func<__ftype__, __ftype1__> element_fun)
            => new __vtype1__(/*# fields.ForEach(f => { */element_fun(__f__)/*# }, comma); */);

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __vtype1__ Copy(Func<__ftype__, int, __ftype1__> element_index_fun)
            => new __vtype1__(/*# fields.ForEach((f, i) => { */element_index_fun(__f__, __i__)/*# }, comma); */);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(__ftype1__[] array, int start)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = /*# if (ft != ft1) { */(__ftype1__)/*# } */__f__;
            //# });
        }

        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo<T>(T[] array, int start, Func<__ftype__, T> element_fun)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = element_fun(__f__);
            //# });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo<T>(T[] array, int start, Func<__ftype__, int, T> element_index_fun)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = element_index_fun(__f__, __i__);
            //# });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly __ftype__[] ToArray()
            => new __ftype__[] { /*# fields.ForEach((f, i) => { */__f__/*# }, comma); */ };

        #endregion

        #region Properties and Indexers

        //# fields.ForEach(f => {
        //# var pf = "P_" + f;
        /// <summary>
        /// Property for the field __f__.
        /// Useful when properties are required, but the field __f__ is recommended for general use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [XmlIgnore]
        public __ftype__ __pf__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return __f__;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                __f__ = value;
            }
        }

        //# });
        /// <summary>
        /// Enumerates all elements of this vector.
        /// </summary>
        public readonly IEnumerable<__ftype__> Elements
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                //# fields.ForEach(f => {
                yield return __f__;
                //# });
            }
        }

        /// <summary>
        /// Gets or sets element with given index.
        /// </summary>
        public unsafe __ftype__ this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                fixed (__ftype__* ptr = __getptr__) { return ptr[index]; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (__ftype__* ptr = __getptr__) { ptr[index] = value; }
            }
        }

        /// <summary>
        /// Returns the index of the largest dimension of the vector.
        /// </summary>
        public readonly int MajorDim
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                //# if (d == 2) {
                return X >= Y ? 0 : 1;
                //# }
                //# if (d == 3) {
                return X >= Y ? (X >= Z ? 0 : 2) : (Y >= Z ? 1 : 2);
                //# }
                //# if (d == 4) {
                return X >= Y
                        ? (X >= Z ? (X >= W ? 0 : 3) : (Z >= W ? 2 : 3))
                        : (Y >= Z ? (Y >= W ? 1 : 3) : (Z >= W ? 2 : 3));
                //# }
            }
        }

        /// <summary>
        /// Returns the index of the smallest dimension of the vector.
        /// </summary>
        public readonly int MinorDim
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                //# if (d == 2) {
                return X <= Y ? 0 : 1;
                //# }
                //# if (d == 3) {
                return X <= Y ? (X <= Z ? 0 : 2) : (Y <= Z ? 1 : 2);
                //# }
                //# if (d == 4) {
                return X <= Y
                        ? (X <= Z ? (X <= W ? 0 : 3) : (Z <= W ? 2 : 3))
                        : (Y <= Z ? (Y <= W ? 1 : 3) : (Z <= W ? 2 : 3));
                //# }
            }
        }

        /// <summary>
        /// Returns the minimum element of the vector.
        /// </summary>
        public readonly __ftype__ MinElement
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Fun.Min(/*# fields.ForEach(f => { */__f__/*# }, comma); */);
        }

        /// <summary>
        /// Returns the maximum element of the vector.
        /// </summary>
        public readonly __ftype__ MaxElement
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Fun.Max(/*# fields.ForEach(f => { */__f__/*# }, comma); */);
        }

        //# if (ft.IsReal) {
        //# var condArray = new[] { "Finite", "NaN", "Infinity", "PositiveInfinity", "NegativeInfinity", "Tiny" };
        //# var scopeArray = new[] { "Fun", ftype, ftype, ftype, ftype, "Fun" };
        //# var quantArray = new[] { "Any", "All" };
        //# var actArray = new[] { oror, andand };
        //# condArray.ForEach(scopeArray, (cond, scope) => {
        //# quantArray.ForEach(actArray, (qant, act) => {
        public readonly bool __qant____cond__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => /*# fields.ForEach((f, i) => { */__scope__.Is__cond__(__f__)/*# }, act); */;
        }

        //# }); // quantArray
        //# }); // condArray
        /// <summary>
        /// Returns true if the absolute value of each component of the vector is smaller than Constant&lt;__ftype__&gt;.PositiveTinyValue, false otherwise.
        /// </summary>
        public readonly bool IsTiny
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AllTiny;
        }

        /// <summary>
        /// Returns true if any component of the vector is NaN, false otherwise.
        /// </summary>
        public readonly bool IsNaN
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyNaN;
        }

        /// <summary>
        /// Returns true if any component of the vector is infinite (positive or negative), false otherwise.
        /// </summary>
        public readonly bool IsInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyInfinity;
        }

        /// <summary>
        /// Returns true if any component of the vector is infinite and positive, false otherwise.
        /// </summary>
        public readonly bool IsPositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyPositiveInfinity;
        }

        /// <summary>
        /// Returns true if any component of the vector is infinite and negative, false otherwise.
        /// </summary>
        public readonly bool IsNegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AnyNegativeInfinity;
        }

        /// <summary>
        /// Returns whether all components of the vector are finite (i.e. not NaN and not infinity).
        /// </summary>
        public readonly bool IsFinite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !(IsInfinity || IsNaN);
        }

        //# } // ft.IsReal
        #endregion

        #region Constants

        /// <summary>
        /// Number of elements in this vector.
        /// </summary>
        public const int Dimension = __d__;

        /// <summary>
        /// All elements zero.
        /// </summary>
        public static __vtype__ Zero
        {
            get { return new __vtype__(/*# d.ForEach(i => { */0/*# }, comma); */); }
        }

        //# if (ft.IsReal) {
        /// <summary>
        /// All elements half.
        /// </summary>
        public static __vtype__ Half
        {
            get { return new __vtype__(/*# d.ForEach(i => { */0.5/*# }, comma); */); }
        }
        //# } // ft.IsReal

        /// <summary>
        /// All elements one.
        /// </summary>
        public static __vtype__ One
        {
            get { return new __vtype__(/*# d.ForEach(i => { */1/*# }, comma); */); }
        }

        /// <summary>
        /// All elements set to maximum possible value.
        /// </summary>
        public static __vtype__ MaxValue
        {
            get { return new __vtype__(/*# d.ForEach(i => { */Constant<__ftype__>.ParseableMaxValue/*# }, comma); */); }
        }

        /// <summary>
        /// All elements set to minimum possible value.
        /// </summary>
        public static __vtype__ MinValue
        {
            get { return new __vtype__(/*# d.ForEach(i => { */Constant<__ftype__>.ParseableMinValue/*# }, comma); */); }
        }

        //# if (ft.IsReal) {
         /// <summary>
        /// All elements set to negative infinity.
        /// </summary>
        public static __vtype__ NegativeInfinity
        {
            get { return new __vtype__(/*# d.ForEach(i => { */__ftype__.NegativeInfinity/*# }, comma); */); }
        }

        /// <summary>
        /// All elements set to positive infinity.
        /// </summary>
        public static __vtype__ PositiveInfinity
        {
            get { return new __vtype__(/*# d.ForEach(i => { */__ftype__.PositiveInfinity/*# }, comma); */); }
        }

        /// <summary>
        /// All elements set to NaN.
        /// </summary>
        public static __vtype__ NaN
        {
            get { return new __vtype__(/*# d.ForEach(i => { */__ftype__.NaN/*# }, comma); */); }
        }

        //# }
        //# fields.ForEach((f, j) => {
        /// <summary>
        /// Normalized __f__-axis.
        /// </summary>
        public static __vtype__ __f__Axis
        {
            get { return new __vtype__(/*# d.ForEach(i => { var v = i != j ? 0 : 1; */__v__/*# }, comma); */); }
        }

        //# });
        /// <summary>
        /// An array of accessor functions for the coordinates of the vector.
        /// </summary>
        public static readonly Func<__vtype__, __ftype__>[] SelectorArray =
            new Func<__vtype__, __ftype__>[] { /*# fields.ForEach(f => { */v => v.__f__/*# }, comma); */ };

        /// <summary>
        /// Element getter function.
        /// </summary>
        public static readonly Func<__vtype__, int, __ftype__> Getter =
            (v, i) =>
            {
                switch (i)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: return v.__f__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<__vtype__, int, __ftype__> Setter =
            (ref __vtype__ v, int i, __ftype__ s) =>
            {
                switch (i)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: v.__f__ = s; return;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Element getter function with long index.
        /// </summary>
        public static readonly Func<__vtype__, long, __ftype__> LongGetter =
            (v, i) =>
            {
                switch (i)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: return v.__f__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Element setter action with long index.
        /// </summary>
        public static readonly ActionRefValVal<__vtype__, long, __ftype__> LongSetter =
            (ref __vtype__ v, long i, __ftype__ s) =>
            {
                switch (i)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: v.__f__ = s; return;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            };

        #endregion

        #region Static factories

        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var vtype1 = vt1.Name;
        //#     if (ft != ft1) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ From__vtype1__(__vtype1__ v)
            => new __vtype__(v);

        //#     }
        //# }
        //# if (d == 3 || d == 4) {
        //#     foreach (var t1 in Meta.ColorTypes) { var ft1 = t1.FieldType;
        //#         if (coltovecsupported(ft1, ft)) {
        //#             var type1 = t1.Name;
        //#             var ftype1 = ft1.Name;
        //#             var convert = ft != ft1 ? "("+ ftype +")" : "";
        //#             var maxval = colmaxvalmap[ft1];
        /// <summary>
        /// Creates a vector from the given <see cref="__type1__"/> color.
        //# if (iscolormapped(ft, ft1)) {
        /// The values are not mapped from the <see cref="__type1__"/> color range.
        //# }
        //# if (d == 4 && !t1.HasAlpha) {
        /// W is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ From__type1__(__type1__ c) => new __vtype__(c);

        //#     } }
        //# }
        //# if (ft.IsReal) {
        //# if (d == 2) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ FromPolar(__ftype__ angleInRadians, __ftype__ radius)
            => new __vtype__(Fun.Cos(angleInRadians) * radius, Fun.Sin(angleInRadians) * radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ FromPolar(__ftype__ angleInRadians)
            => new __vtype__(Fun.Cos(angleInRadians), Fun.Sin(angleInRadians));

        //# } else if (d == 3) {
        private static readonly __vtype__[] s_fromCubeCode =
            new __vtype__[] { -__vtype__.XAxis, -__vtype__.YAxis, -__vtype__.ZAxis,
                        __vtype__.XAxis, __vtype__.YAxis, __vtype__.ZAxis };

        /// <summary>
        /// Return the vector for the supplied cube face code.
        /// 0 ... -XAxis, 1 ... -YAxis, 2 ... -ZAsix, 3 ... XAxis, 4 ... YAxis, 5 ... ZAxis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ FromCubeFaceCode(int i) { return s_fromCubeCode[i]; }

        //# } // d = 3
        //# } // ft.IsReal
        #endregion

        #region Norms

        /// <summary>
        /// Returns the squared length of the vector.
        /// </summary>
        public readonly __ftype__ LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return /*# fields.ForEach(f => { */__f__ * __f__ /*# }, add); */; }
        }

        //# if (ft == Meta.FloatType && d == 4) {
#if NETCOREAPP3_1_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static __ctype__ Length_Sse41(__vtype__ vec)
        {
            unsafe
            {
                //# if (d == 2) {
                var vv = Vector128.Create(vec.X, vec.Y, 0.0f, 0.0f);
                vv = Sse41.DotProduct(vv, vv, 0x31);
                //# } else if (d == 3) {
                var vv = Vector128.Create(vec.X, vec.Y, vec.Z, 0.0f);
                vv = Sse41.DotProduct(vv, vv, 0x71);
                //# } else { // d == 4
                var x = (float*)&vec;
                var vv = Sse.LoadVector128(x);
                vv = Sse41.DotProduct(vv, vv, 0xF1);
                //# }
                var l2 = vv.GetElement(0);
                return MathF.Sqrt(l2);
            }
        }
#endif

        //# } // end ft == Meta.FloatType
        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        public readonly __ctype__ Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                //# if (ft == Meta.FloatType && d == 4) {
#if NETCOREAPP3_1_OR_GREATER
                if (Sse41.IsSupported)
                    return Length_Sse41(this);
#endif
                //# }
                return Fun.Sqrt(/*# fields.ForEach(f => { */__f__ * __f__ /*# }, add); */);
            }
        }

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |x| + |y| + ...
        /// </summary>
        public readonly __ftype__ Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return /*# fields.ForEach(f => { if (unsigned) {*/__f__/*# } else {*/Fun.Abs(__f__)/*# } }, add); */; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the vector. This is the
        /// length of the vector.
        /// </summary>
        public readonly __ctype__ Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(/*# fields.ForEach(f => { */__f__ * __f__/*# }, add); */); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the vector. This is
        /// calculated as max(|x|, |y|, ...).
        /// </summary>
        public readonly __ftype__ NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(/*# fields.ForEach(f => { if (unsigned) {*/__f__/*# } else {*/Fun.Abs(__f__)/*# } }, comma); */); }
        }

        /// <summary>
        /// Returns the minimum norm of the vector. This is calculated as
        /// min(|x|, |y|, ...).
        /// </summary>
        public readonly __ftype__ NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(/*# fields.ForEach(f => { if (unsigned) {*/__f__/*# } else {*/Fun.Abs(__f__)/*# } }, comma); */); }
        }

        /// <summary>
        /// Returns a normalized copy of this vector.
        /// </summary>
        public readonly __vctype__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var s = Length; if (s == 0) return __vctype__.Zero;
                s = 1 / s;
                return new __vctype__(/*# fields.ForEach(f => { */__f__ * s/*# }, comma); */);
            }
        }

        //# if (d == 2 && !unsigned) {
        /// <summary>
        /// Vector rotated 90° counter clockwise: (-Y, X)
        /// </summary>
        public readonly __vtype__ Rot90
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __vtype__(-Y, X); }
        }

        /// <summary>
        /// Vector rotated 180° counter clockwise: (-X, -Y)
        /// </summary>
        public readonly __vtype__ Rot180
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __vtype__(-X, -Y); }
        }

        /// <summary>
        /// Vector rotated 270° counter clockwise: (Y, -X)
        /// </summary>
        public readonly __vtype__ Rot270
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __vtype__(Y, -X); }
        }

        //# } // d == 2
        //# if (ft.IsReal) {
        //# if (d == 2) {
        /// <summary>
        /// Returns a copy of the vector with the maximum component length of
        /// exactly 1. This corresponds to mapping the vector onto an origin-
        /// centered square with side length 2.
        /// </summary>
        public readonly __vtype__ CubeMapped
        {
            get
            {
                __ftype__ x = Fun.Abs(X);
                __ftype__ y = Fun.Abs(Y);
                return x > y
                        ? new __vtype__(Fun.Sign(X), Y / x)
                        : new __vtype__(X / y, Fun.Sign(Y));
            }
        }

        //# } // d == 2
        //# else if (d == 3) {
        //# foreach (var face in new bool[] { false, true }) {
        //#     string facepar = face ? "OnFace(out int face)" : "";
        /// <summary>
        /// Returns a copy of the vector with the maximum component length of
        /// exactly 1. This corresponds to mapping the vector onto an origin-
        /// centered cube with side length 2.
        //# if (face) {
        /// The out parameter face indicate which face the vector was mapped to:
        /// 0 ... -XAxis, 1 ... -YAxis, 2 ... -ZAsix, 3 ... XAxis, 4 ... YAxis, 5 ... ZAxis.
        //# }
        /// </summary>
        public readonly __vtype__ CubeMapped__facepar__
        {
            //# if (!face) {
            get
            {
            //# }
                __ftype__ x = Fun.Abs(X);
                __ftype__ y = Fun.Abs(Y);
                __ftype__ z = Fun.Abs(Z);
                if (x > y)
                {
                    if (x > z)
                    {
                        double s = 1 / x;/*# if (face) { */ face = X < 0 ? 0 : 3;/*# } */
                        return new __vtype__(Fun.Sign(X), Y * s, Z * s);
                    }
                    else
                    {
                        double s = 1 / z;/*# if (face) { */ face = Z < 0 ? 2 : 5;/*# } */
                        return new __vtype__(X * s, Y * s, Fun.Sign(Z));
                    }
                }
                else
                {
                    if (y > z)
                    {
                        double s = 1 / y;/*# if (face) { */ face = Y < 0 ? 1 : 4;/*# } */
                        return new __vtype__(X * s, Fun.Sign(Y), Z * s);
                    }
                    else
                    {
                        double s = 1 / z;/*# if (face) { */ face = Z < 0 ? 2 : 5;/*# } */
                        return new __vtype__(X * s, Y * s, Fun.Sign(Z));
                    }
                }
            //# if (!face) {
            }
            //# }
        }

        //# } // face
        /// <summary>
        /// Return an index for the cube face onto which the vector points.
        /// 0 ... -XAxis, 1 ... -YAxis, 2 ... -ZAsix, 3 ... XAxis, 4 ... YAxis, 5 ... ZAxis.
        /// </summary>
        public readonly int CubeFaceCode
        {
            get
            {
                double x = Fun.Abs(X);
                double y = Fun.Abs(Y);
                double z = Fun.Abs(Z);
                int c;
                double v;
                if (x > y)
                {
                    if (x > z) { c = 0; v = X; } else { c = 2; v = Z; }
                }
                else
                {
                    if (y > z) { c = 1; v = Y; } else { c = 2; v = Z; }
                }
                return v < 0 ? c : c + 3;
            }
        }

        //# } // d == 3
        //# else {
        /// <summary>
        /// Returns a copy of the vector with the maximum component length of
        /// exactly 1. This corresponds to mapping the vector onto an origin-
        /// centered cube with side length 2.
        /// </summary>
        public readonly __vtype__ CubeMapped
        {
            get
            {
                var s = 1 / Fun.Max(/*# fields.ForEach(f => { */Fun.Abs(__f__)/*# }, comma); */);
                return new __vtype__(/*# fields.ForEach(f => { */__f__ * s/*# }, comma); */);
            }
        }

        //# } // d > 3
        //# } // ft.IsReal
        #endregion

        #region Static methods for F# core and Aardvark library support

        //# if (!unsigned) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Abs(__vtype__ v)
            => v.Abs();

        //# }
        //# if (ft.IsReal) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Floor(__vtype__ v)
            => v.Floor();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Ceiling(__vtype__ v)
            => v.Ceiling();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Round(__vtype__ v)
            => v.Round();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Truncate(__vtype__ v)
            => v.Truncate();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Acos(__vtype__ v)
            => Fun.Acos(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Acoshb(__vtype__ v)
            => Fun.Acosh(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Cos(__vtype__ v)
            => Fun.Cos(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Cosh(__vtype__ v)
            => Fun.Cosh(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Asin(__vtype__ v)
            => Fun.Asin(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Asinhb(__vtype__ v)
            => Fun.Asinh(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Sin(__vtype__ v)
            => Fun.Sin(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Sinh(__vtype__ v)
            => Fun.Sinh(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Atan(__vtype__ v)
            => Fun.Atan(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Atanhb(__vtype__ v)
            => Fun.Atanh(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Atan2(__vtype__ a, __vtype__ b)
            => Fun.Atan2(a, b);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Tan(__vtype__ v)
            => Fun.Tan(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Tanh(__vtype__ v)
            => Fun.Tanh(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Sqrt(__vtype__ v)
            => Fun.Sqrt(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ CubeRoot(__vtype__ v)
            => Fun.Cbrt(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Exp(__vtype__ v)
            => Fun.Exp(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Log(__vtype__ v)
            => Fun.Log(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ LogBinary(__vtype__ v)
            => Fun.Log2(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Log10(__vtype__ v)
            => Fun.Log10(v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ CopySgn(__vtype__ value, __vtype__ sign)
            => Fun.CopySign(value, sign);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ CopySgn(__vtype__ value, __ftype__ sign)
            => Fun.CopySign(value, sign);

        //# } // ft.IsReal
        //# foreach(var tt in fdtypes) {
        //# var vtt = Meta.VecTypeOf(d, tt);
        //# if (ft.IsReal && tt != ft) continue;
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ LinearInterp(__tt.Name__ t, __vtype__ a, __vtype__ b)
            => Fun.Lerp(t, a, b);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ LinearInterp(__vtt.Name__ t, __vtype__ a, __vtype__ b)
            => Fun.Lerp(t, a, b);

        //#}
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Min(__vtype__ v0, __vtype__ v1)
            => Fun.Min(v0, v1);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Min(__vtype__ v, __ftype__ x)
            => Fun.Min(v, x);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Max(__vtype__ v0, __vtype__ v1)
            => Fun.Max(v0, v1);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Max(__vtype__ v, __ftype__ x)
            => Fun.Max(v, x);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Saturate(__vtype__ v)
            => Fun.Saturate(v);

        //# if (!unsigned) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ DivideByInt(__vtype__ v, int x)
            => v / x;

        //# }
        #endregion

        #region Operations

        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var ftype1 = ft1.Name;
        /// <summary>
        /// Sets the elements of a vector to the given __ftype1__ elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(/*# args.ForEach(a => { */__ftype1__ __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f, a) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */__a__;
            //# });
        }

        //# }
        //# if (!unsigned) {
        /// <summary>
        /// Returns a negated copy of the specified vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ operator -(__vtype__ v)
            => new __vtype__(/*# fields.ForEach(f => { */-v.__f__/*# }, comma); */);

        //# }
        //# if (!ft.IsReal) {
        /// <summary>
        /// Returns the component-wise bitwise complement of the specified vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ operator ~(__vtype__ v)
            => new __vtype__(/*# fields.ForEach(f => { */~v.__f__/*# }, comma); */);

        //# }
        //# if (d == 2 && !unsigned) {
        /// <summary>
        /// Returns a vector that is orthogonal to this one (i.e. {x,y} -> {-y,x}).
        /// </summary>
        public readonly __vtype__ Orthogonal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __vtype__(-__f1__, __f0__); }
        }

        //# }
        //# if (ft.IsReal) {
        /// <summary>
        /// Gets a copy of this vector containing the reciprocal (1/x) of each element.
        /// </summary>
        public readonly __vtype__ Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __vtype__(/*# fields.ForEach(f => { */1 / __f__/*# }, comma); */); }
        }

        //# }
        //# {
        //# var ops = new[] {" + ", " - ", " * ", " / ", " % "};
        //# var opactions = new[]  { "operator +", "operator -", "operator *", "operator /", "operator %"};
        //# var opnames = new[] {"sum", "difference", "product", "fraction", "remainder"};
        //# for (int o = 0; o < ops.Length; o++) {
        //#     var op = ops[o];
        //#     var opname = opnames[o];
        //#     var opaction = opactions[o];
        /// <summary>
        /// Returns the component-wise __opname__ of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ __opaction__(__vtype__ a, __vtype__ b)
            => new __vtype__(/*# fields.ForEach(f => { */a.__f____op__b.__f__/*# }, comma); */);

        /// <summary>
        /// Returns the component-wise __opname__ of a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ __opaction__(__vtype__ v, __ftype__ s)
            => new __vtype__(/*# fields.ForEach(f => { */v.__f____op__s/*#  }, comma); */);

        /// <summary>
        /// Returns the component-wise __opname__ of a scalar and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ __opaction__(__ftype__ s, __vtype__ v)
            => new __vtype__(/*# fields.ForEach(f => { */s__op__v.__f__/*# }, comma); */);

        //# }
        //# }
        //# if (!ft.IsReal) {
        //# var ops = new[] {"&", "|", "^"};
        //# var opnames = new[] {"bitwise and", "bitwise or", "bitwise exclusive or" };
        //# for (int o = 0; o < ops.Length; o++) {
        //#     var op = ops[o];
        //#     var opname = opnames[o];
        /// <summary>
        /// Returns the component-wise __opname__ of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ operator __op__(__vtype__ a, __vtype__ b)
            => new __vtype__(/*# fields.ForEach(f => { */a.__f__ __op__ b.__f__/*# }, comma); */);

        /// <summary>
        /// Returns the component-wise __opname__ of a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ operator __op__(__vtype__ v, __ftype__ s)
            => new __vtype__(/*# fields.ForEach(f => { */v.__f__ __op__ s/*#  }, comma); */);

        /// <summary>
        /// Returns the component-wise __opname__ of a scalar and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ operator __op__(__ftype__ s, __vtype__ v)
            => new __vtype__(/*# fields.ForEach(f => { */s __op__ v.__f__/*# }, comma); */);

        //# }
        /// <summary>
        /// Returns the component-wise left bitshift of a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ operator <<(__vtype__ v, int s)
            => new __vtype__(/*# fields.ForEach(f => { */v.__f__ << s/*#  }, comma); */);

        /// <summary>
        /// Returns the component-wise right bitshift of a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ operator >>(__vtype__ v, int s)
            => new __vtype__(/*# fields.ForEach(f => { */v.__f__ >> s/*#  }, comma); */);

        //# }
        /// Attention: NEVER implement operators &lt;, &lt;=, &gt;=, &gt;,
        /// since these are not defined in a Vector space.
        /// Use AllSmaller() and similar comparators!
        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ == b.__f__/*# }, andand); */;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__vtype__ v, __ftype__ s)
        {
            return /*# fields.ForEach(f => { */v.__f__ == s/*# }, andand); */;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__ftype__ s, __vtype__ v)
        {
            return /*# fields.ForEach(f => { */s == v.__f__/*# }, andand); */;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ != b.__f__/*# }, oror); */;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__vtype__ v, __ftype__ s)
        {
            return /*# fields.ForEach(f => { */v.__f__ != s/*# }, oror); */;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__ftype__ s, __vtype__ v)
        {
            return /*# fields.ForEach(f => { */s != v.__f__/*# }, oror); */;
        }

        #endregion

        #region IEquatable<__vtype__> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(__vtype__ other)
        {
            return /*# fields.ForEach(f => { */__f__.Equals(other.__f__)/*# }, andand); */;
        }

        #endregion

        #region Overrides

        public override readonly string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public readonly string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public readonly string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public readonly string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin /*# fields.ForEach(f => {*/+ __f__.ToString(format, fp) /*# }, addbetween); */ + end;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.GetCombined(/*# fields.ForEach(f => { */__f__/*# }, comma); */);
        }

        public override readonly bool Equals(object other)
            => (other is __vtype__ o) ? Equals(o) : false;

        public readonly Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")/*# fields.ForEach(f => {*/
                + __f__.ToString(null, CultureInfo.InvariantCulture) /*# }, addqcommaspace); */
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Parsing

        public static __vtype__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __vtype__(/*# d.ForEach(p => { */
                __ftype__.Parse(x[__p__], CultureInfo.InvariantCulture)/*# }, comma); */
            );
        }

        public static __vtype__ Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<__ftype__>.Parse, __vtype__.Setter);
        }

        public static __vtype__ Parse(Text t, int bracketLevel)
        {
            return t.NestedBracketSplit(bracketLevel, Text<__ftype__>.Parse, __vtype__.Setter);
        }

        #endregion

        #region Swizzle Methods

        //# var snames  = unsigned ? new string[] { "O", "I", "X", "Y", "Z", "W" } : new string[] { "O", "I", "N", "X", "Y", "Z", "W" };
        //# var svalues = unsigned ? new string[] { "0", "1", "X", "Y", "Z", "W" } : new string[] { "0", "1", "-1","X", "Y", "Z", "W" };
        //# var isConst = new Func<int, bool>(i => i < (unsigned ? 2 : 3));
        //# var s = d + (unsigned ? 2 : 3);
        //# var getName = new Func<bool, int, string>((anyN, i) => anyN && i == 1 ? "P" : snames[i]);
        //# for (int xi = 0; xi < s; xi++) { var x = svalues[xi];
        //#     for (int yi = 0; yi < s; yi++) { var y = svalues[yi];
        //#         var anyN = !unsigned && (xi == 2 || yi == 2); // replace I by P if any N
        //#         var name = getName(anyN, xi) + getName(anyN, yi);
        //#         if (isConst(xi) && isConst(yi)) { // check for constant -> otherwise property
        //#             if (d == 2) { // only constants of matching size
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static __v2type__ __name__ => new __v2type__(__x__, __y__);
        //#             }
        //#         }
        //#         else {
        //#             if (xi == yi || isConst(xi) || isConst(yi)) { // readonly if the same or constants
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public readonly __v2type__ __name__ => new __v2type__(__x__, __y__);
        //#             } else {
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __v2type__ __name__
        {
            readonly get => new __v2type__(__x__, __y__);
            set { __x__ = value.X; __y__ = value.Y; }
        }
        //#             }
        //#         }
        //#     }
        //# }
        //# for (int xi = 0; xi < s; xi++) { var x = svalues[xi];
        //#     for (int yi = 0; yi < s; yi++) { var y = svalues[yi];
        //#         for (int zi = 0; zi < s; zi++) { var z = svalues[zi];
        //#             var anyN = !unsigned && (xi == 2 || yi == 2 || zi == 2); // replace I by P if any N
        //#             var name = getName(anyN, xi) + getName(anyN, yi) + getName(anyN, zi);
        //#             if (isConst(xi) && isConst(yi) && isConst(zi)) { // check for constant -> otherwise property
        //#                 if (d == 3) { // only constants of matching size
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static __v3type__ __name__ => new __v3type__(__x__, __y__, __z__);
        //#                 }
        //#             }
        //#             else {
        //#                 if (xi == yi || xi == zi || yi == zi || isConst(xi) || isConst(yi) || isConst(zi)) { // readonly if the same or constants
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public readonly __v3type__ __name__ => new __v3type__(__x__, __y__, __z__);
        //#                 } else {
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __v3type__ __name__
        {
            readonly get => new __v3type__(__x__, __y__, __z__);
            set { __x__ = value.X; __y__ = value.Y; __z__ = value.Z; }
        }
        //#                 }
        //#             }
        //#         }
        //#     }
        //# }
        //# // if (d != 4) { // lets try if it does not explode
        //#     for (int xi = 0; xi < s; xi++) { var x = svalues[xi];
        //#         for (int yi = 0; yi < s; yi++) { var y = svalues[yi];
        //#             for (int zi = 0; zi < s; zi++) { var z = svalues[zi];
        //#                 for (int wi = 0; wi < s; wi++) { var w = svalues[wi];
        //#                     var anyN = !unsigned && (xi == 2 || yi == 2 || zi == 2 || wi == 2); // replace I by P if any N
        //#                     var name = getName(anyN, xi) + getName(anyN, yi) + getName(anyN, zi) + getName(anyN, wi);
        //#                     if (isConst(xi) && isConst(yi) && isConst(zi) && isConst(wi)) { // check for constant -> otherwise property
        //#                         if (d == 4) { // only constants of matching size
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static __v4type__ __name__ => new __v4type__(__x__, __y__, __z__, __w__);
        //#                         }
        //#                     }
        //#                     else {
        //#                         if (xi == yi || xi == zi || xi == wi ||
        //#                             yi == zi || yi == wi || zi == wi ||
        //#                             isConst(xi) || isConst(yi) || isConst(zi) || isConst(wi)) { // readonly if the same or constants
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public readonly __v4type__ __name__ => new __v4type__(__x__, __y__, __z__, __w__);
        //#                         } else {
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __v4type__ __name__
        {
            readonly get => new __v4type__(__x__, __y__, __z__, __w__);
            set { __x__ = value.X; __y__ = value.Y; __z__ = value.Z; __w__ = value.W; }
        }
        //#                         }
        //#                     }
        //#                 }
        //#             }
        //#         }
        //#     }
        //# // }

        #endregion

        #region IVector<double> Members

        /// <summary>
        /// By using long indices, the IVector&lt;double&gt; interface is
        /// accessed.
        /// </summary>
        public double this[long i]
        {
            readonly get { return (double)this[(int)i]; }
            set { this[(int)i] = (__ftype__)value; }
        }

        #endregion

        #region ISize__d____fchar__ Members

        public readonly __vtype__ Size__d____fchar__ { get { return this; } }

        #endregion

        #region IVector

        public readonly long Dim
        {
            get { return __d__; }
        }

        public readonly object GetValue(long index)
        {
            return (object)this[(int)index];
        }

        public void SetValue(object value, long index)
        {
            this[(int)index] = (__ftype__)value;
        }

        #endregion

    }

    public class __vtype__EqualityComparer : IEqualityComparer<__vtype__>
    {
        public static __vtype__EqualityComparer Default
            => new __vtype__EqualityComparer();

        #region IEqualityComparer<__vtype__> Members

        public bool Equals(__vtype__ v0, __vtype__ v1)
        {
            return v0 == v1;
        }

        public int GetHashCode(__vtype__ v)
        {
            return v.GetHashCode();
        }

        #endregion
    }

    public static partial class Fun
    {
        //# Func<Meta.ElementwiseFun.Parameter, Meta.SimpleType> getParamType =
        //#    (p) =>
        //#    {
        //#        var elemType = p.ElementType ?? ft;
        //#        return (p.IsScalar()) ? elemType : Meta.VecTypeOf(d, elemType);
        //#    };
        //#
        //# foreach (var group in Meta.ElementwiseFuns.Keys) {
        //# var isEmpty = Meta.ElementwiseFuns[group].TrueForAll(f => !f.Domain.Contains(ft));
        //# if (!isEmpty) {
        #region __group__

        //# foreach (var fun in Meta.ElementwiseFuns[group]) {
        //# if (!fun.Domain.Contains(ft)) continue;
        //# var retType = Meta.VecTypeOf(d, fun.ReturnType ?? ft).Name;
        /// <summary>
        /// Applies Fun.__fun.Name__ to each element of the given vector(s).
        /// </summary>
        [Pure]
        //# if (fun.Obsolete) {
        [Obsolete]
        //# }
        //# if (!fun.EditorBrowsable) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __retType__ __fun.Name__(/*# fun.Parameters.ForEach((p, i) => {
        var name = p.Name;
        var ptype = getParamType(p).Name;
        var ext = fun.IsExtension && (i == 0);
        var varg = fun.HasVarArgs && (i == fun.Parameters.Length - 1);
        if (ext) { */this /*# } if (varg) { */params /*# } */__ptype__/*# if (varg) {*/[]/*# }*/ __name__/*#}, comma); */)
        {
            return new __retType__(/*# fields.ForEach(f => {*/__fun.Name__(/*# fun.Parameters.ForEach((p, i) => {
            var vec = !p.IsScalar();
            var varg = fun.HasVarArgs && (i == fun.Parameters.Length - 1);
            */__p.Name__/*#if (vec) { if (varg) {*/.Map(a => a.__f__)/*# } else {*/.__f__/*#} } }, comma); */)/*#}, comma); */);
        }

        //# }
        #endregion

        //# } // isEmpty
        //# }
        #region ApproximateEquals

        /// <summary>
        /// Returns whether the given vectors are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __vtype__ a, __vtype__ b, __ftype__ tolerance)
        {
            return /*# fields.ForEach(f => { */ApproximateEquals(a.__f__, b.__f__, tolerance)/*# }, andand); */;
        }

        //# if (ft.IsReal) {
        /// <summary>
        /// Returns whether the given vectors are equal within
        /// Constant{__ftype__}.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __vtype__ a, __vtype__ b)
        {
            return ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
        }

        //# }

        #endregion

        #region IsTiny

        /// <summary>
        /// Returns whether the absolute value of each component of the given <see cref="__vtype__"/> is smaller than <paramref name="epsilon"/>.
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTiny(this __vtype__ v, __ftype__ epsilon)
            => Vec.AllTiny(v, epsilon);

        //# if (ft.IsReal) {
        /// <summary>
        /// Returns whether the absolute value of each component of the given <see cref="__vtype__"/> is smaller than Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTiny(__vtype__ v)
            => v.IsTiny;

        //# }
        #endregion
        //# if (ft.IsReal) {

        #region Special Floating Point Value Checks

        /// <summary>
        /// Returns whether any component of the given <see cref="__vtype__"/> is NaN.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(__vtype__ v)
            => v.IsNaN;

        /// <summary>
        /// Returns whether any component of the the given <see cref="__vtype__"/> is infinity (positive or negative).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(__vtype__ v)
            => v.IsInfinity;

        /// <summary>
        /// Returns whether any component of the the given <see cref="__vtype__"/> is positive infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(__vtype__ v)
            => v.IsPositiveInfinity;

        /// <summary>
        /// Returns whether any component of the the given <see cref="__vtype__"/> is negative infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(__vtype__ v)
            => v.IsNegativeInfinity;

        /// <summary>
        /// Returns whether all components of the the given <see cref="__vtype__"/> are finite (i.e. not NaN and not infinity).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(__vtype__ v)
            => v.IsFinite;

        #endregion
        //# }
    }

    //# if (ft.IsReal) {
    public static partial class Conversion
    {
        #region Angles (Radians, Degrees, Gons)

        //# var units = new[] { "Radians", "Degrees", "Gons" };
        //# units.ForEach(u1 => {
        //# units.ForEach(u2 => { if (u1 == u2) return;
        //# var n1 = u1.ToLower();
        //# var n2 = u2.ToLower();
        /// <summary>
        /// Converts the angles given in __n2__ to __n1__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ __u1__From__u2__(this __vtype__ __n2__)
            => new __vtype__(/*# fields.ForEach(f => {*/
                   __u1__From__u2__(__n2__.__f__)/*# }, comma); */
               );

        //# }); });
        #endregion
    }

    //# }
    /// <summary>
    /// Contains static methods
    /// </summary>
    public static partial class Vec
    {
        #region Length

        /// <summary>
        /// Returns the squared length of the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ LengthSquared(__vtype__ v)
            => v.LengthSquared;

        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ Length(__vtype__ v)
            => v.Length;

        #endregion

        #region Normalize

        //# if (ft.IsReal) {
        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref __vtype__ v)
        {
            var s = v.Length; if (s == 0) return;
            s = 1 / s;
            //# fields.ForEach(f => {
            v.__f__ *= s;
            //# });
        }

        //# } // ft.IsReal
        /// <summary>
        /// Returns a normalized copy of this vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vctype__ Normalized(__vtype__ v)
            => v.Normalized;

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |x| + |y| + ...
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Norm1(__vtype__ v)
            => v.Norm1;

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the vector. This is the
        /// length of the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ Norm2(__vtype__ v)
            => v.Norm2;

        /// <summary>
        /// Returns the infinite (or maximum) norm of the vector. This is
        /// calculated as max(|x|, |y|, ...).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ NormMax(__vtype__ v)
            => v.NormMax;

        /// <summary>
        /// Returns the minimum norm of the vector. This is calculated as
        /// min(|x|, |y|, ...).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ NormMin(__vtype__ v)
            => v.NormMin;

        /// <summary>
        /// Gets the p-norm. This is calculated as the p-th root of (|x|^n + |y|^n + ...).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ Norm(this __vtype__ v, __ctype__ p)
        {
            return (/*# fields.ForEach(f => { */
                Fun.Abs(v.__f__).Pow(p)/*# }, add); */).Pow(1 / p);
        }

        #endregion

        #region Distance functions

        /// <summary>
        /// Returns the squared distance between the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ DistanceSquared(this __vtype__ a, __vtype__ b)
            //# if (unsigned) {
            => /*# fields.ForEach(f => { */Fun.Square((a.__f__ < b.__f__) ? (b.__f__ - a.__f__) : (a.__f__ - b.__f__))/*# }, add); */;
            //# } else {
            => /*# fields.ForEach(f => { */Fun.Square(b.__f__ - a.__f__)/*# }, add); */;
            //# }

        /// <summary>
        /// Returns the distance between the given points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ Distance(this __vtype__ a, __vtype__ b)
            => Fun.Sqrt(DistanceSquared(a, b));

        /// <summary>
        /// Returns the Manhatten (or 1-) distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Distance1(this __vtype__ a, __vtype__ b)
            //# if (unsigned) {
            => /*# fields.ForEach(f => { */((a.__f__ < b.__f__) ? (b.__f__ - a.__f__) : (a.__f__ - b.__f__))/*# }, add); */;
            //# } else {
            => /*# fields.ForEach(f => { */Fun.Abs(b.__f__ - a.__f__)/*# }, add); */;
            //# }

        /// <summary>
        /// Returns the p-distance between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ Distance(this __vtype__ a, __vtype__ b, __ctype__ p)
            //# if (unsigned) {
            => (/*# fields.ForEach(f => { */((a.__f__ < b.__f__) ? (b.__f__ - a.__f__) : (a.__f__ - b.__f__)).Pow(p)/*# }, add); */).Pow(1 / p);
            //# } else {
            => (/*# fields.ForEach(f => { */Fun.Abs(b.__f__ - a.__f__).Pow(p)/*# }, add); */).Pow(1 / p);
            //# }

        /// <summary>
        /// Returns the maximal absolute distance between the components of
        /// the two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ DistanceMax(this __vtype__ a, __vtype__ b)
            //# if (unsigned) {
            => Fun.Max(/*# fields.ForEach(f => { */((a.__f__ < b.__f__) ? (b.__f__ - a.__f__) : (a.__f__ - b.__f__))/*# }, comma); */);
            //# } else {
            => Fun.Max(/*# fields.ForEach(f => { */Fun.Abs(b.__f__ - a.__f__)/*# }, comma); */);
            //# }

        /// <summary>
        /// Returns the minimal absolute distance between the components of
        /// the two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ DistanceMin(this __vtype__ a, __vtype__ b)
            //# if (unsigned) {
            => Fun.Min(/*# fields.ForEach(f => { */((a.__f__ < b.__f__) ? (b.__f__ - a.__f__) : (a.__f__ - b.__f__))/*# }, comma); */);
            //# } else {
            => Fun.Min(/*# fields.ForEach(f => { */Fun.Abs(b.__f__ - a.__f__)/*# }, comma); */);
            //# }

        //# foreach (var hasT in new[] { false, true }) {
        //# var cast = (ft != ct) ? "(" + vctype + ") " : "";
        /// <summary>
        /// Returns the minimal euclidean distance between the supplied query
        /// point and the line segment defined by the two supplied line end
        /// points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ DistanceToLine(
                this __vtype__ query, __vtype__ p0, __vtype__ p1/*# if (hasT) { */, out __ctype__ t/*# } */)
        {
            //# if (ft.IsReal) {
            var p0p1 = p1 - p0;
            var p0q = query - p0;
            /*# if (!hasT) {
            */var /*# } */t = Dot(p0q, p0p1);
            if (t <= 0) { /*# if (hasT) { */t = 0; /*# } */return p0q.Length; }
            var denom = p0p1.LengthSquared;
            if (t >= denom) { /*# if (hasT) { */t = 1; /*# } */return Distance(query, p1); }
            t /= denom;
            return (p0q - t * p0p1).Length;
            //# } else {
            return DistanceToLine(__cast__query, __cast__p0, __cast__p1/*# if (hasT) { */, out t/*# } */);
            //# }
        }

        /// <summary>
        /// Returns the minimal euclidean distance between the supplied query
        /// point and the infinite line defined by two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ DistanceToInfiniteLine(
                this __vtype__ query, __vtype__ p0, __vtype__ p1/*# if (hasT) { */, out __ctype__ t/*# } */)
        {
            //# if (ft.IsReal) {
            var p0p1 = p1 - p0;
            var p0q = query - p0;
            /*# if (!hasT) {
            */var /*# } */t = Dot(p0q, p0p1);
            var denom = p0p1.LengthSquared;
            t /= denom;
            return (p0q - t * p0p1).Length;
            //# } else {
            return DistanceToInfiniteLine(__cast__query, __cast__p0, __cast__p1/*# if (hasT) { */, out t/*# } */);
            //# }
        }

        //# } // hasT
        #endregion

        #region Operations

        //# if (d == 2 && !unsigned) {
        /// <summary>
        /// Returns a vector that is orthogonal to the given one (i.e. {x,y} -> {-y,x}).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Orthogonal(__vtype__ v)
            => v.Orthogonal;

        //# }
        //# if (ft.IsReal) {
        /// <summary>
        /// Gets a copy of the given vector containing the reciprocal (1/x) of each element.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Reciprocal(__vtype__ v)
            => v.Reciprocal;

        //# }
        //# if (!unsigned) {
        /// <summary>
        /// Negates the vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate(this ref __vtype__ v)
        {
            //# fields.ForEach(f => {
            v.__f__ = -v.__f__;
            //# });
        }

        /// <summary>
        /// Returns the outer product (tensor-product) of a * b^T as a __d__x__d__ matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mtype__ Outer(this __vtype__ a, __vtype__ b)
        {
            return new __mtype__(/*# d.ForEach(i => { var fi = fields[i]; */
                        /*# d.ForEach(j => { var fj = fields[j]; */a.__fi__ * b.__fj__/*# }, comma); }, comma); */);
        }

        //# }
        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Dot(this __vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ * b.__f__/*# }, add); */;
        }

        //# if (d == 3 && !unsigned) {
        /// <summary>
        /// Returns the skew-symmetric "cross" matrix (A^T = -A) of the vector v.
        /// </summary>
        public static __mtype__ CrossMatrix(this __vtype__ v)
        {
            return new __mtype__(0, -v.Z, +v.Y,
                            +v.Z, 0, -v.X,
                            -v.Y, +v.X, 0);
        }

        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DirFlags DirFlags(this __vtype__ v)
        {
            //# var posTiny = ft.IsReal ? "Constant<" + ftype + ">.PositiveTinyValue" : "0";
            //# var negTiny = ft.IsReal ? "Constant<" + ftype + ">.NegativeTinyValue" : "0";
            DirFlags flags = Aardbase.DirFlags.None;
            //# fields.ForEach(f => {
            if (v.__f__ > __posTiny__) flags |= Aardbase.DirFlags.Positive__f__;
            if (v.__f__ < __negTiny__) flags |= Aardbase.DirFlags.Negative__f__;
            //# });
            return flags;
        }

        //# if (ft.IsReal) {
        /// <summary>
        /// Returns the reflection direction of the given vector v (pointing to the surface) for the given normal (should be normalized).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Reflect(this __vtype__ v, __vtype__ normal)
        {
            return v - 2 * v.Dot(normal) * normal;
        }

        /// <summary>
        /// Returns the refraction direction of the given vector v (pointing to the surface) for the given normal and ratio of refraction indices (n_out / n_in).
        /// Both the input vectors should be normalized.
        /// Returns a zero-vector in case of total internal reflection.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Refract(this __vtype__ v, __vtype__ normal, __ftype__ eta)
        {
            var t = v.Dot(normal);
            var k = 1 - eta * eta * (1 - t * t);

            if (k < 0)
            {
                return __vtype__.Zero;
            }
            else
            {
                return eta * v - (eta * t + Fun.Sqrt(k)) * normal;
            }
        }

        //# } // ft.IsReal
        //# if (d == 3 && !unsigned) {
        /// <summary>
        /// Returns the cross product of two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Cross(this __vtype__ a, __vtype__ b)
        {
            return new __vtype__(
            a.__f1__ * b.__f2__ - a.__f2__ * b.__f1__,
            a.__f2__ * b.__f0__ - a.__f0__ * b.__f2__,
            a.__f0__ * b.__f1__ - a.__f1__ * b.__f0__
            );
        }

        //# }
        //# if (d == 2 && !unsigned) {
        /// <summary>
        /// Returns the cross product of vector a.
        /// In 2D the cross product is simply a vector that is normal
        /// to the given vector (i.e. {x,y} -> {-y,x})
        /// </summary>
        public static __vtype__ Cross(this __vtype__ a)
        {
            return new __vtype__(-a.__f1__, a.__f0__);
        }

        //# }
        #endregion

        #region Comparisons

        //# var bops = new[,] { { "<",  "Smaller"        }, { ">" , "Greater"},
        //#                     { "<=", "SmallerOrEqual" }, { ">=", "GreaterOrEqual"},
        //#                     { "==", "Equal" },          { "!=", "Different" } };
        //# var attention = "ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).";
        //# for(int o = 0; o < bops.GetLength(0); o++) {
        //#     string bop = " " + bops[o,0] + " ", opName = bops[o,1];
        /// <summary>
        /// Returns whether ALL elements of a are __opName__ the corresponding element of b.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(this __vtype__ a, __vtype__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of v are __opName__ s.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(this __vtype__ v, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */v.__f____bop__s/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ ALL elements of v.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(__ftype__ s, __vtype__ v)
        {
            return (/*# fields.ForEach(f => { */s__bop__v.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is __opName__ the corresponding element of b.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(this __vtype__ a, __vtype__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of v is __opName__ s.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(this __vtype__ v, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */v.__f____bop__s/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ AT LEAST ONE element of v.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(__ftype__ s, __vtype__ v)
        {
            return (/*# fields.ForEach(f => { */s__bop__v.__f__/*# }, oror); */);
        }

        //# }

        /// <summary>
        /// Compare x-coordinate before y-coordinate, aso.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LexicalCompare(this __vtype__ v0, __vtype__ v1)
        {
            //# fields.ForEach(f => {
            if (v0.__f__ < v1.__f__) return -1;
            if (v0.__f__ > v1.__f__) return +1;
            //# });
            return 0;
        }

        #endregion

        #region Min- / MaxElement

        /// <summary>
        /// Returns the minimum element of the given <see cref="__vtype__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ MinElement(__vtype__ v)
            => v.MinElement;

        /// <summary>
        /// Returns the maximum element of the given <see cref="__vtype__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ MaxElement(__vtype__ v)
            => v.MaxElement;

        #endregion

        //# if (d == 3 && ft.IsReal) {
        #region Axis aligned normal

        /// <summary>
        /// Returns an arbitrary normal vector, which
        /// is also normal to either the x, y or z-axis.
        /// </summary>
        public static __vtype__ AxisAlignedNormal(this __vtype__ v)
        {
            __vtype__ vector;
            __ftype__ x = v.X.Abs();
            __ftype__ y = v.Y.Abs();
            __ftype__ z = v.Z.Abs();

            if (x < y)
            {
                if (x < z)
                    vector = __vtype__.XAxis;
                else
                    vector = __vtype__.ZAxis;
            }
            else
            {
                if (y < z)
                    vector = __vtype__.YAxis;
                else
                    vector = __vtype__.ZAxis;
            }

            return v.Cross(vector).Normalized;
        }

        #endregion

        //# }
        //# if (ft.IsReal) {
        #region Angle between two vectors

        /// <summary>
        /// Computes the angle between two given vectors in radians. The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ AngleBetweenFast(this __vtype__ x, __vtype__ y)
        {
            return Fun.AcosClamped(x.Dot(y));
        }

        /// <summary>
        /// Computes the angle between two given vectors in radians using a numerically stable algorithm.
        /// The input vectors have to be normalized.
        /// </summary>
        // https://scicomp.stackexchange.com/a/27769
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ AngleBetween(this __vtype__ x, __vtype__ y)
        {
            var a = x + y;
            var b = x - y;
            return 2 * Fun.Atan2(b.Length, a.Length);
        }

        //# if (d == 2) {
        /// <summary>
        /// Computes the signed angle between two given vectors in radians. The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ctype__ AngleBetweenSigned(this __vtype__ x, __vtype__ y)
        {
            var a = x.X * y.Y - x.Y * y.X;
            var b = x.X * y.X + x.Y * y.Y;
            return Fun.Atan2(a, b);
        }

        //# }
        #endregion

        //# }
        #region AnyTiny, AllTiny

        /// <summary>
        /// Returns whether the absolute value of any component of the given <see cref="__vtype__"/> is smaller than <paramref name="epsilon"/>.
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyTiny(this __vtype__ v, __ftype__ epsilon)
            => /*# fields.ForEach(f => { */v.__f__.IsTiny(epsilon)/*# }, oror);*/;

        /// <summary>
        /// Returns whether the absolute value of each component of the given <see cref="__vtype__"/> is smaller than <paramref name="epsilon"/>.
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllTiny(this __vtype__ v, __ftype__ epsilon)
            => /*# fields.ForEach(f => { */v.__f__.IsTiny(epsilon)/*# }, andand);*/;

        #endregion

        //# if (ft.IsReal) {
        #region Special Floating Point Value Checks

        //# var condArray = new[] { "Finite", "NaN", "Infinity", "PositiveInfinity", "NegativeInfinity", "Tiny" };
        //# var quantArray = new[] { "Any", "All" };
        //# condArray.ForEach(cond => {
        //# quantArray.ForEach(qant => {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool __qant____cond__(__vtype__ v)
            => v.__qant____cond__;

        //# }); // quantArray
        //# }); // condArray
        #endregion

        //# if (d == 2 && !unsigned) {
        #region 2D Vector Arithmetics

        /// <summary>
        /// Dot product of vector with dir rotated by 90 degrees counterclockwise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Dot90(this __vtype__ v, __vtype__ dir)
        {
            return v.Y * dir.X - v.X * dir.Y;
        }

        /// <summary>
        /// Dot product of vector with dir rotated by 180 degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Dot180(this __vtype__ v, __vtype__ dir)
        {
            return -(v.X * dir.X + v.Y * dir.Y);
        }

        /// <summary>
        /// Dot product of vector with dir rotated by 270 degrees counterclockwise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Dot270(this __vtype__ v, __vtype__ dir)
        {
            return v.X * dir.Y - v.Y * dir.X;
        }

        /// <summary>
        /// Returns the left value of the direction v with respect to the
        /// line from p0 to p1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ DirLeftOfLineValue(this __vtype__ v, __vtype__ p0, __vtype__ p1)
        {
            return v.X * (p0.Y - p1.Y) + v.Y * (p1.X - p0.X);
        }

        /// <summary>
        /// Returns the right value of the direction v with respect to the
        /// line from p0 to p1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ DirRightOfLineValue(this __vtype__ v, __vtype__ p0, __vtype__ p1)
        {
            return v.X * (p1.Y - p0.Y) + v.Y * (p0.X - p1.X);
        }

        /// <summary>
        /// Returns the left value of the point p with respect to the
        /// line from p0 to p1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ PosLeftOfLineValue(this __vtype__ p, __vtype__ p0, __vtype__ p1)
        {
            return (p.X - p0.X) * (p0.Y - p1.Y) + (p.Y - p0.Y) * (p1.X - p0.X);
        }

        /// <summary>
        /// Returns the right value of the point p with respect to the
        /// line from p0 to p1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ PosRightOfLineValue(this __vtype__ p, __vtype__ p0, __vtype__ p1)
        {
            return (p.X - p0.X) * (p1.Y - p0.Y) + (p.Y - p0.Y) * (p0.X - p1.X);
        }

        #endregion

        //# } // d == 2
        #region Linear Combination

        //# if (ft.IsReal) {
        //# for (int tpc = 2; tpc < 8; tpc++ ) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ LinCom(/*# tpc.ForEach(i => { */__vtype__ p__i__/*# }, comma); */, ref Tup__tpc__<__ftype__> w)
        {
            return /*# tpc.ForEach(i => { */p__i__ * w.E__i__/*# }, add); */;
        }

        //# } // tpc
        //# } // isreal
        #endregion

        //# } // ft.IsReal
        #region ArrayExtensions

        //# foreach (var it in Meta.IndexTypes) { var itype = it.Name;
        //#     var prefix = it == Meta.LongType ? "Long" : "";
        public static __itype__ __prefix__IndexOfClosestPoint(this __vtype__[] pointArray, __vtype__ point)
        {
            var bestDist = DistanceSquared(point, pointArray[0]);
            __itype__ bestIndex = 0;
            __itype__ count = pointArray.__prefix__Length;
            for (__itype__ i = 1; i < count; i++)
            {
                var dist = DistanceSquared(point, pointArray[i]);
                if (dist < bestDist) { bestDist = dist; bestIndex = i; }
            }
            return bestIndex;
        }

        public static __itype__ __prefix__IndexOfClosestPoint(
                this __vtype__[] array, __itype__ start, __itype__ count,
                __vtype__ point)
        {
            var bestDist = DistanceSquared(point, array[start]);
            __itype__ bestIndex = 0;
            for (__itype__ i = start + 1, e = start + count; i < e; i++)
            {
                var dist = DistanceSquared(point, array[i]);
                if (dist < bestDist) { bestDist = dist; bestIndex = i; }
            }
            return bestIndex;
        }

        public static __itype__ __prefix__IndexOfClosestPoint<T>(
                this T[] array, __itype__ start, __itype__ count,
                Func<T, __vtype__> pointSelector, __vtype__ point)
        {
            var bestDist = DistanceSquared(point, pointSelector(array[start]));
            __itype__ bestIndex = 0;
            for (__itype__ i = start + 1, e = start + count; i < e; i++)
            {
                var dist = DistanceSquared(point, pointSelector(array[i]));
                if (dist < bestDist) { bestDist = dist; bestIndex = i; }
            }
            return bestIndex;
        }

        //# } // it
        //# var opArray = new[] { " < ", " > " };
        //# var exArray = new[] { "min", "max" };
        //# foreach (var isList in new[] { false, true }) {
        //# fields.ForEach(f => {
        //# for (int ei = 0; ei < 2; ei++) {
        //#     var op = opArray[ei]; var ex = exArray[ei]; var fun = ex.Capitalized();
        //# foreach (var it in Meta.IndexTypes) { var itype = it.Name;
        //#     if (isList && it == Meta.LongType) continue;
        //#     var prefix = it == Meta.LongType ? "Long" : "";
        //#     var arrayLen = isList ? "Count" : prefix + "Length";
        //#     var atype = isList ? "IList<" + vtype + ">" : vtype + "[]";
        //#     var listText = isList ? "list" : "array";
        /// <summary>
        /// Returns the index of the element with the __ex__imal __f__ coordinate
        /// within the __listText__.
        /// </summary>
        public static __itype__ __prefix__IndexOf__fun____f__(this __atype__ vectorArray, __itype__ count = 0)
        {
            var __ex__imum = vectorArray[0].__f__;
            __itype__ __ex__Index = 0;
            if (count == 0) count = vectorArray.__arrayLen__;
            for (__itype__ i = 1; i < count; i++)
            {
                var p = vectorArray[i].__f__;
                if (p__op____ex__imum) { __ex__imum = p; __ex__Index = i; }
            }
            return __ex__Index;
        }

        //# } // it
        //# } // fi
        //# });
        //# } // isList
        /// <summary>
        /// Returns the array of the index-th coordinate of each vector.
        /// </summary>
        public static __ftype__[] CopyCoord(this __vtype__[] self, int index)
        {
            switch (index)
            {
                //# fields.ForEach((f, i) => {
                case __i__: return self.Map(v => v.__f__);
                //# });
                default: throw new IndexOutOfRangeException();
            }
        }

        //# if (ft.IsReal) {
        public static __vtype__ WeightedSum(
                this __vtype__[] vectorArray, __ftype__[] weightArray)
        {
            var r = __vtype__.Zero;
            var count = vectorArray.LongLength;
            for (long i = 0; i < count; i++) r += vectorArray[i] * weightArray[i];
            return r;
        }

        //# } // ft.IsReal
        #endregion
    }

    public static class IRandomUniform__vtype__Extensions
    {
        #region IRandomUniform extensions for __vtype__

        //# string[] variants;
        //# if (ft == Meta.FloatType) {
        //#     variants = new string[] { "", "Closed", "Open" };
        //# } else if (ft == Meta.DoubleType) {
        //#     variants = new string[] { "", "Closed", "Open", "Full", "FullClosed", "FullOpen" };
        //# } else if (!unsigned) {
        //#     variants = new string[] { "", "NonZero" };
        //# } else {
        //#     variants = new string[] { "" };
        //# }
        //# foreach (var v in variants) {
        /// <summary>
        /// Uses Uniform__fcaps____v__() to generate the elements of a __vtype__ vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Uniform__vtype____v__(this IRandomUniform rnd)
        {
            return new __vtype__(/*# fields.ForEach(f => { */rnd.Uniform__fcaps____v__()/*#  }, comma); */);
        }

        //# if (ft.IsReal && d < 4) {
        /// <summary>
        /// Uses Uniform__fcaps____v__() to generate the elements of a __vtype__
        /// vector within the given Box__d____fchar__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Uniform__vtype____v__(this IRandomUniform rnd, Box__d____fchar__ box)
        {
            return new __vtype__(/*# fields.ForEach(f => { */box.Min.__f__ + rnd.Uniform__fcaps____v__() * (box.Max.__f__ - box.Min.__f__)/*#  }, comma); */);
        }

        //# }
        //# }
        //# if (ft.IsReal) {
        //# var constant = (ft != Meta.DoubleType) ? "ConstantF" : "Constant";
        //# variants = (ft == Meta.FloatType) ? new string[] { "" } : new string[] { "", "Full" };
        //# foreach (var v in variants) {
        //# if (d == 2) {
        /// <summary>
        /// Returns a uniformly distributed vector (corresponds to a
        /// uniformly distributed point on the unit circle). Uses Uniform__fcaps____v__() internally.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Uniform__vtype____v__Direction(this IRandomUniform rnd)
        {
            __ftype__ phi = rnd.Uniform__fcaps____v__() * __constant__.PiTimesTwo;
            return new __vtype__(Fun.Cos(phi), Fun.Sin(phi));
        }

        //# } else if (d == 3) {
        /// <summary>
        /// Returns a uniformly distributed vector (corresponds to a
        /// uniformly distributed point on the surface of the unit sphere).
        /// Note however, that the returned vector will never be equal to
        /// [0, 0, -1]. Uses Uniform__fcaps____v__() internally.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Uniform__vtype____v__Direction(this IRandomUniform rnd)
        {
            __ftype__ phi = rnd.Uniform__fcaps____v__() * __constant__.PiTimesTwo;
            __ftype__ z = 1 - rnd.Uniform__fcaps____v__() * 2;
            __ftype__ s = Fun.Sqrt(1 - z * z);
            return new __vtype__(Fun.Cos(phi) * s, Fun.Sin(phi) * s, z);
        }

        //# var sphereVariants = new string[] { "Closed", "Open" };
        //# foreach (var sv in sphereVariants) {
        //# if (sv == "Closed") {
        /// <summary>
        /// Uniform vector in the closed unit sphere (i.e vectors to
        /// the surface of the sphere may be generated). Uses Uniform__vtype____sv__() internally.
        /// </summary>
        //# } else {
        /// <summary>
        /// Uniform vector inside the open unit sphere (i.e. no vector
        /// ends on the surface of the sphere). Uses Uniform__vtype____sv__() internally.
        /// </summary>
        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Uniform__vtype____v____sv__Sphere(this IRandomUniform rnd)
        {
            __ftype__ r2;
            __vtype__ p;
            __vtype__ c_shift = -__vtype__.Half;
            do
            {
                p = (rnd.Uniform__vtype____sv__() + c_shift) * 2;
                r2 = p.LengthSquared;
            }
            while (r2 >= 1);
            return p;
        }

        //# }
        //# } //d
        //# }
        //# } else if (!unsigned) { //ft.Real
        /// <summary>
        /// Uses Uniform__fcaps__(__ftype__) to generate the elements of a __vtype__ vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Uniform__vtype__(this IRandomUniform rnd, __ftype__ size)
        {
            return new __vtype__(/*# fields.ForEach(f => { */rnd.Uniform__fcaps__(size)/*#  }, comma); */);
        }

        /// <summary>
        /// Uses Uniform__fcaps__(__ftype__) to generate the elements of a __vtype__ vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vtype__ Uniform__vtype__(this IRandomUniform rnd, __vtype__ size)
        {
            return new __vtype__(/*# fields.ForEach(f => { */rnd.Uniform__fcaps__(size.__f__)/*#  }, comma); */);
        }

        //# }
        #endregion
    }

    #endregion

    //# } // vt
}