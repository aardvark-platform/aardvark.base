using System;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# Action andand = () => Out(" && ");
    //# Action add = () => Out(" + ");
    //# Action addbetween = () => Out(" + between ");
    //# Action addqcommaspace = () => Out(" + \", \" ");
    //# Action comma = () => Out(", ");
    //# Action oror = () => Out(" || ");
    //# Action semicolon = () => Out("; ");
    //# Action xor = () => Out(" ^ ");
    //#
    //# Func<Meta.SimpleType, Meta.SimpleType, bool> ismapped =
    //#     (t1, t2) => (t1 != t2) && !(t1.IsReal && t2.IsReal);
    //#
    //# Func<Meta.SimpleType, Meta.SimpleType, bool> coltovecsupported =
    //#     (cft, vft) => (!cft.IsReal || vft.IsReal) && (cft != Meta.UIntType || vft != Meta.IntType);
    //#
    //# var ftoftmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "Col.ByteFromFloatClamped" },
    //#         { Meta.UShortType, "Col.UShortFromFloatClamped" },
    //#         { Meta.UIntType, "Col.UIntFromFloatClamped" },
    //#         { Meta.FloatType, "" },
    //#         { Meta.DoubleType, "(double)" },
    //#     };
    //# var dtoftmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "Col.ByteFromDoubleClamped" },
    //#         { Meta.UShortType, "Col.UShortFromDoubleClamped" },
    //#         { Meta.UIntType, "Col.UIntFromDoubleClamped" },
    //#         { Meta.FloatType, "(float)" },
    //#         { Meta.DoubleType, "" },
    //#     };
    //# var fttodmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "Col.DoubleFromByte" },
    //#         { Meta.UShortType, "Col.DoubleFromUShort" },
    //#         { Meta.UIntType, "Col.DoubleFromUInt" },
    //#         { Meta.FloatType, "(double)" },
    //#         { Meta.DoubleType, "" },
    //#     };
    //# var btoftmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "" },
    //#         { Meta.UShortType, "Col.UShortFromByte" },
    //#         { Meta.UIntType, "Col.UIntFromByte" },
    //#         { Meta.FloatType, "Col.FloatFromByte" },
    //#         { Meta.DoubleType, "Col.DoubleFromByte" },
    //#     };
    //# var fttobmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "" },
    //#         { Meta.UShortType, "Col.ByteFromUShort" },
    //#         { Meta.UIntType, "Col.ByteFromUInt" },
    //#         { Meta.FloatType, "Col.ByteFromFloatClamped" },
    //#         { Meta.DoubleType, "Col.ByteFromDoubleClamped" },
    //#     };
    //# var maxvalmap = new Dictionary<Meta.SimpleType, string>
    //#     {
    //#         { Meta.ByteType, "255" },
    //#         { Meta.UShortType, "2^16 - 1" },
    //#         { Meta.UIntType, "2^32 - 1" },
    //#         { Meta.FloatType, "1" },
    //#         { Meta.DoubleType, "1" },
    //#     };
    //# var fdtypes = new[] { Meta.FloatType, Meta.DoubleType };
    //# foreach (var t in Meta.ColorTypes) {
    //#     var type = t.Name;
    //#     var ft = t.FieldType;
    //#     var ht = (ft != Meta.FloatType) ? Meta.HighPrecisionTypeOf(ft) : ft;
    //#     var ct = Meta.ComputationTypeOf(ft);
    //#     var htype = ht.Name;
    //#     var ctype = ct.Name;
    //#     var hnd = ht != Meta.DoubleType; // high not double
    //#     var dblt = Meta.ColorTypeOf(t.Len, Meta.DoubleType);
    //#     var dbltype = dblt.Name;
    //#     var fltt = Meta.ColorTypeOf(t.Len, Meta.FloatType);
    //#     var flttype = fltt.Name;
    //#     var isByte = ft == Meta.ByteType;
    //#     var isUShort = ft == Meta.UShortType;
    //#     var isFloat = ft == Meta.FloatType;
    //#     var isDouble = ft == Meta.DoubleType;
    //#     var isReal = ft.IsReal;
    //#     var ftype = ft.Name;
    //#     var fcaps = ft.Caps;
    //#     var fields = t.Fields;
    //#     var dim = fields.Length;
    //#     var channels = t.Channels;
    //#     var args = fields.ToLower();
    //#     var cargs = channels.ToLower();
    //#     var f_to_ft = ftoftmap[ft];
    //#     var d_to_ft = dtoftmap[ft];
    //#     var ft_to_d = fttodmap[ft];
    //#     var b_to_ft = btoftmap[ft];
    //#     var ft_to_b = fttobmap[ft];
    //#     var fabs_p = isReal ? "Fun.Abs(" : "";
    //#     var q_fabs = isReal ? ")" : "";
    //#     var getptr = "&" + fields[0];
    //#     var rgba = t.HasAlpha ? "RGBA" : "RGB";
    //#     var maxval = maxvalmap[ft];
    #region __type__

    /// <summary>
    /// Represents an __rgba__ color with each channel stored as a <see cref="__ftype__"/> value within [0, __maxval__].
    /// </summary>
    [Serializable]
    public partial struct __type__ : IFormattable, IEquatable<__type__>, IRGB/*# if (t.HasAlpha) { */, IOpacity/*# } */
    {
        #region Constructors

        /// <summary>
        /// Creates a color from the given <see cref="__ftype__"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */__ftype__ __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = __a__/*# }, semicolon); */;
        }

        //# if (!isReal) {
        /// <summary>
        /// Creates a color from the given <see cref="int"/> values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */int __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = (__ftype__)__a__/*# }, semicolon); */;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */long __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = (__ftype__)__a__/*# }, semicolon); */;
        }

        //# }
        //# if (!isFloat) {
        /// <summary>
        /// Creates a color from the given <see cref="float"/> values.
        //# if (!isDouble) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */float __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f,a) => {
            __f__ = __f_to_ft__(__a__);
            //# });
        }

        //# }
        //# if (!isDouble) {
        /// <summary>
        /// Creates a color from the given <see cref="double"/> values.
        //# if (!isFloat) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# args.ForEach(a => { */double __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f,a) => {
            __f__ = __d_to_ft__(__a__);
            //# });
        }

        //# }
        //# if (t.HasAlpha) {
        /// <summary>
        /// Creates a color from the given <see cref="__ftype__"/> RGB values.
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */__ftype__ __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = __a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# if (!isReal) {
        /// <summary>
        /// Creates a color from the given <see cref="int"/> RGB values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */int __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = (__ftype__)__a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        /// <summary>
        /// Creates a color from the given <see cref="long"/> RGB values.
        /// The values are not mapped to the <see cref="__type__"/> color range.
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */long __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = (__ftype__)__a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# }
        //# if (!isFloat) {
        /// <summary>
        /// Creates a color from the given <see cref="float"/> RGB values.
        //# if (!isDouble) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */float __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c,a) => { */
            __c__ = __f_to_ft__(__a__)/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# }
        //# if (!isDouble) {
        /// <summary>
        /// Creates a color from the given <see cref="double"/> RGB values.
        //# if (!isFloat) {
        /// The values are mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# }
        /// The alpha channel is set to __maxval__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# cargs.ForEach(a => { */double __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c,a) => { */__c__ = __d_to_ft__(__a__)/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# }
        //# } // t.HasAlpha
        /// <summary>
        /// Creates a color from a single <see cref="__ftype__"/> value.
        //# if (t.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ gray)
        {
            /*# channels.ForEach(
                    c => { */__c__ = gray/*# },
                    semicolon); if (t.HasAlpha) {*/; A = __t.MaxValue__/*# } */;
        }

        //# if (!isReal) {
        /// <summary>
        /// Creates a color from a single <see cref="float"/> value.
        /// The value is mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# if (t.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(float gray)
        {
            var value = __f_to_ft__(gray);
            /*# channels.ForEach(
                    c => { */__c__ = value/*# },
                    semicolon); if (t.HasAlpha) {*/; A = __t.MaxValue__/*# } */;
        }

        /// <summary>
        /// Creates a color from a single <see cref="double"/> value.
        /// The value is mapped from [0, 1] to the <see cref="__type__"/> color range.
        //# if (t.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(double gray)
        {
            var value = __d_to_ft__(gray);
            /*# channels.ForEach(
                    c => { */__c__ = value/*# },
                    semicolon); if (t.HasAlpha) {*/; A = __t.MaxValue__/*# } */;
        }

        //# } // !isReal
        //# foreach (var t1 in Meta.ColorTypes) {
        //#     var convert = t.FieldType != t1.FieldType
        //#         ? "Col." + t.FieldType.Caps + "From" + t1.FieldType.Caps
        //#         : "";
        /// <summary>
        /// Creates a color from the given <see cref="__t1.Name__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (t.HasAlpha && !t1.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__t1.Name__ color)
        {
            //# channels.ForEach(c => {
            __c__ = __convert__(color.__c__);
            //# });
            //# if (t.HasAlpha) {
            //#     if (t1.HasAlpha) {
            A = __convert__(color.A);
            //#     } else {
            A = __t.MaxValue__;
            //#     }
            //# }
        }

        //#if (t.HasAlpha && !t1.HasAlpha) { // build constructor from Color3 with explicit alpha
        /// <summary>
        /// Creates a color from the given <see cref="__t1.Name__"/> color and an alpha value.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__t1.Name__ color, __ftype__ alpha)
        {
            //# channels.ForEach(Meta.VecFields, (c, vf) => {
            __c__ = __convert__(color.__c__);
            //# });
            A = alpha;
        }

        //# }
        //# } // end For
        //# for (int d = 3; d <= 4; d++) {
        //#     foreach (var vft in Meta.VecFieldTypes) { if (coltovecsupported(ft, vft)) {
        //#         var vt = Meta.VecTypeOf(d, vft);
        //#         var convert = ft != vft
        //#             ? "("+ ft.Name+")"
        //#             : "";
        /// <summary>
        /// Creates a color from the given <see cref="__vt.Name__"/> vector.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (d < dim) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__vt.Name__ vec)
        {
            //# fields.ForEach(Meta.VecFields, (c, vf, i) => {
            __c__ = /*# if (i < d) { */__convert__(vec.__vf__);/*# } else {*/__maxval__;/*# }*/
            //# });
        }

        //# } } }
        //# if (t.HasAlpha) {
        //# foreach (var vft in Meta.VecFieldTypes) { if (coltovecsupported(ft, vft)) {
        //#     var vt = Meta.VecTypeOf(3, vft);
        //#     var convert = ft != vft
        //#         ? "("+ ft.Name+")"
        //#         : "";
        /// <summary>
        /// Creates a color from the given <see cref="__vt.Name__"/> vector and an alpha value.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped to the <see cref="__type__"/> color range.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__vt.Name__ vec, __ftype__ alpha)
        {
            //# channels.ForEach(Meta.VecFields, (c, vf) => {
            __c__ = __convert__(vec.__vf__);
            //# });
            A = alpha;
        }

        //# } } }
        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(Func<int, __ftype__> index_fun)
        {
            //# fields.ForEach((f, fi) => {
            __f__ = index_fun(__fi__);
            //# });
        }

        #endregion

        #region Conversions

        //# foreach (var t1 in Meta.ColorTypes) if (t1 != t) {
        //#     var type1 = t1.Name;
        /// <summary>
        /// Converts the given color to a <see cref="__type1__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type1__"/> color range.
        //# }
        //# if (t1.HasAlpha && !t.HasAlpha) {
        /// The alpha channel is set to __maxvalmap[t1.FieldType]__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type1__(__type__ color)
            => new __type1__(color);

        //# }
        //# for (int d = 3; d <= 4; d++) {
        //#     foreach (var vft in Meta.VecFieldTypes) { if (coltovecsupported(ft, vft)) {
        //#         var vt = Meta.VecTypeOf(d, vft);
        //#         var convert = ft != vft ? "("+ vft.Name+")" : "";
        /// <summary>
        /// Converts the given color to a <see cref="__vt.Name__"/> vector.
        //# if (d == 4 && dim == 3) {
        /// W is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __vt.Name__(__type__ color)
            => new __vt.Name__(/*# channels.ForEach(c => { */
                __convert__(color.__c__)/*# }, comma);
                if (d == 4) {
                    if (t.HasAlpha) { */,
                __convert__(color.A)/*#
                    } else { */,
                __convert__(__t.MaxValue__)/*#
                    }
                } */
                );

        //# } } }
        //# foreach (var t1 in Meta.ColorTypes) if (t1 != t) {
        //#     var type1 = t1.Name;
        /// <summary>
        /// Converts the given color to a <see cref="__type1__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type1__"/> color range.
        //# }
        //# if (t1.HasAlpha && !t.HasAlpha) {
        /// The alpha channel is set to __maxvalmap[t1.FieldType]__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type1__ To__type1__() => (__type1__)this;

        /// <summary>
        /// Creates a color from the given <see cref="__type1__"/> color.
        //# if (ismapped(ft, t1.FieldType)) {
        /// The values are mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (t.HasAlpha && !t1.HasAlpha) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__type1__(__type1__ c) => new __type__(c);

        //# }
        //# for (int d = 3; d <= 4; d++) {
        //#     foreach (var vft in Meta.VecFieldTypes) { if (coltovecsupported(ft, vft)) {
        //#         var type1 = Meta.VecTypeOf(d, vft).Name;
        /// <summary>
        /// Converts the given color to a <see cref="__type1__"/> vector.
        //# if (d == 4 && dim == 3) {
        /// W is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type1__ To__type1__() => (__type1__)this;

        /// <summary>
        /// Creates a color from a <see cref="__type1__"/> vector.
        //# if (ismapped(ft, vft)) {
        /// The values are not mapped to the <see cref="__type__"/> color range.
        //# }
        //# if (dim == 4 && d == 3) {
        /// The alpha channel is set to __maxval__.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__type1__(__type1__ c) => new __type__(c);

        //# } } }
        //# foreach (var t1 in Meta.ColorTypes) {
        //#     if (t.Fields.Length != t1.Fields.Length) continue;
        //#     var type1 = t1.Name;
        //#     var ftype1 = t1.FieldType.Name;
        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public __type1__ Map(Func<__ftype__, __ftype1__> channel_fun)
        {
            return new __type1__(/*# fields.ForEach(f => { */channel_fun(__f__)/*# }, comma); */);
        }

        //# }
        public void CopyTo<T>(T[] array, int start, Func<__ftype__, T> element_fun)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = element_fun(__f__);
            //# });
        }

        public void CopyTo<T>(T[] array, int start, Func<__ftype__, int, T> element_index_fun)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = element_index_fun(__f__, __i__);
            //# });
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Indexer in canonical order 0=R, 1=G, 2=B, 3=A (availability depending on color type).
        /// </summary>
        public unsafe __ftype__ this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (__ftype__* ptr = __getptr__) { ptr[i] = value; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (__ftype__* ptr = __getptr__) { return ptr[i]; }
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// __type__ with all components zero.
        /// </summary>
        public static __type__ Zero => new __type__(/*# fields.ForEach(f => {*/__t.MinValue__/*#}, comma); */);

        public static __type__ Black => new __type__(__t.MinValue__);

        public static __type__ Red => new __type__(__t.MaxValue__, __t.MinValue__, __t.MinValue__);
        public static __type__ Green => new __type__(__t.MinValue__, __t.MaxValue__, __t.MinValue__);
        public static __type__ Blue => new __type__(__t.MinValue__, __t.MinValue__, __t.MaxValue__);
        public static __type__ Cyan => new __type__(__t.MinValue__, __t.MaxValue__, __t.MaxValue__);
        public static __type__ Magenta => new __type__(__t.MaxValue__, __t.MinValue__, __t.MaxValue__);
        public static __type__ Yellow => new __type__(__t.MaxValue__, __t.MaxValue__, __t.MinValue__);
        public static __type__ White => new __type__(__t.MaxValue__);

        public static __type__ DarkRed => new __type__(__t.MaxValue__ / 2, __t.MinValue__ / 2, __t.MinValue__ / 2);
        public static __type__ DarkGreen => new __type__(__t.MinValue__ / 2, __t.MaxValue__ / 2, __t.MinValue__ / 2);
        public static __type__ DarkBlue => new __type__(__t.MinValue__ / 2, __t.MinValue__ / 2, __t.MaxValue__ / 2);
        public static __type__ DarkCyan => new __type__(__t.MinValue__ / 2, __t.MaxValue__ / 2, __t.MaxValue__ / 2);
        public static __type__ DarkMagenta => new __type__(__t.MaxValue__ / 2, __t.MinValue__ / 2, __t.MaxValue__ / 2);
        public static __type__ DarkYellow => new __type__(__t.MaxValue__ / 2, __t.MaxValue__ / 2, __t.MinValue__ / 2);
        public static __type__ Gray => new __type__(__t.MaxValue__ / 2);

        //# for (int i = 1; i < 10; i++) {
        //# var val = 0.1 * i; int percent = 10 * i;
        public static __type__ Gray__percent__ => new __type__(__d_to_ft__(__val__));
        //# }

        public static __type__ VRVisGreen => new __type__(__d_to_ft__(0.698), __d_to_ft__(0.851), __d_to_ft__(0.008));

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ == b.__f__/*# }, andand); */;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ != b.__f__/*# }, oror); */;
        }

        #endregion

        #region Color Arithmetic

        //# fdtypes.ForEach(rt => {
        //# var rtype = rt.Name;
        //# if (!ft.IsReal || ft == rt) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ col, __rtype__ scalar)
        {
            //# if (fdtypes.Contains(ft)) {
            return new __type__(/*# fields.ForEach(f => { */
                col.__f__ * scalar/*# }, comma); */);
            //# } else {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)Fun.Round(col.__f__ * scalar)/*# }, comma); */);
            //# }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__rtype__ scalar, __type__ col)
            => col * scalar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__type__ col, __rtype__ scalar)
        {
            __rtype__ f = 1 / scalar;
            //# if (fdtypes.Contains(ft)) {
            return new __type__(/*# fields.ForEach(f => { */
                col.__f__ * f/*# }, comma); */);
            //# } else {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)Fun.Round(col.__f__ * f)/*# }, comma); */);
            //# }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__rtype__ scalar, __type__ col)
        {
            //# if (fdtypes.Contains(ft)) {
            return new __type__(/*# fields.ForEach(f => { */
                scalar / col.__f__/*# }, comma); */);
            //# } else {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)Fun.Round(scalar / col.__f__)/*# }, comma); */);
            //# }
        }

        //# } });
        //# foreach (var t1 in Meta.ColorTypes) { if (t1.HasAlpha != t.HasAlpha) continue;
        //#
        //#     var type1 = t1.Name; var ft1 = t1.FieldType;
        //#     var ft1_from_ft = t1 != t
        //#         ? (ft.IsReal && ft1.IsReal ? "(" + ftype + ")" : "Col." + ft.Caps + "From" + ft1.Caps)
        //#         : "";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator +(__type__ c0, __type1__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ + __ft1_from_ft__(c1.__f__))/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator -(__type__ c0, __type1__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ - __ft1_from_ft__(c1.__f__))/*# }, comma); */);
        }

        //# } // t1
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ c0, __type__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(c0.__f__ * c1.__f__)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator /(__type__ c0, __type__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(c0.__f__ / c1.__f__)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator +(__type__ col, __ftype__ scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(col.__f__ + scalar)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator +(__ftype__ scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(scalar + col.__f__)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator -(__type__ col, __ftype__ scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(col.__f__ - scalar)/*# }, comma); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator -(__ftype__ scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */(__ftype__)(scalar - col.__f__)/*# }, comma); */);
        }

        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(__ftype__ min, __ftype__ max)
        {
            //# channels.ForEach(c => {
            __c__ = __c__.Clamp(min, max);
            //# });
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__ Clamped(__ftype__ min, __ftype__ max)
        {
            return new __type__(/*# channels.ForEach(
                c => { */__c__.Clamp(min, max)/*# }, comma);
                if (t.HasAlpha) {*/, A/*# } */);
        }

        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __htype__ Norm1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return /*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, add); */; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ctype__ Norm2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(/*# channels.ForEach(c => { */__c__ * __c__/*# }, add); */); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ftype__ NormMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Max(/*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, comma); */); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ftype__ NormMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Min(/*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, comma); */); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        public override int GetHashCode()
        {
            return HashCode.GetCombined(/*# t.Fields.ForEach(f => { */__f__/*# }, comma); */);
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")/*# fields.ForEach(f => {*/
                + __f__.ToString(null, CultureInfo.InvariantCulture) /*# }, addqcommaspace); */
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Element setter action.
        /// </summary>
        public static readonly ActionRefValVal<__type__, int, __ftype__> Setter =
            (ref __type__ color, int i, __ftype__ value) =>
            {
                switch (i)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: color.__f__ = value; return;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            };

        /// <summary>
        /// Returns the given color, with each element divided by <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ DivideByInt(__type__ c, int x)
            => c / x;

        #endregion

        #region Parsing

        public static __type__ Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(/*# t.Len.ForEach(p => { */
                __ftype__.Parse(x[__p__], CultureInfo.InvariantCulture)/*# }, comma); */
            );
        }

        public static __type__ Parse(Text t, int bracketLevel = 1)
        {
            return t.NestedBracketSplit(bracketLevel, Text<__ftype__>.Parse, __type__.Setter);
        }

        public static __type__ Parse(Text t)
        {
            return t.NestedBracketSplit(1, Text<__ftype__>.Parse, __type__.Setter);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider fp)
        {
            return ToString(format, fp, "[", ", ", "]");
        }

        /// <summary>
        /// Outputs e.g. a 3D-Vector in the form "(begin)x(between)y(between)z(end)".
        /// </summary>
        public string ToString(string format, IFormatProvider fp, string begin, string between, string end)
        {
            if (fp == null) fp = CultureInfo.InvariantCulture;
            return begin /*# fields.ForEach(f => {*/+ __f__.ToString(format, fp) /*# }, addbetween); */ + end;
        }

        #endregion

        #region IEquatable<__type__> Members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
        {
            return /*# fields.ForEach(f => { */__f__.Equals(other.__f__)/*# }, andand); */;
        }

        #endregion

        #region IRGB Members

        double IRGB.Red
        {
            get { return __ft_to_d__(R); }
            set { R = __d_to_ft__(value); }
        }

        double IRGB.Green
        {
            get { return __ft_to_d__(G); }
            set { G = __d_to_ft__(value); }
        }

        double IRGB.Blue
        {
            get { return __ft_to_d__(B); }
            set { B = __d_to_ft__(value); }
        }

        #endregion

        //# if (t.HasAlpha) {
        #region IOpacity Members

        public double Opacity
        {
            get { return __ft_to_d__(A); }
            set { A = __d_to_ft__(value); }
        }

        #endregion

        //# }
    }

    public static partial class Fun
    {
        #region Interpolation

        //# if (!fdtypes.Contains(ft)) {
        //# fdtypes.ForEach(rt => { var rtype = rt.Name;
        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static __type__ Lerp(this __rtype__ x, __type__ a, __type__ b)
        {
            return new __type__(/*# fields.ForEach(f => {*/Lerp(x, a.__f__, b.__f__)/*#}, comma); */);
        }

        //# });
        //# } else {
        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static __type__ Lerp(this __ftype__ x, __type__ a, __type__ b)
        {
            return new __type__(/*# fields.ForEach(f => {*/Lerp(x, a.__f__, b.__f__)/*#}, comma); */);
        }
        //# }
        #endregion

        #region ApproximateEquals

        //# if (isReal) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b)
        {
            return ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
        }

        //# }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, __ftype__ tolerance)
        {
            return /*# fields.ForEach(f => {*/ApproximateEquals(a.__f__, b.__f__, tolerance)/*# }, andand);*/;
        }

        #endregion
    }

    public static partial class Col
    {
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
        public static bool All__opName__(this __type__ a, __type__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of col are __opName__ s.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(this __type__ col, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */col.__f____bop__s/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ ALL elements of col.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All__opName__(__ftype__ s, __type__ col)
        {
            return (/*# fields.ForEach(f => { */s__bop__col.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is __opName__ the corresponding element of b.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(this __type__ a, __type__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is __opName__ s.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(this __type__ col, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */col.__f____bop__s/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ AT LEAST ONE element of col.
        /// __attention__
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any__opName__(__ftype__ s, __type__ col)
        {
            return (/*# fields.ForEach(f => { */s__bop__col.__f__/*# }, oror); */);
        }
        //# }

        #endregion

        #region Linear Combination

        //# for (int tpc = 4; tpc < 7; tpc+=2) {
        //# foreach (var rt in new[] { fltt, dblt }) { var rtype = rt.Name; var wtype = rt.FieldType.Name; var rtc = rt.FieldType.Caps[0];
        //#     var convert = ft.IsReal ? ""
        //#        : "Col." + ft.Caps + "From" + ft.Caps + "In"
        //#          + (ft.Name == "uint" ? "Double" : rt.FieldType.Caps) + "Clamped";
        //# if (!isReal || wtype == ftype) {
        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ LinCom(
            /*# tpc.ForEach(i => { */__type__ p__i__/*# }, comma); */, ref Tup__tpc__<__wtype__> w)
        {
            return new __type__(/*# channels.ForEach(ch => { */
                __convert__(/*# tpc.ForEach(i => { */p__i__.__ch__ * w.E__i__/*# }, add); */)/*# }, comma); */);
        }

        //# }
        //# if (!isReal) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ LinComRaw__rtc__(
            /*# tpc.ForEach(i => { */__type__ p__i__/*# }, comma); */, ref Tup__tpc__<__wtype__> w)
        {
            return new __rtype__(/*# channels.ForEach(ch => { */
                /*# tpc.ForEach(i => { */p__i__.__ch__ * w.E__i__/*# }, add); }, comma); */);
        }

        //# } // !isReal
        //# } // rt
        //# } // tpc
        #endregion
    }

    //# if (ft != Meta.ByteType && ft != Meta.UShortType) {
    public static class IRandomUniform__type__Extensions
    {
        #region IRandomUniform extensions for __type__

        //# string[] variants;
        //# if (ft == Meta.FloatType) {
        //#     variants = new string[] { "", "Closed", "Open" };
        //# } else if (ft == Meta.DoubleType) {
        //#     variants = new string[] { "", "Closed", "Open", "Full", "FullClosed", "FullOpen" };
        //# } else {
        //#     variants = new string[] { "" };
        //# }
        //# foreach (var v in variants) {
        /// <summary>
        /// Uses Uniform__fcaps____v__() to generate the elements of a __type__ color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Uniform__type____v__(this IRandomUniform rnd)
        {
            return new __type__(/*# fields.ForEach(f => { */rnd.Uniform__fcaps____v__()/*#  }, comma); */);
        }

        //# }
        #endregion
    }

    //# }
    #endregion

    //# }
}
