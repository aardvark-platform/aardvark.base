using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    //# Action<Action> Ignore = a => {};
    //# Action space = () => Out(" ");
    //# Action comma = () => Out(", ");
    //# Action add = () => Out(" + ");
    //# Action mul = () => Out(" * ");
    //# Action andand = () => Out(" && ");
    //# var bools = new[] { false, true };
    //# Func<int, bool, string> tni = (n, v) =>
    //#     v ? "t" + n + ".Getter(t" + n + ".Data, i)" : "t" + n + ".Data[i]";
    //# Func<int, bool, string> tnin = (n, v) =>
    //#     v ? "t" + n + ".Getter(t" + n + ".Data, i" + n + ")" : "t" + n + ".Data[i" + n + "]";
    //# Func<int, bool, string> tnt = (n, v) =>
    //#     v ? "T" + n + "d, T" + n : "T" + n;
    //# Func<int, bool, string> tdi = (n, v) =>
    //#     v ? "t" + Meta.VecArgs[n-1] + ".Getter(t" + Meta.VecArgs[n-1] + ".Data, i)"
    //#       : "t" + Meta.VecArgs[n-1] + ".Data[i]";
    //# Func<int, bool, string> tdin = (n, v) =>
    //#     v ? "t" + Meta.VecArgs[n-1] + ".Getter(t" + Meta.VecArgs[n-1] + ".Data, i" + n + ")"
    //#       : "t" + Meta.VecArgs[n-1] + ".Data[i" + n + "]";
    //# Func<int, bool, string> tdt = (n, v) =>
    //#     v ? "T" + Meta.VecArgs[n-1] + "d, T" + Meta.VecArgs[n-1] : "T" + Meta.VecArgs[n-1];
    //# var itza = new[] { "0L", "V2l.Zero", "V3l.Zero", "V4l.Zero" };
    //# var itoa = new[] { "1L", "V2l.One", "V3l.One", "V4l.One" };
    //# var deltacomments = new[] { "Element stride", "Element and line stride", "Stride in each dimension", "Stride in each dimension" };
    //# var zerocoords = new[] { "coordinate 0", "coordinates [0, 0]", "coordinates [0, 0, 0]", "coordinates [0, 0, 0, 0]" };
    //# var vectn = Meta.GenericTensorTypes[0].Name;
    //# var mattn = Meta.GenericTensorTypes[2].Name;
    //# var voltn = Meta.GenericTensorTypes[4].Name;
    //# var tn4tn = Meta.GenericTensorTypes[6].Name;
    //# Meta.GenericTensorTypes.ForEach((tt, tti) => {
    //#     var ttn = tt.Name;
    //#     var ttsub1n = tti > 1 ? Meta.GenericTensorTypes[tti-2].Name : "";
    //#     var ttsub2n = tti > 3 ? Meta.GenericTensorTypes[tti-4].Name : tti > 1 ? "Item" : "";
    //#     var ttnl = ttn.ToLower();
    //#     var ttsub1nl = ttsub1n.ToLower();
    //#     var ttsub2nl = ttsub2n.ToLower();
    //#     var dt = tt.DataType; var dtn = dt.Name; 
    //#     var vt = tt.ViewType; var vtn = vt.Name;
    //#     var dvtn = dt == vt ? dtn : dtn + ", " + vtn;
    //#     var d = tt.Dim;
    //#     var it = tt.IndexType; var itn = it.Name;
    //#     var iit = tt.IntIndexType; var iitn = iit.Name;
    //#     var itzero = itza[d-1];
    //#     var itone = itoa[d-1];
    //#     var deltacomment = deltacomments[d-1];
    //#     var ifa = d > 1 ? ((Meta.VecType)it).Fields : new[] { "X" };
    //#     var dotx = d > 1 ? ".X" : "";
    //#     var ideltas = d == 1 ? new[] { "Delta" } : ifa.Select(s => "Delta." + s).ToArray();
    //#     var iaa = ifa.ToLower();
    //#     var ifa1 = ifa.Take(ifa.Count() - 1);
    //#     var iaa1 = ifa1.ToLower();
    //#     var rifa = ifa.Reverse().ToArray();
    //#     var riaa = rifa.ToLower().ToArray();
    //#     var r1ifa = rifa.Take(rifa.Count() - 1);
    //#     var r1iaa = riaa.Take(riaa.Count() - 1);
    //#     var fz = ifa[d-1];
    //#     var az = iaa[d-1];
    //#     var rifa1 = rifa.Skip(1);
    //#     var riaa1 = riaa.Skip(1);
    //#     var t0i = dt == vt ? "Data[i]" : "Getter(Data, i)";
    //# Ignore(() => {
    public class MakeIntellisense {
        public void Happy() {
    //# });
    //#     Action<string, bool, Action, bool, bool> Loop = (t, info, body, crd, vec) =>
    //#     { var ifd = info ? "" : "Info.";
            long i = __t__FirstIndex;/*# if (vec) { */ __itn__ v;/*# } */
            if (__t____ifd__DX == 1)
            {
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f__;
                //# });
                long xs = __t____ifd__SX;
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                /*# if (vec) { */v.__f__ = __t__F__f__; /*# } */for (long __a__e = i + __a__s/*# if (crd) { */, __a__ = __t__F__f__/*# } */; i != __a__e; i += __a__j/*# if (crd) { */, __a__++/*# } if (vec) { */, v.__f__++/*# } */) {
                //# });
                /*# if (vec) { */v.X = __t__FX; /*# } */for (long xe = i + xs/*# if (crd) { */, x = __t__FX/*# } */; i != xe; i++/*# if (crd) { */, x++/*# } if (vec) { */, v.X++/*# } */) {
                    //# body();
                }/*# r1ifa.ForEach(f => { */ }/*# }); */
            }
            //# if (d > 1) {
            else if (__t____ifd__D__fz__ == 1)
            {
                //# rifa1.ForEach(riaa1, (f, a, fi) => { var f1 = rifa[(fi+2)%d];
                long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f____f1__;
                //# });
                long __az__s = __t____ifd__S__fz__;
                //# rifa1.ForEach(riaa1, (f, a) => {
                /*# if (vec) { */v.__f__ = __t__F__f__; /*# } */for (long __a__e = i + __a__s/*# if (crd) { */, __a__ = __t__F__f__/*# } */; i != __a__e; i += __a__j/*# if (crd) { */, __a__++/*# } if (vec) { */, v.__f__++/*# } */) {
                //# });
                /*# if (vec) { */v.__fz__ = __t__F__fz__; /*# } */for (long __az__e = i + __az__s/*# if (crd) { */, __az__ = __t__F__fz__/*# } */; i != __az__e; i++/*# if (crd) { */, __az__++/*# } if (vec) { */, v.__fz__++/*# } */) {
                    //# body();
                }/*# r1ifa.ForEach(f => { */ }/*# }); */
            }
            //# }
            else
            {
                //# rifa.ForEach(riaa, (f, a) => {
                long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f__;
                //# });
                //# rifa.ForEach(riaa, (f, a) => {
                /*# if (vec) { */v.__f__ = __t__F__f__; /*# } */for (long __a__e = i + __a__s/*# if (crd) { */, __a__ = __t__F__f__/*# } */; i != __a__e; i += __a__j/*# if (crd) { */, __a__++/*# } if (vec) { */, v.__f__++/*# } */) {
                //# });
                    //# body();
                /*# rifa.ForEach(f => { */}/*# }, space); */
            }
    //#     };
    //#     // tc: type count, pc: parallel loop count (< d)
    //#     Action<string, bool, bool, int, int, Action<bool>> LoopN = (t, info, opt, tc, pc, body) =>
    //#     { var ifd = info ? "" : "Info."; var dif = info ? "" : ".Info";
            __t____ifd__CheckMatchingSize(/*# tc.ForEach(1, i => { */t__i____dif__/*# }, comma); */);
            //# if (opt) {
            if (__t____ifd__HasMatchingLayout(/*# tc.ForEach(1, i => { */t__i____dif__/*# }, comma); */))
            {
                long i = FirstIndex;
                if (__t____ifd__DX == 1)
                {
                    //# r1ifa.ForEach(r1iaa, (f, a) => {
                    long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f__;
                    //# });
                    long xs = __t____ifd__SX;
                    //# r1ifa.ForEach(r1iaa, (f, a) => {
                    for (long __a__e = i + __a__s; i != __a__e; i += __a__j) {
                    //# });
                    for (long xe = i + xs; i != xe; i++) {
                        //# body(false);
                    }/*# r1ifa.ForEach(f => { */ }/*# }); */
                }
                //# if (d > 1) {
                else if (__t____ifd__D__fz__ == 1)
                {
                    //# rifa1.ForEach(riaa1, (f, a, fi) => { var f1 = rifa[(fi+2)%d];
                    long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f____f1__;
                    //# });
                    long __az__s = __t____ifd__S__fz__;
                    //# rifa1.ForEach(riaa1, (f, a) => {
                    for (long __a__e = i + __a__s; i != __a__e; i += __a__j) {
                    //# });
                    for (long __az__e = i + __az__s; i != __az__e; i++) {
                        //# body(false);
                    }/*# r1ifa.ForEach(f => { */ }/*# }); */
                }
                //# }
                else
                {
                    //# rifa.ForEach(riaa, (f, a) => {
                    long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f__;
                    //# });
                    //# rifa.ForEach(riaa, (f, a) => {
                    for (long __a__e = i + __a__s; i != __a__e; i += __a__j) {
                    //# });
                        //# body(false);
                    /*# rifa.ForEach(f => { */}/*# }, space); */
                }
            }
            else
            //# }
            {
                long i = __t__FirstIndex/*# tc.ForEach(1, i => { */, i__i__ = t__i__.FirstIndex/*# }); */;
                if (__t____ifd__DX == 1 && /*# tc.ForEach(1, i => { */t__i__.__ifd__DX == 1/*# }, andand); */)
                {
                    //# r1ifa.ForEach(r1iaa, (f, a) => {
                    long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f__/*# tc.ForEach(1, i => { */, __a__j__i__ = t__i__.__ifd__J__f__/*# }); */;
                    //# });
                    long xs = __t____ifd__SX;
                    //# r1ifa.ForEach(r1iaa, (f, a) => {
                    for (long __a__e = i + __a__s; i != __a__e; i += __a__j/*# tc.ForEach(1, i => { */, i__i__ += __a__j__i__/*# }); */) {
                    //# });
                    for (long xe = i + xs; i != xe; i++/*# tc.ForEach(1, i => { */, i__i__++/*# }); */) {
                        //# body(true);
                    }/*# r1ifa.ForEach(f => { */ }/*# }); */
                }
                //# if (d > 1) {
                else if (__t____ifd__D__fz__ == 1 && /*# tc.ForEach(1, i => { */t__i__.__ifd__D__fz__ == 1/*# }, andand); */)
                {
                    //# rifa1.ForEach(riaa1, (f, a, fi) => { var f1 = rifa[(fi+2)%d];
                    long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f____f1__/*# tc.ForEach(1, i => { */, __a__j__i__ = t__i__.__ifd__J__f____f1__/*# }); */;
                    //# });
                    long __az__s = __t____ifd__S__fz__;
                    //# rifa1.ForEach(riaa1, (f, a) => {
                    for (long __a__e = i + __a__s; i != __a__e; i += __a__j/*# tc.ForEach(1, i => { */, i__i__ += __a__j__i__/*# }); */) {
                    //# });
                    for (long __az__e = i + __az__s; i != __az__e; i++/*# tc.ForEach(1, i => { */, i__i__++/*# }); */) {
                        //# body(true);
                    }/*# r1ifa.ForEach(f => { */ }/*# }); */
                }
                //# }
                else
                {
                    //# rifa.ForEach(riaa, (f, a) => {
                    long __a__s = __t____ifd__DS__f__, __a__j = __t____ifd__J__f__/*# tc.ForEach(1, i => { */, __a__j__i__ = t__i__.__ifd__J__f__/*# }); */;
                    //# });
                    //# rifa.ForEach(riaa, (f, a) => {
                    for (long __a__e = i + __a__s; i != __a__e; i += __a__j/*# tc.ForEach(1, i => { */, i__i__ += __a__j__i__/*# }); */) {
                    //# });
                        //# body(true);
                    /*# rifa.ForEach(f => { */}/*# }, space); */
                }
            }
    //#     };
    //# Ignore(() => {
        } // Happy
    } // MakeIntellisense
    //# });
    //# if (dt == vt) {
    #region __ttn__Info

    /// <summary>
    /// A __ttnl__ info contains the complete indexing information for a __ttnl__, but no data array.
    /// </summary>
    [Serializable]
    public struct __ttn__Info : ITensorInfo
    {
        /// <summary>
        /// Index of element with __zerocoords[d-1]__ in the underlying data array.
        /// </summary>
        public long Origin;

        /// <summary>
        /// Size of the __ttnl__.
        /// </summary>
        public __itn__ Size;

        /// <summary>
        /// __deltacomment__.
        /// </summary>
        public __itn__ Delta;

        /// <summary>
        /// Coordinates of the first element.
        /// May differ from __zerocoords[d-1]__ for subwindows that retain the original coordinate space.
        /// </summary>
        public __itn__ First;

        #region Constructors

        /// <summary>
        /// Construct a __ttn__Info given a complete specification.
        /// </summary>
        /// <param name="origin">Location of [0,0] element within data array.</param>
        /// <param name="size">Size of the __ttnl__.</param>
        /// <param name="delta">__deltacomment__.</param>
        /// <param name="first">Coordinates of the first element.</param>
        public __ttn__Info(long origin, __itn__ size, __itn__ delta, __itn__ first)
        {
            Origin = origin;
            Size = size;
            Delta = delta;
            First = first;
        }

        /// <summary>
        /// Construct a __ttn__Info given a complete specification.
        /// </summary>
        /// <param name="origin">Location of [0,0] element within data array.</param>
        /// <param name="size">Size of the __ttnl__.</param>
        /// <param name="delta">__deltacomment__.</param>
        public __ttn__Info(long origin, __itn__ size, __itn__ delta)
            : this(origin, size, delta, __itzero__)
        { }

        //# if (d == 1) {
        /// <summary>
        /// Construct __ttnl__ info of specified size.
        /// </summary>
        public __ttn__Info(__itn__ size)
            : this(0L, size, 1L)
        { }

        public __ttn__Info(__ttn__Info info)
            : this(0L, info.Size, 1L)
        {
            F = info.F;
        }

        //# } else {
        private static __itn__ NewDelta(__itn__ size)
        {
            return new __itn__(1L/*# if (d > 1) { */, size.X/*# }
                                     if (d > 2) { */, size.X * size.Y/*# }
                                     if (d > 3) { */, size.X * size.Y * size.Z/*# } */);
        }

        private static __itn__ NewDelta(__itn__ size, __itn__ delta)
        {
            //# if (d == 2) {
            if (delta.Y == 1L) return new __itn__(size.Y, 1L);
            //# } else if (d == 3) {
            if (delta.Z == 1L) return new __itn__(size.Z, size.Z * size.X, 1L);
            if (delta.Y == 1L) return new __itn__(size.Y * size.Z, 1L, size.Y);
            //# } else if (d == 4) {
            if (delta.W == 1L) return new __itn__(size.W, size.W * size.X, size.W * size.X * size.Y, 1L);
            if (delta.Z == 1L) return new __itn__(size.Z * size.W, size.Z * size.W * size.X, 1L, size.Z);
            if (delta.Y == 1L) return new __itn__(size.Y * size.Z * size.W, 1L, size.Y, size.Y * size.Z);
            //# }
            return NewDelta(size);
        }

        /// <summary>
        /// Construct __ttnl__ info of specified size.
        /// </summary>
        public __ttn__Info(__itn__ size)
            : this(0L, size, NewDelta(size))
        { }

        /// <summary>
        /// Construct __ttnl__ info of specified size and delta.
        /// </summary>
        public __ttn__Info(__itn__ size, __itn__ delta)
            : this(0L, size, NewDelta(size, delta))
        { }

        public __ttn__Info(__ttn__Info info)
            : this(0L, info.Size, NewDelta(info.Size, info.Delta))
        {
            F = info.F;
        }

        //# }
        /// <summary>
        /// Construct __ttnl__ info of specified size.
        /// </summary>
        public __ttn__Info(__iitn__ size)
            : this((__itn__)size)
        { }

        //# if (d > 1) {
        /// <summary>
        /// Construct __ttnl__ info of specified size and delta.
        /// </summary>
        public __ttn__Info(__iitn__ size, __iitn__ delta)
            : this((__itn__)size, (__itn__)delta)
        { }

        /// <summary>
        /// Construct __ttnl__ info of specified size.
        /// </summary>
        public __ttn__Info(/*# ifa.ForEach(f => { */long size__f__/*# }, comma); */)
            : this(new __itn__(/*# ifa.ForEach(f => { */size__f__/*# }, comma); */))
        { }

        /// <summary>
        /// Construct __ttnl__ info of specified size.
        /// </summary>
        public __ttn__Info(/*# ifa.ForEach(f => { */int size__f__/*# }, comma); */)
            : this(new __itn__(/*# ifa.ForEach(f => { */size__f__/*# }, comma); */))
        { }

        //# } // (d > 1)
        #endregion

        #region Properties

        /// <summary>
        /// Return the rank or dimension of the the __ttnl__ (__d__).
        /// </summary>
        public int Rank { get { return __d__; } }

        /// <summary>
        /// Total number of element in the __ttnl__.
        /// </summary>
        //# if (d == 1) {
        public long Count { get { return Size; } }
        //# } else {
        public long Count { get { return /*# ifa.ForEach((f) => {*/ Size.__f__/*# }, mul);*/; } }
        //# }

        /// <summary>
        /// End (one step beyond the last element in all dimensions).
        /// </summary>
        public __itn__ End { get { return First + Size; } }

        //# for (int di = 0; di < (1 << d); di++) {
        //#     var pnt = d.Range().Select(i => (di & (1 << i)) == 0 ? "O" : "I").Join();
        //#     var arg = d.Range().Select(i => "First." + ifa[i] + ((di & (1 << i)) == 0 ? "" : " + Size." + ifa[i])).Join(", ");
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ __pnt__ { get { return /*# if (di == 0) {*/First;/*#}
                                                    else if (di == (1 << d) - 1) {*/First + Size;/*#}
                                                    else {*/new __itn__(__arg__);/*#}*/ } }
        //# }

        /// <summary>
        /// Size
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ S { get { return Size; } set { Size = value; } }

        /// <summary>
        /// Delta
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ D { get { return Delta; } set { Delta = value; } }

        /// <summary>
        /// First
        /// </summary>
        //# if (d == 1) {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ F { get { return First; } set { Origin -= Delta * (value - First); First = value; } }
        //# } else {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ F { get { return First; } set { Origin -= Delta.Dot(value - First); First = value; } }
        //# }

        /// <summary>
        /// End (one step beyond the last element in all dimensions).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ E { get { return First + Size; } }

        //# if (d == 1) {
        /// <summary>
        /// Cummulative delta for all elements up to this dimension.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long DS { get { return Delta * Size; } }

        /// <summary>
        /// Size
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long SX { get { return Size; } set { Size = value; } }

        /// <summary>
        /// Delta
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long DX { get { return Delta; } set { Delta = value; } }

        /// <summary>
        /// First
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long FX { get { return First; } set { Origin -= Delta * (value - First); First = value; } }

        /// <summary>
        /// End (one step beyond the last element).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long EX { get { return First + Size; } }

        /// <summary>
        /// Cummulative delta for all elements up to this dimension.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long DSX { get { return Delta * Size; } }

        /// <summary>
        /// Jump
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long JX { get { return Delta; } }


        //# } // (d == 1)
        //# if (d > 1) {
        //# foreach (var f in ifa) {
        /// <summary>
        /// Size.__f__
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long S__f__ { get { return Size.__f__; } set { Size.__f__ = value; } }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// Delta.__f__
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long D__f__ { get { return Delta.__f__; } set { Delta.__f__ = value; } }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// First.__f__
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long F__f__
        {
            get { return First.__f__; }
            set { Origin -= Delta.__f__ * (value - First.__f__); First.__f__ = value; }
        }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// End in dimension __f__ (one step beyond the last element). 
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long E__f__
        {
            get { return First.__f__ + Size.__f__; }
        }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// Cummulative delta for all elements up to this dimension.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long DS__f__ { get { return Size.__f__ * Delta.__f__; } }

        //# } // foreach
        //# ifa.ForEach((f, i) => { var f1 = i > 0 ? Meta.VecFields[i-1] : "";
        /// <summary>
        /// Jump this many elements in the underlying data array when stepping
        /// between elements in dimension __f__.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long J__f__ { get { return Delta.__f__/*# if (i > 0) { */ - Size.__f1__ * Delta.__f1__/*# } */; } }

        public long GetJ__f__(__itn__ size)
        {
            return Delta.__f__/*# if (i > 0) { */ - size.__f1__ * Delta.__f1__/*# } */;
        }

        //# }); // ForEach
        //# ifa.ForEach(f2 => {
        //# ifa.ForEach(f1 => { var f0 = f1 == f2 ? "0" : f1;
        /// <summary>
        /// Jump this many elements in the underlying data array when stepping
        /// between elements in dimension __f2__ in a loop around another loop
        /// in dimension __f0__.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long J__f2____f0__ { get { return Delta.__f2__/*# if (f1 != f2) { */ - Size.__f1__ * Delta.__f1__/*# } */; } }

        public long GetJ__f2____f0__(__itn__ size)
        {
            return Delta.__f2__/*# if (f1 != f2) { */ - size.__f1__ * Delta.__f1__/*# } */;
        }

        //# }); // ifa
        //# }); // ifa
        //# } // (d > 1)
        /// <summary>
        /// Return the index of the first element in the underlying data array.
        /// </summary>
        public long FirstIndex { get { return Index(First); } }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long OriginIndex { get { return Origin; } set { Origin = value; } }

        /// <summary>
        /// Get or set the size of the tensor in each dimension as an
        /// array of longs.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long[] SizeArray
        {
            get { return new long[] { /*# ifa.ForEach((f) => { */S__f__/*# }, comma); */ }; }
            set { /*# ifa.ForEach((f, i) => { */S__f__ = value[__i__]; /*# }); */ }
        }

        /// <summary>
        /// Get or set the deltas of the tensor in each dimension as an
        /// array of longs.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long[] DeltaArray
        {
            get { return new long[] { /*# ifa.ForEach((f) => { */D__f__/*# }, comma); */ }; }
            set { /*# ifa.ForEach((f, i) => { */D__f__ = value[__i__]; /*# }); */ }
        }

        /// <summary>
        /// Get or set the first coords of the tensor in each dimension as an
        /// array of longs.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long[] FirstArray
        {
            get { return new long[] { /*# ifa.ForEach((f) => { */F__f__/*# }, comma); */ }; }
            set { /*# ifa.ForEach((f, i) => { */F__f__ = value[__i__]; /*# }); */ }
        }

        //# if (d > 1) {
        //# ifa.ForEach(iaa, (f,a) => {
        /// <summary>
        /// Return the indices into the underlying data array in the specified
        /// order.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IEnumerable<long> Indices__f__
        {
            get
            {
                long i = FirstIndex;
                for (long __a__e = i + DS__f__, __a__j = J__f__0; i != __a__e; i += __a__j)
                    yield return i;
            }
        }

        //# });
        //# ifa.ForEach(iaa, (f2,a2) => { var f0 = "0";
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2) {
        /// <summary>
        /// Return the indices into the underlying data array in the specified
        /// order, with the first coordinate being the outer loop, and the second
        /// coordinate being the inner loop.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IEnumerable<long> Indices__f2____f1__
        {
            get
            {
                long i = FirstIndex;
                for (long __a2__e = i + DS__f2__, __a2__j = J__f2____f1__; i != __a2__e; i += __a2__j)
                for (long __a1__e = i + DS__f1__, __a1__j = J__f1____f0__; i != __a1__e; i += __a1__j)
                    yield return i;
            }
        }

        //# }});
        //# });
        //# } // (d > 1)
        //# if (d > 2) {
        //# ifa.ForEach(iaa, (f3,a3) => { var f0 = "0";
        //# ifa.ForEach(iaa, (f2,a2) => { if (f2 != f3) {
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2 && f1 != f3) {
        /// <summary>
        /// Return the indices into the underlying data array in the specified
        /// order, with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IEnumerable<long> Indices__f3____f2____f1__
        {
            get
            {
                long i = FirstIndex;
                for (long __a3__e = i + DS__f3__, __a3__j = J__f3____f2__; i != __a3__e; i += __a3__j)
                for (long __a2__e = i + DS__f2__, __a2__j = J__f2____f1__; i != __a2__e; i += __a2__j)
                for (long __a1__e = i + DS__f1__, __a1__j = J__f1____f0__; i != __a1__e; i += __a1__j)
                    yield return i;
            }
        }

        //# }});
        //# }});
        //# });
        //# } // (d > 2)
        //# if (d > 3) {
        //# ifa.ForEach(iaa, (f4,a4) => { var f0 = "0";
        //# ifa.ForEach(iaa, (f3,a3) => { if (f3 != f4) {
        //# ifa.ForEach(iaa, (f2,a2) => { if (f2 != f3 && f2 != f4) {
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2 && f1 != f3 && f1 != f4) {
        /// <summary>
        /// Return the indices into the underlying data array in the specified
        /// order, with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IEnumerable<long> Indices__f4____f3____f2____f1__
        {
            get
            {
                long i = FirstIndex;
                for (long __a4__e = i + DS__f4__, __a4__j = J__f4____f2__; i != __a4__e; i += __a4__j)
                for (long __a3__e = i + DS__f3__, __a3__j = J__f3____f2__; i != __a3__e; i += __a3__j)
                for (long __a2__e = i + DS__f2__, __a2__j = J__f2____f1__; i != __a2__e; i += __a2__j)
                for (long __a1__e = i + DS__f1__, __a1__j = J__f1____f0__; i != __a1__e; i += __a1__j)
                    yield return i;
            }
        }

        //# }});
        //# }});
        //# }});
        //# });
        //# } // (d > 3)
        //# if (d == 2) {
        public __ttn__Info Transposed
        {
            get { return new __ttn__Info(Origin, Size.YX, Delta.YX, First.YX); }
        }

        //# } // d == 2
        #endregion

        #region Indexing Helper Methods

        //# if (d == 1) {
        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        public long Index(long coord) { return Origin + coord * Delta; }

        //# }
        //# if (d > 1) {
        public long Dv(/*# iaa.ForEach(a => { */long d__a__/*# }, comma); */)
        {
            return /*# iaa.ForEach(ifa, (a, f) => {*/d__a__ * Delta.__f__/*# }, add);*/;
        }

        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        public long Index(/*# iaa.ForEach(a => { */long __a__/*# }, comma); */) 
        {
            return Origin + /*# iaa.ForEach(ifa, (a, f) => {*/__a__ * Delta.__f__/*# }, add);*/;
        }

        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        public long Index(__itn__ v)
        {
            return Origin + /*# ifa.ForEach(f => {*/v.__f__ * Delta.__f__/*# }, add);*/;
        }

        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        public long Index(__iitn__ v)
        {
            return Origin + /*# ifa.ForEach(f => {*/v.__f__ * Delta.__f__/*# }, add);*/;
        }

        //# }
        #endregion

        #region Interpretation and Parts
       
        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(__itn__ begin, __itn__ size)
        {
            return new __ttn__Info(Origin, size, Delta, begin);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__Info Sub__ttn__(__itn__ begin, __itn__ size)
        {
            return new __ttn__Info(Index(begin), size, Delta);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(__iitn__ begin, __iitn__ size)
        {
            return new __ttn__Info(Origin, (__itn__)size, Delta, (__itn__)begin);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__Info Sub__ttn__(__iitn__ begin, __iitn__ size)
        {
            return new __ttn__Info(Index((__itn__)begin), (__itn__)size, Delta);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(__itn__ begin, __itn__ size, __itn__ delta)
        {
            return new __ttn__Info(Origin, size, delta, begin);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__Info Sub__ttn__(__itn__ begin, __itn__ size, __itn__ delta)
        {
            return new __ttn__Info(Index(begin), size, delta);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(__iitn__ begin, __iitn__ size, __iitn__ delta)
        {
            return new __ttn__Info(Origin, (__itn__)size, (__itn__)delta, (__itn__)begin);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__Info Sub__ttn__(__iitn__ begin, __iitn__ size, __iitn__ delta)
        {
            return new __ttn__Info(Index((__itn__)begin), (__itn__)size, (__itn__)delta);
        }

        //# if (d > 1) {
        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(__itn__ begin, __itn__ size, __itn__ delta, __itn__ first)
        {
            return new __ttn__Info(Index(begin), size, delta) { F = first };
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(__iitn__ begin, __iitn__ size, __iitn__ delta, __iitn__ first)
        {
            return new __ttn__Info(Index((__itn__)begin), (__itn__)size, (__itn__)delta)
                            { F = (__itn__)first };
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */)
        {
            return new __ttn__Info(Origin, new __itn__(/*# ifa.ForEach(f => { */size__f__/*# }, comma); */),
                                  Delta, new __itn__(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */));
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__Info Sub__ttn__(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */)
        {
            return new __ttn__Info(Index(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */),
                                  new __itn__(/*# ifa.ForEach(f => { */size__f__/*# }, comma); */),
                                  Delta);
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__Info Sub__ttn__Window(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long delta__f__/*# }, comma); */)
        {
            return new __ttn__Info(Origin, new __itn__(/*# ifa.ForEach(f => { */size__f__/*# }, comma); */),
                                  new __itn__(/*# ifa.ForEach(f => { */delta__f__/*# }, comma); */),
                                  new __itn__(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */));
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__Info Sub__ttn__(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long delta__f__/*# }, comma); */)
        {
            return new __ttn__Info(Index(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */),
                                  new __itn__(/*# ifa.ForEach(f => { */size__f__/*# }, comma); */),
                                  new __itn__(/*# ifa.ForEach(f => { */delta__f__/*# }, comma); */));
        }

        /// <summary>
        /// A SubVector does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __vectn__ with zero as first coordinates.
        /// </summary>
        public __vectn__Info Sub__vectn__(__itn__ begin, long size, long delta)
        {
            return new __vectn__Info(Index(begin), size, delta);
        }

        /// <summary>
        /// A SubVector does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __vectn__ with zero as first coordinates.
        /// </summary>
        public __vectn__Info Sub__vectn__(__iitn__ begin, long size, long delta)
        {
            return new __vectn__Info(Index(begin), size, delta);
        }

        //# if (d == 2) {
        /// <summary>
        /// If the lines of the matrix are stored consecutively without gaps,
        /// the whole matrix can be viewed as a single vector.
        /// This methods returns a __vectn__ with zero as first coordinates.
        /// </summary>
        public __vectn__Info As__vectn__()
        {
            if (JY != 0)
                throw new Exception("cannot represent this matrix as vector");
            return new __vectn__Info(Origin, SX * SY, DX);
        }

        //# for (int ai = 0; ai < d; ai++) {
        //#     var a = iaa[ai];
        //#     var fa = ""; ifa.ForEach((f,i) => { if (i != ai) fa = fa + f; });
        //#     var fn = fa == "X" ? "Row" : "Col";
        //#     var da = ""; ifa.ForEach((f,i) => { if (i != ai) da = da + f; else da = da + "O"; });
        //#     foreach (var win in new bool[] { false, true }) { var winstr = win ? "Window" : "";
        /// <summary>
        /// Return a full __fa__ __ttsub1nl__ slice of the __ttnl__.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttsub1nl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttsub1n__Info __fn____winstr__(long __a__)
        {
            return new __ttsub1n__Info(Index(/*# ifa.ForEach((f,i) => { var ff = i != ai ? "F" + f : a; */__ff__/*# }, comma);
                            */), S.__fa__, D.__fa__)/*# if (win) { */
            { F = F.__fa__ }/*# } */;
        }

        //# } // win
        //# } // ai
        //# } // (d == 2)
        //# if (d == 3) {
        /// <summary>
        /// If the lines and planes of the volume are stored consecutively
        /// without gaps, the whole volume can be viewed as a single vector.
        /// This methods returns a __vectn__ with zero as first coordinates.
        /// </summary>
        public __vectn__Info As__vectn__()
        {
            if (JX != 0 || JY != 0)
                throw new Exception("cannot represent this volume as vector");
            return new __vectn__Info(Origin, SX * SY * SZ, DX);
        }

        /// <summary>
        /// If the lines of the volume are stored consecutively without gaps,
        /// they can be merged, and the volume can be viewed as a matrix.
        /// This methods returns a __mattn__ with zero as first coordinates.
        /// </summary>
        public __mattn__Info As__mattn__XYxZ()
        {
            if (JY != 0)
                throw new Exception("cannot represent this volume as matrix");
            return new __mattn__Info(Origin, new V2l(SX * SY, SZ), D.XZ);
        }

        /// <summary>
        /// If the planes of the volume are stored consecutively without gaps,
        /// they can be merged, and the volume can be viewed as a matrix.
        /// This methods returns a __mattn__ with zero as first coordinates.
        /// </summary>
        public __mattn__Info As__mattn__XxYZ()
        {
            if (JZ != 0)
                throw new Exception("cannot represent this volume as matrix");
            return new __mattn__Info(Origin, new V2l(SX, SY * SZ), D.XY);
        }

        /// <summary>
        /// A SubMatrix does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __mattn__ with zero as first coordinates.
        /// </summary>
        public __mattn__Info Sub__mattn__(__itn__ origin, V2l size, V2l delta)
        {
            return new __mattn__Info(Index(origin), size, delta);
        }

        /// <summary>
        /// A SubMatrix does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __mattn__ with zero as first coordinates.
        /// </summary>
        public __mattn__Info Sub__mattn__(__iitn__ origin, V2i size, V2l delta)
        {
            return new __mattn__Info(Index(origin), (V2l)size, delta);
        }

        /// <summary>
        /// A SubMatrix does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __mattn__ with zero as first coordinates.
        /// </summary>
        public __mattn__Info Sub__mattn__(
                long bx, long by, long bz,
                long sx, long sy, long dx, long dy)
        {
            return new __mattn__Info(Index(bx, by, bz), new V2l(sx, sy), new V2l(dx, dy));
        }

        //# } // (d == 3)
        //# if (d == 4) {
        /// <summary>
        /// If the lines and planes of the volume are stored consecutively
        /// without gaps, the whole volume can be viewed as a single vector.
        /// This methods returns a __vectn__ with zero as first coordinates.
        /// </summary>
        public __vectn__Info As__vectn__()
        {
            if (JX != 0 || JY != 0 || JZ != 0)
                throw new Exception("cannot represent this tensor4 as vector");
            return new __vectn__Info(Origin, SX * SY * SZ * SW, DX);
        }

        //# } // (d == 4)
        //# for (int ai = 0; ai < d; ai++) {
        //#     var a = iaa[ai];
        //#     var fa = ""; ifa.ForEach((f,i) => { if (i != ai) fa = fa + f; });
        //#     var da = ""; ifa.ForEach((f,i) => { if (i != ai) da = da + f; else da = da + "O"; });
        //#     foreach (var win in new bool[] { false, true }) { var winstr = win ? "Window" : "";
        /// <summary>
        /// Return a full __fa__ __ttsub1nl__ slice of the __ttnl__.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttsub1nl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttsub1n__Info Sub__fa____ttsub1n____winstr__(long __a__)
        {
            return new __ttsub1n__Info(Index(/*# ifa.ForEach((f,i) => { var ff = i != ai ? "F" + f : a; */__ff__/*# }, comma);
                            */), S.__fa__, D.__fa__)/*# if (win) { */ { F = F.__fa__ }/*# } */;
        }

        /// <summary>
        /// Return a full __fa__ __ttsub1nl__ slice of the __ttnl__ as a special __ttnl__
        /// that replicates the __ttsub1nl__ the supplied number of times. The special
        /// __ttnl__ can only be correctly used to read from. Other operations are not guaranteed.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttnl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttn__Info Sub__fa____ttsub1n__AsReadOnly__ttn____winstr__(long __a__, long size__a__)
        {
            return new __ttn__Info(Index(/*# ifa.ForEach((f,i) => { var ff = i != ai ? "F" + f : a; */__ff__/*# }, comma);
                            */), new __itn__(/*# ifa.ForEach((f,i) => { var ff = i != ai ? "S" + f : "size" + a; */__ff__/*# }, comma);
                            */), D.__da__)/*# if (win) { */ { F = F }/*# } */;
        }

        //# } // win
        //# } // ai
        //# for (int ai = 0; ai < d-1; ai++) { var a = iaa[ai];
        //# for (int bi = ai + 1; bi < d; bi++) { var b = iaa[bi];
        //#     var fa = ""; ifa.ForEach((f,i) => { if (i != ai && i != bi) fa = fa + f; });
        //#     var da = ""; ifa.ForEach((f,i) => { if (i != ai && i != bi) da = da + f; else da = da + "O"; });
        //#     var delta = d > 2 ? "D." + da : itn + ".Zero";
        //#     foreach (var win in new bool[] { false, true }) { var winstr = win ? "Window" : "";
        /// <summary>
        /// Return a full __fa__ __ttsub2nl__ slice of the __ttnl__ as a special __ttnl__
        /// that replicates the __ttsub2nl__ the supplied number of times. The special
        /// __ttnl__ can only be correctly used to read from. Other operations are not guaranteed.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttnl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttn__Info Sub__fa____ttsub2n__AsReadOnly__ttn____winstr__(long __a__, long __b__, long size__a__, long size__b__)
        {
            return new __ttn__Info(Index(/*# ifa.ForEach((f,i) => { var ff = i == ai ? a : i == bi ? b : "F" + f; */__ff__/*# }, comma);
                            */), new __itn__(/*# ifa.ForEach((f,i) => { var ff = i == ai ? "size" + a : i == bi ? "size" + b : "S" + f; */__ff__/*# }, comma);
                            */), __delta__)/*# if (win) { */ { F = F }/*# } */;
        }

        //# } // win
        //# } // bi
        //# } // ai
        //# } // d > 1
        #endregion

        #region Actions for each Element

        public void ForeachIndex(Action<long> i_action)
        {
            //# Loop("", true, () => {
                    i_action(i);
            //# }, false, false);
        }

        //# { var i_action = ""; iaa.ForEach(a => i_action += a + "_"); i_action += "i_action";
        public void ForeachIndex(Action</*# iaa.ForEach(f => {*/long/*# }, comma);*/, long> __i_action__)
        {
            //# Loop("", true, () => {
                    __i_action__(/*# iaa.ForEach(a => {*/__a__/*# }, comma);*/, i);
            //# }, true, false);
        }
        //# } // var i_action

        public void ForeachCoord(Action<__itn__> v_action)
        {
            //# if (d == 1) {
            //# Loop("", true, () => {
                    v_action(x);
            //# }, true, false);
            //# } else {
            //# Loop("", true, () => {
                    v_action(v);
            //# }, false, true);
            //# }
        }

        public void ForeachIndex(__ttn__Info t1, Action<long, long> i_i1_act)
        {
            //# LoopN("", true, true, 2, 0, ix => { var i1 = ix ? "i1" : "i";
                        i_i1_act(i, __i1__);
            //# });
        }

        //# for (int itc = 1; itc < 3; itc++) { var istr = "i_"; for (int i1 = 1; i1 < itc; i1++) istr += "i" + i1 + "_";
        //# foreach (bool idx in new[] { false, true }) { var idxstr = idx ? "Index" : ""; if (!idx && itc > 1) continue;
        //# if (d > 1) {
        //# ifa.ForEach(iaa, (f,a) => {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// and calls the supplied action with the index into the data
        /// array as parameter. Optionally, the coordinates of each element
        /// are also fed into the supplied action.
        /// </summary>
        //# var elementAct = (crd ? a + "_" : "") + (idx ? istr : "") + "elementAct";
        public void Foreach__f____idxstr__(/*# for (int i1 = 1; i1 < itc; i1++) { */__ttn__Info t__i1__, /*# } */
                Action</*# if (crd) { */long/*# if (idx) { */, /*# }} if (idx) { */long/*# for (int i1 = 1; i1 < itc; i1++) { */, long/*# }} */> __elementAct__)
        {
            long i = FirstIndex/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ = t__i1__.FirstIndex/*# } */;
            for (long __a__e = i + DS__f__, __a__j = J__f__0/*# for (int i1 = 1; i1 < itc; i1++) { */, __a__j__i1__ = t__i1__.J__f__0/*# } if (crd) { */, __a__ = F__f__/*# } */; i != __a__e; i += __a__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a__j__i1__/*# }
                 if (crd) { */, __a__++/*# } */)
                __elementAct__(/*# if (crd) { */__a__/*# if (idx) { */, /*# }} if (idx) { */i/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__/*# } } */);
        }

        //# } // crd
        //# });
        //# ifa.ForEach(iaa, (f2,a2) => { var f0 = "0";
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2) {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        //# foreach (bool pre in new[] { false, true }) {
        //# foreach (bool post in new[] { false, true }) {
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop, and calls the supplied action
        /// with the index into the data array as parameter. Optionally, the
        /// coordinates of each element are also fed into the supplied action
        /// (in the order specified in the name of the function), and also
        /// optionally pre-line and post-line actions can be specified.
        /// </summary>
        //# var preLineAct = (crd ? a2 + "_" : "") + "preLineAct";
        //# var elementAct = (crd ? a2 + "_" + a1 + "_" : "") + (idx ? istr : "") + "elementAct";
        //# var postLineAct = (crd ? a2 + "_" : "") + "postLineAct";
        public void Foreach__f2____f1____idxstr__(/*# for (int i1 = 1; i1 < itc; i1++) { */__ttn__Info t__i1__, /*# } */
                /*# if (pre) { */Action/*# if (crd) { */<long>/*# } */ __preLineAct__,
                /*# } */Action</*# if (crd) { */long, long/*# if (idx) { */, /*# }} if (idx) { */long/*# for (int i1 = 1; i1 < itc; i1++) { */, long/*# }} */> __elementAct__/*# if (post) { */,
                Action/*# if (crd) { */<long>/*# } */ __postLineAct__/*# } */)
        {
            long __a2__s = DS__f2__, __a2__j = J__f2____f1__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a2__j__i1__ = t__i1__.J__f2____f1__/*# } */;
            long __a1__s = DS__f1__, __a1__j = J__f1____f0__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a1__j__i1__ = t__i1__.J__f1____f0__/*# } */;
            long i = FirstIndex/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ = t__i1__.FirstIndex/*# } */;
            for (long __a2__e = i + __a2__s/*# if (crd) { */, __a2__ = F__f2__/*# } */; i != __a2__e; i += __a2__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a2__j__i1__/*# }
                 if (crd) { */, __a2__++/*# } */)
            {
            //# if (pre) {
            __preLineAct__(/*# if (crd) { */__a2__/*# } */);
            //# }
            for (long __a1__e = i + __a1__s/*# if (crd) { */, __a1__ = F__f1__/*# } */; i != __a1__e; i += __a1__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a1__j__i1__/*# }
                 if (crd) { */, __a1__++/*# } */)
                __elementAct__(/*# if (crd) { */__a2__, __a1__/*# if (idx) { */, /*# }} if (idx) { */i/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__/*# } } */);
            //# if (post) {
            __postLineAct__(/*# if (crd) { */__a2__/*# } */);
            //# }
            }
        }

        //# } // post
        //# } // pre
        //# } // crd
        //# }});
        //# });
        //# } // (d > 1)
        //# if (d > 2) {
        //# ifa.ForEach(iaa, (f3,a3) => { var f0 = "0";
        //# ifa.ForEach(iaa, (f2,a2) => { if (f2 != f3) {
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2 && f1 != f3) {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        //# foreach (bool pre in new[] { false, true }) {
        //# foreach (bool post in new[] { false, true }) {
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop, and calls the supplied action
        /// with the index into the data array as parameter. Optionally, the
        /// coordinates of each element are also fed into the supplied action
        /// (in the order specified in the name of the function), and also
        /// optionally pre-line, post-line, pre-plane, and post-plane
        /// actions can be specified.
        /// </summary>
        //# var prePlaneAct = (crd ? a3 + "_" : "") + "prePlaneAct";
        //# var preLineAct = (crd ? a3 + "_" + a2 + "_" : "") + "preLineAct";
        //# var elementAct = (crd ? a3 + "_" + a2 + "_" + a1 + "_" : "") + (idx ? istr : "") + "elementAct";
        //# var postLineAct = (crd ? a3 + "_" + a2 + "_" : "") + "postLineAct";
        //# var postPlaneAct = (crd ? a3 + "_" : "") + "postPlaneAct";
        public void Foreach__f3____f2____f1____idxstr__(/*# for (int i1 = 1; i1 < itc; i1++) { */__ttn__Info t__i1__, /*# } if (pre) { */
                Action/*# if (crd) { */<long>/*# } */ __prePlaneAct__,
                Action/*# if (crd) { */<long, long>/*# } */ __preLineAct__,/*# } */
                Action</*# if (crd) { */long, long, long/*# if (idx) { */, /*# }} if (idx) { */long/*# for (int i1 = 1; i1 < itc; i1++) { */, long/*# }} */> __elementAct__/*# if (post) { */,
                Action/*# if (crd) { */<long, long>/*# } */ __postLineAct__,
                Action/*# if (crd) { */<long>/*# } */ __postPlaneAct__/*# } */)
        {
            long __a3__s = DS__f3__, __a3__j = J__f3____f2__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a3__j__i1__ = t__i1__.J__f3____f2__/*# } */;
            long __a2__s = DS__f2__, __a2__j = J__f2____f1__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a2__j__i1__ = t__i1__.J__f2____f1__/*# } */;
            long __a1__s = DS__f1__, __a1__j = J__f1____f0__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a1__j__i1__ = t__i1__.J__f1____f0__/*# } */;
            long i = FirstIndex/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ = t__i1__.FirstIndex/*# } */;
            for (long __a3__e = i + __a3__s/*# if (crd) { */, __a3__ = F__f3__/*# } */; i != __a3__e; i += __a3__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a3__j__i1__/*# }
                 if (crd) { */, __a3__++/*# } */)
            {
            //# if (pre) {
            __prePlaneAct__(/*# if (crd) { */__a3__/*# } */);
            //# }
            for (long __a2__e = i + __a2__s/*# if (crd) { */, __a2__ = F__f2__/*# } */; i != __a2__e; i += __a2__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a2__j__i1__/*# }
                 if (crd) { */, __a2__++/*# } */)
            {
            //# if (pre) {
            __preLineAct__(/*# if (crd) { */__a3__, __a2__/*# } */);
            //# }
            for (long __a1__e = i + __a1__s/*# if (crd) { */, __a1__ = F__f1__/*# } */; i != __a1__e; i += __a1__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a1__j__i1__/*# }
                 if (crd) { */, __a1__++/*# } */)
                __elementAct__(/*# if (crd) { */__a3__, __a2__, __a1__/*# if (idx) { */, /*# }} if (idx) { */i/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__/*# } } */);
            //# if (post) {
            __postLineAct__(/*# if (crd) { */__a3__, __a2__/*# } */);
            //# }
            }
            //# if (post) {
            __postPlaneAct__(/*# if (crd) { */__a3__/*# } */);
            //# }
            }
        }

        //# } // post
        //# } // pre
        //# } // crd
        //# }});
        //# }});
        //# });
        //# } // (d > 2)
        //# if (d > 3) {
        //# ifa.ForEach(iaa, (f4,a4) => { var f0 = "0";
        //# ifa.ForEach(iaa, (f3,a3) => { if (f3 != f4) {
        //# ifa.ForEach(iaa, (f2,a2) => { if (f2 != f3 && f2 != f4) {
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2 && f1 != f3 && f1 != f4) {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        //# foreach (bool pre in new[] { false, true }) {
        //# foreach (bool post in new[] { false, true }) {
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop, and calls the supplied action
        /// with the index into the data array as parameter. Optionally, the
        /// coordinates of each element are also fed into the supplied action
        /// (in the order specified in the name of the function), and also
        /// optionally pre-line, post-line, pre-plane, and post-plane
        /// actions can be specified.
        /// </summary>
        //# var preVolumeAct = (crd ? a4 + "_" : "") + "preVolumeAct";
        //# var prePlaneAct = (crd ? a4 + "_" + a3 + "_" : "") + "prePlaneAct";
        //# var preLineAct = (crd ? a4 + "_" + a3 + "_" + a2 + "_" : "") + "preLineAct";
        //# var elementAct = (crd ? a4 + "_" + a3 + "_" + a2 + "_" + a1 + "_" : "") + (idx ? istr : "") + "elementAct";
        //# var postLineAct = (crd ? a4 + "_" + a3 + "_" + a2 + "_" : "") + "postLineAct";
        //# var postPlaneAct = (crd ? a4 + "_" + a3 + "_" : "") + "postPlaneAct";
        //# var postVolumeAct = (crd ? a4 + "_" : "") + "postVolumeAct";
        public void Foreach__f4____f3____f2____f1____idxstr__(/*# for (int i1 = 1; i1 < itc; i1++) { */__ttn__Info t__i1__, /*# }  if (pre) { */
                Action/*# if (crd) { */<long>/*# } */ __preVolumeAct__,
                Action/*# if (crd) { */<long, long>/*# } */ __prePlaneAct__,
                Action/*# if (crd) { */<long, long, long>/*# } */ __preLineAct__,/*# } */
                Action</*# if (crd) { */long, long, long, long/*# if (idx) { */, /*# }} if (idx) { */long/*# for (int i1 = 1; i1 < itc; i1++) { */, long/*# }} */> __elementAct__/*# if (post) { */,
                Action/*# if (crd) { */<long, long, long>/*# } */ __postLineAct__,
                Action/*# if (crd) { */<long, long>/*# } */ __postPlaneAct__,
                Action/*# if (crd) { */<long>/*# } */ __postVolumeAct__/*# } */)
        {
            long __a4__s = DS__f4__, __a4__j = J__f4____f3__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a4__j__i1__ = t__i1__.J__f4____f3__/*# } */;
            long __a3__s = DS__f3__, __a3__j = J__f3____f2__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a3__j__i1__ = t__i1__.J__f3____f2__/*# } */;
            long __a2__s = DS__f2__, __a2__j = J__f2____f1__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a2__j__i1__ = t__i1__.J__f2____f1__/*# } */;
            long __a1__s = DS__f1__, __a1__j = J__f1____f0__/*# for (int i1 = 1; i1 < itc; i1++) { */, __a1__j__i1__ = t__i1__.J__f1____f0__/*# } */;
            long i = FirstIndex/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ = t__i1__.FirstIndex/*# } */;
            for (long __a4__e = i + __a4__s/*# if (crd) { */, __a4__ = F__f4__/*# } */; i != __a4__e; i += __a4__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a4__j__i1__/*# }
                 if (crd) { */, __a4__++/*# } */)
            {
            //# if (pre) {
            __preVolumeAct__(/*# if (crd) { */__a4__/*# } */);
            //# }
            for (long __a3__e = i + __a3__s/*# if (crd) { */, __a3__ = F__f3__/*# } */; i != __a3__e; i += __a3__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a3__j__i1__/*# }
                 if (crd) { */, __a3__++/*# } */)
            {
            //# if (pre) {
            __prePlaneAct__(/*# if (crd) { */__a4__, __a3__/*# } */);
            //# }
            for (long __a2__e = i + __a2__s/*# if (crd) { */, __a2__ = F__f2__/*# } */; i != __a2__e; i += __a2__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a2__j__i1__/*# }
                 if (crd) { */, __a2__++/*# } */)
            {
            //# if (pre) {
            __preLineAct__(/*# if (crd) { */__a4__, __a3__, __a2__/*# } */);
            //# }
            for (long __a1__e = i + __a1__s/*# if (crd) { */, __a1__ = F__f1__/*# } */; i != __a1__e; i += __a1__j/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__ += __a1__j__i1__/*# }
                 if (crd) { */, __a1__++/*# } */)
                __elementAct__(/*# if (crd) { */__a4__, __a3__, __a2__, __a1__/*# if (idx) { */, /*# }} if (idx) { */i/*# for (int i1 = 1; i1 < itc; i1++) { */, i__i1__/*# } } */);
            //# if (post) {
            __postLineAct__(/*# if (crd) { */__a4__, __a3__, __a2__/*# } */);
            //# }
            }
            //# if (post) {
            __postPlaneAct__(/*# if (crd) { */__a4__, __a3__/*# } */);
            //# }
            }
            //# if (post) {
            __postVolumeAct__(/*# if (crd) { */__a4__/*# } */);
            //# }
            }
        }

        //# } // post
        //# } // pre
        //# } // crd
        //# }});
        //# }});
        //# }});
        //# });
        //# } // (d > 3)
        //# } // idx
        //# } // itc
        #endregion

        #region Checking

        public void CheckMatchingSize(__ttn__Info t1)
        {
            if (Size != t1.Size) throw new ArgumentException("size mismatch");
        }

        public void CheckMatchingSize(__ttn__Info t1, __ttn__Info t2)
        {
            CheckMatchingSize(t1); CheckMatchingSize(t2);
        }

        public void CheckMatchingSize(__ttn__Info t1, __ttn__Info t2, __ttn__Info t3)
        {
            CheckMatchingSize(t1); CheckMatchingSize(t2); CheckMatchingSize(t3);
        }

        public bool HasMatchingLayout(__ttn__Info t1)
        {
            return First == t1.First && Origin == t1.Origin && Delta == t1.Delta;
        }

        public bool HasMatchingLayout(__ttn__Info t1, __ttn__Info t2)
        {
            return HasMatchingLayout(t1) && HasMatchingLayout(t2);
        }

        public bool HasMatchingLayout(__ttn__Info t1, __ttn__Info t2, __ttn__Info t3)
        {
            return HasMatchingLayout(t1) && HasMatchingLayout(t2) && HasMatchingLayout(t3);
        }

        #endregion
    }
        
    #endregion

    //# } // dt == vt
    #region __ttn__<__dvtn__>

    /// <summary>
    /// Generic __ttnl__ of elements with arbitrary stride.
    /// All sizes are given as __d__-dimensional vectors, with the first
    /// parameter specifying the inner dimension.
    /// The __ttnl__ does not exclusively own its underlying data array, it
    /// can also serve as a window into other arrays and tensors. Operations
    /// on __ttnl__s are supported by function arguments which can be easily
    /// exploited by using lambda functions.
    /// Note: stride is called Delta (or D) within this data structure, the
    /// __d__ stride direction(s) are called DX, DY, aso.
    //# if (dt != vt) {
    /// The __ttnl__ has different view and data element types, i.e. although
    /// all data is stored using the data element type, in its interfaces it
    /// acts as a __ttnl__ of view element types.
    //# }
    /// </summary>
    /// <typeparam name="__dtn__">data element type</typeparam>
    //# if (dt != vt) {
    /// <typeparam name="__vtn__">view element type</typeparam>
    //# }
    [Serializable]
    public partial struct __ttn__<__dvtn__> : IValidity, I__ttn__<__vtn__>, IArray__ttn__
    {
        public __dtn__[] Data;
        //# if (dt != vt) {
        public Func<Td[], long, Tv> Getter;
        public Action<Td[], long, Tv> Setter;
        //# }
        public __ttn__Info Info;

        #region Constructors

        /// <summary>
        /// Construct from data array with specified info without copying.
        /// </summary>
        public __ttn__(__dtn__[] data, __ttn__Info info)
        {
            Data = data;
            Info = info;
            //# if (dt != vt) {
            Getter = null;
            Setter = null;
            //# }
        }

        /// <summary>
        /// Construct __ttnl__ of specified size.
        /// </summary>
        public __ttn__(__itn__ size)
            : this(new __dtn__[size/*# if (d > 1) {*/.X * size.Y/*# }
                                           if (d > 2) {*/ * size.Z/*# }
                                               if (d > 3) {*/ * size.W/*# } */], new __ttn__Info(size))
        { }

        /// <summary>
        /// Construct __ttnl__ of specified size.
        /// </summary>
        public __ttn__(__iitn__ size)
            : this((__itn__)size)
        { }

        /// <summary>
        /// Construct __ttnl__ with specified info.
        /// </summary>
        public __ttn__(__ttn__Info info)
            : this(new __dtn__[info.Count], new __ttn__Info(info))
        { }

        //# if (d == 1) {
        /// <summary>
        /// Construct from data array without copying.
        /// </summary>
        public __ttn__(__dtn__[] data) 
            : this(data, new __ttn__Info(data.Length))
        { }

        //# } else if (d > 1) {
        /// <summary>
        /// Construct __ttnl__ of specified size.
        /// </summary>
        public __ttn__(/*# iaa.ForEach(a => { */long s__a__/*# }, comma); */)
            : this(new __itn__(/*# iaa.ForEach(a => { */s__a__/*# }, comma); */))
        { }

        /// <summary>
        /// Construct from data array with specified size without copying.
        /// </summary>
        public __ttn__(__dtn__[] data, /*# iaa.ForEach(a => { */long s__a__/*# }, comma); */)
            : this(data, new __ttn__Info(/*# iaa.ForEach(a => { */s__a__/*# }, comma); */))
        { }

        /// <summary>
        /// Construct with specified size and sets all elements to the supplied value.
        /// </summary>
        public __ttn__(/*# iaa.ForEach(a => { */long s__a__/*# }, comma); */, __vtn__ value)
            : this(new __itn__(/*# iaa.ForEach(a => { */s__a__/*# }, comma); */), value)
        { }

        //# }
        /// <summary>
        /// Construct from data array with specified size without copying.
        /// </summary>
        public __ttn__(__dtn__[] data, __itn__ size)
            : this(data, new __ttn__Info(size))
        { }

        /// <summary>
        /// Construct from data array with specified size without copying.
        /// </summary>
        public __ttn__(__dtn__[] data, __iitn__ size)
            : this(data, new __ttn__Info(size))
        { }

        /// <summary>
        /// Construct __ttnl__ of specified size, with all elements set to
        /// the supplied value.
        /// </summary>
        public __ttn__(__itn__ size, __vtn__ value)
            : this(new __dtn__[size/*# if (d > 1) {*/.X * size.Y/*# }
                                           if (d > 2) {*/ * size.Z/*# }
                                               if (d > 3) {*/ * size.W/*# }*/], new __ttn__Info(size))
        {
            Set(value);
        }

        /// <summary>
        /// Construct matrix of specified size, with all elements set to
        /// the supplied value.
        /// </summary>
        public __ttn__(__iitn__ size, __vtn__ value)
            : this((__itn__)size, value)
        { }

        /// <summary>
        /// Construct from data array with specified size and delta without copying.
        /// </summary>
        public __ttn__(__dtn__[] data, long origin, __itn__ size, __itn__ delta)
            : this(data, new __ttn__Info(origin, size, delta))
        { }

        /// <summary>
        /// Construct from data array with specified size and delta without copying.
        /// </summary>
        public __ttn__(__dtn__[] data, long origin, __itn__ size, __itn__ delta, __itn__ first)
            : this(data, new __ttn__Info(origin, size, delta, first))
        { }

        /// <summary>
        /// Construct with specified size and delta.
        /// </summary>
        public __ttn__(long origin, __itn__ size, __itn__ delta)
            : this(new __dtn__[size/*# if (d > 1) {*/.X * size.Y/*# }
                                           if (d > 2) {*/ * size.Z/*# }
                                              if (d > 3) {*/ * size.W/*# }                                                                   
                                                                   */],
                   new __ttn__Info(origin, size, delta))
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Return the rank or dimension of the __ttnl__ (__d__).
        /// </summary>
        public int Rank { get { return Info.Rank; } }

        public long Origin { get { return Info.Origin; } set { Info.Origin = value; } }

        public __itn__ Size { get { return Info.Size; } set { Info.Size = value; } }

        public __itn__ Delta { get { return Info.Delta; } set { Info.Delta = value; } }

        public __itn__ First { get { return Info.First; } set { Info.First = value; } }

        //# if (dt != vt) {
        public TensorAccessors<__dtn__, __vtn__> Accessors
        {
            get
            {
                return new TensorAccessors<__dtn__, __vtn__>()
                                { Getter = Getter, Setter = Setter };
            }
            set { Getter = value.Getter; Setter = value.Setter; }
        }

        //# }
        /// <summary>
        /// Returns true if the __ttnl__ has a data array.
        /// </summary>
        public bool IsValid { get { return Data != null; } }

        /// <summary>
        /// Returns true if the __ttnl__ does not have a data array.
        /// </summary>
        public bool IsInvalid { get { return Data == null; } }

        /// <summary>
        /// Total number of element in the matrix.
        /// </summary>
        public long Count { get { return Info.Count; } }

        /// <summary>
        /// One step beyond the last element.
        /// </summary>
        public __itn__ End { get { return Info.E; } }

        //# for (int di = 0; di < (1 << d); di++) {
        //#     var pnt = d.Range().Select(i => (di & (1 << i)) == 0 ? "O" : "I").Join();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ __pnt__ { get { return Info.__pnt__; } }
        //# }

        /// <summary>
        /// Size
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ S { get { return Info.S; } set { Info.S = value; } }

        /// <summary>
        /// Delta
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ D { get { return Info.D; } set { Info.D = value; } }

        /// <summary>
        /// First
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ F { get { return Info.F; } set { Info.F = value; } }

        /// <summary>
        /// End: one step beyond the last element.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public __itn__ E { get { return Info.E; } }

        //# if (d == 1) {
        /// <summary>
        /// Cummulative delta for all elements up to this dimension.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long DS { get { return Info.D; } }

        //# }
        //# foreach (var f in ifa) {
        /// <summary>
        /// Size in dimension __f__.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long S__f__ { get { return Info.S__f__; } set { Info.S__f__ = value; } }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// Delta in dimension __f__.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long D__f__ { get { return Info.D__f__; } set { Info.D__f__ = value; } }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// First in dimension __f__.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long F__f__ { get { return Info.F__f__; } set { Info.F__f__ = value; } }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// End in dimension __f__ (one step beyond the last element).
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long E__f__ { get { return Info.E__f__; } }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// Jump this many elements in the underlying data array when stepping
        /// in dimension __f__.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long J__f__ { get { return Info.J__f__; } }

        //# } // foreach
        //# foreach (var f in ifa) {
        /// <summary>
        /// Cummulative delta for all elements up to this dimension.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long DS__f__ { get { return Info.DS__f__; } }

        //# } // foreach
        /// <summary>
        /// Return the index of the first element in the underlying
        /// data array.
        /// </summary>
        public long FirstIndex { get { return Info.FirstIndex; } }

        public long OriginIndex { get { return Info.Origin; } set { Info.Origin = value; } }

        /// <summary>
        /// Get or set the size of the tensor in each dimension as an
        /// array of longs.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long[] SizeArray
        {
            get { return Info.SizeArray; }
            set { Info.SizeArray = value; }
        }

        /// <summary>
        /// Get or set the deltas of the tensor in each dimension as an
        /// array of longs.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long[] DeltaArray
        {
            get { return Info.DeltaArray; }
            set { Info.DeltaArray = value; }
        }

        /// <summary>
        /// Get or set the first coords of the tensor in each dimension as an
        /// array of longs.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long[] FirstArray
        {
            get { return Info.FirstArray; }
            set { Info.FirstArray = value; }
        }

        /// <summary>
        /// Yields all elemnts ordered by index.
        /// </summary>
        public IEnumerable<__vtn__> Elements
        {
            get
            {
                //# Loop("", false, () => {
                    yield return __t0i__;
                //# }, false, false);
            }
        }

        /// <summary>
        /// Return the type of the underlying data array.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Type ArrayType
        {
            get { return typeof(__dtn__[]); }
        }

        /// <summary>
        /// Return the underlying data array as an untyped array.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Array Array
        {
            get { return Data; }
            set { Data = (__dtn__[])value; }
        }

        //# if (d == 2) {
        /// <summary>
        /// Returns a matrix that represents a view on the same data with flipped coordinates.
        /// </summary>
        public __ttn__<__dvtn__> Transposed
        {
            get
            {
                return new __ttn__<__dvtn__>(Data, Info.Transposed)/*#
                            if (dt != vt) { */
                                        { Getter = Getter, Setter = Setter }/*# } */;
            }
        }

        //# } // d == 2
        #endregion

        #region Indexers

        //# if (d > 1) {
        /// <summary>
        /// Get/Set element at specified coordinate.
        /// </summary>
        public __vtn__ this[__itn__ v]
        {
            get
            {
                //# if (dt == vt) {
                return Data[Info.Origin
                              + /*# ifa.ForEach(f =>
                                    { */v.__f__ * Info.Delta.__f__/*# }, add); */];
                //# } else {
                return Getter(Data, Info.Origin
                              + /*# ifa.ForEach(f =>
                                    { */v.__f__ * Info.Delta.__f__/*# }, add); */);
                //# }
            }
            set
            {
                //# if (dt == vt) {
                Data[Info.Origin
                       + /*# ifa.ForEach(f =>
                             { */v.__f__ * Info.Delta.__f__/*# }, add); */] = value;
                //# } else {
                Setter(Data, Info.Origin
                       + /*# ifa.ForEach(f =>
                             { */v.__f__ * Info.Delta.__f__/*# }, add); */, value);
                //# }            
            }
        }

        /// <summary>
        /// Get/Set element at specified coordinate.
        /// </summary>
        public __vtn__ this[__iitn__ v]
        {
            get
            {
                //# if (dt == vt) {
                return Data[Info.Origin
                              + /*# ifa.ForEach(f =>
                                    { */(long)v.__f__ * Info.Delta.__f__/*# }, add); */];
                //# } else {
                return Getter(Data, Info.Origin
                              + /*# ifa.ForEach(f =>
                                    { */(long)v.__f__ * Info.Delta.__f__/*# }, add); */);
                //# }
            }
            set
            {
                //# if (dt == vt) {
                Data[Info.Origin
                       + /*# ifa.ForEach(f =>
                             { */(long)v.__f__ * Info.Delta.__f__/*# }, add); */] = value;
                //# } else {
                Setter(Data, Info.Origin
                       + /*# ifa.ForEach(f =>
                             { */(long)v.__f__ * Info.Delta.__f__/*# }, add); */, value);
                //# }            
            }
        }

        /// <summary>
        /// Get/Set element at the specified index in the underlying data array.
        /// </summary>
        public __vtn__ this[long index]
        {
            get
            {
                //# if (dt == vt) {
                return Data[index];
                //# } else {
                return Getter(Data, index);
                //# }
            }
            set
            {
                //# if (dt == vt) {
                Data[index] = value;
                //# } else {
                Setter(Data, index, value);
                //# }            
            }
        }

        //# }
        /// <summary>
        /// Get/Set element at specified coordinates.
        /// </summary>
        public __vtn__ this[/*# iaa.ForEach(a => { */long __a__/*# }, comma); */]
        {
            get
            {
                //# if (dt == vt) {
                return Data[Info.Origin
                              + /*# iaa.ForEach(ideltas, (a, di) =>
                                    { */__a__ * Info.__di__/*# }, add); */];
                //# } else {
                return Getter(Data, Info.Origin
                              + /*# iaa.ForEach(ideltas, (a, di) =>
                                    { */__a__ * Info.__di__/*# }, add); */);
                //# }
            }
            set
            {
                //# if (dt == vt) {
                Data[Info.Origin
                       + /*# iaa.ForEach(ideltas, (a, di) =>
                             { */__a__ * Info.__di__/*# }, add); */] = value;
                //# } else {
                Setter(Data, Info.Origin
                       + /*# iaa.ForEach(ideltas, (a, di) =>
                             { */__a__ * Info.__di__/*# }, add); */, value);
                //# }            
            }
        }
        
        /// <summary>
        /// Get/Set element at specified coordinates.
        /// </summary>
        public __vtn__ this[/*# iaa.ForEach(a => { */int __a__/*# }, comma); */]
        {
            get
            {
                //# if (dt == vt) {
                return Data[Info.Origin
                              + /*# iaa.ForEach(ideltas, (a, di) =>
                                    { */(long)__a__ * Info.__di__/*# }, add); */];
                //# } else {
                return Getter(Data, Info.Origin
                              + /*# iaa.ForEach(ideltas, (a, di) =>
                                    { */(long)__a__ * Info.__di__/*# }, add); */);
                //# }
            }
            set
            {
                //# if (dt == vt) {
                Data[Info.Origin
                       + /*# iaa.ForEach(ideltas, (a, di) =>
                             { */(long)__a__ * Info.__di__/*# }, add); */] = value;
                //# } else {
                Setter(Data, Info.Origin
                       + /*# iaa.ForEach(ideltas, (a, di) =>
                             { */(long)__a__ * Info.__di__/*# }, add); */, value);
                //# }            
            }
        }

        #endregion

        #region Actions for each Element

        public void ForeachIndex(Action<long> i_action)
        {
            Info.ForeachIndex(i_action);
        }

        //# { var i_action = ""; iaa.ForEach(a => i_action += a + "_"); i_action += "i_action";
        public void ForeachIndex(Action</*# iaa.ForEach(f => {*/long/*# }, comma);*/, long> __i_action__)
        {
            Info.ForeachIndex(__i_action__);
        }
        //# } // var i_action

        public void ForeachCoord(Action<__itn__> v_action)
        {
            Info.ForeachCoord(v_action);
        }

        public void ForeachIndex(__ttn__Info t1, Action<long, long> i_i1_act)
        {
            Info.ForeachIndex(t1, i_i1_act);
        }

        //# if (d > 1) {
        //# foreach (bool idx in new[] { false, true }) { var idxstr = idx ? "Index" : "";
        //# ifa.ForEach(iaa, (f,a) => {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// and calls the supplied action with the index into the data
        /// array as parameter. Optionally, the coordinates of each element
        /// are also fed into the supplied action.
        /// </summary>
        //# var elementAct = (crd ? a + "_" : "") + (idx ? "i_" : "") + "elementAct";
        public void Foreach__f____idxstr__(
                Action</*# if (crd) { */long/*# if (idx) { */, /*# }} if (idx) { */long/*# } */> __elementAct__)
        {
            Info.Foreach__f____idxstr__(__elementAct__);
        }

        //# } // crd        
        //# });
        //# ifa.ForEach(iaa, (f2,a2) => {
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2) {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        //# foreach (bool pre in new[] { false, true }) {
        //# foreach (bool post in new[] { false, true }) {
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop, and calls the supplied action
        /// with the index into the data array as parameter. Optionally, the
        /// coordinates of each element are also fed into the supplied action
        /// (in the order specified in the name of the function), and also
        /// optionally pre-line and post-line actions can be specified.
        /// </summary>
        //# var preLineAct = (crd ? a2 + "_" : "") + "preLineAct";
        //# var elementAct = (crd ? a2 + "_" + a1 + "_" : "") + (idx ? "i_" : "") + "elementAct";
        //# var postLineAct = (crd ? a2 + "_" : "") + "postLineAct";
        public void Foreach__f2____f1____idxstr__(
                /*# if (pre) { */Action/*# if (crd) { */<long>/*# } */ __preLineAct__,
                /*# } */Action</*# if (crd) { */long, long/*# if (idx) { */, /*# }} if (idx) { */long/*# } */> __elementAct__/*# if (post) { */,
                Action/*# if (crd) { */<long>/*# } */ __postLineAct__/*# } */)
        {
            Info.Foreach__f2____f1____idxstr__(
                    /*# if (pre) { */__preLineAct__,
                    /*# } */__elementAct__/*# if (post) {  */,
                    __postLineAct__/*# } */);
        }

        //# } // post
        //# } // pre
        //# } // crd
        //# }});
        //# });
        //# } // idx
        //# } // (d > 1)
        //# if (d > 2) {
        //# foreach (bool idx in new[] { false, true }) { var idxstr = idx ? "Index" : "";
        //# ifa.ForEach(iaa, (f3,a3) => {
        //# ifa.ForEach(iaa, (f2,a2) => { if (f2 != f3) {
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2 && f1 != f3) {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        //# foreach (bool pre in new[] { false, true }) {
        //# foreach (bool post in new[] { false, true }) {
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop, and calls the supplied action
        /// with the index into the data array as parameter. Optionally, the
        /// coordinates of each element are also fed into the supplied action
        /// (in the order specified in the name of the function), and also
        /// optionally pre-line, post-line, pre-plane, and post-plane
        /// actions can be specified.
        /// </summary>
        //# var prePlaneAct = (crd ? a3 + "_" : "") + "prePlaneAct";
        //# var preLineAct = (crd ? a3 + "_" + a2 + "_" : "") + "preLineAct";
        //# var elementAct = (crd ? a3 + "_" + a2 + "_" + a1 + "_" : "") + (idx ? "i_" : "") + "elementAct";
        //# var postLineAct = (crd ? a3 + "_" + a2 + "_" : "") + "postLineAct";
        //# var postPlaneAct = (crd ? a3 + "_" : "") + "postPlaneAct";
        public void Foreach__f3____f2____f1____idxstr__(/*# if (pre) { */
                Action/*# if (crd) { */<long>/*# } */ __prePlaneAct__,
                Action/*# if (crd) { */<long, long>/*# } */ __preLineAct__,/*# } */
                Action</*# if (crd) { */long, long, long/*# if (idx) { */, /*# }} if (idx) { */long/*# } */> __elementAct__/*# if (post) { */,
                Action/*# if (crd) { */<long, long>/*# } */ __postLineAct__,
                Action/*# if (crd) { */<long>/*# } */ __postPlaneAct__/*# } */)
        {
            Info.Foreach__f3____f2____f1____idxstr__(
                    /*# if (pre) { */__prePlaneAct__,
                    __preLineAct__,
                    /*# } */__elementAct__/*# if (post) {  */,
                    __postLineAct__,
                    __postPlaneAct__/*# } */);
        }

        //# } // post
        //# } // pre
        //# } // crd
        //# }});
        //# }});
        //# });
        //# } // idx
        //# } // (d > 2)
        //# if (d > 3) {
        //# foreach (bool idx in new[] { false, true }) { var idxstr = idx ? "Index" : "";
        //# ifa.ForEach(iaa, (f4,a4) => {
        //# ifa.ForEach(iaa, (f3,a3) => { if (f3 != f4) {
        //# ifa.ForEach(iaa, (f2,a2) => { if (f2 != f3 && f2 != f4) {
        //# ifa.ForEach(iaa, (f1,a1) => { if (f1 != f2 && f1 != f3 && f1 != f4) {
        //# foreach (bool crd in new[] { false, true }) { if (!crd && !idx) continue;
        //# foreach (bool pre in new[] { false, true }) {
        //# foreach (bool post in new[] { false, true }) {
        /// <summary>
        /// Loops over  the underlying data array in the specified order,
        /// with the first coordinate being the outer loop, and the last
        /// coordinate being the inner loop, and calls the supplied action
        /// with the index into the data array as parameter. Optionally, the
        /// coordinates of each element are also fed into the supplied action
        /// (in the order specified in the name of the function), and also
        /// optionally pre-line, post-line, pre-plane, and post-plane
        /// actions can be specified.
        /// </summary>
        //# var preVolumeAct = (crd ? a4 + "_" : "") + "preVolumeAct";
        //# var prePlaneAct = (crd ? a4 + "_" + a3 + "_" : "") + "prePlaneAct";
        //# var preLineAct = (crd ? a4 + "_" + a3 + "_" + a2 + "_" : "") + "preLineAct";
        //# var elementAct = (crd ? a4 + "_" + a3 + "_" + a2 + "_" + a1 + "_" : "") + (idx ? "i_" : "") + "elementAct";
        //# var postLineAct = (crd ? a4 + "_" + a3 + "_" + a2 + "_" : "") + "postLineAct";
        //# var postPlaneAct = (crd ? a4 + "_" + a3 + "_" : "") + "postPlaneAct";
        //# var postVolumeAct = (crd ? a4 + "_" : "") + "postVolumeAct";
        public void Foreach__f4____f3____f2____f1____idxstr__(/*# if (pre) { */
                Action/*# if (crd) { */<long>/*# } */ __preVolumeAct__,
                Action/*# if (crd) { */<long, long>/*# } */ __prePlaneAct__,
                Action/*# if (crd) { */<long, long, long>/*# } */ __preLineAct__,/*# } */
                Action</*# if (crd) { */long, long, long, long/*# if (idx) { */, /*# }} if (idx) { */long/*# } */> __elementAct__/*# if (post) { */,
                Action/*# if (crd) { */<long, long, long>/*# } */ __postLineAct__,
                Action/*# if (crd) { */<long, long>/*# } */ __postPlaneAct__,
                Action/*# if (crd) { */<long>/*# } */ __postVolumeAct__/*# } */)
        {
            Info.Foreach__f4____f3____f2____f1____idxstr__(
                    /*# if (pre) { */__preVolumeAct__,
                    __prePlaneAct__,
                    __preLineAct__,
                    /*# } */__elementAct__/*# if (post) {  */,
                    __postLineAct__,
                    __postPlaneAct__,
                    __postVolumeAct__/*# } */);
        }

        //# } // post
        //# } // pre
        //# } // crd
        //# }});
        //# }});
        //# }});
        //# });
        //# } // idx
        //# } // (d > 3)
        #endregion

        #region Reinterpretation and Parts

        //# bools.ForEach(t1v => { var pt1 = t1v ? "<T1>" : "";
        //# var dvt1tn = dtn + (t1v ? ", T1" : (dt != vt ? ", " + vtn : ""));
        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(__itn__ begin, __itn__ size)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__Window(begin, size))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn____pt1__(__itn__ begin, __itn__ size)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__(begin, size))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(__iitn__ begin, __iitn__ size)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__Window(begin, size))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn____pt1__(__iitn__ begin, __iitn__ size)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__(begin, size))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(__itn__ begin, __itn__ size, __itn__ delta)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__Window(begin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn____pt1__(__itn__ begin, __itn__ size, __itn__ delta)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__(begin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(__iitn__ begin, __iitn__ size, __iitn__ delta)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__Window(begin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn____pt1__(__iitn__ begin, __iitn__ size, __iitn__ delta)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__(begin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        //# if (d > 1) {  
        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(__itn__ begin, __itn__ size, __itn__ delta, __itn__ first)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__Window(begin, size, delta, first))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(__iitn__ begin, __iitn__ size, __iitn__ delta, __iitn__ first)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__ttn__Window(begin, size, delta, first))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */)
        {
            return new __ttn__<__dvt1tn__>(Data,
                            Info.Sub__ttn__Window(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */,
                                           /*# ifa.ForEach(f => { */size__f__/*# }, comma); */))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn____pt1__(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */)
        {
            return new __ttn__<__dvt1tn__>(Data,
                            Info.Sub__ttn__(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */,
                                               /*# ifa.ForEach(f => { */size__f__/*# }, comma); */))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This method retains the coordinates of the parent __ttn__.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn__Window__pt1__(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long delta__f__/*# }, comma); */)
        {
            return new __ttn__<__dvt1tn__>(Data,
                            Info.Sub__ttn__Window(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */,
                                           /*# ifa.ForEach(f => { */size__f__/*# }, comma); */,
                                           /*# ifa.ForEach(f => { */delta__f__/*# }, comma); */))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__ttn____pt1__(
                /*# ifa.ForEach(f => { */long begin__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long size__f__/*# }, comma); */,
                /*# ifa.ForEach(f => { */long delta__f__/*# }, comma); */)
        {
            return new __ttn__<__dvt1tn__>(Data,
                            Info.Sub__ttn__(/*# ifa.ForEach(f => { */begin__f__/*# }, comma); */,
                                               /*# ifa.ForEach(f => { */size__f__/*# }, comma); */,
                                               /*# ifa.ForEach(f => { */delta__f__/*# }, comma); */))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        //# if (d == 2) {
        /// If the lines of the matrix are stored consecutively without gaps,
        /// the whole matrix can be viewed as a single vector.
        //# } else {
        /// If the lines and planes of the volume are stored consecutively
        /// without gaps, the whole volume can be viewed as a single vector.
        //# }
        /// </summary>
        public __vectn__<__dvt1tn__> As__vectn____pt1__()
        {
            return new __vectn__<__dvt1tn__>(Data, Info.As__vectn__())/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A SubVector does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __vectn__<__dvt1tn__> Sub__vectn____pt1__(__itn__ origin, long size, long delta)
        {
            return new __vectn__<__dvt1tn__>(Data, Info.Sub__vectn__(origin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A SubVector does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __vectn__<__dvt1tn__> Sub__vectn____pt1__(__iitn__ origin, long size, long delta)
        {
            return new __vectn__<__dvt1tn__>(Data, Info.Sub__vectn__(origin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        //# if (d == 2) {
        //# for (int ai = 0; ai < d; ai++) {var a = iaa[ai];
        //#     var fa = ""; ifa.ForEach((f,i) => { if (i != ai) fa = fa + f; });
        //#     var fn = fa == "X" ? "Row" : "Col";
        //#     foreach (var win in new bool[] { false, true }) { var winstr = win ? "Window" : "";
        /// <summary>
        /// Return a full __fa__ __ttsub1nl__ slice of the __ttnl__.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttsub1nl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttsub1n__<__dvt1tn__> __fn____winstr____pt1__(long __a__)
        {
            return new __ttsub1n__<__dvt1tn__>(Data, Info.__fn____winstr__(__a__))/*#
                            if (dt != vt && pt1 == "") { */
            { Getter = Getter, Setter = Setter }/*# } */;
        }

        //# } // win
        //# } // ai
        //# } // d == 2
        //# if (d == 3) {
        /// <summary>
        /// If the lines of the volume are stored consecutively without gaps,
        /// they can be merged, and the volume can be viewed as a matrix.
        /// </summary>
        public __mattn__<__dvt1tn__> As__mattn__XYxZ__pt1__()
        {
            return new __mattn__<__dvt1tn__>(Data, Info.As__mattn__XYxZ())/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// If the planes of the volume are stored consecutively without gaps,
        /// they can be merged, and the volume can be viewed as a matrix.
        /// </summary>
        public __mattn__<__dvt1tn__> As__mattn__XxYZ__pt1__()
        {
            return new __mattn__<__dvt1tn__>(Data, Info.As__mattn__XxYZ())/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __mattn__<__dvt1tn__> Sub__mattn____pt1__(__itn__ origin, V2l size, V2l delta)
        {
            return new __mattn__<__dvt1tn__>(Data, Info.Sub__mattn__(origin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __mattn__<__dvt1tn__> Sub__mattn____pt1__(__iitn__ origin, V2i size, V2l delta)
        {
            return new __mattn__<__dvt1tn__>(Data, Info.Sub__mattn__(origin, size, delta))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// A Sub__ttn__ does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public __mattn__<__dvt1tn__> Sub__mattn____pt1__(
                long beginX, long beginY, long beginZ,
                long sizeX, long sizeY, long deltaX, long deltaY)
        {
            return new __mattn__<__dvt1tn__>(Data,
                    Info.Sub__mattn__(beginX, beginY, beginZ,
                                         sizeX, sizeY, deltaX, deltaY))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        //# } // (d == 3)
        //# for (int ai = 0; ai < d; ai++) {
        //#     var a = iaa[ai];
        //#     var fa = ""; ifa.ForEach((f,i) => { if (i != ai) fa = fa + f; });
        //#     foreach (var win in new bool[] { false, true }) { var winstr = win ? "Window" : "";
        /// <summary>
        /// Return a full __fa__ __ttsub1nl__ slice of the __ttnl__.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttsub1nl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttsub1n__<__dvt1tn__> Sub__fa____ttsub1n____winstr____pt1__(long __a__)
        {
            return new __ttsub1n__<__dvt1tn__>(Data, Info.Sub__fa____ttsub1n____winstr__(__a__))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        /// <summary>
        /// Return a full __fa__ __ttsub1nl__ slice of the __ttnl__ as a special __ttnl__
        /// that replicates the __ttsub1nl__ the supplied number of times. The special
        /// __ttnl__ can only be correctly used to read from. Other operations are not guaranteed.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttsub1nl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__fa____ttsub1n__AsReadOnly__ttn____winstr____pt1__(long __a__, long size__a__)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__fa____ttsub1n__AsReadOnly__ttn____winstr__(__a__, size__a__))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        //# } // win
        //# } // ai
        //# for (int ai = 0; ai < d-1; ai++) { var a = iaa[ai];
        //# for (int bi = ai + 1; bi < d; bi++) { var b = iaa[bi];
        //#     var fa = ""; ifa.ForEach((f,i) => { if (i != ai && i != bi) fa = fa + f; });
        //#     foreach (var win in new bool[] { false, true }) { var winstr = win ? "Window" : "";
        /// <summary>
        /// Return a full __fa__ __ttsub2nl__ slice of the __ttnl__ as a special __ttnl__
        /// that replicates the __ttsub2nl__ the supplied number of times. The special
        /// __ttnl__ can only be correctly used to read from. Other operations are not guaranteed.
        //# if (win) {
        /// This method retains the coordinates of the parent __ttnl__.
        //# } else {
        /// This methods returns a __ttnl__ with zero as first coordinates.
        //# }
        /// </summary>
        public __ttn__<__dvt1tn__> Sub__fa____ttsub2n__AsReadOnly__ttn____winstr____pt1__(long __a__, long __b__, long size__a__, long size__b__)
        {
            return new __ttn__<__dvt1tn__>(Data, Info.Sub__fa____ttsub2n__AsReadOnly__ttn____winstr__(__a__, __b__, size__a__, size__b__))/*#
                            if (dt != vt && pt1 == "") { */
                                    { Getter = Getter, Setter = Setter }/*# } */;
        }

        //# } // win
        //# } // bi
        //# } // ai
        //# } // (d > 1)
        //# }); // t1v
        #endregion

        #region Copying

        /// <summary>
        /// Elementwise copy.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvtn__> Copy()
        {
            //# if (dt != vt) {
            return new __ttn__<__dvtn__>(new Td[Data.LongLength], Info) { Getter = Getter, Setter = Setter }.Set(this);
            //# } else {
            return new __ttn__<__dvtn__>(Info.S).Set(this);
            //# }
        }

        /// <summary>
        /// Elementwise copy.
        /// This method retains the coordinates of the original __ttn__.
        /// </summary>
        public __ttn__<__dvtn__> CopyWindow()
        {
            return new __ttn__<__dvtn__>(Info.S)
                    { F = F, /*# if (dt != vt) { */Getter = Getter, Setter = Setter/*# } */ }.Set(this);
        }

        /// <summary>
        /// Elementwise copy with function application.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<T1> Map<T1>(Func<__vtn__, T1> fun)
        {
            return new __ttn__<T1>(Info.S).SetMap(this, fun);
        }

        /// <summary>
        /// Elementwise copy with function application.
        /// This method retains the coordinates of the original __ttn__.
        /// </summary>
        public __ttn__<T1> MapWindow<T1>(Func<__vtn__, T1> fun)
        {
            return new __ttn__<T1>(Info.S) { F = F }.SetMap(this, fun);
        }

        //# if (dt != vt) {
        public __ttn__<__vtn__> CopyView()
        {
            return new __ttn__<__vtn__>(Info.S).Set(this);
        }

        public __ttn__<__vtn__> CopyViewWindow()
        {
            return new __ttn__<__vtn__>(Info.S) { F = F }.Set(this);
        }

        /// <summary>
        /// Elementwise copy with function application.
        /// This methods returns a __ttn__ with zero as first coordinates.
        /// </summary>
        public __ttn__<__dvtn__> Map(Func<__vtn__, __vtn__> fun)
        {
            return new __ttn__<__dvtn__>(new Td[Data.LongLength], Info) { Getter = Getter, Setter = Setter }.SetMap(this, fun);
        }

        /// <summary>
        /// Elementwise copy with function application.
        /// This method retains the complete layout of the original __ttn__.
        /// </summary>
        public __ttn__<__dvtn__> MapWindow(Func<__vtn__, __vtn__> fun)
        {
            return new __ttn__<__dvtn__>(new Td[Data.LongLength], Info)
                        { F = F, Getter = Getter, Setter = Setter }.SetMap(this, fun);
        }

        //# } // dt != vt
        //# if (dt == vt) {
        /// <summary>
        /// Returns a tensor repeated in each dimension by the specified
        /// count.
        /// </summary>
        public __ttn__<__dvtn__> Repeated(__itn__ count)
        {
            var size = Size;
            count *= size;
            var t = new __ttn__<__dvtn__>(count);
            //# if (d == 1) {
            for (__itn__ p = 0; p < count; p += size)
                t.Sub__ttn__(p, size).Set(this);
            //# } else { // d > 1
            __itn__ p;
            //# rifa.ForEach(f => {
            for (p.__f__ = 0; p.__f__ < count.__f__; p.__f__ += size.__f__)
                //# });
                t.Sub__ttn__(p, size).Set(this);
            //# } // d > 1
            return t;
        }

        //# } // dt == vt
        #endregion

        #region Manipulation Methods

        /// <summary>
        /// Apply the supplied function on the elements of the __ttnl__.
        /// The value of the elements is given to fun.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> Apply(Func<__vtn__, __vtn__> element_elementFun)
        {
            long i = FirstIndex;
            if (Info.JX == 1)
            {
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                for (long __a__e = i + Info.DS__f__, __a__j = Info.J__f__; i != __a__e; i += __a__j)
                //# });
                for (long xe = i + Info.SX; i != xe; i++)
                    //# if (dt != vt) {
                    Setter(Data, i, element_elementFun(Getter(Data, i)));
                    //# } else {
                    Data[i] = element_elementFun(Data[i]);
                    //# }
            }
            else
            {
                //# rifa.ForEach(riaa, (f, a) => {
                for (long __a__e = i + Info.DS__f__, __a__j = Info.J__f__; i != __a__e; i += __a__j)
                //# }); if (dt != vt) {
                    Setter(Data, i, element_elementFun(Getter(Data, i)));
                    //# } else {
                    Data[i] = element_elementFun(Data[i]);
                    //# }
            }
            return this;
        }

        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v); var t1i = tni(1, t1v); var t1i1 = tnin(1, t1v);
        /// <summary>
        /// Apply the supplied function to each pair of corresponding
        /// elements of the original __ttnl__ and the supplied __ttnl__.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> Apply<__t1t__>(
                __ttn__<__t1t__> t1, Func<__vtn__, T1, __vtn__> element_element1_elementFun)
        {
            //# LoopN("", false, dt == vt && !t1v, 2, 0, ix => { var t1ix = ix ? t1i1 : t1i;
                        //# if (dt == vt) {
                        Data[i] = element_element1_elementFun(__t0i__, __t1ix__);
                        //# } else {
                        Setter(Data, i, element_element1_elementFun(__t0i__, __t1ix__));
                        //# }
            //# });
            return this;
        }

        //# }); // t1v        
        //# if (d > 1) {
        /// <summary>
        /// Apply the supplied function on the elements of the __ttnl__.
        /// The value of the elements is given to fun.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> ApplyByCoord(Func<__vtn__, __itn__, __vtn__> element_crd_elementFun)
        {
            long i = FirstIndex; __itn__ vi;
            if (Info.JX == 1)
            {
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                vi.__f__ = First.__f__;
                for (long __a__e = i + Info.DS__f__, __a__j = Info.J__f__; i != __a__e; i += __a__j, vi.__f__++)
                {
                //# });
                vi.X = First.X;
                for (long xe = i + Info.SX; i != xe; i++, vi.X++)
                    //# if (dt == vt) {
                    Data[i] = element_crd_elementFun(Data[i], vi);
                    //# } else {
                    Setter(Data, i, element_crd_elementFun(Getter(Data, i), vi));
                    //# }
                //# for (int i = 0; i < d - 1; i++) {
                }
                //# }
            }
            else
            {
                //# rifa.ForEach(riaa, (f, a) => {
                vi.__f__ = First.__f__;
                for (long __a__e = i + Info.DS__f__, __a__j = Info.J__f__; i != __a__e; i += __a__j, vi.__f__++)
                {
                //# });
                    //# if (dt == vt) {
                    Data[i] = element_crd_elementFun(Data[i], vi);
                    //# } else {
                    Setter(Data, i, element_crd_elementFun(Getter(Data, i), vi));
                    //# }
                //# for (int i = 0; i < d; i++) {
                }
                //# }
            }
            return this;
        }

        //# } // d > 1
        /// <summary>
        /// Set each element to the value of a function of the element coords.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> ApplyByCoord(Func<__vtn__, /*# iaa.ForEach(a => { */long/*# }, comma); */, __vtn__> fun)
        {
            long i = FirstIndex;
            if (Info.JX == 1)
            {
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                for (long __a__e = i + Info.DS__f__, __a__j = Info.J__f__, __a__ = Info.F__f__; i != __a__e; i += __a__j, __a__++)
                //# });
                for (long xe = i + Info.SX, x = Info.FX; i != xe; i++, x++)
                    //# if (dt == vt) {
                    Data[i] = fun(Data[i], /*# iaa.ForEach(a => { */__a__/*# }, comma); */);
                    //# } else {
                    Setter(Data, i, fun(Getter(Data, i), /*# iaa.ForEach(a => { */__a__/*# }, comma); */));
                    //# }
            }
            else
            {
                //# rifa.ForEach(riaa, (f, a) => {
                for (long __a__e = i + Info.DS__f__, __a__j = Info.J__f__, __a__ = Info.F__f__; i != __a__e; i += __a__j, __a__++)
                //# }); if (dt == vt) {
                    Data[i] = fun(Data[i], /*# iaa.ForEach(a => { */__a__/*# }, comma); */);
                    //# } else {
                    Setter(Data, i, fun(Getter(Data, i), /*# iaa.ForEach(a => { */__a__/*# }, comma); */));
                    //# }
            }
            return this;
        }

        /// <summary>
        /// Sets all elements to the supplied value.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> Set(__vtn__ v)
        {
            //# Loop("", false, () => {
                    //# if (dt == vt) {
                    Data[i] = v;
                    //# } else {
                    Setter(Data, i, v);
                    //# }
            //# }, false, false);
            return this;
        }

        /// <summary>
        /// Set each element to the value of a function of the element coords.
        /// </summary>
        /// <returns>this</returns>
        //# { string fun = "elementFun"; riaa.ForEach(a => { fun = a + "_" + fun; });
        public __ttn__<__dvtn__> SetByCoord(Func</*# iaa.ForEach(a => { */long/*# }, comma); */, __vtn__> __fun__)
        {
            //# Loop("", false, () => {
                    //# if (dt == vt) {
                    Data[i] = __fun__(/*# iaa.ForEach(a => { */__a__/*# }, comma); */);
                    //# } else {
                    Setter(Data, i, __fun__(/*# iaa.ForEach(a => { */__a__/*# }, comma); */));
                    //# }
            //# }, true, false);
            return this;
        }
        //# } // fun

        //# if (d > 1) {
        /// <summary>
        /// Set each element to the value of a function of the element coords.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetByCoord(Func<__itn__, __vtn__> crd_elementFun)
        {
            //# Loop("", false, () => {
                    //# if (dt == vt) {
                    Data[i] = crd_elementFun(v);
                    //# } else {
                    Setter(Data, i, crd_elementFun(v));
                    //# }
            //# }, false, true);
            return this;
        }

        /// <summary>
        /// Set each element to the value of a function of the element coords.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetByCoord</*# r1iaa.ForEach(a => { */T__a__/*# }, comma); */>(
                //# r1iaa.ForEach((a, i) => {
                Func</*# riaa.Take(i+1).ForEach(fa => { */long/*# }, comma); */, /*# riaa.Take(i+1).ForEach(fa => { */T__fa__/*# }, comma); */> __a__Fun,
                //# });
                Func</*# iaa.ForEach(a => { */long/*# }, comma); */, /*# r1iaa.ForEach(a => { */T__a__/*# }, comma); */, __vtn__> fun)
        {
            long i = FirstIndex;
            if (Info.JX == 1)
            {
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                long __a__s = Info.DS__f__, __a__j = Info.J__f__;
                //# });
                long xs = Info.SX;
                //# int fi = 0;
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                for (long __a__e = i + __a__s, __a__ = Info.F__f__; i != __a__e; i += __a__j, __a__++) {
                    var __a__Val = __a__Fun(/*# riaa.Take(fi+1).ForEach(fa => { */__fa__/*# }, comma); riaa.Take(fi).ForEach(fa => { */, __fa__Val/*# }); */);
                //# fi++; });
                for (long xe = i + xs, x = Info.FX; i != xe; i++, x++) {
                    //# if (dt == vt) {
                    Data[i] = fun(/*# riaa.ForEach(a => { */__a__/*# }, comma); */, /*# r1iaa.ForEach(a => { */__a__Val/*# }, comma); */);
                    //# } else {
                    Setter(Data, i, fun(/*# riaa.ForEach(a => { */__a__/*# }, comma); */, /*# r1iaa.ForEach(a => { */__a__Val/*# }, comma); */));
                    //# }
                }/*# r1ifa.ForEach(f => { */ }/*# }); */
            }
            else
            {
                //# rifa.ForEach(riaa, (f, a) => {
                long __a__s = Info.DS__f__, __a__j = Info.J__f__;
                //# });
                //# fi = 0;
                //# r1ifa.ForEach(r1iaa, (f, a) => {
                for (long __a__e = i + __a__s, __a__ = Info.F__f__; i != __a__e; i += __a__j, __a__++) {
                    var __a__Val = __a__Fun(/*# riaa.Take(fi+1).ForEach(fa => { */__fa__/*# }, comma); riaa.Take(fi).ForEach(fa => { */, __fa__Val/*# }); */);
                //# fi++; });
                for (long xe = i + xs, x = Info.FX; i != xe; i += xj, x++) {
                    //# if (dt == vt) {
                    Data[i] = fun(/*# riaa.ForEach(a => { */__a__/*# }, comma); */, /*# r1iaa.ForEach(a => { */__a__Val/*# }, comma); */);
                    //# } else {
                    Setter(Data, i, fun(/*# riaa.ForEach(a => { */__a__/*# }, comma); */, /*# r1iaa.ForEach(a => { */__a__Val/*# }, comma); */));
                    //# }
                }/*# r1ifa.ForEach(f => { */ }/*# }); */
            }

            return this;
        }

        //# } // d > 1
        /// <summary>
        /// Set each element to the value of a function of the index of the
        /// elements of the supplied __ttnl__. The function may access the
        /// elements of the supplied __ttnl__ to compute the elements of the
        /// __ttnl__.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetByIndex(Func<long, __vtn__> index_elementFun)
        {
            //# Loop("", false, () => {
                    //# if (dt == vt) {
                    Data[i] = index_elementFun(i);
                    //# } else {
                    Setter(Data, i, index_elementFun(i));
                    //# }
            //# }, false, false);
            return this;
        }

        /// <summary>
        /// Set from a tensor that conforms to the corresponding tensor
        /// interface. Note, that this function checks if faster set
        /// operations are available and uses them if appropriate.
        /// </summary>
        /// <param name="it1"></param>
        /// <returns></returns>
        public __ttn__<__dvtn__> Set(I__ttn__<__vtn__> it1)
        {
            if (it1 is __ttn__<__vtn__>) return Set((__ttn__<__vtn__>)it1);
            //# Loop("", false, () => {
                    //# if (dt == vt) {
                    Data[i] = it1[/*# iaa.ForEach(a => { */__a__/*# }, comma); */];
                    //# } else {
                    Setter(Data, i, it1[/*# iaa.ForEach(a => { */__a__/*# }, comma); */]);
                    //# }
            //# }, true, false);
            return this;
        }

        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v); var t1i = tni(1, t1v); var t1i1 = tnin(1, t1v);
        /// <summary>
        /// Copy all elements from another __ttnl__.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> Set/*# if (t1v) {*/<T1>/*# }*/(__ttn__</*# if (t1v) {*/T1, /*# }*/__vtn__> t1)
        {
            //# LoopN("", false, dt == vt && !t1v, 2, 0, ix => { var t1ix = ix ? t1i1 : t1i;
                        //# if (dt == vt) {
                        Data[i] = __t1ix__;
                        //# } else {
                        Setter(Data, i, __t1ix__);
                        //# }
            //# });
            return this;
        }

        /// <summary>
        /// Set the elements of a __ttnl__ to the result of a function of
        /// the elements of the supplied __ttnl__.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetMap<__t1t__>(
                __ttn__<__t1t__> t1, Func<T1, __vtn__> element1_elementFun)
        {
            //# LoopN("", false, dt == vt && !t1v, 2, 0, ix => { var t1ix = ix ? t1i1 : t1i;
                        //# if (dt == vt) {
                        Data[i] = element1_elementFun(__t1ix__);
                        //# } else {
                        Setter(Data, i, element1_elementFun(__t1ix__));
                        //# }
            //# });
            return this;
        }

        /// <summary>
        /// Set each element to the value of a function of the index of the
        /// elements of the supplied __ttnl__. The function may access the
        /// elements of the supplied __ttnl__ to compute the elements of the
        /// __ttnl__.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetByIndex<__t1t__>(
                __ttn__<__t1t__> t1, Func<long, __vtn__> index1_elementFun)
        {
            //# LoopN("", false, dt == vt && !t1v, 2, 0, ix => { var i1 = ix ? "i1" : "i";
                        //# if (dt == vt) {
                        Data[i] = index1_elementFun(__i1__);
                        //# } else {
                        Setter(Data, i, index1_elementFun(__i1__));
                        //# }
            //# });
            return this;
        }

        //# }); // t1v
        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v); var t1i = tni(1, t1v); var t1i1 = tnin(1, t1v);
        //# bools.ForEach(t2v => { var t2t = tnt(2, t2v); var t2i = tni(2, t2v); var t2i2 = tnin(2, t2v);
        /// <summary>
        /// Set the elements of a __ttnl__ to the result of a function of
        /// corresponding pairs of elements of the two supplied tensors.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetMap2<__t1t__, __t2t__>(
                    __ttn__<__t1t__> t1, __ttn__<__t2t__> t2,
                    Func<T1, T2, __vtn__> element1_element2_elementFun)
        {
            //# LoopN("", false, dt == vt && !t1v && !t2v, 3, 0, ix => {
            //# var t1ix = ix ? t1i1 : t1i;
            //# var t2ix = ix ? t2i2 : t2i;
            //# if (dt == vt) {
            Data[i] = element1_element2_elementFun(__t1ix__, __t2ix__);
            //# } else {
            Setter(Data, i, element1_element2_elementFun(__t1ix__, __t2ix__));
            //# }
            //# });
            return this;
        }

        /// <summary>
        /// Set each element to the value of a function of the index of the
        /// elements of the supplied __ttnl__. The function may access the
        /// elements of the supplied __ttnl__ to compute the elements of the
        /// __ttnl__.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetByIndex<__t1t__, __t2t__>(
                __ttn__<__t1t__> t1, __ttn__<__t2t__> t2,
                Func<long, long, __vtn__> index1_index2_elementFun)
        {
            //# LoopN("", false, dt == vt && !t1v && !t2v, 3, 0, ix => {
                        //# var i1 = ix ? "i1" : "i";
                        //# var i2 = ix ? "i2" : "i";
                        //# if (dt == vt) {
                        Data[i] = index1_index2_elementFun(__i1__, __i2__);
                        //# } else {
                        Setter(Data, i, index1_index2_elementFun(__i1__, __i2__));
                        //# }
            //# });
            return this;
        }

        //# }); }); // t2v, t1v
        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v); var t1i = tni(1, t1v); var t1i1 = tnin(1, t1v);
        //# bools.ForEach(t2v => { var t2t = tnt(2, t2v); var t2i = tni(2, t2v); var t2i2 = tnin(2, t2v);
        //# bools.ForEach(t3v => { var t3t = tnt(3, t3v); var t3i = tni(3, t3v); var t3i3 = tnin(3, t3v);
        /// <summary>
        /// Set the elements of a tensor to the result of a function of
        /// corresponding triples of elements of the three supplied tensors.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetMap3<__t1t__, __t2t__, __t3t__>(
                __ttn__<__t1t__> t1, __ttn__<__t2t__> t2, __ttn__<__t3t__> t3,
                Func<T1, T2, T3, __vtn__> element1_element2_element3_elementFun)
        {
            //# LoopN("", false, dt == vt && !t1v && !t2v && !t3v, 4, 0, ix => {
            //# var t1ix = ix ? t1i1 : t1i;
            //# var t2ix = ix ? t2i2 : t2i;
            //# var t3ix = ix ? t3i3 : t3i;
            //# if (dt == vt) {
            Data[i] = element1_element2_element3_elementFun(__t1ix__, __t2ix__, __t3ix__);
            //# } else {
            Setter(Data, i, element1_element2_element3_elementFun(__t1ix__, __t2ix__, __t3ix__));
            //# }
            //# });
            return this;
        }

        /// <summary>
        /// Set each element to the value of a function of the index of the
        /// elements of the supplied __ttnl__. The function may access the
        /// elements of the supplied __ttnl__ to compute the elements of the
        /// __ttnl__.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetByIndex<__t1t__, __t2t__, __t3t__>(
                __ttn__<__t1t__> t1, __ttn__<__t2t__> t2, __ttn__<__t3t__> t3,
                Func<long, long, long, __vtn__> index1_index2_index3_elementFun)
        {
            //# LoopN("", false, dt == vt && !t1v && !t2v && !t3v, 4, 0, ix => {
                        //# var i1 = ix ? "i1" : "i";
                        //# var i2 = ix ? "i2" : "i";
                        //# var i3 = ix ? "i3" : "i";
                        //# if (dt == vt) {
                        Data[i] = index1_index2_index3_elementFun(__i1__, __i2__, __i3__);
                        //# } else {
                        Setter(Data, i, index1_index2_index3_elementFun(__i1__, __i2__, __i3__));
                        //# }
            //# });
            return this;
        }

        //# }); }); }); // t3v, t2v, t1v
        /// <summary>
        /// Set the __ttnl__ to be the convolution of the supplied image and filter tensors.
        /// </summary>
        public __ttn__<__dvtn__> SetConvolution<Ti, Tf, Tm, Ts>(
                __ttn__<Ti> image, __ttn__<Tf> filter, Func<Ti, Tf, Tm> mulFun,
                Ts bias, Func<Ts, Tm, Ts> sumFun, Func<Ts, __vtn__> castFun)
        {
            Info.CheckMatchingSize(new __ttn__Info(__itone__ + image.S - filter.S));
            var r = this; r.F = image.F;
            r.SetByCoord(c => castFun(image.Sub__ttn__(c, filter.Info.S)
                                        .InnerProduct(filter, mulFun, bias, sumFun)));
            return this;
        }

        //# if (d == 2) {
        //# bools.ForEach(txv => { var txt = tdt(1, txv); var txi = tdi(1, txv); var txi1 = tdin(1, txv);
        //# bools.ForEach(tyv => { var tyt = tdt(2, tyv); var tyi = tdi(2, tyv); var tyi2 = tdin(2, tyv);
        /// <summary>
        /// Set the matrix to be the outer product of the two supplied vectors.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetOuterProduct<__txt__, __tyt__>(
                    __vectn__<__txt__> tx, __vectn__<__tyt__> ty,
                    Func<Tx, Ty, __vtn__> fun)
        {
            long i = FirstIndex;
            if (Info.JX == 1 && tx.Info.JX == 1 && ty.Info.JX == 1)
            {
                long ys = Info.DSY, yj = Info.JY;
                long xs = Info.SX;
                for (long ye = i + ys, i2 = ty.FirstIndex; i != ye; i += yj, i2++) {
                for (long xe = i + xs, i1 = tx.FirstIndex; i != xe; i++, i1++) {
                    //# if (dt != vt) {
                    Setter(Data, i, fun(__txi1__, __tyi2__));
                    //# } else {
                    Data[i] = fun(__txi1__, __tyi2__);
                    //# }
                } }
            }
            else
            {
                long ys = Info.DSY, yj = Info.JY, j2 = ty.Info.JX;
                long xs = Info.DSX, xj = Info.JX, j1 = tx.Info.JX;
                for (long ye = i + ys, i2 = ty.FirstIndex; i != ye; i += yj, i2 += j2) {
                for (long xe = i + xs, i1 = tx.FirstIndex; i != xe; i += xj, i1 += j1) {
                    //# if (dt != vt) {
                    Setter(Data, i, fun(__txi1__, __tyi2__));
                    //# } else {
                    Data[i] = fun(__txi1__, __tyi2__);
                    //# }
                } }
            }
            return this;
        }

        //# }); });
        //# } // d == 2
        //# if (d == 3) {
        //# bools.ForEach(txv => { var txt = tdt(1, txv); var txi = tdi(1, txv); var txi1 = tdin(1, txv);
        //# bools.ForEach(tyv => { var tyt = tdt(2, tyv); var tyi = tdi(2, tyv); var tyi2 = tdin(2, tyv);
        //# bools.ForEach(tzv => { var tzt = tdt(3, tzv); var tzi = tdi(3, tzv); var tzi3 = tdin(3, tzv);
        /// <summary>
        /// Set the volume to be the outer product of the three supplied vectors.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetOuterProduct<__txt__, __tyt__, __tzt__>(
                    __vectn__<__txt__> tx, __vectn__<__tyt__> ty, __vectn__<__tzt__> tz,
                    Func<Tx, Ty, Tz, __vtn__> fun)
        {
            long i = FirstIndex;
            if (Info.JX == 1 && tx.Info.JX == 1 && ty.Info.JX == 1 && tz.Info.JX == 1)
            {
                long zs = Info.DSZ, zj = Info.JZ;
                long ys = Info.DSY, yj = Info.JY;
                long xs = Info.SX;
                for (long ze = i + zs, i3 = tz.FirstIndex; i != ze; i += zj, i3++) {
                for (long ye = i + ys, i2 = ty.FirstIndex; i != ye; i += yj, i2++) {
                for (long xe = i + xs, i1 = tx.FirstIndex; i != xe; i++, i1++) {
                    //# if (dt != vt) {
                    Setter(Data, i, fun(__txi1__, __tyi2__, __tzi3__));
                    //# } else {
                    Data[i] = fun(__txi1__, __tyi2__, __tzi3__);
                    //# }
                } } }
            }
            else
            {
                long zs = Info.DSZ, zj = Info.JZ, j3 = tz.Info.JX;
                long ys = Info.DSY, yj = Info.JY, j2 = ty.Info.JX;
                long xs = Info.DSX, xj = Info.JX, j1 = tx.Info.JX;
                for (long ze = i + zs, i3 = tz.FirstIndex; i != ze; i += zj, i3 += j3) {
                for (long ye = i + ys, i2 = ty.FirstIndex; i != ye; i += yj, i2 += j2) {
                for (long xe = i + xs, i1 = tx.FirstIndex; i != xe; i += xj, i1 += j1) {
                    //# if (dt != vt) {
                    Setter(Data, i, fun(__txi1__, __tyi2__, __tzi3__));
                    //# } else {
                    Data[i] = fun(__txi1__, __tyi2__, __tzi3__);
                    //# }
                } } }
            }
            return this;
        }

        //# }); }); });
        //# } // d == 3
        //# if (d == 4) {
        //# bools.ForEach(txv => { var txt = tdt(1, txv); var txi = tdi(1, txv); var txi1 = tdin(1, txv);
        //# bools.ForEach(tyv => { var tyt = tdt(2, tyv); var tyi = tdi(2, tyv); var tyi2 = tdin(2, tyv);
        //# bools.ForEach(tzv => { var tzt = tdt(3, tzv); var tzi = tdi(3, tzv); var tzi3 = tdin(3, tzv);
        //# bools.ForEach(twv => { var twt = tdt(4, twv); var twi = tdi(4, twv); var twi4 = tdin(4, twv);
        /// <summary>
        /// Set the four-dimensional tensor to be the outer product of the
        /// four supplied vectors.
        /// </summary>
        /// <returns>this</returns>
        public __ttn__<__dvtn__> SetOuterProduct<__txt__, __tyt__, __tzt__, __twt__>(
                    __vectn__<__txt__> tx, __vectn__<__tyt__> ty, __vectn__<__tzt__> tz, __vectn__<__twt__> tw,
                    Func<Tx, Ty, Tz, Tw, __vtn__> fun)
        {
            long i = FirstIndex;
            if (Info.JX == 1 && tx.Info.JX == 1 && ty.Info.JX == 1 && tz.Info.JX == 1 && tw.Info.JX == 1)
            {
                long ws = Info.DSW, wj = Info.JW;
                long zs = Info.DSZ, zj = Info.JZ;
                long ys = Info.DSY, yj = Info.JY;
                long xs = Info.SX;
                for (long we = i + ws, i4 = tw.FirstIndex; i != we; i += wj, i4++) {
                for (long ze = i + zs, i3 = tz.FirstIndex; i != ze; i += zj, i3++) {
                for (long ye = i + ys, i2 = ty.FirstIndex; i != ye; i += yj, i2++) {
                for (long xe = i + xs, i1 = tx.FirstIndex; i != xe; i++, i1++) {
                    //# if (dt != vt) {
                    Setter(Data, i, fun(__txi1__, __tyi2__, __tzi3__, __twi4__));
                    //# } else {
                    Data[i] = fun(__txi1__, __tyi2__, __tzi3__, __twi4__);
                    //# }
                } } } }
            }
            else
            {
                long ws = Info.DSW, wj = Info.JW, j4 = tw.Info.JX;
                long zs = Info.DSZ, zj = Info.JZ, j3 = tz.Info.JX;
                long ys = Info.DSY, yj = Info.JY, j2 = ty.Info.JX;
                long xs = Info.DSX, xj = Info.JX, j1 = tx.Info.JX;
                for (long we = i + ws, i4 = tw.FirstIndex; i != we; i += wj, i4 += j4) {
                for (long ze = i + zs, i3 = tz.FirstIndex; i != ze; i += zj, i3 += j3) {
                for (long ye = i + ys, i2 = ty.FirstIndex; i != ye; i += yj, i2 += j2) {
                for (long xe = i + xs, i1 = tx.FirstIndex; i != xe; i += xj, i1 += j1) {
                    //# if (dt != vt) {
                    Setter(Data, i, fun(__txi1__, __tyi2__, __tzi3__, __twi4__));
                    //# } else {
                    Data[i] = fun(__txi1__, __tyi2__, __tzi3__, __twi4__);
                    //# }
                } } } }
            }
            return this;
        }

        //# }); }); }); });
        //# } // d == 4
        #endregion

        #region Scalar Methods and Functions

        //# foreach (var hasBreak in new[] { false, true }) {
        public Tr Norm<Tr, Ti>(
                Func<__vtn__, Ti> elementFun, Tr bias, Func<Tr, Ti, Tr> sumFun/*# if (hasBreak) { */, Func<Tr, bool> breakIfTrueFun/*# } */)
        {
            Tr result = bias;
            //# Loop("", false, () => {
                    result = sumFun(result, elementFun(__t0i__));
                    //# if (hasBreak) {
                    if (breakIfTrueFun(result)) return result;
                    //# }
            //# }, false, false);
            return result;
        }

        public Tr Norm<Tr, Ti>(Func<__vtn__, /*# iaa.ForEach(a => {*/long/*# }, comma);*/, Ti> elementFun,
                Tr bias, Func<Tr, Ti, Tr> sumFun/*# if (hasBreak) { */, Func<Tr, bool> breakIfTrueFun/*# } */)
        {
            Tr result = bias;
            //# Loop("", false, () => {
                    result = sumFun(result, elementFun(__t0i__, /*# iaa.ForEach(a => {*/__a__/*# }, comma);*/));
                    //# if (hasBreak) {
                    if (breakIfTrueFun(result)) return result;
                    //# }
            //# }, true, false);
            return result;
        }

        //# if (d > 1) {
        public Tr Norm<Tr, Ti>(Func<__vtn__, __itn__, Ti> elementFun,
                Tr bias, Func<Tr, Ti, Tr> sumFun/*# if (hasBreak) { */, Func<Tr, bool> breakIfTrueFun/*# } */)
        {
            Tr result = bias;
            //# Loop("", false, () => {
                    result = sumFun(result, elementFun(__t0i__, v));
                    //# if (hasBreak) {
                    if (breakIfTrueFun(result)) return result;
                    //# }
            //# }, false, true);
            return result;
        }

        //# }
        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v); var t1i = tni(1, t1v); var t1i1 = tnin(1, t1v);
        public Ts InnerProduct<__t1t__, Tm, Ts>(
            __ttn__<__t1t__> t1,
            Func<__vtn__, T1, Tm> mulFun, Ts bias, Func<Ts, Tm, Ts> sumFun/*# if (hasBreak) { */, Func<Ts, bool> breakIfTrueFun/*# } */)
        {
            Ts result = bias;
            //# LoopN("", false, dt == vt && !t1v, 2, 0, ix => { var t1ix = ix ? t1i1 : t1i;
                        result = sumFun(result, mulFun(__t0i__, __t1ix__));
                        //# if (hasBreak) {
                        if (breakIfTrueFun(result)) return result;
                        //# }
            //# });
            return result;
        }

        //# }); // t1v
        //# } // hasBreak
        #endregion

        #region Creator Functions

        //# bools.ForEach(t1v => {
        /// <summary>
        /// Create a new __ttnl__ as copy of the supplied __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> Create/*# if (t1v) { */<T1d>/*# } */(
                __ttn__</*# if (t1v) { */T1d, /*# } */__vtn__> t1)
        {
            return new __ttn__<__dvtn__>(t1.Info.Size).Set(t1);
        }

        //# }); // t1v
        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v);
        /// <summary>
        /// Create a new __ttnl__ by applying a function on each element of
        /// the supplied __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> Map<__t1t__>(
                __ttn__<__t1t__> t1, Func<T1, __vtn__> element1_elementFun)
        {
            return new __ttn__<__dvtn__>(t1.Info.Size).SetMap(t1, element1_elementFun);
        }

        /// <summary>
        /// Create a new __ttnl__ by applying a function to each index of the
        /// elements of the supplied __ttnl__. The function may access the
        /// elements of the supplied __ttnl__ to compute the elements of the new
        /// __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> CreateByIndex<__t1t__>(
                __ttn__<__t1t__> t1, Func<long, __vtn__> index1_elementFun)
        {
            return new __ttn__<__dvtn__>(t1.Info.Size).SetByIndex(t1, index1_elementFun);
        }

        //# }); // t1v
        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v);
        //# bools.ForEach(t2v => { var t2t = tnt(2, t2v);
        /// <summary>
        /// Create a new __ttnl__ by applying a function on each element of
        /// the supplied __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> Map2<__t1t__, __t2t__>(
                __ttn__<__t1t__> t1, __ttn__<__t2t__> t2,
                Func<T1, T2, __vtn__> element1_element2_elementFun)
        {
            return new __ttn__<__dvtn__>(t1.Info.Size).SetMap2(t1, t2, element1_element2_elementFun);
        }

        /// <summary>
        /// Create a new __ttnl__ by applying a function to each index of the
        /// elements of the supplied __ttnl__. The function may access the
        /// elements of the supplied __ttnl__ to compute the elements of the new
        /// __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> CreateByIndex<__t1t__, __t2t__>(
                __ttn__<__t1t__> t1, __ttn__<__t2t__> t2,
                Func<long, long, __vtn__> index1_index2_fun)
        {
            return new __ttn__<__dvtn__>(t1.Info.Size).SetByIndex(t1, t2, index1_index2_fun);
        }

        //# }); }); // t2v, t1v
        //# bools.ForEach(t1v => { var t1t = tnt(1, t1v);
        //# bools.ForEach(t2v => { var t2t = tnt(2, t2v);
        //# bools.ForEach(t3v => { var t3t = tnt(3, t3v);
        /// <summary>
        /// Create a new __ttnl__ by applying a function on each element of
        /// the supplied __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> Map3<__t1t__, __t2t__, __t3t__>(
                __ttn__<__t1t__> t1, __ttn__<__t2t__> t2, __ttn__<__t3t__> t3,
                Func<T1, T2, T3, __vtn__> element1_element2_element3_elementFun)
        {
            return new __ttn__<__dvtn__>(t1.Info.Size).SetMap3(t1, t2, t3, element1_element2_element3_elementFun);
        }

        /// <summary>
        /// Create a new __ttnl__ by applying a function to each index of the
        /// elements of the supplied __ttnl__. The function may access the
        /// elements of the supplied __ttnl__ to compute the elements of the new
        /// __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> CreateByIndex<__t1t__, __t2t__, __t3t__>(
                __ttn__<__t1t__> t1, __ttn__<__t2t__> t2, __ttn__<__t3t__> t3,
                Func<long, long, long, __vtn__> index1_index2_index3_elementFun)
        {
            return new __ttn__<__dvtn__>(t1.Info.Size).SetByIndex(t1, t2, t3, index1_index2_index3_elementFun);
        }

        //# }); }); }); // t3v, t2v, t1v
        /// <summary>
        /// Create a new __ttnl__ by applying a function to the coordinates of
        /// the elements the newly created __ttnl__.
        /// </summary>
        public static __ttn__<__dvtn__> Create(__itn__ size, Func<__itn__, __vtn__> fun)
        {
            return new __ttn__<__dvtn__>(size).SetByCoord(fun);
        }

        /// <summary>
        /// Create a new __ttn__ by convolving the image with the filter. The mulFun
        /// is used to combine single elements of the image with the filter. The result
        /// is summed up using the sumFun starting with bias. The complete sum is put
        /// through the castFun and put into the resulting __ttn__.
        /// </summary>
        public static __ttn__<__dvtn__> CreateConvolution<Ti, Tf, Tm, Ts>(
                __ttn__<Ti> image, __ttn__<Tf> filter, Func<Ti, Tf, Tm> mulFun,
                Ts bias, Func<Ts, Tm, Ts> sumFun, Func<Ts, __vtn__> castFun)
        {
            __itn__ size = 1 + image.S - filter.S;
            if (/*# r1ifa.ForEach(f => { */size.__f__ <= 0 ||/*# });*/size/*# if (d > 1) {*/.X/*# }*/ <= 0) throw new Exception("filter to large");
            return new __ttn__<__dvtn__>(size)
                    .SetConvolution(image, filter, mulFun, bias, sumFun, castFun);
        }

        //# if (d == 2) {
        //# bools.ForEach(txv => { var txt = tdt(1, txv);
        //# bools.ForEach(tyv => { var tyt = tdt(2, tyv);
        /// <summary>
        /// Create a new matrix as the outer product of the two supplied vectors.
        /// </summary>
        public static __ttn__<__dvtn__> CreateOuterProduct<__txt__, __tyt__>(
                    __vectn__<__txt__> tx, __vectn__<__tyt__> ty,
                    Func<Tx, Ty, __vtn__> fun)
        {
            return new __ttn__<__dvtn__>(new V2l(tx.S, ty.S))
                            { FX = tx.F, FY = ty.F }
                            .SetOuterProduct<__txt__, __tyt__>(tx, ty, fun);
        }

        //# }); });
        //# } // d == 2 
        //# if (d == 3) {
        //# bools.ForEach(txv => { var txt = tdt(1, txv);
        //# bools.ForEach(tyv => { var tyt = tdt(2, tyv);
        //# bools.ForEach(tzv => { var tzt = tdt(3, tzv);
        /// <summary>
        /// Create a new volume as the outer product of the three supplied vectors.
        /// </summary>
        public __ttn__<__dvtn__> CreateOuterProduct<__txt__, __tyt__, __tzt__>(
                    __vectn__<__txt__> tx, __vectn__<__tyt__> ty, __vectn__<__tzt__> tz,
                    Func<Tx, Ty, Tz, __vtn__> fun)
        {
            return new __ttn__<__dvtn__>(new V3l(tx.S, ty.S, tz.S))
                            { FX = tx.F, FY = ty.F, FZ = tz.F }
                            .SetOuterProduct<__txt__, __tyt__, __tzt__>(tx, ty, tz, fun);
        }

        //# }); }); });
        //# } // d == 3
        //# if (d == 4) {
        //# bools.ForEach(txv => { var txt = tdt(1, txv);
        //# bools.ForEach(tyv => { var tyt = tdt(2, tyv);
        //# bools.ForEach(tzv => { var tzt = tdt(3, tzv);
        //# bools.ForEach(twv => { var twt = tdt(4, twv);
        /// <summary>
        /// Create a new four-dimensional tensor as the outer product of the
        /// four supplied vectors.
        /// </summary>
        public __ttn__<__dvtn__> CreateOuterProduct<__txt__, __tyt__, __tzt__, __twt__>(
                    __vectn__<__txt__> tx, __vectn__<__tyt__> ty, __vectn__<__tzt__> tz, __vectn__<__twt__> tw,
                    Func<Tx, Ty, Tz, Tw, __vtn__> fun)
        {
            return new __ttn__<__dvtn__>(new V4l(tx.S, ty.S, tz.S, tw.S))
                            { FX = tx.F, FY = ty.F, FZ = tz.F, FW = tw.F }
                            .SetOuterProduct<__txt__, __tyt__, __tzt__, __twt__>(tx, ty, tz, tw, fun);
        }

        //# }); }); }); });
        //# } // d == 4
        #endregion

        #region Sampling

        //# if (d == 1) {
        /// <summary>
        /// Sample the vector using 2 samples and the supplied
        /// interpolation function. If an integer coordinte is
        /// supplied the returned value is the same as the indexer
        /// of the vector. No bounds checking is performed.
        /// </summary>
        public TRes SampleRaw2<TRes>(
                double x,
                Func<double, __vtn__, __vtn__, TRes> ipl)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; long d = Info.Delta;
            long i = Info.Origin + xi * d;
            //# if (dt == vt) {
            return ipl(x - xid, Data[i], Data[i + d]);
            //# } else {
            return ipl(x - xid, Getter(Data, i), Getter(Data, i + d));
            //# }
        }

        public TRes Sample2Clamped<TRes>(
                double x,
                Func<double, __vtn__, __vtn__, TRes> ipl)
        {
            return Sample2(x, ipl, Tensor.Index2SamplesClamped);
        }

        /// <summary>
        /// Sample the vector using 2 samples and the supplied
        /// interpolation function. If an integer coordinte is
        /// supplied the returned value is the same as the indexer
        /// of the vector.
        /// </summary>
        public TRes Sample2<TRes>(
                double x,
                Func<double, __vtn__, __vtn__, TRes> ipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_indexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long d1 = Info.Delta;
            long i = Info.Origin + xi * d1;
            var d = index_min_max_delta_indexFun(xi, FX, EX, d1);
            //# if (dt == vt) {
            return ipl(xf, Data[i + d.E0], Data[i + d.E1]);
            //# } else {
            return ipl(xf, Getter(Data, i + d.E0), Getter(Data, i + d.E1));
            //# }
        }

        public TRes SampleRaw4<T1, TRes>(
                double x,
                Func<double, Tup4<T1>> ipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, TRes> smp)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long d = Info.Delta;
            long i1 = Info.Origin + xi * d, i2 = i1 + d;
            var w = ipl(xf);
            //# if (dt == vt) {
            return smp(Data[i1 - d], Data[i1], Data[i2], Data[i2 + d], ref w);
            //# } else {
            return smp(Getter(Data, i1 - d), Getter(Data, i1), Getter(Data, i2), Getter(Data, i2 + d), ref w);
            //# }
        }

        public TRes Sample4Clamped<T1, TRes>(
                double x,
                Func<double, Tup4<T1>> ipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, TRes> smp)
        {
            return Sample4(x, ipl, smp, Tensor.Index4SamplesClamped);
        }

        public TRes Sample4<T1, TRes>(
                double x,
                Func<double, Tup4<T1>> ipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, TRes> smp,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_indexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long d1 = Info.Delta;
            var d = index_min_max_delta_indexFun(xi, FX, EX, d1);
            long i = Info.Origin + xi * d1;
            var w = ipl(xf);
            //# if (dt == vt) {
            return smp(Data[i + d.E0], Data[i + d.E1], Data[i + d.E2], Data[i + d.E3], ref w);
            //# } else {
            return smp(Getter(Data, i + d.E0), Getter(Data, i + d.E1), Getter(Data, i + d.E2), Getter(Data, i + d.E3), ref w);
            //# }
        }

        public TRes SampleRaw6<T1, TRes>(
                double x,
                Func<double, Tup6<T1>> ipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, TRes> smp)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long d = Info.Delta;
            long i2 = Info.Origin + xi * d, i1 = i2 - d, i3 = i2 + d, i4 = i3 + d;
            var w = ipl(xf);
            //# if (dt == vt) {
            return smp(Data[i1 - d], Data[i1], Data[i2], Data[i3], Data[i4], Data[i4 + d], ref w);
            //# } else {
            return smp(Getter(Data, i1 - d), Getter(Data, i1), Getter(Data, i2), Getter(Data, i3), Getter(Data, i4), Getter(Data, i4 + d), ref w);
            //# }
        }

        public TRes Sample6Clamped<T1, TRes>(
                double x,
                Func<double, Tup6<T1>> ipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, TRes> smp)
        {
            return Sample6(x, ipl, smp, Tensor.Index6SamplesClamped);
        }

        public TRes Sample6<T1, TRes>(
                double x,
                Func<double, Tup6<T1>> ipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, TRes> smp,
                Func<long, long, long, long, Tup6<long>> index_min_max_delta_indexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long d1 = Info.Delta;
            var d = index_min_max_delta_indexFun(xi, FX, EX, d1);
            long i = Info.Origin + xi * d1;
            var w = ipl(xf);
            //# if (dt == vt) {
            return smp(Data[i + d.E0], Data[i + d.E1], Data[i + d.E2], Data[i + d.E3], Data[i + d.E4], Data[i + d.E5], ref w);
            //# } else {
            return smp(Getter(Data, i + d.E0), Getter(Data, i + d.E1), Getter(Data, i + d.E2), Getter(Data, i + d.E3), Getter(Data, i + d.E4), Getter(Data, i + d.E5), ref w);
            //# }
        }

        //# }
        //# if (d == 2) {
        public TRes SampleRaw4<T1, TRes>(
                V2d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, TRes> yipl)
        {
            return SampleRaw4(v.X, v.Y, xipl, yipl);
        }

        /// <summary>
        /// Sample the matrix using 4 samples and the supplied
        /// interpolation function. If only integer coordintes are
        /// supplied the returned value is the same as the indexer
        /// of the matrix, i.e. pixel centers are assumed to be on
        /// integer coordinates. No bounds checking is performed.
        /// </summary>
        public TRes SampleRaw4<T1, TRes>(
                double x, double y,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, TRes> yipl)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy = Info.Delta.Y;
            long i0 = Info.Origin + xi * dx + yi * dy, i1 = i0 + dy;
            //# if (dt == vt) {
            return yipl(yf, xipl(xf, Data[i0], Data[i0 + dx]),
                            xipl(xf, Data[i1], Data[i1 + dx]));
            //# } else {
            return yipl(yf, xipl(xf, Getter(Data, i0), Getter(Data, i0 + dx)),
                            xipl(xf, Getter(Data, i1), Getter(Data, i1 + dx)));
            //# }
        }

        public TRes Sample4Clamped<T1, TRes>(
                V2d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, TRes> yipl)
        {
            return Sample4(v.X, v.Y, xipl, yipl,
                           Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        public TRes Sample4Clamped<T1, TRes>(
                double x, double y,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, TRes> yipl)
        {
            return Sample4(x, y, xipl, yipl,
                           Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        public TRes Sample4<T1, TRes>(
                V2d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, TRes> yipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_yIndexFun)
        {
            return Sample4(v.X, v.Y, xipl, yipl,
                           index_min_max_delta_xIndexFun,
                           index_min_max_delta_yIndexFun);
        }

        /// <summary>
        /// Sample the matrix using 4 samples and the supplied
        /// interpolation functions., If only integer coordintes are
        /// supplied the returned value is the same as the indexer
        /// of the matrix, i.e. pixel centers are assumed to be on
        /// integer coordinates.
        /// </summary>
        public TRes Sample4<T1, TRes>(
                double x, double y,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, TRes> yipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_yIndexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx1 = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy1 = Info.Delta.Y;
            long i = Info.Origin + xi * dx1 + yi * dy1;
            var dx = index_min_max_delta_xIndexFun(xi, FX, EX, dx1);
            dx.E0 += i; dx.E1 += i;
            var dy = index_min_max_delta_yIndexFun(yi, FY, EY, dy1);
            //# if (dt == vt) {
            return yipl(yf, xipl(xf, Data[dx.E0 + dy.E0], Data[dx.E1 + dy.E0]),
                            xipl(xf, Data[dx.E0 + dy.E1], Data[dx.E1 + dy.E1]));
            //# } else {
            return yipl(yf, xipl(xf, Getter(Data, dx.E0 + dy.E0), Getter(Data, dx.E1 + dy.E0)),
                            xipl(xf, Getter(Data, dx.E0 + dy.E1), Getter(Data, dx.E1 + dy.E1)));
            //# }
        }

        //# foreach (var clampFun in new[] { false, true }) {
        public void SetScaled4<T1/*# if (clampFun) { */, T2/*# } */>(Matrix<__dvtn__> sourceMat,
                double xScale, double yScale, double xShift, double yShift,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1/*# if (clampFun) { */, T2/*# }
                                                         else { */, __vtn__/*# } */> yipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_yIndexFun/*#
                if (clampFun) { */,
                Func<T2, __vtn__> clampFun/*# } */)
        {
            var dxa = new Tup2<long>[SX];
            var xfa = new double[SX];
            long fx = sourceMat.FX, ex = sourceMat.EX, dx1 = sourceMat.DX;
            for (long tix = 0, tsx = SX; tix < tsx; tix++, xShift += xScale)
            {
                double xid = Fun.Floor(xShift); long xi = (long)xid; double xf = xShift - xid;
                var dx = index_min_max_delta_xIndexFun(xi, fx, ex, dx1);
                var dxi = xi * dx1; dx.E0 += dxi; dx.E1 += dxi;
                dxa[tix] = dx; xfa[tix] = xf;
            }
            var dya = new Tup2<long>[SY];
            var yfa = new double[SY];
            long o = sourceMat.Origin, fy = sourceMat.FY, ey = sourceMat.EY, dy1 = sourceMat.DY;
            for (long tiy = 0, tsy = SY; tiy < tsy; tiy++, yShift += yScale)
            {
                double yid = Fun.Floor(yShift); long yi = (long)yid; double yf = yShift - yid;
                var dy = index_min_max_delta_yIndexFun(yi, fy, ey, dy1);
                var dyi = o + yi * dy1; dy.E0 += dyi; dy.E1 += dyi;
                dya[tiy] = dy; yfa[tiy] = yf;
            }
            var data = Data;
            //# Loop("", false, () => {
                //# if (dt == vt) {
                    //# if (clampFun) {
                    data[i] = clampFun(sourceMat.Sample4(dxa[x], dya[y], xfa[x], yfa[y], xipl, yipl));
                    //# } else {
                    data[i] = sourceMat.Sample4(dxa[x], dya[y], xfa[x], yfa[y], xipl, yipl);
                    //# }
                //# } else {
                    //# if (clampFun) {
                    Setter(data, i, clampFun(sourceMat.Sample4(dxa[x], dya[y], xfa[x], yfa[y], xipl, yipl)));
                    //# } else {
                    Setter(data, i, sourceMat.Sample4(dxa[x], dya[y], xfa[x], yfa[y], xipl, yipl));
                    //# }
                //# }
            //# }, true, false);
        }

        //# } // clampFun
        public TRes Sample4<T1, TRes>(
                Tup2<long> dx, Tup2<long> dy, double xf, double yf,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, TRes> yipl)
        {
            //# if (dt == vt) {
            return yipl(yf, xipl(xf, Data[dx.E0 + dy.E0], Data[dx.E1 + dy.E0]),
                            xipl(xf, Data[dx.E0 + dy.E1], Data[dx.E1 + dy.E1]));
            //# } else {
            return yipl(yf, xipl(xf, Getter(Data, dx.E0 + dy.E0), Getter(Data, dx.E1 + dy.E0)),
                            xipl(xf, Getter(Data, dx.E0 + dy.E1), Getter(Data, dx.E1 + dy.E1)));
            //# }
        }

        public TRes SampleRaw16<T1, T2, T3, TRes>(
                V2d v,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, TRes> ysmp)
        {
            return SampleRaw16(v.X, v.Y, xipl, yipl, xsmp, ysmp);
        }
   
        public TRes SampleRaw16<T1, T2, T3, TRes>(
                double x, double y,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, TRes> ysmp)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy = Info.Delta.Y;
            long d2 = dx + dx;
            long i1 = Info.Origin + xi * dx + yi * dy, i0 = i1 - dy, i2 = i1 + dy, i3 = i2 + dy;
            var wx = xipl(xf); var wy = yipl(yf);
            //# if (dt == vt) {
            return ysmp(xsmp(Data[i0 - dx], Data[i0], Data[i0 + dx], Data[i0 + d2], ref wx),
                        xsmp(Data[i1 - dx], Data[i1], Data[i1 + dx], Data[i1 + d2], ref wx),
                        xsmp(Data[i2 - dx], Data[i2], Data[i2 + dx], Data[i2 + d2], ref wx),
                        xsmp(Data[i3 - dx], Data[i3], Data[i3 + dx], Data[i3 + d2], ref wx), ref wy);
            //# } else {
            return ysmp(xsmp(Getter(Data, i0 - dx), Getter(Data, i0), Getter(Data, i0 + dx), Getter(Data, i0 + d2), ref wx),
                        xsmp(Getter(Data, i1 - dx), Getter(Data, i1), Getter(Data, i1 + dx), Getter(Data, i1 + d2), ref wx),
                        xsmp(Getter(Data, i2 - dx), Getter(Data, i2), Getter(Data, i2 + dx), Getter(Data, i2 + d2), ref wx),
                        xsmp(Getter(Data, i3 - dx), Getter(Data, i3), Getter(Data, i3 + dx), Getter(Data, i3 + d2), ref wx), ref wy);
            //# }
        }

        public TRes Sample16Clamped<T1, T2, T3, TRes>(
                V2d v,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, TRes> ysmp)
        {
            return Sample16(v.X, v.Y, xipl, yipl, xsmp, ysmp,
                            Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public TRes Sample16Clamped<T1, T2, T3, TRes>(
                double x, double y,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, TRes> ysmp)
        {
            return Sample16(x, y, xipl, yipl, xsmp, ysmp,
                            Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        public TRes Sample16<T1, T2, T3, TRes>(
                V2d v,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, TRes> ysmp,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_yIndexFun)
        {
            return Sample16(v.X, v.Y, xipl, yipl, xsmp, ysmp,
                            index_min_max_delta_xIndexFun,
                            index_min_max_delta_yIndexFun);
        }
        
        public TRes Sample16<T1, T2, T3, TRes>(
                double x, double y,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, TRes> ysmp,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_yIndexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx1 = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy1 = Info.Delta.Y;
            long i = Info.Origin + xi * dx1 + yi * dy1;
            var dx = index_min_max_delta_xIndexFun(xi, FX, EX, dx1);
            dx.E0 += i; dx.E1 += i; dx.E2 += i; dx.E3 += i;
            var dy = index_min_max_delta_yIndexFun(yi, FY, EY, dy1);
            var wx = xipl(xf); var wy = yipl(yf);
            //# if (dt == vt) {
            return ysmp(xsmp(Data[dx.E0 + dy.E0], Data[dx.E1 + dy.E0], Data[dx.E2 + dy.E0], Data[dx.E3 + dy.E0], ref wx),
                        xsmp(Data[dx.E0 + dy.E1], Data[dx.E1 + dy.E1], Data[dx.E2 + dy.E1], Data[dx.E3 + dy.E1], ref wx),
                        xsmp(Data[dx.E0 + dy.E2], Data[dx.E1 + dy.E2], Data[dx.E2 + dy.E2], Data[dx.E3 + dy.E2], ref wx),
                        xsmp(Data[dx.E0 + dy.E3], Data[dx.E1 + dy.E3], Data[dx.E2 + dy.E3], Data[dx.E3 + dy.E3], ref wx), ref wy);
            //# } else {
            return ysmp(xsmp(Getter(Data, dx.E0 + dy.E0), Getter(Data, dx.E1 + dy.E0), Getter(Data, dx.E2 + dy.E0), Getter(Data, dx.E3 + dy.E0), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E1), Getter(Data, dx.E1 + dy.E1), Getter(Data, dx.E2 + dy.E1), Getter(Data, dx.E3 + dy.E1), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E2), Getter(Data, dx.E1 + dy.E2), Getter(Data, dx.E2 + dy.E2), Getter(Data, dx.E3 + dy.E2), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E3), Getter(Data, dx.E1 + dy.E3), Getter(Data, dx.E2 + dy.E3), Getter(Data, dx.E3 + dy.E3), ref wx), ref wy);
            //# }
        }

        //# foreach (var clampFun in new[] { false, true }) {
        public void SetScaled16<T1,T2, T3/*# if (clampFun) { */, T4/*# } */>(Matrix<__dvtn__> sourceMat,
                double xScale, double yScale, double xShift, double yShift,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>/*# if (clampFun) { */, T4/*# }
                                                         else { */, __vtn__/*# } */> ysmp,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_yIndexFun/*#
                if (clampFun) { */,
                Func<T4, __vtn__> clampFun/*# } */)
        {
            var dxa = new Tup4<long>[SX];
            var wxa = new Tup4<T1>[SX];
            long fx = sourceMat.FX, ex = sourceMat.EX, dx1 = sourceMat.DX;
            for (long tix = 0, tsx = SX; tix < tsx; tix++, xShift += xScale)
            {
                double xid = Fun.Floor(xShift); long xi = (long)xid; double xf = xShift - xid;
                var dx = index_min_max_delta_xIndexFun(xi, fx, ex, dx1);
                var dxi = xi * dx1; dx.E0 += dxi; dx.E1 += dxi; dx.E2 += dxi; dx.E3 += dxi;
                dxa[tix] = dx; wxa[tix] = xipl(xf);
            }
            var dya = new Tup4<long>[SY];
            var wya = new Tup4<T2>[SY];
            long o = sourceMat.Origin, fy = sourceMat.FY, ey = sourceMat.EY, dy1 = sourceMat.DY;
            for (long tiy = 0, tsy = SY; tiy < tsy; tiy++, yShift += yScale)
            {
                double yid = Fun.Floor(yShift); long yi = (long)yid; double yf = yShift - yid;
                var dy = index_min_max_delta_yIndexFun(yi, fy, ey, dy1);
                var dyi = o + yi * dy1; dy.E0 += dyi; dy.E1 += dyi; dy.E2 += dyi; dy.E3 += dyi;
                dya[tiy] = dy; wya[tiy] = yipl(yf);
            }
            var data = Data;
            //# Loop("", false, () => {
                //# if (dt == vt) {
                //# if (clampFun) {
                    data[i] = clampFun(sourceMat.Sample16(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp));
                //# } else {
                    data[i] = sourceMat.Sample16(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp);
                //# }
                //# } else {
                //# if (clampFun) {
                    Setter(data, i, clampFun(sourceMat.Sample16(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp)));
                //# } else {
                    Setter(data, i, sourceMat.Sample16(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp));
                //# }
                //# }
            //# }, true, false);
        }

        //# } // clampFun
        public TRes Sample16<T1, T2, T3, TRes>(
                Tup4<long> dx, Tup4<long> dy, Tup4<T1> wx, Tup4<T2> wy,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, TRes> ysmp)
        {
            //# if (dt == vt) {
            return ysmp(xsmp(Data[dx.E0 + dy.E0], Data[dx.E1 + dy.E0], Data[dx.E2 + dy.E0], Data[dx.E3 + dy.E0], ref wx),
                        xsmp(Data[dx.E0 + dy.E1], Data[dx.E1 + dy.E1], Data[dx.E2 + dy.E1], Data[dx.E3 + dy.E1], ref wx),
                        xsmp(Data[dx.E0 + dy.E2], Data[dx.E1 + dy.E2], Data[dx.E2 + dy.E2], Data[dx.E3 + dy.E2], ref wx),
                        xsmp(Data[dx.E0 + dy.E3], Data[dx.E1 + dy.E3], Data[dx.E2 + dy.E3], Data[dx.E3 + dy.E3], ref wx), ref wy);
            //# } else {
            return ysmp(xsmp(Getter(Data, dx.E0 + dy.E0), Getter(Data, dx.E1 + dy.E0), Getter(Data, dx.E2 + dy.E0), Getter(Data, dx.E3 + dy.E0), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E1), Getter(Data, dx.E1 + dy.E1), Getter(Data, dx.E2 + dy.E1), Getter(Data, dx.E3 + dy.E1), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E2), Getter(Data, dx.E1 + dy.E2), Getter(Data, dx.E2 + dy.E2), Getter(Data, dx.E3 + dy.E2), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E3), Getter(Data, dx.E1 + dy.E3), Getter(Data, dx.E2 + dy.E3), Getter(Data, dx.E3 + dy.E3), ref wx), ref wy);
            //# }
        }

        public TRes SampleRaw36<T1, T2, T3, TRes>(
                V2d v,
                Func<double, Tup6<T1>> xipl,
                Func<double, Tup6<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>, TRes> ysmp)
        {
            return SampleRaw36(v.X, v.Y, xipl, yipl, xsmp, ysmp);
        }

        public TRes SampleRaw36<T1, T2, T3, TRes>(
                double x, double y,
                Func<double, Tup6<T1>> xipl,
                Func<double, Tup6<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>, TRes> ysmp)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy = Info.Delta.Y;
            long i2 = Info.Origin + xi * dx + yi * dy;
            long i1 = i2 - dy, i0 = i1 - dy, i3 = i2 + dy, i4 = i3 + dy, i5 = i4 + dy;
            long d2 = dx + dx, d3 = d2 + dx;
            var wx = xipl(xf); var wy = yipl(yf);
            //# if (dt == vt) {
            return ysmp(xsmp(Data[i0 - d2], Data[i0 - dx], Data[i0], Data[i0 + dx], Data[i0 + d2], Data[i0 + d3], ref wx),
                        xsmp(Data[i1 - d2], Data[i1 - dx], Data[i1], Data[i1 + dx], Data[i1 + d2], Data[i1 + d3], ref wx),
                        xsmp(Data[i2 - d2], Data[i2 - dx], Data[i2], Data[i2 + dx], Data[i2 + d2], Data[i2 + d3], ref wx),
                        xsmp(Data[i3 - d2], Data[i3 - dx], Data[i3], Data[i3 + dx], Data[i3 + d2], Data[i3 + d3], ref wx),
                        xsmp(Data[i4 - d2], Data[i4 - dx], Data[i4], Data[i4 + dx], Data[i4 + d2], Data[i4 + d3], ref wx),
                        xsmp(Data[i5 - d2], Data[i5 - dx], Data[i5], Data[i5 + dx], Data[i5 + d2], Data[i5 + d3], ref wx), ref wy);
            //# } else {
            return ysmp(xsmp(Getter(Data, i0 - d2), Getter(Data, i0 - dx), Getter(Data, i0), Getter(Data, i0 + dx), Getter(Data, i0 + d2), Getter(Data, i0 + d3), ref wx),
                        xsmp(Getter(Data, i1 - d2), Getter(Data, i1 - dx), Getter(Data, i1), Getter(Data, i1 + dx), Getter(Data, i1 + d2), Getter(Data, i1 + d3), ref wx),
                        xsmp(Getter(Data, i2 - d2), Getter(Data, i2 - dx), Getter(Data, i2), Getter(Data, i2 + dx), Getter(Data, i2 + d2), Getter(Data, i2 + d3), ref wx),
                        xsmp(Getter(Data, i3 - d2), Getter(Data, i3 - dx), Getter(Data, i3), Getter(Data, i3 + dx), Getter(Data, i3 + d2), Getter(Data, i3 + d3), ref wx),
                        xsmp(Getter(Data, i4 - d2), Getter(Data, i4 - dx), Getter(Data, i4), Getter(Data, i4 + dx), Getter(Data, i4 + d2), Getter(Data, i4 + d3), ref wx),
                        xsmp(Getter(Data, i5 - d2), Getter(Data, i5 - dx), Getter(Data, i5), Getter(Data, i5 + dx), Getter(Data, i5 + d2), Getter(Data, i5 + d3), ref wx), ref wy);
            //# }
        }

        public TRes Sample36Clamped<T1, T2, T3, TRes>(
                V2d v,
                Func<double, Tup6<T1>> xipl,
                Func<double, Tup6<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>, TRes> ysmp)
        {
            return Sample36(v.X, v.Y, xipl, yipl, xsmp, ysmp,
                            Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        public TRes Sample36Clamped<T1, T2, T3, TRes>(
                double x, double y,
                Func<double, Tup6<T1>> xipl,
                Func<double, Tup6<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>, TRes> ysmp)
        {
            return Sample36(x, y, xipl, yipl, xsmp, ysmp,
                            Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        public TRes Sample36<T1, T2, T3, TRes>(
                V2d v,
                Func<double, Tup6<T1>> xipl,
                Func<double, Tup6<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>, TRes> ysmp,
                Func<long, long, long, long, Tup6<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup6<long>> index_min_max_delta_yIndexFun)
        {
            return Sample36(v.X, v.Y, xipl, yipl, xsmp, ysmp,
                            index_min_max_delta_xIndexFun,
                            index_min_max_delta_yIndexFun);
        }

        public TRes Sample36<T1, T2, T3, TRes>(
                double x, double y,
                Func<double, Tup6<T1>> xipl,
                Func<double, Tup6<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>, TRes> ysmp,
                Func<long, long, long, long, Tup6<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup6<long>> index_min_max_delta_yIndexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx1 = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy1 = Info.Delta.Y;
            long i = Info.Origin + xi * dx1 + yi * dy1;
            var dx = index_min_max_delta_xIndexFun(xi, FX, EX, dx1);
            dx.E0 += i; dx.E1 += i; dx.E2 += i; dx.E3 += i; dx.E4 += i; dx.E5 += i;
            var dy = index_min_max_delta_yIndexFun(yi, FY, EY, dy1);
            var wx = xipl(xf); var wy = yipl(yf);
            //# if (dt == vt) {
            return ysmp(xsmp(Data[dx.E0 + dy.E0], Data[dx.E1 + dy.E0], Data[dx.E2 + dy.E0], Data[dx.E3 + dy.E0], Data[dx.E4 + dy.E0], Data[dx.E5 + dy.E0], ref wx),
                        xsmp(Data[dx.E0 + dy.E1], Data[dx.E1 + dy.E1], Data[dx.E2 + dy.E1], Data[dx.E3 + dy.E1], Data[dx.E4 + dy.E1], Data[dx.E5 + dy.E1], ref wx),
                        xsmp(Data[dx.E0 + dy.E2], Data[dx.E1 + dy.E2], Data[dx.E2 + dy.E2], Data[dx.E3 + dy.E2], Data[dx.E4 + dy.E2], Data[dx.E5 + dy.E2], ref wx),
                        xsmp(Data[dx.E0 + dy.E3], Data[dx.E1 + dy.E3], Data[dx.E2 + dy.E3], Data[dx.E3 + dy.E3], Data[dx.E4 + dy.E3], Data[dx.E5 + dy.E3], ref wx),
                        xsmp(Data[dx.E0 + dy.E4], Data[dx.E1 + dy.E4], Data[dx.E2 + dy.E4], Data[dx.E3 + dy.E4], Data[dx.E4 + dy.E4], Data[dx.E5 + dy.E4], ref wx),
                        xsmp(Data[dx.E0 + dy.E5], Data[dx.E1 + dy.E5], Data[dx.E2 + dy.E5], Data[dx.E3 + dy.E5], Data[dx.E4 + dy.E5], Data[dx.E5 + dy.E5], ref wx), ref wy);
            //# } else {
            return ysmp(xsmp(Getter(Data, dx.E0 + dy.E0), Getter(Data, dx.E1 + dy.E0), Getter(Data, dx.E2 + dy.E0), Getter(Data, dx.E3 + dy.E0), Getter(Data, dx.E4 + dy.E0), Getter(Data, dx.E5 + dy.E0), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E1), Getter(Data, dx.E1 + dy.E1), Getter(Data, dx.E2 + dy.E1), Getter(Data, dx.E3 + dy.E1), Getter(Data, dx.E4 + dy.E1), Getter(Data, dx.E5 + dy.E1), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E2), Getter(Data, dx.E1 + dy.E2), Getter(Data, dx.E2 + dy.E2), Getter(Data, dx.E3 + dy.E2), Getter(Data, dx.E4 + dy.E2), Getter(Data, dx.E5 + dy.E2), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E3), Getter(Data, dx.E1 + dy.E3), Getter(Data, dx.E2 + dy.E3), Getter(Data, dx.E3 + dy.E3), Getter(Data, dx.E4 + dy.E3), Getter(Data, dx.E5 + dy.E3), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E4), Getter(Data, dx.E1 + dy.E4), Getter(Data, dx.E2 + dy.E4), Getter(Data, dx.E3 + dy.E4), Getter(Data, dx.E4 + dy.E4), Getter(Data, dx.E5 + dy.E4), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E5), Getter(Data, dx.E1 + dy.E5), Getter(Data, dx.E2 + dy.E5), Getter(Data, dx.E3 + dy.E5), Getter(Data, dx.E4 + dy.E5), Getter(Data, dx.E5 + dy.E5), ref wx), ref wy);
            //# }
        }

        //# foreach (var clampFun in new[] { false, true }) {
        public void SetScaled36<T1, T2, T3/*# if (clampFun) { */, T4/*# } */>(Matrix<__dvtn__> sourceMat,
                double xScale, double yScale, double xShift, double yShift,
                Func<double, Tup6<T1>> xipl,
                Func<double, Tup6<T2>> yipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>/*# if (clampFun) { */, T4/*# }
                                                         else { */, __vtn__/*# } */> ysmp,
                Func<long, long, long, long, Tup6<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup6<long>> index_min_max_delta_yIndexFun/*#
                if (clampFun) { */,
                Func<T4, __vtn__> clampFun/*# } */)
        {
            var dxa = new Tup6<long>[SX];
            var wxa = new Tup6<T1>[SX];
            long fx = sourceMat.FX, ex = sourceMat.EX, dx1 = sourceMat.DX;
            for (long tix = 0, tsx = SX; tix < tsx; tix++, xShift += xScale)
            {
                double xid = Fun.Floor(xShift); long xi = (long)xid; double xf = xShift - xid;
                var dx = index_min_max_delta_xIndexFun(xi, fx, ex, dx1);
                var dxi = xi * dx1;
                dx.E0 += dxi; dx.E1 += dxi; dx.E2 += dxi;
                dx.E3 += dxi; dx.E4 += dxi; dx.E5 += dxi;
                dxa[tix] = dx; wxa[tix] = xipl(xf);
            }
            var dya = new Tup6<long>[SY];
            var wya = new Tup6<T2>[SY];
            long o = sourceMat.Origin, fy = sourceMat.FY, ey = sourceMat.EY, dy1 = sourceMat.DY;
            for (long tiy = 0, tsy = SY; tiy < tsy; tiy++, yShift += yScale)
            {
                double yid = Fun.Floor(yShift); long yi = (long)yid; double yf = yShift - yid;
                var dy = index_min_max_delta_yIndexFun(yi, fy, ey, dy1);
                var dyi = o + yi * dy1;
                dy.E0 += dyi; dy.E1 += dyi; dy.E2 += dyi;
                dy.E3 += dyi; dy.E4 += dyi; dy.E5 += dyi;
                dya[tiy] = dy; wya[tiy] = yipl(yf);
            }
            var data = Data;
            //# Loop("", false, () => {
                //# if (dt == vt) {
                //# if (clampFun) {
                    data[i] = clampFun(sourceMat.Sample36(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp));
                //# } else {
                    data[i] = sourceMat.Sample36(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp);
                //# }
                //# } else {
                //# if (clampFun) {
                    Setter(data, i, clampFun(sourceMat.Sample36(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp)));
                //# } else {
                    Setter(data, i, sourceMat.Sample36(dxa[x], dya[y], wxa[x], wya[y], xsmp, ysmp));
                //# }
                //# }
            //# }, true, false);
        }

        //# } // clampFun
        public TRes Sample36<T1, T2, T3, TRes>(
                Tup6<long> dx, Tup6<long> dy, Tup6<T1> wx, Tup6<T2> wy, 
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, __vtn__, __vtn__, Tup6<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, T3, T3, Tup6<T2>, TRes> ysmp)
        {
            //# if (dt == vt) {
            return ysmp(xsmp(Data[dx.E0 + dy.E0], Data[dx.E1 + dy.E0], Data[dx.E2 + dy.E0], Data[dx.E3 + dy.E0], Data[dx.E4 + dy.E0], Data[dx.E5 + dy.E0], ref wx),
                        xsmp(Data[dx.E0 + dy.E1], Data[dx.E1 + dy.E1], Data[dx.E2 + dy.E1], Data[dx.E3 + dy.E1], Data[dx.E4 + dy.E1], Data[dx.E5 + dy.E1], ref wx),
                        xsmp(Data[dx.E0 + dy.E2], Data[dx.E1 + dy.E2], Data[dx.E2 + dy.E2], Data[dx.E3 + dy.E2], Data[dx.E4 + dy.E2], Data[dx.E5 + dy.E2], ref wx),
                        xsmp(Data[dx.E0 + dy.E3], Data[dx.E1 + dy.E3], Data[dx.E2 + dy.E3], Data[dx.E3 + dy.E3], Data[dx.E4 + dy.E3], Data[dx.E5 + dy.E3], ref wx),
                        xsmp(Data[dx.E0 + dy.E4], Data[dx.E1 + dy.E4], Data[dx.E2 + dy.E4], Data[dx.E3 + dy.E4], Data[dx.E4 + dy.E4], Data[dx.E5 + dy.E4], ref wx),
                        xsmp(Data[dx.E0 + dy.E5], Data[dx.E1 + dy.E5], Data[dx.E2 + dy.E5], Data[dx.E3 + dy.E5], Data[dx.E4 + dy.E5], Data[dx.E5 + dy.E5], ref wx), ref wy);
            //# } else {
            return ysmp(xsmp(Getter(Data, dx.E0 + dy.E0), Getter(Data, dx.E1 + dy.E0), Getter(Data, dx.E2 + dy.E0), Getter(Data, dx.E3 + dy.E0), Getter(Data, dx.E4 + dy.E0), Getter(Data, dx.E5 + dy.E0), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E1), Getter(Data, dx.E1 + dy.E1), Getter(Data, dx.E2 + dy.E1), Getter(Data, dx.E3 + dy.E1), Getter(Data, dx.E4 + dy.E1), Getter(Data, dx.E5 + dy.E1), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E2), Getter(Data, dx.E1 + dy.E2), Getter(Data, dx.E2 + dy.E2), Getter(Data, dx.E3 + dy.E2), Getter(Data, dx.E4 + dy.E2), Getter(Data, dx.E5 + dy.E2), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E3), Getter(Data, dx.E1 + dy.E3), Getter(Data, dx.E2 + dy.E3), Getter(Data, dx.E3 + dy.E3), Getter(Data, dx.E4 + dy.E3), Getter(Data, dx.E5 + dy.E3), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E4), Getter(Data, dx.E1 + dy.E4), Getter(Data, dx.E2 + dy.E4), Getter(Data, dx.E3 + dy.E4), Getter(Data, dx.E4 + dy.E4), Getter(Data, dx.E5 + dy.E4), ref wx),
                        xsmp(Getter(Data, dx.E0 + dy.E5), Getter(Data, dx.E1 + dy.E5), Getter(Data, dx.E2 + dy.E5), Getter(Data, dx.E3 + dy.E5), Getter(Data, dx.E4 + dy.E5), Getter(Data, dx.E5 + dy.E5), ref wx), ref wy);
            //# }
        }
        //# } // d == 2
        //# if (d == 3) {


        public TRes SampleRaw8<T1, T2, TRes>(
                V3d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, TRes> zipl)
        {
            return SampleRaw8(v.X, v.Y, v.Z, xipl, yipl, zipl);
        }

        /// <summary>
        /// Sample the volume using 8 samples and the supplied
        /// interpolation functions. If only integer coordintes are
        /// supplied the returned value is the same as the indexer
        /// of the volume, i.e. voxel centers are assumed to be on
        /// integer coordinates. No bounds checking is performed.
        /// </summary>
        public TRes SampleRaw8<T1, T2, TRes>(
                double x, double y, double z,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, TRes> zipl)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy = Info.Delta.Y;
            double zid = Fun.Floor(z); long zi = (long)zid; double zf = z - zid; long dz = Info.Delta.Z;
            long i = Info.Origin + xi * Info.Delta.X + yi * Info.Delta.Y + zi * Info.Delta.Z;
            long ix = i + dx; long iy = i + dy; long ixy = ix + dy;
            //# if (dt == vt) {
            return zipl(zf, yipl(yf, xipl(xf, Data[i], Data[ix]),
                                     xipl(xf, Data[iy], Data[ixy])),
                            yipl(yf, xipl(xf, Data[i + dz], Data[ix + dz]),
                                     xipl(xf, Data[iy + dz], Data[ixy + dz])));
            //# } else {
            return zipl(zf, yipl(yf, xipl(xf, Getter(Data, i), Getter(Data, ix)),
                                     xipl(xf, Getter(Data, iy), Getter(Data, ixy))),
                            yipl(yf, xipl(xf, Getter(Data, i + dz), Getter(Data, ix + dz)),
                                     xipl(xf, Getter(Data, iy + dz), Getter(Data, ixy + dz))));
            //# }
        }

        public TRes Sample8Clamped<T1, T2, TRes>(
                V3d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, TRes> zipl)
        {
            return Sample8(v.X, v.Y, v.Z, xipl, yipl, zipl,
                           Tensor.Index2SamplesClamped,
                           Tensor.Index2SamplesClamped,
                           Tensor.Index2SamplesClamped);
        }

        public TRes Sample8Clamped<T1, T2, TRes>(
                double x, double y, double z,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, TRes> zipl)
        {
            return Sample8(x, y, z, xipl, yipl, zipl,
                           Tensor.Index2SamplesClamped,
                           Tensor.Index2SamplesClamped,
                           Tensor.Index2SamplesClamped);
        }

        public TRes Sample8<T1, T2, TRes>(
                V3d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, TRes> zipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_yIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_zIndexFun)
        {
            return Sample8(v.X, v.Y, v.Z, xipl, yipl, zipl,
                           index_min_max_delta_xIndexFun,
                           index_min_max_delta_yIndexFun,
                           index_min_max_delta_zIndexFun);
        }
   
        /// <summary>
        /// Sample the volume using 8 samples and the supplied
        /// interpolation functions. If only integer coordintes are
        /// supplied the returned value is the same as the indexer
        /// of the volume, i.e. voxel centers are assumed to be on
        /// integer coordinates.
        /// </summary>
        public TRes Sample8<T1, T2, TRes>(
                double x, double y, double z,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, TRes> zipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_yIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_zIndexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx1 = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy1 = Info.Delta.Y;
            double zid = Fun.Floor(z); long zi = (long)zid; double zf = z - zid; long dz1 = Info.Delta.Z;
            long i = Info.Origin + xi * dx1 + yi * dy1 + zi * dz1;
            var dx = index_min_max_delta_xIndexFun(xi, FX, EX, dx1); dx.E0 += i; dx.E1 += i;
            var dyz = Fun.OuterSum(index_min_max_delta_yIndexFun(yi, FY, EY, dy1),
                                   index_min_max_delta_zIndexFun(zi, FZ, EZ, dz1));
            //# if (dt == vt) {
            return zipl(zf, yipl(yf, xipl(xf, Data[dx.E0 + dyz.E0], Data[dx.E1 + dyz.E0]),
                                     xipl(xf, Data[dx.E0 + dyz.E1], Data[dx.E1 + dyz.E1])),
                            yipl(yf, xipl(xf, Data[dx.E0 + dyz.E2], Data[dx.E1 + dyz.E2]),
                                     xipl(xf, Data[dx.E0 + dyz.E3], Data[dx.E1 + dyz.E3])));
            //# } else {
            return zipl(zf, yipl(yf, xipl(xf, Getter(Data, dx.E0 + dyz.E0), Getter(Data, dx.E1 + dyz.E0)),
                                     xipl(xf, Getter(Data, dx.E0 + dyz.E1), Getter(Data, dx.E1 + dyz.E1))),
                            yipl(yf, xipl(xf, Getter(Data, dx.E0 + dyz.E2), Getter(Data, dx.E1 + dyz.E2)),
                                     xipl(xf, Getter(Data, dx.E0 + dyz.E3), Getter(Data, dx.E1 + dyz.E3))));
            //# }
        }

        public TRes SampleRaw64<T1, T2, T3, T4, T5, TRes>(
                V3d v,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                Func<double, Tup4<T3>> zipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T4> xsmp,
                FuncRef1<T4, T4, T4, T4, Tup4<T2>, T5> ysmp,
                FuncRef1<T5, T5, T5, T5, Tup4<T3>, TRes> zsmp)
        {
            return SampleRaw64(v.X, v.Y, v.Z, xipl, yipl, zipl, xsmp, ysmp, zsmp);
        }
            
        public TRes SampleRaw64<T1, T2, T3, T4, T5, TRes>(
                double x, double y, double z,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                Func<double, Tup4<T3>> zipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T4> xsmp,
                FuncRef1<T4, T4, T4, T4, Tup4<T2>, T5> ysmp,
                FuncRef1<T5, T5, T5, T5, Tup4<T3>, TRes> zsmp)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy = Info.Delta.Y;
            double zid = Fun.Floor(z); long zi = (long)zid; double zf = z - zid; long dz = Info.Delta.Z;
            long i11 = Info.Origin + xi * dx + yi * dy + zi * dz;
            long d2 = dx + dx;
            long i10 = i11 - dy, i12 = i11 + dy, i13 = i12 + dy;
            long i00 = i10 - dz, i01 = i11 - dz, i02 = i12 - dz, i03 = i13 - dz;
            long i20 = i10 + dz, i21 = i11 + dz, i22 = i12 + dz, i23 = i13 + dz;
            long i30 = i20 + dz, i31 = i21 + dz, i32 = i22 + dz, i33 = i23 + dz;
            var wx = xipl(xf); var wy = yipl(yf); var wz = zipl(zf);
            //# if (dt == vt) {
            return zsmp(ysmp(xsmp(Data[i00 - dx], Data[i00], Data[i00 + dx], Data[i00 + d2], ref wx),
                             xsmp(Data[i01 - dx], Data[i01], Data[i01 + dx], Data[i01 + d2], ref wx),
                             xsmp(Data[i02 - dx], Data[i02], Data[i02 + dx], Data[i02 + d2], ref wx),
                             xsmp(Data[i03 - dx], Data[i03], Data[i03 + dx], Data[i03 + d2], ref wx), ref wy),
                        ysmp(xsmp(Data[i10 - dx], Data[i10], Data[i10 + dx], Data[i10 + d2], ref wx),
                             xsmp(Data[i11 - dx], Data[i11], Data[i11 + dx], Data[i11 + d2], ref wx),
                             xsmp(Data[i12 - dx], Data[i12], Data[i12 + dx], Data[i12 + d2], ref wx),
                             xsmp(Data[i13 - dx], Data[i13], Data[i13 + dx], Data[i13 + d2], ref wx), ref wy),
                        ysmp(xsmp(Data[i20 - dx], Data[i20], Data[i20 + dx], Data[i20 + d2], ref wx),
                             xsmp(Data[i21 - dx], Data[i21], Data[i21 + dx], Data[i21 + d2], ref wx),
                             xsmp(Data[i22 - dx], Data[i22], Data[i22 + dx], Data[i22 + d2], ref wx),
                             xsmp(Data[i23 - dx], Data[i23], Data[i23 + dx], Data[i23 + d2], ref wx), ref wy),
                        ysmp(xsmp(Data[i30 - dx], Data[i30], Data[i30 + dx], Data[i30 + d2], ref wx),
                             xsmp(Data[i31 - dx], Data[i31], Data[i31 + dx], Data[i31 + d2], ref wx),
                             xsmp(Data[i32 - dx], Data[i32], Data[i32 + dx], Data[i32 + d2], ref wx),
                             xsmp(Data[i33 - dx], Data[i33], Data[i33 + dx], Data[i33 + d2], ref wx), ref wy), ref wz);
            //# } else {
            return zsmp(ysmp(xsmp(Getter(Data, i00 - dx), Getter(Data, i00), Getter(Data, i00 + dx), Getter(Data, i00 + d2), ref wx),
                             xsmp(Getter(Data, i01 - dx), Getter(Data, i01), Getter(Data, i01 + dx), Getter(Data, i01 + d2), ref wx),
                             xsmp(Getter(Data, i02 - dx), Getter(Data, i02), Getter(Data, i02 + dx), Getter(Data, i02 + d2), ref wx),
                             xsmp(Getter(Data, i03 - dx), Getter(Data, i03), Getter(Data, i03 + dx), Getter(Data, i03 + d2), ref wx), ref wy),
                        ysmp(xsmp(Getter(Data, i10 - dx), Getter(Data, i10), Getter(Data, i10 + dx), Getter(Data, i10 + d2), ref wx),
                             xsmp(Getter(Data, i11 - dx), Getter(Data, i11), Getter(Data, i11 + dx), Getter(Data, i11 + d2), ref wx),
                             xsmp(Getter(Data, i12 - dx), Getter(Data, i12), Getter(Data, i12 + dx), Getter(Data, i12 + d2), ref wx),
                             xsmp(Getter(Data, i13 - dx), Getter(Data, i13), Getter(Data, i13 + dx), Getter(Data, i13 + d2), ref wx), ref wy),
                        ysmp(xsmp(Getter(Data, i20 - dx), Getter(Data, i20), Getter(Data, i20 + dx), Getter(Data, i20 + d2), ref wx),
                             xsmp(Getter(Data, i21 - dx), Getter(Data, i21), Getter(Data, i21 + dx), Getter(Data, i21 + d2), ref wx),
                             xsmp(Getter(Data, i22 - dx), Getter(Data, i22), Getter(Data, i22 + dx), Getter(Data, i22 + d2), ref wx),
                             xsmp(Getter(Data, i23 - dx), Getter(Data, i23), Getter(Data, i23 + dx), Getter(Data, i23 + d2), ref wx), ref wy),
                        ysmp(xsmp(Getter(Data, i30 - dx), Getter(Data, i30), Getter(Data, i30 + dx), Getter(Data, i30 + d2), ref wx),
                             xsmp(Getter(Data, i31 - dx), Getter(Data, i31), Getter(Data, i31 + dx), Getter(Data, i31 + d2), ref wx),
                             xsmp(Getter(Data, i32 - dx), Getter(Data, i32), Getter(Data, i32 + dx), Getter(Data, i32 + d2), ref wx),
                             xsmp(Getter(Data, i33 - dx), Getter(Data, i33), Getter(Data, i33 + dx), Getter(Data, i33 + d2), ref wx), ref wy), ref wz);
            //# }
        }

        public TRes Sample64Clamped<T1, T2, T3, T4, T5, TRes>(
                V3d v,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                Func<double, Tup4<T3>> zipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T4> xsmp,
                FuncRef1<T4, T4, T4, T4, Tup4<T2>, T5> ysmp,
                FuncRef1<T5, T5, T5, T5, Tup4<T3>, TRes> zsmp)
        {
            return Sample64(v.X, v.Y, v.Z, xipl, yipl, zipl, xsmp, ysmp, zsmp,
                            Tensor.Index4SamplesClamped,
                            Tensor.Index4SamplesClamped,
                            Tensor.Index4SamplesClamped);
        }

        public TRes Sample64Clamped<T1, T2, T3, T4, T5, TRes>(
                double x, double y, double z,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                Func<double, Tup4<T3>> zipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T4> xsmp,
                FuncRef1<T4, T4, T4, T4, Tup4<T2>, T5> ysmp,
                FuncRef1<T5, T5, T5, T5, Tup4<T3>, TRes> zsmp)
        {
            return Sample64(x, y, z, xipl, yipl, zipl, xsmp, ysmp, zsmp,
                            Tensor.Index4SamplesClamped,
                            Tensor.Index4SamplesClamped,
                            Tensor.Index4SamplesClamped);
        }
        
        public TRes Sample64<T1, T2, T3, T4, T5, TRes>(
                V3d v,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                Func<double, Tup4<T3>> zipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T4> xsmp,
                FuncRef1<T4, T4, T4, T4, Tup4<T2>, T5> ysmp,
                FuncRef1<T5, T5, T5, T5, Tup4<T3>, TRes> zsmp,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_yIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_zIndexFun)
        {
            return Sample64(v.X, v.Y, v.Z, xipl, yipl, zipl, xsmp, ysmp, zsmp,
                            index_min_max_delta_xIndexFun,
                            index_min_max_delta_yIndexFun,
                            index_min_max_delta_zIndexFun);
        }

        public TRes Sample64<T1, T2, T3, T4, T5, TRes>(
                double x, double y, double z,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                Func<double, Tup4<T3>> zipl,
                FuncRef1<__vtn__, __vtn__, __vtn__, __vtn__, Tup4<T1>, T4> xsmp,
                FuncRef1<T4, T4, T4, T4, Tup4<T2>, T5> ysmp,
                FuncRef1<T5, T5, T5, T5, Tup4<T3>, TRes> zsmp,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_yIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_zIndexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx1 = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy1 = Info.Delta.Y;
            double zid = Fun.Floor(z); long zi = (long)zid; double zf = z - zid; long dz1 = Info.Delta.Z;
            long i = Info.Origin + xi * dx1 + yi * dy1 + zi * dz1;
            var dx = index_min_max_delta_xIndexFun(xi, FX, EX, dx1);
            dx.E0 += i; dx.E1 += i; dx.E2 += i; dx.E3 += i;
            var dyz = Fun.OuterSum(index_min_max_delta_yIndexFun(yi, FY, EY, dy1),
                                   index_min_max_delta_zIndexFun(zi, FZ, EZ, dz1));
            var wx = xipl(xf); var wy = yipl(yf); var wz = zipl(zf);
            //# if (dt == vt) {
            return zsmp(ysmp(xsmp(Data[dx.E0 + dyz.E00], Data[dx.E1 + dyz.E00], Data[dx.E2 + dyz.E00], Data[dx.E3 + dyz.E00], ref wx),
                             xsmp(Data[dx.E0 + dyz.E01], Data[dx.E1 + dyz.E01], Data[dx.E2 + dyz.E01], Data[dx.E3 + dyz.E01], ref wx),
                             xsmp(Data[dx.E0 + dyz.E02], Data[dx.E1 + dyz.E02], Data[dx.E2 + dyz.E02], Data[dx.E3 + dyz.E02], ref wx),
                             xsmp(Data[dx.E0 + dyz.E03], Data[dx.E1 + dyz.E03], Data[dx.E2 + dyz.E03], Data[dx.E3 + dyz.E03], ref wx), ref wy),
                        ysmp(xsmp(Data[dx.E0 + dyz.E04], Data[dx.E1 + dyz.E04], Data[dx.E2 + dyz.E04], Data[dx.E3 + dyz.E04], ref wx),
                             xsmp(Data[dx.E0 + dyz.E05], Data[dx.E1 + dyz.E05], Data[dx.E2 + dyz.E05], Data[dx.E3 + dyz.E05], ref wx),
                             xsmp(Data[dx.E0 + dyz.E06], Data[dx.E1 + dyz.E06], Data[dx.E2 + dyz.E06], Data[dx.E3 + dyz.E06], ref wx),
                             xsmp(Data[dx.E0 + dyz.E07], Data[dx.E1 + dyz.E07], Data[dx.E2 + dyz.E07], Data[dx.E3 + dyz.E07], ref wx), ref wy),
                        ysmp(xsmp(Data[dx.E0 + dyz.E08], Data[dx.E1 + dyz.E08], Data[dx.E2 + dyz.E08], Data[dx.E3 + dyz.E08], ref wx),
                             xsmp(Data[dx.E0 + dyz.E09], Data[dx.E1 + dyz.E09], Data[dx.E2 + dyz.E09], Data[dx.E3 + dyz.E09], ref wx),
                             xsmp(Data[dx.E0 + dyz.E10], Data[dx.E1 + dyz.E10], Data[dx.E2 + dyz.E10], Data[dx.E3 + dyz.E10], ref wx),
                             xsmp(Data[dx.E0 + dyz.E11], Data[dx.E1 + dyz.E11], Data[dx.E2 + dyz.E11], Data[dx.E3 + dyz.E11], ref wx), ref wy),
                        ysmp(xsmp(Data[dx.E0 + dyz.E12], Data[dx.E1 + dyz.E12], Data[dx.E2 + dyz.E12], Data[dx.E3 + dyz.E12], ref wx),
                             xsmp(Data[dx.E0 + dyz.E13], Data[dx.E1 + dyz.E13], Data[dx.E2 + dyz.E13], Data[dx.E3 + dyz.E13], ref wx),
                             xsmp(Data[dx.E0 + dyz.E14], Data[dx.E1 + dyz.E14], Data[dx.E2 + dyz.E14], Data[dx.E3 + dyz.E14], ref wx),
                             xsmp(Data[dx.E0 + dyz.E15], Data[dx.E1 + dyz.E15], Data[dx.E2 + dyz.E15], Data[dx.E3 + dyz.E15], ref wx), ref wy), ref wz);
            //# } else {
            return zsmp(ysmp(xsmp(Getter(Data, dx.E0 + dyz.E00), Getter(Data, dx.E1 + dyz.E00), Getter(Data, dx.E2 + dyz.E00), Getter(Data, dx.E3 + dyz.E00), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E01), Getter(Data, dx.E1 + dyz.E01), Getter(Data, dx.E2 + dyz.E01), Getter(Data, dx.E3 + dyz.E01), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E02), Getter(Data, dx.E1 + dyz.E02), Getter(Data, dx.E2 + dyz.E02), Getter(Data, dx.E3 + dyz.E02), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E03), Getter(Data, dx.E1 + dyz.E03), Getter(Data, dx.E2 + dyz.E03), Getter(Data, dx.E3 + dyz.E03), ref wx), ref wy),
                        ysmp(xsmp(Getter(Data, dx.E0 + dyz.E04), Getter(Data, dx.E1 + dyz.E04), Getter(Data, dx.E2 + dyz.E04), Getter(Data, dx.E3 + dyz.E04), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E05), Getter(Data, dx.E1 + dyz.E05), Getter(Data, dx.E2 + dyz.E05), Getter(Data, dx.E3 + dyz.E05), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E06), Getter(Data, dx.E1 + dyz.E06), Getter(Data, dx.E2 + dyz.E06), Getter(Data, dx.E3 + dyz.E06), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E07), Getter(Data, dx.E1 + dyz.E07), Getter(Data, dx.E2 + dyz.E07), Getter(Data, dx.E3 + dyz.E07), ref wx), ref wy),
                        ysmp(xsmp(Getter(Data, dx.E0 + dyz.E08), Getter(Data, dx.E1 + dyz.E08), Getter(Data, dx.E2 + dyz.E08), Getter(Data, dx.E3 + dyz.E08), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E09), Getter(Data, dx.E1 + dyz.E09), Getter(Data, dx.E2 + dyz.E09), Getter(Data, dx.E3 + dyz.E09), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E10), Getter(Data, dx.E1 + dyz.E10), Getter(Data, dx.E2 + dyz.E10), Getter(Data, dx.E3 + dyz.E10), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E11), Getter(Data, dx.E1 + dyz.E11), Getter(Data, dx.E2 + dyz.E11), Getter(Data, dx.E3 + dyz.E11), ref wx), ref wy),
                        ysmp(xsmp(Getter(Data, dx.E0 + dyz.E12), Getter(Data, dx.E1 + dyz.E12), Getter(Data, dx.E2 + dyz.E12), Getter(Data, dx.E3 + dyz.E12), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E13), Getter(Data, dx.E1 + dyz.E13), Getter(Data, dx.E2 + dyz.E13), Getter(Data, dx.E3 + dyz.E13), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E14), Getter(Data, dx.E1 + dyz.E14), Getter(Data, dx.E2 + dyz.E14), Getter(Data, dx.E3 + dyz.E14), ref wx),
                             xsmp(Getter(Data, dx.E0 + dyz.E15), Getter(Data, dx.E1 + dyz.E15), Getter(Data, dx.E2 + dyz.E15), Getter(Data, dx.E3 + dyz.E15), ref wx), ref wy), ref wz);
            //# }
        }

        //# } // d == 3
        //# if (d == 4) {
        public TRes SampleRaw16<T1, T2, T3, TRes>(
                V4d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, T3> zipl,
                Func<double, T3, T3, TRes> wipl)
        {
            return SampleRaw16(v.X, v.Y, v.Z, v.W, xipl, yipl, zipl, wipl);
        }
            
        /// <summary>
        /// Sample the tensor4 using 16 samples and the supplied
        /// interpolation functions. If only integer coordinates are
        /// supplied the returned value is the same as the indexer
        /// of the tensor4, i.e. voxel centers are assumed to be on
        /// integer coordinates. No bounds checking is performed.
        /// </summary>
        public TRes SampleRaw16<T1, T2, T3, TRes>(
                double x, double y, double z, double w,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, T3> zipl,
                Func<double, T3, T3, TRes> wipl)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy = Info.Delta.Y;
            double zid = Fun.Floor(z); long zi = (long)zid; double zf = z - zid; long dz = Info.Delta.Z;
            double wid = Fun.Floor(w); long wi = (long)wid; double wf = w - wid; long dw = Info.Delta.W;
            long i = Info.Origin + xi * dx + yi * dy + zi * dz + wi * dw;
            long ix = i + dx, iy = i + dy, ixy = ix + dy;
            long iz = i + dz, ixz = ix + dz, iyz = iy + dz, ixyz = ixy + dz;
            //# if (dt == vt) {
            return wipl(wf, zipl(zf, yipl(yf, xipl(xf, Data[i], Data[ix]),
                                              xipl(xf, Data[iy], Data[ixy])),
                                     yipl(yf, xipl(xf, Data[iz], Data[ixz]),
                                              xipl(xf, Data[iyz], Data[ixyz]))),
                            zipl(zf, yipl(yf, xipl(xf, Data[i + dw], Data[ix + dw]),
                                              xipl(xf, Data[iy + dw], Data[ixy + dw])),
                                     yipl(yf, xipl(xf, Data[iz + dw], Data[ixz + dw]),
                                              xipl(xf, Data[iyz + dw], Data[ixyz + dw]))));
            //# } else {
            return wipl(wf, zipl(zf, yipl(yf, xipl(xf, Getter(Data, i), Getter(Data, ix)),
                                              xipl(xf, Getter(Data, iy), Getter(Data, ixy))),
                                     yipl(yf, xipl(xf, Getter(Data, iz), Getter(Data, ixz)),
                                              xipl(xf, Getter(Data, iyz), Getter(Data, ixyz)))),
                            zipl(zf, yipl(yf, xipl(xf, Getter(Data, i + dw), Getter(Data, ix + dw)),
                                              xipl(xf, Getter(Data, iy + dw), Getter(Data, ixy + dw))),
                                     yipl(yf, xipl(xf, Getter(Data, iz + dw), Getter(Data, ixz + dw)),
                                              xipl(xf, Getter(Data, iyz + dw), Getter(Data, ixyz + dw)))));
            //# }
        }

        public TRes Sample16Clamped<T1, T2, T3, TRes>(
                V4d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, T3> zipl,
                Func<double, T3, T3, TRes> wipl)
        {
            return Sample16(v.X, v.Y, v.Z, v.W, xipl, yipl, zipl, wipl,
                            Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped,
                            Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }
            
        public TRes Sample16Clamped<T1, T2, T3, TRes>(
                double x, double y, double z, double w,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, T3> zipl,
                Func<double, T3, T3, TRes> wipl)
        {
            return Sample16(x, y, z, w, xipl, yipl, zipl, wipl,
                            Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped,
                            Tensor.Index2SamplesClamped, Tensor.Index2SamplesClamped);
        }

        public TRes Sample16<T1, T2, T3, TRes>(
                V4d v,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, T3> zipl,
                Func<double, T3, T3, TRes> wipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_yIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_zIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_wIndexFun)
        {
            return Sample16(v.X, v.Y, v.Z, v.W, xipl, yipl, zipl, wipl,
                            index_min_max_delta_xIndexFun,
                            index_min_max_delta_yIndexFun,
                            index_min_max_delta_zIndexFun,
                            index_min_max_delta_wIndexFun);
        }
            
        /// <summary>
        /// Sample the tensor4 using 16 neighbouring samples and the
        /// supplied interpolation functions and border handling functions.
        /// If only integer coordinates are supplied the returned value is
        /// the same as the indexer of the tensor4, i.e. voxel centers are
        /// assumed to be on integer coordinates.
        /// </summary>
        public TRes Sample16<T1, T2, T3, TRes>(
                double x, double y, double z, double w,
                Func<double, __vtn__, __vtn__, T1> xipl,
                Func<double, T1, T1, T2> yipl,
                Func<double, T2, T2, T3> zipl,
                Func<double, T3, T3, TRes> wipl,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_yIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_zIndexFun,
                Func<long, long, long, long, Tup2<long>> index_min_max_delta_wIndexFun)
        {
            double xid = Fun.Floor(x); long xi = (long)xid; double xf = x - xid; long dx1 = Info.Delta.X;
            double yid = Fun.Floor(y); long yi = (long)yid; double yf = y - yid; long dy1 = Info.Delta.Y;
            double zid = Fun.Floor(z); long zi = (long)zid; double zf = z - zid; long dz1 = Info.Delta.Z;
            double wid = Fun.Floor(w); long wi = (long)wid; double wf = w - wid; long dw1 = Info.Delta.W;
            long i = Info.Origin + xi * dx1 + yi * dy1 + zi * dz1 + wi * dw1;
            var dx = index_min_max_delta_xIndexFun(xi, FX, EX, dx1); dx.E0 += i; dx.E1 += i;
            var dyzw = Fun.OuterSum(index_min_max_delta_yIndexFun(yi, FY, EY, dy1),
                                    index_min_max_delta_zIndexFun(zi, FZ, EZ, dz1),
                                    index_min_max_delta_wIndexFun(wi, FW, EW, dw1));
            //# if (dt == vt) {
            return wipl(wf, zipl(zf, yipl(yf, xipl(xf, Data[dx.E0 + dyzw.E0], Data[dx.E1 + dyzw.E0]),
                                              xipl(xf, Data[dx.E0 + dyzw.E1], Data[dx.E1 + dyzw.E1])),
                                     yipl(yf, xipl(xf, Data[dx.E0 + dyzw.E2], Data[dx.E1 + dyzw.E2]),
                                              xipl(xf, Data[dx.E0 + dyzw.E3], Data[dx.E1 + dyzw.E3]))),
                            zipl(zf, yipl(yf, xipl(xf, Data[dx.E0 + dyzw.E4], Data[dx.E1 + dyzw.E4]),
                                              xipl(xf, Data[dx.E0 + dyzw.E5], Data[dx.E1 + dyzw.E5])),
                                     yipl(yf, xipl(xf, Data[dx.E0 + dyzw.E6], Data[dx.E1 + dyzw.E6]),
                                              xipl(xf, Data[dx.E0 + dyzw.E7], Data[dx.E1 + dyzw.E7]))));
            //# } else {
            return wipl(wf, zipl(zf, yipl(yf, xipl(xf, Getter(Data, dx.E0 + dyzw.E0), Getter(Data, dx.E1 + dyzw.E0)),
                                              xipl(xf, Getter(Data, dx.E0 + dyzw.E1), Getter(Data, dx.E1 + dyzw.E1))),
                                     yipl(yf, xipl(xf, Getter(Data, dx.E0 + dyzw.E2), Getter(Data, dx.E1 + dyzw.E2)),
                                              xipl(xf, Getter(Data, dx.E0 + dyzw.E3), Getter(Data, dx.E1 + dyzw.E3)))),
                            zipl(zf, yipl(yf, xipl(xf, Getter(Data, dx.E0 + dyzw.E4), Getter(Data, dx.E1 + dyzw.E4)),
                                              xipl(xf, Getter(Data, dx.E0 + dyzw.E5), Getter(Data, dx.E1 + dyzw.E5))),
                                     yipl(yf, xipl(xf, Getter(Data, dx.E0 + dyzw.E6), Getter(Data, dx.E1 + dyzw.E6)),
                                              xipl(xf, Getter(Data, dx.E0 + dyzw.E7), Getter(Data, dx.E1 + dyzw.E7)))));
            //# }
        }

        //# }
        #endregion

        //# if (d == 2) {
        #region Drawing

        /// <summary>
        /// Draws a horizontal line. The parameters x0, x1, and y are rounded
        /// to the nearest integer coordinates.
        /// </summary>
        public void SetLineX(double x0, double x1, double y, __vtn__ value)
        {
            SetLineX((long)(x0 + 0.5), (long)(x1 + 0.5), (long)(y + 0.5), value);
        }

        /// <summary>
        /// Draws a vertical line. The parameters x, y0, and y1 are rounded
        /// to the nearest integer coordinates.
        /// </summary>
        public void SetLineY(double x, double y0, double y1, __vtn__ value)
        {
            SetLineY((long)(x + 0.5), (long)(y0 + 0.5), (long)(y1 + 0.5), value);
        }

        /// <summary>
        /// Draws a horizontal line.
        /// </summary>
        public void SetLineX(long x0, long x1, long y, __vtn__ value)
        {
            long xmin = Fun.Max(Fun.Min(x0, x1), Info.First.X);
            long xmax = Fun.Min(Fun.Max(x0, x1), Info.First.X + Info.Size.X - 1);
            if (y >= Info.First.Y && y < Info.First.Y + Info.Size.Y)
                for (long x = xmin; x <= xmax; x++) this[x, y] = value;
        }

        /// <summary>
        /// Draws a vertical line.
        /// </summary>
        public void SetLineY(long x, long y0, long y1, __vtn__ value)
        {
            long ymin = Fun.Max(Fun.Min(y0, y1), Info.First.Y);
            long ymax = Fun.Min(Fun.Max(y0, y1), Info.First.Y + Info.Size.Y - 1);
            if (x >= Info.First.X && x < Info.First.X + Info.Size.X)
                for (long y = ymin; y <= ymax; y++) this[x, y] = value;
        }

        /// <summary>
        /// Draws a rectangular frame. The corner positions are
        /// rounded to the nearest integer coordinates.
        /// </summary>
        public void SetRectangle(V2d p0, V2d p1, __vtn__ value)
        {
            SetRectangle((long)(p0.X + 0.5), (long)(p0.Y + 0.5),
                         (long)(p1.X + 0.5), (long)(p1.Y + 0.5), value);
        }
        /// <summary>
        /// Draws a rectangular frame. The parameters x0, x1, y0, and y1 are
        /// rounded to the nearest integer coordinates.
        /// </summary>
        public void SetRectangle(double x0, double y0, double x1, double y1, __vtn__ value)
        {
            SetRectangle((long)(x0 + 0.5), (long)(y0 + 0.5),
                         (long)(x1 + 0.5), (long)(y1 + 0.5), value);
        }

        /// <summary>
        /// Draws a rectangular frame given the coordinates of two opposing corners.
        /// </summary>
        public void SetRectangle(V2i p0, V2i p1, __vtn__ value)
        {
            SetRectangle((long)p0.X, (long)p0.Y, (long)p1.X, (long)p1.Y, value);
        }

        /// <summary>
        /// Draws a rectangular frame given the coordinates of two opposing corners.
        /// </summary>
        public void SetRectangle(V2l v0, V2l v1, __vtn__ value)
        {
            SetRectangle(v0.X, v0.Y, v1.X, v1.Y, value);
        }

        /// <summary>
        /// Draws a rectangular frame given the coordinates of two opposing corners.
        /// </summary>
        public void SetRectangle(long x0, long y0, long x1, long y1, __vtn__ value)
        {
            long ymin = Fun.Min(y0, y1), ymax = Fun.Max(y0, y1);
            SetLineX(x0, x1, ymin, value);
            SetLineY(x0, ymin + 1L, ymax - 1L, value);
            SetLineY(x1, ymin + 1L, ymax - 1L, value);
            SetLineX(x0, x1, ymax, value);
        }

        /// <summary>
        /// Sets the supplied rectangle and its interior to the supplied value.
        /// Note that the coordinates are rounded to the nearest integer coordinate.
        /// </summary>
        public void SetRectangleFilled(V2d p0, V2d p1, __vtn__ value)
        {
            SetRectangleFilled((long)(p0.X + 0.5), (long)(p0.Y + 0.5),
                               (long)(p1.X + 0.5), (long)(p1.Y + 0.5), value);
        }

        /// <summary>
        /// Sets the supplied rectangle and its interior to the supplied value.
        /// Note that the coordinates are rounded to the nearest integer coordinate.
        /// </summary>
        public void SetRectangleFilled(double x0, double y0, double x1, double y1, __vtn__ value)
        {
            SetRectangleFilled((long)(x0 + 0.5), (long)(y0 + 0.5),
                               (long)(x1 + 0.5), (long)(y1 + 0.5), value);
        }

        /// <summary>
        /// Sets the supplied rectangle given by its minimal and maximal pixel
        /// position vectors and its interior to the supplied value.
        /// </summary>
        public void SetRectangleFilled(V2i p0, V2i p1, __vtn__ value)
        {
            SetRectangleFilled((long)p0.X, (long)p0.Y, (long)p1.X, (long)p1.Y, value);
        }

        /// <summary>
        /// Sets the supplied rectangle given by its minimal and maximal pixel
        /// position vectors and its interior to the supplied value.
        /// </summary>
        public void SetRectangleFilled(V2l p0, V2l p1, __vtn__ value)
        {
            SetRectangleFilled(p0.X, p0.Y, p1.X, p1.Y, value);
        }

        /// <summary>
        /// Sets the supplied rectangle given by its minimal and maximal pixel
        /// coordinates and its interior to the supplied value.
        /// </summary>
        public void SetRectangleFilled(long x0, long y0, long x1, long y1, __vtn__ value)
        {
            long xmin = Fun.Max(Fun.Min(x0, x1), Info.First.X);
            long xmax = Fun.Min(Fun.Max(x0, x1), Info.First.X + Info.Size.X - 1);
            if (xmin > xmax) return;
            long ymin = Fun.Max(Fun.Min(y0, y1), Info.First.Y);
            long ymax = Fun.Min(Fun.Max(y0, y1), Info.First.Y + Info.Size.Y - 1);
            if (ymin > ymax) return;

            SubMatrix(x0, y0, 1 + x1 - x0, 1 + y1 - y0).Set(value);
        }

        /// <summary>
        /// Draws a vertical/horizontal cross around p. 
        /// </summary>
        public void SetCross(V2d p, double radius, __vtn__ value)
        {
            SetCross((long)(p.X + 0.5), (long)(p.Y + 0.5), (long)(radius + 0.5), value);
        }

        /// <summary>
        /// Draws a vertical/horizontal cross around [x,y].
        /// </summary>
        public void SetCross(double x, double y, double radius, __vtn__ value)
        {
            SetCross((long)(x + 0.5), (long)(y + 0.5), (long)(radius + 0.5), value);
        }

        /// <summary>
        /// Draws a vertical/horizontal cross around p.
        /// </summary>
        public void SetCross(V2i p, int radius, __vtn__ value)
        {
            SetCross((long)p.X, (long)p.Y, (long)radius, value);
        }

        /// <summary>
        /// Draws a vertical/horizontal cross around p.
        /// </summary>
        public void SetCross(V2l p, long radius, __vtn__ value)
        {
            SetCross(p.X, p.Y, radius, value);
        }

        /// <summary>
        /// Draws a vertical/horizontal cross around [x, y].
        /// </summary>
        public void SetCross(long x, long y, long radius, __vtn__ value)
        {
            SetLineX(x - radius, x + radius, y, value);
            SetLineY(x, y - radius, y + radius, value);
        }

        /// <summary>
        /// Draws a X-style cross around [x, y]. Note that radius
        /// is the horizontal/vertical distance of the corner.
        /// </summary>
        public void SetCrossX(V2d p, double radius, __vtn__ value)
        {
            SetCrossX((long)(p.X + 0.5), (long)(p.Y + 0.5), (long)(radius + 0.5), value);
        }

        /// <summary>
        /// Draws a X-style cross around [x, y]. Note that radius
        /// is the horizontal/vertical distance of the corner.
        /// </summary>
        public void SetCrossX(double x, double y, double radius, __vtn__ value)
        {
            SetCrossX((long)(x + 0.5), (long)(y + 0.5), (long)(radius + 0.5), value);
        }

        /// <summary>
        /// Draws a X-style cross around [x, y]. Note that radius
        /// is the horizontal/vertical distance of the corner.
        /// </summary>
        public void SetCrossX(V2i p, int radius, __vtn__ value)
        {
            SetCrossX((long)p.X, (long)p.Y, (long)radius, value);
        }

        /// <summary>
        /// Draws a X-style cross around [x, y]. Note that radius
        /// is the horizontal/vertical distance of the corner.
        /// </summary>
        public void SetCrossX(V2l p, long radius, __vtn__ value)
        {
            SetCrossX(p.X, p.Y, radius, value);
        }

        /// <summary>
        /// Draws a X-style cross around [x, y]. Note that radius
        /// is the horizontal/vertical distance of the corner.
        /// </summary>
        public void SetCrossX(long x, long y, long radius, __vtn__ value)
        {
            SetLine(x - radius, y - radius, x + radius, y + radius, value);
            SetLine(x + radius, y - radius, x - radius, y + radius, value);
        }

        public void SetSquare(
                V2d p, double radius, __vtn__ value)
        {
            SetSquare((long)(p.X + 0.5), (long)(p.Y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetSquare(
                double x, double y, double radius, __vtn__ value)
        {
            SetSquare((long)(x + 0.5), (long)(y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetSquare(
                V2i p, int radius, __vtn__ value)
        {
            SetSquare((long)p.X, (long)p.Y, (long)radius, value);
        }

        public void SetSquare(
                V2l p, long radius, __vtn__ value)
        {
            SetSquare(p.X, p.Y, radius, value);
        }

        public void SetSquare(
                long x, long y, long radius, __vtn__ value)
        {
            SetRectangle(x - radius, y - radius, x + radius, y + radius, value);
        }

        public void SetSquareFilled(
                V2d p, double radius, __vtn__ value)
        {
            SetSquareFilled((long)(p.X + 0.5), (long)(p.Y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetSquareFilled(
                double x, double y, double radius, __vtn__ value)
        {
            SetSquareFilled((long)(x + 0.5), (long)(y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetSquareFilled(
                V2i p, int radius, __vtn__ value)
        {
            SetSquareFilled((long)p.X, (long)p.Y, (long)radius, value);
        }

        public void SetSquareFilled(
                V2l p, long radius, __vtn__ value)
        {
            SetSquareFilled(p.X, p.Y, radius, value);
        }

        public void SetSquareFilled(
                long x, long y, long radius, __vtn__ value)
        {
            SetRectangleFilled(x - radius, y - radius, x + radius, y + radius, value);
        }

        public void SetLine(
                V2d p0, V2d p1, __vtn__ value)
        {
            SetLine((long)(p0.X + 0.5), (long)(p0.Y + 0.5), (long)(p1.X + 0.5), (long)(p1.Y + 0.5), value);
        }

        public void SetLine(
                double x0, double y0, double x1, double y1, __vtn__ value)
        {
            SetLine((long)(x0 + 0.5), (long)(y0 + 0.5), (long)(x1 + 0.5), (long)(y1 + 0.5), value);
        }

        public void SetLine(
                V2i p0, V2i p1, __vtn__ value)
        {
            SetLine((long)p0.X, (long)p0.Y, (long)p1.X, (long)p1.Y, value);
        }

        public void SetLine(
                V2l p0, V2l p1, __vtn__ value)
        {
            SetLine(p0.X, p0.Y, p1.X, p1.Y, value);
        }

        /// <summary>
        ///  Bresenham Line Algorithm
        /// </summary>
        public void SetLine(
                long x0, long y0, long x1, long y1, __vtn__ value)
        {
            //-- Trivial Cases
            if (x0 == x1) { SetLineY(x0, y0, y1, value); return; }
            if (y0 == y1) { SetLineX(x0, x1, y1, value); return; }

            //-- Bresenham
            long dx, dy;
            long incx, incy;

            if (x1 >= x0)
            {
                dx = x1 - x0; incx = 1;
            }
            else
            {
                dx = x0 - x1; incx = -1;
            }

            if (y1 >= y0)
            {
                dy = y1 - y0;  incy = 1;
            }
            else
            {
                dy = y0 - y1; incy = -1;
            }

            long xmin = Info.First.X, xmax = xmin + Info.Size.X - 1;
            long ymin = Info.First.Y, ymax = ymin + Info.Size.Y - 1;
            bool xok = x0 >= xmin && x0 <= xmax;
            bool yok = y0 >= ymin && y0 <= ymax;
            long balance;

            if (dx >= dy)
            {
                dy <<= 1;
                balance = dy - dx;
                dx <<= 1;

                while (x0 != x1)
                {
                    if (xok && yok) this[x0, y0] = value;
                    if (balance >= 0)
                    {
                        y0 += incy; yok = y0 >= ymin && y0 <= ymax;
                        balance -= dx;
                    }
                    balance += dy;
                    x0 += incx; xok = x0 >= xmin && x0 <= xmax;
                }
                if (xok && yok) this[x0, y0] = value;
            }
            else
            {
                dx <<= 1;
                balance = dx - dy;
                dy <<= 1;

                while (y0 != y1)
                {
                    if (xok && yok) this[x0, y0] = value;
                    if (balance >= 0)
                    {
                        x0 += incx; xok = x0 >= xmin && x0 <= xmax;
                        balance -= dy;
                    }
                    balance += dx;
                    y0 += incy; yok = y0 >= ymin && y0 <= ymax;
                }
                if (xok && yok) this[x0, y0] = value;
            }
        }

        /// <summary>
        /// Set all pixels whose square region is intersected by the line from p0 to p1
        /// to the supplied value. Does not perform any bounds checks.
        /// </summary>
        public void SetLineAllTouchedRaw(V2d p0, V2d p1, __vtn__ value)
        {
            SetLineAllTouchedRaw(p0.X, p0.Y, p1.X, p1.Y, value);
        }

        /// <summary>
        /// Set all pixels whose square region is intersected by the line from (x0,y0) to (x1,y1)
        /// to the supplied value. Does not perform any bounds checks.
        /// </summary>
        public void SetLineAllTouchedRaw(double x0, double y0, double x1, double y1, __vtn__ value)
        {
            var ix = (long)Fun.Floor(x0);
            var iy = (long)Fun.Floor(y0);
            var dx = x1 - x0;
            var dy = y1 - y0;

            var len = Fun.Sqrt(dx * dx + dy * dy);

            double tx, ty, dtx, dty;
            long dix, diy;

            if (dx > 0.0)       { dtx = len / dx; tx = dtx * (1.0 - (x0 - ix)); dix = Info.DX; }
            else if (dx < 0.0)  { dtx = -len / dx; tx = dtx * (x0 - ix); dix = -Info.DX; }
            else                { dtx = 0.0; tx = len; dix = 0; }

            if (dy > 0.0)       { dty = len / dy; ty = dty * (1.0 - (y0 - iy)); diy = Info.DY; }
            else if (dy < 0.0)  { dty = -len / dy; ty = dty * (y0 - iy); diy = -Info.DY; }
            else                { dty = 0.0; ty = len; diy = 0; }

            var mdata = Data;/*# if (dt != vt) { */ var mset = Setter;/*# } */
            double t = 0.0;
            long index = Info.Origin + ix * Info.DX + iy * Info.DY;
            do
            {
                //# if (dt != vt) {
                mset(mdata, index, value);
                //# } else {
                mdata[index] = value;
                //# }
                if (tx < ty)    { t = tx; tx += dtx; index += dix; }
                else            { t = ty; ty += dty; index += diy; }
            }
            while (t < len);
        }

        public void SetCircle(
                V2d p, double radius, __vtn__ value)
        {
            SetCircle((long)(p.X + 0.5), (long)(p.Y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetCircle(
                double x, double y, double radius, __vtn__ value)
        {
            SetCircle((long)(x + 0.5), (long)(y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetCircle(
                V2i p, int radius, __vtn__ value)
        {
            SetCircle((long)p.X, (long)p.Y, (long)radius, value);
        }

        public void SetCircle(
                V2l p, long radius, __vtn__ value)
        {
            SetCircle(p.X, p.Y, radius, value);
        }

        public void SetCircle(
                long x, long y, long radius, __vtn__ value)
        {
            long f = 1 - radius;
            long ddF_x = 0;
            long ddF_y = -2 * radius;
            long rx = 0;
            long ry = radius;

            long xmin = Info.First.X, xmax = xmin + Info.Size.X - 1;
            long ymin = Info.First.Y, ymax = ymin + Info.Size.Y - 1;

            if (x >= xmin && x <= xmax)
            {
                var ym = y - radius; if (ym >= ymin && ym <= ymax) this[x, ym] = value;
                var yp = y + radius; if (yp >= ymin && yp <= ymax) this[x, yp] = value;
            }
            if (y >= xmin && y <= ymax)
            {
                var xm = x - radius; if (xm >= xmin && xm <= xmax) this[xm, y] = value;
                var xp = x + radius; if (xp >= xmin && xp <= xmax) this[xp, y] = value;
            }
            bool xmxOk = true, xpxOk = true, xmyOk = true, xpyOk = true;
            while (rx < ry)
            {
                if (f >= 0)
                {
                    ry--; ddF_y += 2; f += ddF_y;
                }
                rx++;
                ddF_x += 2;
                f += ddF_x + 1;

                long xmx = x - rx; xmxOk = xmx >= xmin && xmx <= xmax;
                long xpx = x + rx; xpxOk = xpx >= xmin && xpx <= xmax;
                long xmy = x - ry; xmyOk = xmy >= xmin && xmy <= xmax;
                long xpy = x + ry; xpyOk = xpy >= xmin && xpy <= xmax;
                long ymx = y - rx, ypx = y + rx;
                long ymy = y - ry, ypy = y + ry;

                if (ymx >= ymin && ymx <= ymax)
                {
                    if (xmyOk) this[xmy, ymx] = value;
                    if (xpyOk) this[xpy, ymx] = value;
                }
                if (ypx >= ymin && ypx <= ymax)
                {
                    if (xmyOk) this[xmy, ypx] = value;
                    if (xpyOk) this[xpy, ypx] = value;
                }
                if (ymy >= ymin && ymy <= ymax)
                {
                    if (xmxOk) this[xmx, ymy] = value;
                    if (xpxOk) this[xpx, ymy] = value;
                }
                if (ypy >= ymin && ypy <= ymax)
                {
                    if (xmxOk) this[xmx, ypy] = value;
                    if (xpxOk) this[xpx, ypy] = value;
                }
            }
        }

        public void SetCircleFilled(
                V2d p, double radius, __vtn__ value)
        {
            SetCircleFilled((long)(p.X + 0.5), (long)(p.Y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetCircleFilled(
                double x, double y, double radius, __vtn__ value)
        {
            SetCircleFilled((long)(x + 0.5), (long)(y + 0.5), (long)(radius + 0.5), value);
        }

        public void SetCircleFilled(
                V2i p, int radius, __vtn__ value)
        {
            SetCircleFilled((long)p.X, (long)p.Y, (long)radius, value);
        }

        public void SetCircleFilled(
                V2l p, long radius, __vtn__ value)
        {
            SetCircleFilled(p.X, p.Y, radius, value);
        }

        public void SetCircleFilled(
                long x0, long y0, long radius, __vtn__ value)
        {
            long x = radius;
            long y = 0;
            long xChange = 1 - (radius << 1);
            long yChange = 0;
            long radiusError = 0;

            long xmin = Info.First.X, xmax = xmin + Info.Size.X - 1;
            long ymin = Info.First.Y, ymax = ymin + Info.Size.Y - 1;

            long mdx = DX; var mdata = Data;/*# if (dt != vt) { */ var mset = Setter;/*# } */

            while (x >= y)
            {
                if (y0 + y >= ymin && y0 + y <= ymax)
                {
                    var xb = Fun.Max(x0 - x, xmin); var xe = Fun.Min(x0 + x, xmax);
                    if (xb <= xe)
                        for (long i = Info.Index(xb, y0 + y), e = Info.Index(xe + 1, y0 + y); i != e; i += mdx)
                        //# if (dt != vt) {
                        mset(mdata, i, value);
                        //# } else {
                        mdata[i] = value;
                        //# }
                }
                if (y0 - y >= ymin && y0 - y <= ymax)
                {
                    var xb = Fun.Max(x0 - x, xmin); var xe = Fun.Min(x0 + x, xmax);
                    if (xb <= xe)
                        for (long i = Info.Index(xb, y0 - y), e = Info.Index(xe + 1, y0 - y); i != e; i += mdx)
                            //# if (dt != vt) {
                            mset(mdata, i, value);
                            //# } else {
                            mdata[i] = value;
                            //# }
                }
                if (y0 + x >= ymin && y0 + x <= ymax)
                {
                    var xb = Fun.Max(x0 - y, xmin); var xe = Fun.Min(x0 + y, xmax);
                    if (xb <= xe)
                        for (long i = Info.Index(xb, y0 + x), e = Info.Index(xe + 1, y0 + x); i != e; i += mdx)
                            //# if (dt != vt) {
                            mset(mdata, i, value);
                            //# } else {
                            mdata[i] = value;
                            //# }
                }
                if (y0 - x >= ymin && y0 - x <= ymax)
                {
                    var xb = Fun.Max(x0 - y, xmin); var xe = Fun.Min(x0 + y, xmax);
                    if (xb <= xe)
                        for (long i = Info.Index(xb, y0 - x), e = Info.Index(xe + 1, y0 - x); i != e; i += mdx)
                            //# if (dt != vt) {
                            mset(mdata, i, value);
                            //# } else {
                            mdata[i] = value;
                            //# }
                }
                y++;
                radiusError += yChange;
                yChange += 2;
                if (((radiusError << 1) + xChange) > 0)
                {
                    x--;
                    radiusError += xChange;
                    xChange += 2;
                }
            }
        }

        //# foreach (var hasVertexArray in new[] { false, true }) {
        /// <summary>
        /// Set all matrix pixels inside the specified polygon to the supplied
        /// value. This is a raw routine that does no clipping and assumes that
        /// all pixels that need to be set are within the matrix. The polygon
        /// is assumed to consist of two y-monotone point sequences between
        /// the points with minimum and maximum y-coordinate. Note that convex
        /// and non-concave polygons automatically fullfill this condition.
        /// Rasterization is performed according to DirextX11 rules.
        /// </summary>
        //# var polygon = hasVertexArray ? "vertexArray[polygon[" : "polygon[";
        //# var q = hasVertexArray ? "]]" : "]";
        //# var pt = hasVertexArray ? "int" : "V2d";
        public void SetMonotonePolygonFilledRaw(
                __pt__[] polygon,/*# if (hasVertexArray) { */ V2d[] vertexArray,/*# } */
                __vtn__ value,
                Winding winding = Winding.CCW,
                double eps = 1e-8)
        {
            var pc = polygon.Length;
            int diLeft = winding == Winding.CCW ? 1 : pc - 1;
            int diRight = winding == Winding.CCW ? pc - 1 : 1;
            int eLeft = 0, end = 0;
            double yeLeft = __polygon__0__q__.Y;
            for (int pi = 1; pi < pc; pi++) // find min in (y)eLeft and max in end
            {
                var y = __polygon__pi__q__.Y;
                if (y < yeLeft) { eLeft = pi; yeLeft = y; }
                if (y > __polygon__end__q__.Y) end = pi;
            }
            int eRight = eLeft, sLeft = 0, sRight = 0;
            double yeRight = yeLeft;
            double xfLeft = 0, fdeltaLeft = 0.0, xfRight = 0, fdeltaRight = 0.0;
            long iLeft = 0, ideltaLeft = 0, iRight = 0, ideltaRight = 0;
            long mdx = DX, mdy = DY; var mdata = Data;/*# if (dt != vt) { */ var mset = Setter;/*# } */
            long yi = (long)Fun.Ceiling(yeLeft - 0.5), yieLeft = yi, yieRight = yi;
            var lineOrigin = Origin + yi * mdy;
            bool exit = false;
            while (true)
            {
                if (yi == yieLeft) // next left edge
                {
                    double ysLeft = 0.0;
                    do
                    {
                        sLeft = eLeft; if (sLeft == end) { exit = true; break; }
                        eLeft = (sLeft + diLeft) % pc;
                        ysLeft = yeLeft;
                        yeLeft = __polygon__eLeft__q__.Y;
                        yieLeft = (long)Fun.Ceiling(yeLeft - 0.5);
                    }
                    while (yi == yieLeft); // ensure we have an edge that makes a difference
                    if (exit) break;
                    double deltaLeft = yeLeft - ysLeft;
                    if (deltaLeft < 0.0) { exit = true; break; }
                    if (deltaLeft < eps) deltaLeft = eps;
                    deltaLeft = (__polygon__eLeft__q__.X - __polygon__sLeft__q__.X) / deltaLeft;
                    ideltaLeft = (long)Fun.Ceiling(deltaLeft);
                    fdeltaLeft = deltaLeft - (double)ideltaLeft;
                    double xLeft = __polygon__sLeft__q__.X + (0.5 + (double)yi - ysLeft) * deltaLeft;
                    iLeft = (long)Fun.Ceiling(xLeft - 0.5);
                    xfLeft = xLeft - (double)iLeft + 0.5 - eps; // offset for stable 0.0 comparison
                    iLeft = lineOrigin + iLeft * mdx;
                    ideltaLeft = mdy + ideltaLeft * mdx;
                }
                if (exit) break;
                if (yi == yieRight) // next right edge
                {
                    double ysRight = 0.0;
                    do
                    {
                        sRight = eRight; if (sRight == end) { exit = true; break; }
                        eRight = (sRight + diRight) % pc;
                        ysRight = yeRight;
                        yeRight = __polygon__eRight__q__.Y;
                        yieRight = (long)Fun.Ceiling(yeRight - 0.5);
                    }
                    while (yi == yieRight); // ensure we have an edge that makes a difference
                    if (exit) break;
                    double deltaRight = yeRight - ysRight;
                    if (deltaRight < 0.0) { exit = true; break; }
                    if (deltaRight < eps) deltaRight = eps;
                    deltaRight = (__polygon__eRight__q__.X - __polygon__sRight__q__.X) / deltaRight;
                    ideltaRight = (long)Fun.Ceiling(deltaRight);
                    fdeltaRight = deltaRight - (double)ideltaRight;
                    double xRight = __polygon__sRight__q__.X + (0.5 + (double)yi - ysRight) * deltaRight;
                    iRight = (long)Fun.Ceiling(xRight - 0.5);
                    xfRight = xRight - (double)iRight + 0.5 - eps; // offset for stable 0.0 comparison
                    iRight = lineOrigin + iRight * mdx;
                    ideltaRight = mdy + ideltaRight * mdx;
                }
                if (exit) break;
                long yimin = Fun.Min(yieLeft, yieRight);
                while (yi < yimin)
                {
                    for (long i = iLeft; i < iRight; i += mdx)
                    {
                        //# if (dt != vt) {
                        mset(mdata, i, value);
                        //# } else {
                        mdata[i] = value;
                        //# }
                    }
                    iLeft += ideltaLeft; xfLeft += fdeltaLeft;
                    if (xfLeft < 0.0) { xfLeft += 1.0; iLeft -= mdx; }
                    iRight += ideltaRight; xfRight += fdeltaRight;
                    if (xfRight < 0.0) { xfRight += 1.0; iRight -= mdx; }
                    ++yi; lineOrigin += mdy;
                }
            }
        }

        //# } // hasVertexArray
        #endregion

        //# }
        #region I__ttn__

        /// <summary>
        /// Dimension of the generic __ttn__.
        /// </summary>
        public __itn__ Dim { get { return Info.Size; } }

        public object GetValue(/*# iaa.ForEach(a => { */long __a__/*# }, comma); */)
        {
            return (object)this[/*# iaa.ForEach(a => { */__a__/*# }, comma); */];
        }

        public void SetValue(object value, /*# iaa.ForEach(a => { */long __a__/*# }, comma); */)
        {
            this[/*# iaa.ForEach(a => { */__a__/*# }, comma); */] = (__vtn__)value;
        }

        //# if (d > 1) {
        public object GetValue(__itn__ v)
        {
            return (object)this[v];
        }

        public void SetValue(object value, __itn__ v)
        {
            this[v] = (__vtn__)value;
        }

        //#}
        #endregion

    }

    #endregion

    //# }); // tt
}