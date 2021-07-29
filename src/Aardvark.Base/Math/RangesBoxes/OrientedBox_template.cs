using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //# for (int n = 2; n <= 3; n++) {
    //#   var m = n + 1;
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "OrientedBox" + n + tc;
    //#   var type2 = "OrientedBox"+ n + tc2;
    //#   var vnt = "V" + n + tc;
    //#   var boxnt = "Box" + n + tc;
    //#   var rotnt = "Rot" + n + tc;
    //#   var euclideannt = "Euclidean" + n + tc;
    //#   var mmmt = "M" + m + m + tc;
    [DataContract]
    public struct __type__
    {
        [DataMember]
        public __boxnt__ Box;
        [DataMember]
        public __euclideannt__ Trafo;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__boxnt__ box)
        {
            Box = box;
            Trafo = __euclideannt__.Identity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__boxnt__ box, __rotnt__ rot)
        {
            Box = box;
            Trafo = new __euclideannt__(rot, __vnt__.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__boxnt__ box, __vnt__ trans)
        {
            Box = box;
            Trafo = new __euclideannt__(__rotnt__.Identity, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__boxnt__ box, __rotnt__ rot, __vnt__ trans)
        {
            Box = box;
            Trafo = new __euclideannt__(rot, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__boxnt__ box, __euclideannt__ trafo)
        {
            Box = box;
            Trafo = trafo;
        }

        public __boxnt__ AxisAlignedBox => Box.Transformed((__mmmt__)Trafo);

        public __vnt__[] Corners
        {
            get
            {
                var m = (__mmmt__)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }
        //# if (n == 2) {

        public __vnt__[] CornersCCW
        {
            get
            {
                var m = (__mmmt__)Trafo;
                return Box.ComputeCornersCCW().Map(p => m.TransformPos(p));
            }
        }
        //# }
    }

    //# } }
}
