using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Capsule3" + tc;
    //#   var type2 = "Capsule3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var line3t = "Line3" + tc;
    //#   var cylinder3t = "Cylinder3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var sphere3t = "Sphere3" + tc;
    //#   var iboundingbox = "IBoundingBox3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var fourbythree = isDouble ? "(4.0 / 3.0)" : "(4.0f / 3.0f)";
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    #region __type__

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>, IValidity, __iboundingbox__
    {
        [DataMember]
        public __v3t__ P0;
        [DataMember]
        public __v3t__ P1;
        [DataMember]
        public __ftype__ Radius;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v3t__ p0, __v3t__ p1, __ftype__ radius)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__line3t__ axis, __ftype__ radius)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ capsule)
        {
            P0 = capsule.P0;
            P1 = capsule.P1;
            Radius = capsule.Radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ capsule)
        {
            P0 = (__v3t__)capsule.P0;
            P1 = (__v3t__)capsule.P1;
            Radius = (__ftype__)capsule.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type2__ c)
            => new __type__(c);

        #endregion

        #region Constants

        public static readonly __type__ Invalid = new __type__(__v3t__.NaN, __v3t__.NaN, -1);

        #endregion

        #region Properties

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0.0;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0.0;
        }

        public __line3t__ Axis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __line3t__(P0, P1);
        }

        public __cylinder3t__ Cylider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __cylinder3t__(P0, P1, Radius);
        }

        #endregion

        #region __iboundingbox__ Members

        public __box3t__ BoundingBox3__tc__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __box3t__(new __sphere3t__(P0, Radius).BoundingBox3__tc__, new __sphere3t__(P1, Radius).BoundingBox3__tc__);
        }

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => HashCode.GetCombined(P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__v3t__.Parse(x[0]), __v3t__.Parse(x[1]), __ftype__.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

    }

    #endregion

    //# }
}
