using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
    //# var vftmap = new Dictionary<Meta.SimpleType, Meta.SimpleType[]>
    //#     {
    //#         { Meta.ByteType, new[] { Meta.IntType, Meta.LongType } },
    //#         { Meta.UShortType, new[] { Meta.IntType, Meta.LongType } },
    //#         { Meta.UIntType, new[] { Meta.LongType } },
    //#         { Meta.FloatType, new[] { Meta.FloatType, Meta.DoubleType } },
    //#         { Meta.DoubleType, new[] { Meta.DoubleType } },
    //#     };
    //# foreach (var t in Meta.ColorTypes) {
    //#     var type = t.Name;
    //#     var ft = t.FieldType;
    //#     var ht = Meta.HighPrecisionTypeOf(ft);
    //#     var htype = ht.Name;
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
    //#     var fields = t.Fields;
    //#     var channels = t.Channels;
    //#     var args = fields.ToLower();
    //#     var cargs = channels.ToLower();
    //#     var d_to_ft = dtoftmap[ft];
    //#     var ft_to_d = fttodmap[ft];
    //#     var b_to_ft = btoftmap[ft];
    //#     var ft_to_b = fttobmap[ft];
    //#     var fabs_p = isReal ? "Fun.Abs(" : "";
    //#     var q_fabs = isReal ? ")" : "";
    #region __type__

    [Serializable]
    public partial struct __type__ : IFormattable, IEquatable<__type__>, IRGB/*# if (t.HasAlpha) { */, IOpacity/*# } */
    {
        #region Constructors

        public __type__(/*# args.ForEach(a => { */__ftype__ __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = __a__/*# }, semicolon); */;
        }

        public __type__(/*# args.ForEach(a => { */int __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = (__ftype__)__a__/*# }, semicolon); */;
        }

        public __type__(/*# args.ForEach(a => { */long __a__/*# }, comma); */)
        {
            /*# fields.ForEach(args, (f,a) => { */__f__ = (__ftype__)__a__/*# }, semicolon); */;
        }

        //# if (!isDouble) {
        public __type__(/*# args.ForEach(a => { */double __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f,a) => {
            __f__ = __d_to_ft__(__a__);
            //# });
        }

        //# } // !isDouble
        //# if (t.HasAlpha) {
        public __type__(/*# cargs.ForEach(a => { */__ftype__ __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = __a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        public __type__(/*# cargs.ForEach(a => { */int __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = (__ftype__)__a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        public __type__(/*# cargs.ForEach(a => { */long __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c, a) => { */__c__ = (__ftype__)__a__/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# if (!isDouble) {
        public __type__(/*# cargs.ForEach(a => { */double __a__/*# }, comma); */)
        {
            /*# channels.ForEach(args,
                    (c,a) => { */__c__ = __d_to_ft__(__a__)/*# }, semicolon); */;
            A = __t.MaxValue__;
        }

        //# } // !isDouble
        //# } // t.HasAlpha
        public __type__(__ftype__ gray)
        {
            /*# channels.ForEach(
                    c => { */__c__ = gray/*# },
                    semicolon); if (t.HasAlpha) {*/; A = __t.MaxValue__/*# } */;
        }

        //# if (!isDouble) {
        public __type__(double gray)
        {
            var value = __d_to_ft__(gray);
            /*# channels.ForEach(
                    c => { */__c__ = value/*# },
                    semicolon); if (t.HasAlpha) {*/; A = __t.MaxValue__/*# } */;
        }

        //# } // !isDouble
        //# foreach (var t1 in Meta.ColorTypes) {
        //#     var convert = t.FieldType != t1.FieldType
        //#         ? "Col." + t.FieldType.Caps + "From" + t1.FieldType.Caps
        //#         : "";
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
        public __type__(__t1.Name__ color, __ftype__ alpha)
        {
            //# channels.ForEach(Meta.VecFields, (c, vf) => {
            __c__ = __convert__(color.__c__);
            //# });
            A = alpha;
        }

        //# } 
        //# } // end For
        //# var vecTypes = new List<Meta.VecType>();
        //# var vecFieldTypes = vftmap[ft];
        //# for (int d = 3; d < 5; d++) {
        //#     foreach (var vft in vecFieldTypes) {
        //#         var vt = Meta.VecTypeOf(d, vft);
        //#         vecTypes.Add(vt);
        //#         var convert = ft != vft ? "("+ ft.Name+")" : "";
        public __type__(__vt.Name__ vec)
        {
            //# channels.ForEach(Meta.VecFields, (c, vf) => {
            __c__ = __convert__(vec.__vf__);
            //# });
            //# if (t.HasAlpha) {
            //#     if (d == 4) {
            A = __convert__(vec.W);
            //#     } else {
            A = __t.MaxValue__;
            //#     }
            //# }
        }

        //#if (t.HasAlpha && d == 3) { // build constructor from Vec3 with explicit alpha
        public __type__(__vt.Name__ vec, __ftype__ alpha)
        {
            //# channels.ForEach(Meta.VecFields, (c, vf) => {
            __c__ = __convert__(vec.__vf__);
            //# });
            A = alpha;
        }

        //#         } 
        //#     }
        //# }
        #endregion

        #region Conversions

        //# foreach (var t1 in Meta.ColorTypes) if (t1 != t) {
        //#     var type1 = t1.Name;
        public static explicit operator __type1__(__type__ color)
        {
            return new __type1__(color);
        }

        //# }
        //# for (int d = 3; d < 5; d++) {
        //#     foreach (var vft in vecFieldTypes) {
        //#         var vt = Meta.VecTypeOf(d, vft);
        //#         var convert = ft != vft ? "("+ vft.Name+")" : "";
        public static explicit operator __vt.Name__(__type__ color)
        {
            return new __vt.Name__(/*# channels.ForEach(c => { */
                __convert__(color.__c__)/*# }, comma); 
                if (d == 4) {
                    if (t.HasAlpha) { */,
                __convert__(color.A)/*#
                    } else { */,
                __convert__(__t.MaxValue__)/*#
                    }
                } */
                );
        }

        //#     }
        //# }
        //# foreach (var t1 in Meta.ColorTypes) if (t1 != t) {
        //#     var type1 = t1.Name;
        public __type1__ To__type1__() { return (__type1__)this; }
        //# }

        /// <summary>
        /// Creates a color from the results of the supplied function of the index.
        /// </summary>
        public __type__(Func<int, __ftype__> index_fun)
        {
            //# fields.ForEach((f, fi) => {
            __f__ = index_fun(__fi__);
            //# });
        }

        //# foreach (var t1 in vecTypes) {
        //#     var type1 = t1.Name;
        public __type1__ To__type1__() { return (__type1__)this; }
        //# }

        //# foreach (var t1 in Meta.ColorTypes) if (t1 != t) {
        //#     var type1 = t1.Name;
        public static readonly Func<__type1__, __type__> From__type1__ = c => new __type__(c);
        //# }

        //# foreach (var t1 in vecTypes) {
        //#     var type1 = t1.Name;
        public static readonly Func<__type1__, __type__> From__type1__ = c => new __type__(c);
        //# }

        //# foreach (var t1 in Meta.ColorTypes) {
        //#     if (t.Fields.Length != t1.Fields.Length) continue;
        //#     var type1 = t1.Name;
        //#     var ftype1 = t1.FieldType.Name;
        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public __type1__ Copy(Func<__ftype__, __ftype1__> channel_fun)
        {
            return Map(channel_fun);
        }

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
        public __ftype__ this[int i]
        {
            set
            {
                switch (i)
                {/*#
                    fields.ForEach((f,i) => {*/
                    case __i__:
                        __f__ = value;
                        break;/*# }); */
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            get
            {
                switch (i)
                {/*#
                    fields.ForEach((f,i) => {*/
                    case __i__:
                        return __f__;/*# }); */
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region Constants

        public static readonly __type__ Black = new __type__(__t.MinValue__);

        public static readonly __type__ Red = new __type__(__t.MaxValue__, __t.MinValue__, __t.MinValue__);
        public static readonly __type__ Green = new __type__(__t.MinValue__, __t.MaxValue__, __t.MinValue__);
        public static readonly __type__ Blue = new __type__(__t.MinValue__, __t.MinValue__, __t.MaxValue__);
        public static readonly __type__ Cyan = new __type__(__t.MinValue__, __t.MaxValue__, __t.MaxValue__);
        public static readonly __type__ Magenta = new __type__(__t.MaxValue__, __t.MinValue__, __t.MaxValue__);
        public static readonly __type__ Yellow = new __type__(__t.MaxValue__, __t.MaxValue__, __t.MinValue__);
        public static readonly __type__ White = new __type__(__t.MaxValue__);

        public static readonly __type__ DarkRed = new __type__(__t.MaxValue__ / 2, __t.MinValue__ / 2, __t.MinValue__ / 2);
        public static readonly __type__ DarkGreen = new __type__(__t.MinValue__ / 2, __t.MaxValue__ / 2, __t.MinValue__ / 2);
        public static readonly __type__ DarkBlue = new __type__(__t.MinValue__ / 2, __t.MinValue__ / 2, __t.MaxValue__ / 2);
        public static readonly __type__ DarkCyan = new __type__(__t.MinValue__ / 2, __t.MaxValue__ / 2, __t.MaxValue__ / 2);
        public static readonly __type__ DarkMagenta = new __type__(__t.MaxValue__ / 2, __t.MinValue__ / 2, __t.MaxValue__ / 2);
        public static readonly __type__ DarkYellow = new __type__(__t.MaxValue__ / 2, __t.MaxValue__ / 2, __t.MinValue__ / 2);
        public static readonly __type__ Gray = new __type__(__t.MaxValue__ / 2);
        
        //# if (isFloat) {
        //#     for (int i = 1; i < 10; i++) { double val = 0.1 * i; int percent = 10 * i;
        public static readonly __type__ Gray__percent__ = new __type__(__val__f);
        //#     }
        public static readonly __type__ VRVisGreen = new __type__(0.698f, 0.851f, 0.008f);
        //# }
        //# if (isByte) {
        public static readonly __type__ VRVisGreen = new __type__(178, 217, 2);
        //# }
        //# if (isUShort) {
        public static readonly __type__ VRVisGreen = new __type__(45743, 53411, 5243);
        //# }

        #endregion

        #region Comparison Operators

        public static bool operator ==(__type__ a, __type__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ == b.__f__/*# }, andand); */;
        }

        public static bool operator !=(__type__ a, __type__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ != b.__f__/*# }, oror); */;
        }

        #endregion

        #region Comparisons

        //# var bops = new[,] { { "<",  "Smaller"        }, { ">" , "Greater"},
        //#                     { "<=", "SmallerOrEqual" }, { ">=", "GreaterOrEqual"},
        //#                     { "==", "Equal" },          { "!=", "Different" } };
        //# var attention1 = "ATTENTION: For example (AllSmaller(a,b)) is not the same as !(AllGreaterOrEqual(a,b)) but !(AnyGreaterOrEqual(a,b)).";
        //# var attention2 = "ATTENTION: For example (a.AllSmaller(b)) is not the same as !(a.AllGreaterOrEqual(b)) but !(a.AnyGreaterOrEqual(b)).";
        //# for(int o = 0; o < bops.GetLength(0); o++) {
        //#     string bop = " " + bops[o,0] + " ", opName = bops[o,1];
        /// <summary>
        /// Returns whether ALL elements of a are __opName__ the corresponding element of b.
        /// __attention1__
        /// </summary>
        public static bool All__opName__(__type__ a, __type__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of this are __opName__ the corresponding element of col.
        /// __attention2__
        /// </summary>
        public bool All__opName__(__type__ col)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__col.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of col are __opName__ s.
        /// __attention1__
        /// </summary>
        public static bool All__opName__(__type__ col, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */col.__f____bop__s/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of col are __opName__ s.
        /// __attention2__
        /// </summary>
        public bool All__opName__(__ftype__ s)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__s/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ ALL elements of col.
        /// __attention1__
        /// </summary>
        public static bool All__opName__(__ftype__ s, __type__ col)
        {
            return (/*# fields.ForEach(f => { */s__bop__col.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is __opName__ the corresponding element of b.
        /// __attention1__
        /// </summary>
        public static bool Any__opName__(__type__ a, __type__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is __opName__ the corresponding element of b.
        /// __attention2__
        /// </summary>
        public bool Any__opName__(__type__ col)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__col.__f__/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is __opName__ s.
        /// __attention1__
        /// </summary>
        public static bool Any__opName__(__type__ col, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */col.__f____bop__s/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of col is __opName__ s.
        /// __attention1__
        /// </summary>
        public bool Any__opName__(__ftype__ s)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__s/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ AT LEAST ONE element of col.
        /// __attention1__
        /// </summary>
        public static bool Any__opName__(__ftype__ s, __type__ col)
        {
            return (/*# fields.ForEach(f => { */s__bop__col.__f__/*# }, oror); */);
        }
        //# }

        #endregion

        #region Color Arithmetic

        public static __type__ operator *(__type__ col, double scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(col.__f__ * scalar)/*# }, comma); */);                
        }

        public static __type__ operator *(double scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(scalar * col.__f__)/*# }, comma); */);
        }

        public static __type__ operator /(__type__ col, double scalar)
        {
            double f = 1.0 / scalar;
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(col.__f__ * f)/*# }, comma); */);
        }

        //# foreach (var t1 in Meta.ColorTypes) { if (t1.HasAlpha != t.HasAlpha) continue;
        //#     
        //#     var type1 = t1.Name; var ft1 = t1.FieldType;
        //#     var ft1_from_ft = t1 != t
        //#         ? (ft.IsReal && ft1.IsReal ? "(" + ftype + ")" : "Col." + ft.Caps + "From" + ft1.Caps)
        //#         : "";
        public static __type__ operator +(__type__ c0, __type1__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ + __ft1_from_ft__(c1.__f__))/*# }, comma); */);
        }

        public static __type__ operator -(__type__ c0, __type1__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ - __ft1_from_ft__(c1.__f__))/*# }, comma); */);
        }

        //# } // t1
        //# if (ft.IsReal) {
        public static __type__ operator *(__type__ c0, __type__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ * c1.__f__)/*# }, comma); */);
        }

        public static __type__ operator /(__type__ c0, __type__ c1)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(c0.__f__ / c1.__f__)/*# }, comma); */);
        }

        public static __type__ operator +(__type__ col, double scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(col.__f__ + scalar)/*# }, comma); */);
        }

        public static __type__ operator +(double scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(scalar + col.__f__)/*# }, comma); */);
        }

        public static __type__ operator -(__type__ col, double scalar)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(col.__f__ - scalar)/*# }, comma); */);
        }

        public static __type__ operator -(double scalar, __type__ col)
        {
            return new __type__(/*# fields.ForEach(f => { */
                (__ftype__)(scalar - col.__f__)/*# }, comma); */);
        }

        //# } // ft.IsReal
        //# for (int d = 3; d < 5; d++) {
        //#     foreach (var vft in vecFieldTypes) {
        //#         var vt = Meta.VecTypeOf(d, vft);
        //#         var vfields = vt.Fields;
        //#         var convert = ft != vft ? "("+ vft.Name+")" : "";
        public static __vt.Name__ operator + (__vt.Name__ vec, __type__ color)
        {
            return new __vt.Name__(/*# vfields.ForEach(channels, (f, c) => { */
                vec.__f__ + __convert__(color.__c__)/*# }, comma); 
                if (d == 4) {
                    if (t.HasAlpha) { */,
                vec.W + __convert__(color.A)/*#
                    } else { */,
                vec.W + __convert__(__t.MaxValue__)/*#
                    }
                } */
                );
        }

        public static __vt.Name__ operator -(__vt.Name__ vec, __type__ color)
        {
            return new __vt.Name__(/*# vfields.ForEach(channels, (f, c) => { */
                vec.__f__ - __convert__(color.__c__)/*# }, comma); 
                if (d == 4) {
                    if (t.HasAlpha) { */,
                vec.W - __convert__(color.A)/*#
                    } else { */,
                vec.W - __convert__(__t.MaxValue__)/*#
                    }
                } */
                );
        }

        //#     }
        //# }
        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(__ftype__ min, __ftype__ max)
        {
            //# channels.ForEach(c => {
            __c__ = __c__.Clamp(min, max);
            //# });
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public __type__ Clamped(__ftype__ min, __ftype__ max)
        {
            return new __type__(/*# channels.ForEach(
                c => { */__c__.Clamp(min, max)/*# }, comma);
                if (t.HasAlpha) {*/, A/*# } */);
        }

        //# if (!isDouble) {
        /// <summary>
        /// Clamps the color channels to the given bounds.
        /// </summary>
        public void Clamp(double min, double max)
        {
            Clamp(__d_to_ft__(min), __d_to_ft__(max));
        }

        /// <summary>
        /// Returns a copy with the color channels clamped to the given bounds.
        /// </summary>
        public __type__ Clamped(double min, double max)
        {
            return Clamped(__d_to_ft__(min), __d_to_ft__(max));
        }

        //# } // !isDouble
        #endregion

        #region Norms

        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |R| + |G| + |B|. /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __htype__ Norm1
        {
            get { return /*# channels.ForEach(c => { */(__htype__)__fabs_p____c____q_fabs__/*# }, add); */; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the color. This is calculated
        /// as sqrt(R^2 + G^2 + B^2). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public double Norm2
        {
            get { return Fun.Sqrt(/*# channels.ForEach(c => { */(double)__c__ * (double)__c__/*# }, add); */); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the color. This is
        /// calculated as max(|R|, |G|, |B|). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ftype__ NormMax
        {
            get { return Fun.Max(/*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, comma); */); }
        }

        /// <summary>
        /// Returns the minimum norm of the color. This is calculated as
        /// min(|R|, |G|, |B|). /*# if (t.HasAlpha) { */The alpha channel is ignored./*# } */
        /// </summary>
        public __ftype__ NormMin
        {
            get { return Fun.Min(/*# channels.ForEach(c => { */__fabs_p____c____q_fabs__/*# }, comma); */); }
        }

        #endregion

        #region Overrides

        public override bool Equals(object other)
        {
            return (other is __type__) ? this == (__type__)other : false;
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombined(/*# t.Fields.ForEach(f => { */__f__/*# }, comma); */);
        }

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")/*# fields.ForEach(f => {*/
                + __f__.ToString(null, Localization.FormatEnUS) /*# }, addqcommaspace); */
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

        //# for (int tpc = 4; tpc < 7; tpc+=2) {
        //# foreach (var rt in new[] { fltt, dblt }) { var rtype = rt.Name; var wtype = rt.FieldType.Name;
        //#     var convert = ft.IsReal ? ""
        //#        : "Col." + ft.Caps + "From" + ft.Caps + "In"
        //#          + (ft.Name == "uint" ? "Double" : rt.FieldType.Caps) + "Clamped";
        /// <summary>
        /// A function that returns the linear combination fo the supplied parameters
        /// with the referenced weight tuple.
        /// </summary>
        public static __type__ LinCom(
            /*# tpc.ForEach(i => { */__type__ p__i__/*# }, comma); */, ref Tup__tpc__<__wtype__> w)
        {
            return new __type__(/*# channels.ForEach(ch => { */
                __convert__(/*# tpc.ForEach(i => { */p__i__.__ch__ * w.E__i__/*# }, add); */)/*# }, comma); */);
        }

        public static __rtype__ LinComRaw__rtype__(
            /*# tpc.ForEach(i => { */__type__ p__i__/*# }, comma); */, ref Tup__tpc__<__wtype__> w)
        {
            return new __rtype__(/*# channels.ForEach(ch => { */
                /*# tpc.ForEach(i => { */p__i__.__ch__ * w.E__i__/*# }, add); }, comma); */);
        }

        //# } // rt
        //# } // tpc
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
                __ftype__.Parse(x[__p__], Localization.FormatEnUS)/*# }, comma); */
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
            return ToString(format, Localization.FormatEnUS);
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
            if (fp == null) fp = Localization.FormatEnUS;
            return begin /*# fields.ForEach(f => {*/+ __f__.ToString(format, fp) /*# }, addbetween); */ + end;
        }

        #endregion

        #region IEquatable<__type__> Members

        public bool Equals(__type__ other)
        {
            return /*# fields.ForEach(f => { */__f__ == other.__f__/*# }, andand); */;
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

    public static partial class ColFun
    {
        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated color between a and b stored in a  __flttype__.
        /// </summary>
        public static __flttype__ LerpRaw__flttype__(this double x, __type__ a, __type__ b)
        {
            return new __flttype__(/*# fields.ForEach(f => { */
                        (float)a.__f__ + ((float)x * (float)((__htype__)b.__f__ - (__htype__)a.__f__))/*#
                        }, comma); */);
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b stored in a __dbltype__.
        /// </summary>
        public static __dbltype__ LerpRaw__dbltype__(this double x, __type__ a, __type__ b)
        {
            return new __dbltype__(/*# fields.ForEach(f => { */
                        (double)a.__f__ + (x * (double)((__htype__)b.__f__ - (__htype__)a.__f__))/*#
                        }, comma); */);
        }

        /// <summary>
        /// Returns the linearly interpolated color between a and b.
        /// </summary>
        public static __type__ Lerp(this double x, __type__ a, __type__ b)
        {
            return new __type__(/*# fields.ForEach(f => { */
                        (__ftype__)((__htype__)a.__f__ + /*# if (hnd) {
                        */(__htype__)/*# } */(x * /*# if (hnd) {
                        */(double)/*# } */((__htype__)b.__f__ - (__htype__)a.__f__)))/*#
                        }, comma); */);
        }

        #endregion
    }

    #endregion

    //# }
}
