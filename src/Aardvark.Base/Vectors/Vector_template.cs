using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# Action comma = () => Out(", ");
    //# Action add = () => Out(" + ");
    //# Action addqcommaspace = () => Out(" + \", \" ");
    //# Action addbetween = () => Out(" + between ");
    //# Action andand = () => Out(" && ");
    //# Action oror = () => Out(" || ");
    //# Action xor = () => Out(" ^ ");
    //# string f0 = Meta.VecFields[0], f1 = Meta.VecFields[1];
    //# string f2 = Meta.VecFields[2], f3 = Meta.VecFields[3];
    //#
    //# foreach (var vt in Meta.VecTypes) {
    //#     int d = vt.Len;
    //#     var ft = vt.FieldType;
    //#     var ct = Meta.ComputationTypeOf(ft);
    //#     var ht = Meta.HighPrecisionTypeOf(ft);
    //#     var vtype = vt.Name;
    //#     var fcaps = ft.Caps;
    //#     var ftype = ft.Name;
    //#     var ctype = ct.Name;
    //#     var htype = ht.Name;
    //#     var v2type = Meta.VecTypeOf(2, ft).Name;
    //#     var v3type = Meta.VecTypeOf(3, ft).Name;
    //#     var v4type = Meta.VecTypeOf(4, ft).Name;
    //#     var fields = vt.Fields;
    //#     var args = fields.ToLower();
    #region __vtype__

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __vtype__ : IVector<double>, ISize__d__d, IFormattable, IEquatable<__vtype__>
    {
        //# fields.ForEach(f => {
        public __ftype__ __f__;
        //# });

        #region Constructors

        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var ftype1 = ft1.Name;
        /// <summary>
        /// Creates a new vector from given __ftype1__ elements.
        /// </summary>
        public __vtype__(/*# args.ForEach(a => { */__ftype1__ __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f, a) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */__a__;
            //# });
        }

        /// <summary>
        /// Creates a new vector by assigning the given __ftype1__ to all elements.
        /// </summary>
        public __vtype__(__ftype1__ v)
        {
            //# fields.ForEach(args, (f, a) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */v;
            //# });
        }

        /// <summary>
        /// Creates a new vector from given array.
        /// </summary>
        public __vtype__(__ftype1__[] a)
        {
            //# fields.ForEach((f, i) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */a[__i__];
            //# });
        }

        /// <summary>
        /// Creates a new vector from given array, starting at specified index.
        /// </summary>
        public __vtype__(__ftype1__[] a, int start)
        {
            //# fields.ForEach((f, i) => {
            __f__ = /*# if (ft != ft1) {*/(__ftype__)/*# } */a[start + __i__];
            //# });
        }

        //# }
        /// <summary>
        /// Creates a vector from the results of the supplied function of the index.
        /// </summary>
        public __vtype__(Func<int, __ftype__> index_fun)
        {
            //# fields.ForEach((f, fi) => {
            __f__ = index_fun(__fi__);
            //# });
        }

        public __vtype__(IVector<double> v)
            : this(/*# d.ForEach(i => {*/v[__i__]/*# }, comma); */)
        { }

        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var vtype1 = vt1.Name;
        //#     if (ft != ft1) {
        /// <summary>
        /// Construct a vector from another vector of type __vtype1__.
        /// </summary>
        public __vtype__(__vtype1__ v)
        {
            //# fields.ForEach(f => {
            __f__ = (__ftype__)v.__f__;
            //# });
        }

        //#     }
        //# }
        //# if (d > 2) { var vt1 = Meta.VecTypeOf(d-1, ft); var fm = fields[d-1];
        public __vtype__(__vt1.Name__ v, __ftype__ w)
        {
            //# vt1.Fields.ForEach(f => {
            __f__ = v.__f__;
            //# });
            __fm__ = w;
        }

        //# }
        //# if (d == 3 || d == 4) {
        //#     foreach (var t1 in Meta.ColorTypes) {
        //#         var ft1 = t1.FieldType;
        //#         if (ft.IsInteger != ft1.IsInteger) continue;
        //#         if (ft == Meta.IntType && ft1 == Meta.UIntType) continue;
        //#         var convert = ft != ft1 ? "("+ ft.Name+")" : "";
        public __vtype__(__t1.Name__ c)
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

        //#     }
        //# }
        #endregion 

        #region Conversions

        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var ftype1 = ft1.Name;
        //#     if (ft != ft1) {
        public static explicit operator __vtype__(__vt1.Name__ v)
        {
            return new __vtype__(/*# fields.ForEach(f => { */(__ftype__)v.__f__/*# }, comma); */);
        }

        //#     }
        public static explicit operator __ftype1__[](__vtype__ v)
        {
            return new __ftype1__[] { /*# fields.ForEach(f => {
                if (ft != ft1) { */(__ftype1__)/*# } */v.__f__/*# }, comma); */ };
        }

        public static explicit operator __vtype__(__ftype1__[] v)
        {
            return new __vtype__(/*# d.ForEach(fi => {
                if (ft != ft1) { */(__ftype__)/*# } */v[__fi__]/*# }, comma); */);
        }

        //# }
        //# if (d == 3 || d == 4) {
        //#     foreach (var t1 in Meta.ColorTypes) {
        //#         var ft1 = t1.FieldType;
        //#         if (ft.IsInteger != ft1.IsInteger) continue;
        //#         if (ft == Meta.IntType && ft1 == Meta.UIntType) continue;
        //#         var convert = ft != ft1 ? "("+ ft1.Name+")" : "";
        public static explicit operator __t1.Name__(__vtype__ v)
        {
            return new __t1.Name__(/*# t1.Channels.ForEach(fields, (c, f) => { */
                __convert__(v.__f__)/*# }, comma);
                if (t1.HasAlpha) {
                    if (d == 4) { */,
                __convert__(v.W)/*#
                    } else { */,
                __convert__(__t1.MaxValue__)/*#
                    }
                } */);
        }

        //#     }
        //# }
        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var vtype1 = vt1.Name;
        //#     var ftype1 = ft1.Name;
        //#     if (ft != ft1) {
        public __vtype1__ To__vtype1__()
        {
            return new __vtype1__(/*# fields.ForEach(f => { */(__ftype1__)__f__/*# }, comma); */);
        }

        //#     }
        //# }
        //# if (d == 3 || d == 4) {
        //#     foreach (var t1 in Meta.ColorTypes) {
        //#         var ft1 = t1.FieldType;
        //#         if (ft.IsInteger != ft1.IsInteger) continue;
        //#         if (ft == Meta.IntType && ft1 == Meta.UIntType) continue;
        //#         var convert = ft != ft1 ? "("+ ft1.Name+")" : "";
        public __t1.Name__ To__t1.Name__()
        {
            return new __t1.Name__(/*# t1.Channels.ForEach(fields, (c, f) => { */
                __convert__(__f__)/*# }, comma);
                if (t1.HasAlpha) {
                    if (d == 4) { */,
                __convert__(W)/*#
                    } else { */,
                 __convert__(__t1.MaxValue__)/*#
                    }
                } */);
        }

        //#     }
        //# }
        //# if (ft.IsReal) {
        //#     var ichar = ft == Meta.DoubleType ? "l" : "i";
        //#     var icast = ft == Meta.DoubleType ? "long" : "int";
        //#     foreach (var floor in new[] { "Floor", "Ceiling" }) {
        public V__d____ichar__ To__floor__V__d____ichar__()
        {
            return new V__d____ichar__(/*# vt.Fields.ForEach(f => { */(__icast__)Fun.__floor__(__f__)/*# }, comma); */);
        }

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
        public __vtype1__ To__vtype1__Inhomo()
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
        public __vtype1__ To__vtype1__Homo()
        {
            return new __vtype1__(/*# fields.ForEach(f => { */__f__/*# }, comma); */, 1);
        }

        //#     }
        //# }
        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var vtype1 = vt1.Name;
        //#     var ftype1 = ft1.Name;
        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public __vtype1__ Copy(Func<__ftype__, __ftype1__> element_fun)
        {
            return new __vtype1__(/*# fields.ForEach(f => { */element_fun(__f__)/*# }, comma); */);
        }

        /// <summary>
        /// Returns a copy with all elements transformed by the supplied function.
        /// </summary>
        public __vtype1__ Copy(Func<__ftype__, int, __ftype1__> element_index_fun)
        {
            return new __vtype1__(/*# fields.ForEach((f, i) => { */element_index_fun(__f__, __i__)/*# }, comma); */);
        }

        public void CopyTo(__ftype1__[] array, int start)
        {
            //# fields.ForEach((f, i) => {
            array[start + __i__] = /*# if (ft != ft1) { */(__ftype1__)/*# } */__f__;
            //# });
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

        public __ftype__[] ToArray()
        {
            return new __ftype__[] { /*# fields.ForEach((f, i) => { */__f__/*# }, comma); */ };
        }

        #endregion

        //# if (d == 3 && ft.IsReal) {
        #region Static Factories

        /// <summary>
        /// Returns an arbitrary normal vector, which
        /// is also normal to either the x, y or z-axis.
        /// </summary>
        public __vtype__ AxisAlignedNormal()
        {
            __vtype__ vector;
            __ftype__ x = X.Abs();
            __ftype__ y = Y.Abs();
            __ftype__ z = Z.Abs();

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

            return Cross(vector).Normalized;
        }

        #endregion

        //# }
        #region Properties and Indexers

        //# fields.ForEach(f => {
        //# var pf = "P_" + f;
        /// <summary>
        /// Property for the field __f__.
        /// Useful when properties are required, but the field __f__ is recommended for general use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __ftype__ __pf__
        {
            get
            {
                return __f__;
            }
            set
            {
                __f__ = value;
            }
        }

        //# });
        /// <summary>
        /// Enumerates all elements of this vector.
        /// </summary>
        public IEnumerable<__ftype__> Elements
        {
            get
            {
                //# fields.ForEach(f => {
                yield return __f__;
                //# });
            }
        }

        /// <summary>
        /// Gets or sets element with given index.
        /// An IndexOutOfRangeException is thrown for invalid index values.
        /// </summary>
        public __ftype__ this[int index]
        {
            get
            {
                switch (index)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: return __f__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: __f__ = value; return;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns the index of the largest dimension of the vector.
        /// </summary>
        public int MajorDim
        {
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
        public int MinorDim
        {
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

        //# if (ft.IsReal) {
        //# var condArray = new[] { "NaN", "Infinity", "PositiveInfinity", "NegativeInfinity", "Tiny" };
        //# var scopeArray = new[] { ftype, ftype, ftype, ftype, "Fun" };
        //# var quantArray = new[] { "Any", "All" };
        //# var actArray = new[] { oror, andand };
        //# condArray.ForEach(scopeArray, (cond, scope) => {
        //# quantArray.ForEach(actArray, (qant, act) => {
        public bool __qant____cond__
        {
            get
            {
                return /*# fields.ForEach((f, i) => { */__scope__.Is__cond__(__f__)/*# }, act); */;
            }
        }

        //# }); // quantArray
        //# }); // condArray
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
        public static readonly __vtype__ Zero = new __vtype__(/*# d.ForEach(i => { */0/*# }, comma); */);

        //# if (ft.IsReal) {
        /// <summary>
        /// All elements zero.
        /// </summary>
        public static readonly __vtype__ Half = new __vtype__(/*# d.ForEach(i => { */0.5/*# }, comma); */);

        //# } // ft.IsReal
        /// <summary>
        /// All elements one.
        /// </summary>
        public static readonly __vtype__ One = new __vtype__(/*# d.ForEach(i => { */1/*# }, comma); */);

        /// <summary>
        /// All elements set to maximum possible value.
        /// </summary>
        public static readonly __vtype__ MaxValue = new __vtype__(/*# d.ForEach(i => { */Constant<__ftype__>.ParseableMaxValue/*# }, comma); */);

        /// <summary>
        /// All elements set to minimum possible value.
        /// </summary>
        public static readonly __vtype__ MinValue = new __vtype__(/*# d.ForEach(i => { */Constant<__ftype__>.ParseableMinValue/*# }, comma); */);

        //# if (ft.IsReal) {
         /// <summary>
        /// All elements set to negative infinity.
        /// </summary>
        public static readonly __vtype__ NegativeInfinity = new __vtype__(/*# d.ForEach(i => { */__ftype__.NegativeInfinity/*# }, comma); */);
        
        /// <summary>
        /// All elements set to positive infinity.
        /// </summary>
        public static readonly __vtype__ PositiveInfinity = new __vtype__(/*# d.ForEach(i => { */__ftype__.PositiveInfinity/*# }, comma); */);
     
        /// <summary>
        /// All elements set to NaN.
        /// </summary>
        public static readonly __vtype__ NaN = new __vtype__(/*# d.ForEach(i => { */__ftype__.NaN/*# }, comma); */);

        //# }
        //# fields.ForEach((f, j) => {
        /// <summary>
        /// Normalized __f__-axis.
        /// </summary>
        public static readonly __vtype__ __f__Axis = new __vtype__(/*# d.ForEach(i => { var v = i != j ? 0 : 1; */__v__/*# }, comma); */);

        //# });
        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var vtype1 = vt1.Name;
        //#     if (ft != ft1) {
        public static readonly Func<__vtype1__, __vtype__> From__vtype1__ = v => new __vtype__(v);
        //#     }
        //# }

        //# if (d == 3 || d == 4) {
        //#     foreach (var t1 in Meta.ColorTypes) {
        //#         var ft1 = t1.FieldType;
        //#         if (ft.IsInteger != ft1.IsInteger) continue;
        //#         if (ft == Meta.IntType && ft1 == Meta.UIntType) continue;
        //#         var type1 = t1.Name;
        public static readonly Func<__type1__, __vtype__> From__type1__ = c => new __vtype__(c);
        //#     }
        //# }

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

        #region Norms

        /// <summary>
        /// Returns the squared length of the vector.
        /// </summary>
        public __ctype__ LengthSquared
        {
            get { return /*# fields.ForEach(f => { */__f__ * __f__ /*# }, add); */; }
        }

        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        public __ctype__ Length
        {
            get { return Fun.Sqrt(/*# fields.ForEach(f => { */__f__ * __f__ /*# }, add); */); }
        }
        
        /// <summary>
        /// Returns the Manhattan (or 1-) norm of the vector. This is
        /// calculated as |x| + |y| + ...
        /// </summary>
        public __ftype__ Norm1
        {
            get { return /*# fields.ForEach(f => { */Fun.Abs(__f__)/*# }, add); */; }
        }

        /// <summary>
        /// Returns the Euclidean (or 2-) norm of the vector. This is the
        /// length of the vector.
        /// </summary>
        public __ctype__ Norm2
        {
            get { return Fun.Sqrt(/*# fields.ForEach(f => { */__f__ * __f__/*# }, add); */); }
        }

        /// <summary>
        /// Returns the infinite (or maximum) norm of the vector. This is
        /// calculated as max(|x|, |y|, ...).
        /// </summary>
        public __ftype__ NormMax
        {
            get { return Fun.Max(/*# fields.ForEach(f => { */Fun.Abs(__f__)/*# }, comma); */); }
        }

        /// <summary>
        /// Returns the minimum norm of the vector. This is calculated as
        /// min(|x|, |y|, ...).
        /// </summary>
        public __ftype__ NormMin
        {
            get { return Fun.Min(/*# fields.ForEach(f => { */Fun.Abs(__f__)/*# }, comma); */); }
        }

        /// <summary>
        /// Gets the p-norm. This is calculated as the p-th root of (|x|^n + |y|^n + ...).
        /// </summary>
        public __ctype__ Norm(__ctype__ p)
        {
            return (/*# fields.ForEach(f => { */
                Fun.Abs(__f__).Pow(p)/*# }, add); */).Pow(1 / p);
        }

        /// <summary>
        /// Returns the squared distance between the given points.
        /// </summary>
        public static double DistanceSquared(__vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */
                (b.__f__ - a.__f__) * (b.__f__ - a.__f__)/*# }, add); */;
        }

        /// <summary>
        /// Returns the distance between the given points.
        /// </summary>
        public static __ctype__ Distance(__vtype__ a, __vtype__ b)
        {
            return /*# if (ctype != "double") {*/(__ctype__)/*# } */Fun.Sqrt(/*# fields.ForEach(f => { */
                        (b.__f__ - a.__f__) * (b.__f__ - a.__f__)/*# }, add); */);
        }

        /// <summary>
        /// Returns the Manhatten (or 1-) distance between two vectors.
        /// </summary>
        public static __ctype__ Distance1(__vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */
                Fun.Abs(b.__f__ - a.__f__)/*# }, add); */;
        }

        /// <summary>
        /// Returns the p-distance between two vectors.
        /// </summary>
        public static __ctype__ Distance(__vtype__ a, __vtype__ b, __ctype__ p)
        {
            return (/*# fields.ForEach(f => { */
                        Fun.Abs(b.__f__ - a.__f__).Pow(p)/*# }, add); */).Pow(1 / p);
        }

        /// <summary>
        /// Returns the maximal absolute distance between the components of
        /// the two vectors.
        /// </summary>
        public static __ftype__ DistanceMax(__vtype__ a, __vtype__ b)
        {
            return Fun.Max(/*# fields.ForEach(f => { */
                        Fun.Abs(b.__f__ - a.__f__)/*# }, comma); */);
        }

        /// <summary>
        /// Returns the minimal absolute distance between the components of
        /// the two vectors.
        /// </summary>
        public static __ftype__ DistanceMin(__vtype__ a, __vtype__ b)
        {
            return Fun.Min(/*# fields.ForEach(f => { */
                        Fun.Abs(b.__f__ - a.__f__)/*# }, comma); */);
        }

        //# if (ft.IsReal) {
        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        public void Normalize()
        {
            var s = Length; if (s == 0) return;
            s = 1 / s;
            /*# fields.ForEach(f => { */__f__ *= s; /*# }); */
        }

        /// <summary>
        /// Returns a normalized copy of this vector.
        /// </summary>
        public __vtype__ Normalized
        {
            get
            {
                var s = Length; if (s == 0) return __vtype__.Zero;
                s = 1 / s;
                return new __vtype__(/*# fields.ForEach(f => { */__f__ * s/*# }, comma); */);
            }
        }

        //# if (d == 2) {
        /// <summary>
        /// Returns a copy of the vector with the maximum component length of
        /// exactly 1. This corresponds to mapping the vector onto an origin-
        /// centered square with side length 2.
        /// </summary>
        public __vtype__ CubeMapped
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

        public __vtype__ Rot90 { get { return new __vtype__(-Y, X); } }
        public __vtype__ Rot180 { get { return new __vtype__(-X, -Y); } }
        public __vtype__ Rot270 { get { return new __vtype__(Y, -X); } }

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
        public __vtype__ CubeMapped__facepar__
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
        private static readonly __vtype__[] s_fromCubeCode =
            new __vtype__[] { -__vtype__.XAxis, -__vtype__.YAxis, -__vtype__.ZAxis,
                        __vtype__.XAxis, __vtype__.YAxis, __vtype__.ZAxis };

        /// <summary>
        /// Return the vector for the supplied cube face code.
        /// 0 ... -XAxis, 1 ... -YAxis, 2 ... -ZAsix, 3 ... XAxis, 4 ... YAxis, 5 ... ZAxis.
        /// </summary>
        public static __vtype__ FromCubeFaceCode(int i) { return s_fromCubeCode[i]; }

        /// <summary>
        /// Return an index for the cube face onto which the vector points.
        /// 0 ... -XAxis, 1 ... -YAxis, 2 ... -ZAsix, 3 ... XAxis, 4 ... YAxis, 5 ... ZAxis.
        /// </summary>
        public int CubeFaceCode
        {
            get
            {
                double x = System.Math.Abs(X);
                double y = System.Math.Abs(Y);
                double z = System.Math.Abs(Z);
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
        public __vtype__ CubeMapped
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

        #region Operations

        //# foreach (var ft1 in Meta.VecFieldTypes) {
        //#     var vt1 = Meta.VecTypeOf(d, ft1);
        //#     var ftype1 = ft1.Name;
        /// <summary>
        /// Sets the elements of a vector to the given __ftype1__ elements.
        /// </summary>
        public void Set(/*# args.ForEach(a => { */__ftype1__ __a__/*# }, comma); */)
        {
            //# fields.ForEach(args, (f, a) => {
            __f__ = /*# if (ft != ft1) { */(__ftype__)/*# } */__a__;
            //# });
        }

        //# }
        //# if (ft.IsReal) {
        /// <summary>
        /// Gets a copy of this vector, with each element set to the largest integer
        /// less than or equal to the element's current value.
        /// </summary>
        public __vtype__ Floor
        {
            get { return new __vtype__(/*# fields.ForEach(f => { */System.Math.Floor(__f__)/*# }, comma); */); }
        }

        /// <summary>
        /// Gets a copy of this vector, with each element set to the smallest integer
        /// greater than or equal to the element's current value.
        /// </summary>
        public __vtype__ Ceiling
        {
            get { return new __vtype__(/*# fields.ForEach(f => { */System.Math.Ceiling(__f__)/*# }, comma); */); }
        }

        /// <summary>
        /// Returns a copy of this vector, with each element rounded to the nearest integer.
        /// </summary>
        public __vtype__ Round()
        {
            return new __vtype__(/*# fields.ForEach(f => { */(__ftype__)System.Math.Round(__f__)/*# }, comma); */);
        }

        /// <summary>
        /// Returns a copy of this vector, with each element rounded to the nearest integer.
        /// </summary>
        public __vtype__ Round(MidpointRounding mode)
        {
            return new __vtype__(/*# fields.ForEach(f => { */(__ftype__)System.Math.Round(__f__, mode)/*# }, comma); */);
        }

        /// <summary>
        /// Returns a copy of this vector, with each element rounded to the nearest integer
        /// to the given number of fractional digits.
        /// </summary>
        public __vtype__ Round(int digits)
        {
            return new __vtype__(/*# fields.ForEach(f => { */(__ftype__)System.Math.Round(__f__, digits)/*# }, comma); */);
        }

        /// <summary>
        /// Returns a copy of this vector, with each element rounded to the nearest integer
        /// to the given number of fractional digits.
        /// </summary>
        public __vtype__ Round(int digits, MidpointRounding mode)
        {
            return new __vtype__(/*# fields.ForEach(f => { */(__ftype__)System.Math.Round(__f__, digits, mode)/*# }, comma); */);
        }
        //# }

        /// <summary>
        /// Gets a copy of this vector with all elements set to their absolute value.
        /// </summary>
        public __vtype__ Abs
        {
            get { return new __vtype__(/*# fields.ForEach(f => { */__f__.Abs()/*# }, comma); */); }
        }

        /// <summary>
        /// Negates this vector and returns this.
        /// </summary>
        public void Negate()
        {
            //# fields.ForEach(f => {
            __f__ = -__f__;
            //# });
        }

        /// <summary>
        /// Gets a negated copy of this vector.
        /// </summary>
        public __vtype__ Negated
        {
            get { return new __vtype__(/*# fields.ForEach(f => { */-__f__/*# }, comma); */); }
        }

        /// <summary>
        /// Returns a negated copy of the specified vector.
        /// </summary>
        public static __vtype__ Negate(__vtype__ v)
        {
            return new __vtype__(/*# fields.ForEach(f => { */-v.__f__/*# }, comma); */);
        }

        /// <summary>
        /// Returns a negated copy of the specified vector.
        /// </summary>
        public static __vtype__ operator -(__vtype__ v)
        {
            return new __vtype__(/*# fields.ForEach(f => { */-v.__f__/*# }, comma); */);
        }

        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        public static __ftype__ Dot(__vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ * b.__f__/*# }, add); */;
        }

        /// <summary>
        /// Returns the dot product with the specified vector.
        /// </summary>
        public __ftype__ Dot(__vtype__ v)
        {
            return /*# fields.ForEach(f => { */__f__ * v.__f__/*# }, add); */;
        }

        //# if (ft.IsInteger) {
        public Vec.DirFlags DirFlags()
        {
            Vec.DirFlags flags = Vec.DirFlags.None;
            //# fields.ForEach(f => {
            if (__f__ > 0) flags |= Vec.DirFlags.Positive__f__;
            if (__f__ < 0) flags |= Vec.DirFlags.Negative__f__;
            //# });
            return flags;
        }

        //# }
        //# if (ft.IsReal) {
        public __vtype__ Reflected(__vtype__ normal)
        {
            return 2 * Dot(normal) * normal - this;
        }

        public Vec.DirFlags DirFlags()
        {
            Vec.DirFlags flags = Vec.DirFlags.None;
            //# fields.ForEach(f => {
            if (__f__ > Constant<__ftype__>.PositiveTinyValue) flags |= Vec.DirFlags.Positive__f__;
            if (__f__ < Constant<__ftype__>.NegativeTinyValue) flags |= Vec.DirFlags.Negative__f__;
            //# });
            return flags;
        }

        //# } // ft.IsReal
        //# if (d == 3) {
        /// <summary>
        /// Returns the cross product of two vectors.
        /// </summary>
        public static __vtype__ Cross(__vtype__ a, __vtype__ b)
        {
            return new __vtype__(
            a.__f1__ * b.__f2__ - a.__f2__ * b.__f1__,
            a.__f2__ * b.__f0__ - a.__f0__ * b.__f2__,
            a.__f0__ * b.__f1__ - a.__f1__ * b.__f0__
            );
        }

        /// <summary>
        /// Returns the cross product with the specified vector.
        /// </summary>
        public __vtype__ Cross(__vtype__ v)
        {
            return new __vtype__(
            __f1__ * v.__f2__ - __f2__ * v.__f1__,
            __f2__ * v.__f0__ - __f0__ * v.__f2__,
            __f0__ * v.__f1__ - __f1__ * v.__f0__
            );
        }
        //# }
        //# if (d == 2) {
        /// <summary>
        /// Returns the cross product of vector a.
        /// In 2D the cross product is simply a vector that is normal 
        /// to the given vector (i.e. {x,y} -> {-y,x})
        /// </summary>
        public static __vtype__ Cross(__vtype__ a)
        {
            return new __vtype__(- a.__f1__, a.__f0__);
        }

        /// <summary>
        /// Returns the cross product of this vector.
        /// In 2D the cross product is simply a vector that is normal 
        /// to the given vector (i.e. {x,y} -> {-y,x})
        /// </summary>
        public __vtype__ Cross()
        {
            return new __vtype__(- __f1__, __f0__);
        }

        //# }
        //# if (ft.IsReal) {
        /// <summary>
        /// Gets a copy of this vector containing the reciprocal (1/x) of each element.
        /// </summary>
        public __vtype__ Reciprocal
        {
            get { return new __vtype__(/*# fields.ForEach(f => { */1 / __f__/*# }, comma); */); }
        }

        //# }
        //# var ops = new[] {" + ", " - ", " * ", " / "};
        //# var opactions = new[,]  { { "operator +", "operator -", "operator *", "operator /"},
        //#                           { "Add", "Subtract", "Multiply", "Divide"}};
        //# var opnames = new[] {"sum", "difference", "product", "fraction"};
        //# for (int o = 0; o < ops.Length; o++) {
        //#     var op = ops[o];
        //#     var opname = opnames[o];
        //#     for (int action = 0; action < 2; action++) {
        //#         var opaction = opactions[action, o];
        /// <summary>
        /// Returns component-wise __opname__ of two vectors.
        /// </summary>
        public static __vtype__ __opaction__(__vtype__ a, __vtype__ b)
        {
            return new __vtype__(/*# fields.ForEach(f => { */a.__f____op__b.__f__/*# }, comma); */);
        }

        /// <summary>
        /// Returns component-wise __opname__ of vector and scalar.
        /// </summary>
        public static __vtype__ __opaction__(__vtype__ v, __ftype__ s)
        {
            return new __vtype__(/*# fields.ForEach(f => { */v.__f____op__s/*#  }, comma); */);
        }

        /// <summary>
        /// Returns component-wise __opname__ of scalar and vector.
        /// </summary>
        public static __vtype__ __opaction__(__ftype__ s, __vtype__ v)
        {
            return new __vtype__(/*# fields.ForEach(f => { */s__op__v.__f__/*# }, comma); */);
        }

        //#     }
        //# }
        /// Attention: NEVER implement operators &lt;, &lt;=, &gt;=, &gt;, 
        /// since these are not defined in a Vector space. 
        /// Use AllSmaller() and similar comparators!
        #endregion

        #region Comparisons

        public bool IsNaN
        {
#pragma warning disable 1718
            get { return this != this; }
#pragma warning restore 1718
        }

        public static bool operator ==(__vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ == b.__f__/*# }, andand); */;
        }

        public static bool operator ==(__vtype__ v, __ftype__ s)
        {
            return /*# fields.ForEach(f => { */v.__f__ == s/*# }, andand); */;
        }

        public static bool operator ==(__ftype__ s, __vtype__ v)
        {
            return /*# fields.ForEach(f => { */s == v.__f__/*# }, andand); */;
        }

        public static bool operator !=(__vtype__ a, __vtype__ b)
        {
            return /*# fields.ForEach(f => { */a.__f__ != b.__f__/*# }, oror); */;
        }

        public static bool operator !=(__vtype__ v, __ftype__ s)
        {
            return /*# fields.ForEach(f => { */v.__f__ != s/*# }, oror); */;
        }

        public static bool operator !=(__ftype__ s, __vtype__ v)
        {
            return /*# fields.ForEach(f => { */s != v.__f__/*# }, oror); */;
        }

        /// <summary>
        /// Returns whether the given vectors are equal within the given tolerance.
        /// </summary>
        public static bool ApproxEqual(__vtype__ a, __vtype__ b, __ftype__ tolerance)
        {
            return /*# fields.ForEach(f => { */(a.__f__ - b.__f__).Abs() <= tolerance/*# }, andand); */;
        }
        
        /// <summary>
        /// Returns whether this vector is equal to the specified vector within the given tolerance.
        /// </summary>
        public bool ApproxEqual(__vtype__ v, __ftype__ tolerance)
        {
            return /*# fields.ForEach(f => { */(__f__ - v.__f__).Abs() <= tolerance/*# }, andand); */;
        }

        //# if (ft.IsReal) {       
        /// <summary>
        /// Returns whether the given vectors are equal within
        /// Constant{__ftype__}.PositiveTinyValue.
        /// </summary>
        public static bool ApproxEqual(__vtype__ a, __vtype__ b)
        {
            return ApproxEqual(a, b, Constant<__ftype__>.PositiveTinyValue);
        }

        //# }
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
        public static bool All__opName__(__vtype__ a, __vtype__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of this are __opName__ the corresponding element of v.
        /// __attention2__
        /// </summary>
        public bool All__opName__(__vtype__ v)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__v.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of v are __opName__ s.
        /// __attention1__
        /// </summary>
        public static bool All__opName__(__vtype__ v, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */v.__f____bop__s/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether ALL elements of v are __opName__ s.
        /// __attention2__
        /// </summary>
        public bool All__opName__(__ftype__ s)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__s/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ ALL elements of v.
        /// __attention1__
        /// </summary>
        public static bool All__opName__(__ftype__ s, __vtype__ v)
        {
            return (/*# fields.ForEach(f => { */s__bop__v.__f__/*# }, andand); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is __opName__ the corresponding element of b.
        /// __attention1__
        /// </summary>
        public static bool Any__opName__(__vtype__ a, __vtype__ b)
        {
            return (/*# fields.ForEach(f => { */a.__f____bop__b.__f__/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of a is __opName__ the corresponding element of b.
        /// __attention2__
        /// </summary>
        public bool Any__opName__(__vtype__ v)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__v.__f__/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of v is __opName__ s.
        /// __attention1__
        /// </summary>
        public static bool Any__opName__(__vtype__ v, __ftype__ s)
        {
            return (/*# fields.ForEach(f => { */v.__f____bop__s/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether AT LEAST ONE element of v is __opName__ s.
        /// __attention1__
        /// </summary>
        public bool Any__opName__(__ftype__ s)
        {
            return (/*# fields.ForEach(f => { */this.__f____bop__s/*# }, oror); */);
        }

        /// <summary>
        /// Returns whether a is __opName__ AT LEAST ONE element of v.
        /// __attention1__
        /// </summary>
        public static bool Any__opName__(__ftype__ s, __vtype__ v)
        {
            return (/*# fields.ForEach(f => { */s__bop__v.__f__/*# }, oror); */);
        }

        //# }
        /// <summary>
        /// Returns the componentwise minimum vector.
        /// </summary>
        public static __vtype__ Min(__vtype__ v0, __vtype__ v1)
        {
            return new __vtype__(/*# fields.ForEach(f => { */Fun.Min(v0.__f__, v1.__f__)/*# }, comma); */);
        }

        /// <summary>
        /// Returns the componentwise maximum vector.
        /// </summary>
        public static __vtype__ Max(__vtype__ v0, __vtype__ v1)
        {
            return new __vtype__(/*# fields.ForEach(f => { */Fun.Max(v0.__f__, v1.__f__)/*# }, comma); */);
        }

        /// <summary>
        /// Compare x-coordinate before y-coordinate, aso.
        /// </summary>
        public static int LexicalCompare(__vtype__ v0, __vtype__ v1)
        {
            //# fields.ForEach(f => {
            if (v0.__f__ < v1.__f__) return -1;
            if (v0.__f__ > v1.__f__) return +1;
            //# });
            return 0;
        }

        /// <summary>
        /// Compare x-coordinate before y-coordinate, aso.
        /// </summary>
        public int LexicalCompare(__vtype__ v1)
        {
            //# fields.ForEach(f => {
            if (__f__ < v1.__f__) return -1;
            if (__f__ > v1.__f__) return +1;
            //# });
            return 0;
        }

        #endregion

        #region IEquatable<__vtype__> Members

        public bool Equals(__vtype__ other)
        {
            return /*# fields.ForEach(f => { */__f__ == other.__f__/*# }, andand); */;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return ToString(null, Localization.FormatEnUS);
        }
        
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

        public override int GetHashCode()
        {
            return HashCode.GetCombined(/*# fields.ForEach(f => { */__f__/*# }, comma); */); 
        }
        
        public override bool Equals(object other)
        {
            return (other is __vtype__) ? this == (__vtype__)other : false;
        }

        public Text ToText(int bracketLevel = 1)
        {
            return
                ((bracketLevel == 1 ? "[" : "")/*# fields.ForEach(f => {*/
                + __f__.ToString(null, Localization.FormatEnUS) /*# }, addqcommaspace); */
                + (bracketLevel == 1 ? "]" : "")).ToText();
        }

        #endregion

        #region Parsing

        public static __vtype__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __vtype__(/*# d.ForEach(p => { */
                __ftype__.Parse(x[__p__], Localization.FormatEnUS)/*# }, comma); */
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

        //# var snames  = new string[] { "O", "I", "X", "Y", "Z", "W" };
        //# var svalues = new string[] { "0", "1", "X", "Y", "Z", "W" };
        //# var s = d + 2;
        //# for (int xi = 0; xi < s; xi++) { var x = svalues[xi];
        //#     for (int yi = 0; yi < s; yi++) { var y = svalues[yi];
        //#         var name = snames[xi] + snames[yi];
        //#         if (xi < 2 && yi < 2) {
        //#             if (d == 2) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly __v2type__ __name__ = new __v2type__(__x__, __y__);
        //#             }
        //#         }
        //#         else {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __v2type__ __name__ { get { return new __v2type__(__x__, __y__); } }      
        //#         }
        //#     }
        //# }
        //# for (int xi = 0; xi < s; xi++) { var x = svalues[xi];
        //#     for (int yi = 0; yi < s; yi++) { var y = svalues[yi];
        //#         for (int zi = 0; zi < s; zi++) { var z = svalues[zi];
        //#             var name = snames[xi] + snames[yi] + snames[zi];
        //#             if (xi < 2 && yi < 2 && zi < 2) {
        //#                 if (d == 3) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly __v3type__ __name__ = new __v3type__(__x__, __y__, __z__);   
        //#                 }
        //#             }
        //#             else {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __v3type__ __name__ { get { return new __v3type__(__x__, __y__, __z__); } }      
        //#             }
        //#         }
        //#     }
        //# }
        //# // if (d != 4) { // lets try if it does not explode
        //#     for (int xi = 0; xi < s; xi++) { var x = svalues[xi];
        //#         for (int yi = 0; yi < s; yi++) { var y = svalues[yi];
        //#             for (int zi = 0; zi < s; zi++) { var z = svalues[zi];
        //#                 for (int wi = 0; wi < s; wi++) { var w = svalues[wi];
        //#                     var name = snames[xi] + snames[yi] + snames[zi] + snames[wi];
        //#                     if (xi < 2 && yi < 2 && zi < 2 && wi < 2) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly __v4type__ __name__ = new __v4type__(__x__, __y__, __z__, __w__);
        //#                     }
        //#                     else {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __v4type__ __name__ { get { return new __v4type__(__x__, __y__, __z__, __w__); } }      
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
            get { return (double)this[(int)i]; }
            set { this[(int)i] = (__ftype__)value; }
        }

        #endregion

        #region ISize__d__d Members

        public V__d__d Size__d__d { get { return (V__d__d)this; } }

        #endregion

        #region IVector

        public long Dim
        {
            get { return __d__; }
        }

        public object GetValue(long index)
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
        public static readonly __vtype__EqualityComparer Default
            = new __vtype__EqualityComparer();

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

    /// <summary>
    /// This static class for extension functions for __vtype__ has a non-
    /// standard name ending in "Fun" in order to shorten the function name
    /// when used as function parameters.
    /// </summary>
    public static partial class VecFun
    {
        //# if (ft.IsReal) {
        //# if (d == 2) {
        #region 2D Vector Arithmetics

        /// <summary>
        /// Dot product of vector with dir rotated by 90 degrees counterclockwise.
        /// </summary>
        public static __ftype__ Dot90(this __vtype__ v, __vtype__ dir)
        {
            return v.Y * dir.X - v.X * dir.Y;
        }

        /// <summary>
        /// Dot product of vector with dir rotated by 180 degrees.
        /// </summary>
        public static __ftype__ Dot180(this __vtype__ v, __vtype__ dir)
        {
            return -(v.X * dir.X + v.Y * dir.Y);
        }

        /// <summary>
        /// Dot product of vector with dir rotated by 270 degrees counterclockwise.
        /// </summary>
        public static __ftype__ Dot270(this __vtype__ v, __vtype__ dir)
        {
            return v.X * dir.Y - v.Y * dir.X;
        }

        /// <summary>
        /// Returns the left value of the direction v with respect to the
        /// line from p0 to p1.
        /// </summary>
        public static __ftype__ DirLeftOfLineValue(this __vtype__ v, __vtype__ p0, __vtype__ p1)
        {
            return v.X * (p0.Y - p1.Y) + v.Y * (p1.X - p0.X);
        }

        /// <summary>
        /// Returns the right value of the direction v with respect to the
        /// line from p0 to p1.
        /// </summary>
        public static __ftype__ DirRightOfLineValue(this __vtype__ v, __vtype__ p0, __vtype__ p1)
        {
            return v.X * (p1.Y - p0.Y) + v.Y * (p0.X - p1.X);
        }

        /// <summary>
        /// Returns the left value of the point p with respect to the
        /// line from p0 to p1.
        /// </summary>
        public static __ftype__ PosLeftOfLineValue(this __vtype__ p, __vtype__ p0, __vtype__ p1)
        {
            return (p.X - p0.X) * (p0.Y - p1.Y) + (p.Y - p0.Y) * (p1.X - p0.X);
        }

        /// <summary>
        /// Returns the right value of the point p with respect to the
        /// line from p0 to p1.
        /// </summary>
        public static __ftype__ PosRightOfLineValue(this __vtype__ p, __vtype__ p0, __vtype__ p1)
        {
            return (p.X - p0.X) * (p1.Y - p0.Y) + (p.Y - p0.Y) * (p0.X - p1.X);
        }

        #endregion

        //# } // d == 2
        #region Distance Functions

        //# foreach (var hasT in new[] { false, true }) {
        /// <summary>
        /// Returns the minimal euclidean distance between the supplied query
        /// point and the line segment defined by the two supplied line end
        /// points.
        /// </summary>
        public static __ftype__ DistanceToLine(
                this __vtype__ query, __vtype__ p0, __vtype__ p1/*# if (hasT) { */, out __ftype__ t/*# } */)
        {
            var p0p1 = p1 - p0;
            var p0q = query - p0;
            /*# if (!hasT) { */
            var /*# } */t = __vtype__.Dot(p0q, p0p1);
            if (t <= 0) { /*# if (hasT) { */t = 0; /*# } */return __vtype__.Distance(query, p0); }
            var denom = p0p1.LengthSquared;
            if (t >= denom) { /*# if (hasT) { */t = 1; /*# } */return __vtype__.Distance(query, p1); }
            t /= denom;
            return __vtype__.Distance(query, p0 + t * p0p1);
        }

        /// <summary>
        /// Returns the minimal euclidean distance between the supplied query
        /// point and the infinite line defined by two points.
        /// </summary>
        public static __ftype__ DistanceToInfiniteLine(
                this __vtype__ query, __vtype__ p0, __vtype__ p1/*# if (hasT) { */, out __ftype__ t/*# } */)
        {
            var p0p1 = p1 - p0;
            var p0q = query - p0;
            /*# if (!hasT) { */
            var /*# } */t = __vtype__.Dot(p0q, p0p1);
            var denom = p0p1.LengthSquared;
            t /= denom;
            return __vtype__.Distance(query, p0 + t * p0p1);
        }

        //# } // hasT
        #endregion

        #region Interpolation

        /// <summary>
        /// Returns the linearly interpolated vector between a and b.
        /// </summary>
        public static __vtype__ Lerp(this __ftype__ t, __vtype__ a, __vtype__ b)
        { return a + (b - a) * t; }

        //# if (ft != ht) {
        /// <summary>
        /// Returns the linearly interpolated vector between a and b.
        /// </summary>
        public static __vtype__ Lerp(this __htype__ t, __vtype__ a, __vtype__ b)
        { return a + (b - a) * (__ftype__)t; }

        //# }
        #endregion

        #region Linear Combination

        //# if (ft.IsReal) {
        //# for (int tpc = 2; tpc < 8; tpc++ ) {
        public static __vtype__ LinCom(/*# tpc.ForEach(i => { */__vtype__ p__i__/*# }, comma); */, ref Tup__tpc__<__ftype__> w)
        {
            return /*# tpc.ForEach(i => { */p__i__ * w.E__i__/*# }, add); */;
        }

        //# } // tpc       
        //# } // isreal
        #endregion

        #region Elementwise Fun

        /// <summary>
        /// Returns element-wise minimum of given vectors.
        /// </summary>
        public static __vtype__ Pow(__vtype__ x, __htype__ y)
        {
            return new __vtype__(/*# fields.ForEach(f => { */
                        Fun.Pow(x.__f__, y)/*# }, comma); */);
        }

        /// <summary>
        /// Returns element-wise minimum of given vectors.
        /// </summary>
        public static __vtype__ Pow(__vtype__ x, __vtype__ y)
        {
            return new __vtype__(/*# fields.ForEach(f => { */
                        Fun.Pow(x.__f__, y.__f__)/*# }, comma); */);
        }

        #endregion

        //# } // ft.IsReal
        #region Elementwise Extrema

        //# for (int k = 2; k < 5; k++) {
        /// <summary>
        /// Returns element-wise minimum of given vectors.
        /// </summary>
        public static __vtype__ Min(/*# k.ForEach(i => {*/__vtype__ v__i__/*#}, comma);*/)
        {
            return new __vtype__(/*# fields.ForEach(f => { */
                        Fun.Min(/*# k.ForEach(i => {*/v__i__.__f__/*#}, comma);*/)/*# }, comma); */);
        }

        /// <summary>
        /// Returns element-wise minimum of given vectors.
        /// </summary>
        public static __vtype__ Max(/*# k.ForEach(i => {*/__vtype__ v__i__/*#}, comma);*/)
        {
            return new __vtype__(/*# fields.ForEach(f => { */
                        Fun.Max(/*# k.ForEach(i => {*/v__i__.__f__/*#}, comma);*/)/*# }, comma); */);
        }

        //# }
        #endregion
        
        #region ArrayExtensions

        //# foreach (var it in Meta.IndexTypes) { var itype = it.Name;
        //#     var prefix = it == Meta.LongType ? "Long" : "";
        public static __itype__ __prefix__IndexOfClosestPoint(this __vtype__[] pointArray, __vtype__ point)
        {
            var bestDist = __vtype__.DistanceSquared(point, pointArray[0]);
            __itype__ bestIndex = 0;
            __itype__ count = pointArray.__prefix__Length;
            for (__itype__ i = 1; i < count; i++)
            {
                var dist = __vtype__.DistanceSquared(point, pointArray[i]);
                if (dist < bestDist) { bestDist = dist; bestIndex = i; }
            }
            return bestIndex;
        }

        public static __itype__ __prefix__IndexOfClosestPoint(
                this __vtype__[] array, __itype__ start, __itype__ count,
                __vtype__ point)
        {
            var bestDist = __vtype__.DistanceSquared(point, array[start]);
            __itype__ bestIndex = 0;
            for (__itype__ i = start + 1, e = start + count; i < e; i++)
            {
                var dist = __vtype__.DistanceSquared(point, array[i]);
                if (dist < bestDist) { bestDist = dist; bestIndex = i; }
            }
            return bestIndex;
        }

        public static __itype__ __prefix__IndexOfClosestPoint<T>(
                this T[] array, __itype__ start, __itype__ count,
                Func<T, __vtype__> pointSelector, __vtype__ point)
        {
            var bestDist = __vtype__.DistanceSquared(point, pointSelector(array[start]));
            __itype__ bestIndex = 0;
            for (__itype__ i = start + 1, e = start + count; i < e; i++)
            {
                var dist = __vtype__.DistanceSquared(point, pointSelector(array[i]));
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

    #endregion

    //# } // vt
}