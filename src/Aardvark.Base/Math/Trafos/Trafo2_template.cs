using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var rt in Meta.RealTypes) {
    //#     var tc = rt.Char;
    //#     var type = "Trafo2" + tc;
    //#     var rtype = rt.Name;
    //#     var m33t = "M33" + tc;
    //#     var v2t = "V2" + tc;
    #region __type__

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __type__
    {
        [DataMember]
        public readonly __m33t__ Forward;
        [DataMember]
        public readonly __m33t__ Backward;

        #region Constructors

        public __type__(__m33t__ forward, __m33t__ backward)
        {
            Forward = forward;
            Backward = backward;
        }

        public __type__(__type__ trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        #endregion

        #region Constants

        public static readonly __type__ Identity =
            new __type__(__m33t__.Identity, __m33t__.Identity);

        #endregion

        #region Properties

        public __type__ Inverse => new __type__(Backward, Forward);

        #endregion

        #region Overrides

        public override int GetHashCode()
            => HashCode.GetCombined(Forward, Backward);

        public override bool Equals(object other)
            => (other is __type__) ? (this == (__type__)other) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        #endregion

        #region Static Creator Functions

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(
                __m33t__.Parse(x[0]),
                __m33t__.Parse(x[1])
            );
        }

        public static __type__ Translation(__v2t__ v)
        {
            return new __type__(__m33t__.Translation(v),
                                __m33t__.Translation(-v));
        }

        public static __type__ Translation(__rtype__ dx, __rtype__ dy)
        {
            return new __type__(__m33t__.Translation(dx, dy),
                                __m33t__.Translation(-dx, -dy));
        }

        public static __type__ Scale(__v2t__ sv)
        {
            return new __type__(__m33t__.Scale(sv),
                                __m33t__.Scale(1 / sv));
        }

        public static __type__ Scale(__rtype__ sx, __rtype__ sy)
        {
            return new __type__(__m33t__.Scale(sx, sy),
                                __m33t__.Scale(1 / sx, 1 / sy));
        }

        public static __type__ Scale(__rtype__ s)
        {
            return new __type__(__m33t__.Scale(s),
                                __m33t__.Scale(1 / s));
        }

        public static __type__ Rotation(__rtype__ angleInRadians)
        {
            return new __type__(__m33t__.Rotation(angleInRadians),
                                __m33t__.Rotation(-angleInRadians));
        }

        public static __type__ RotationInDegrees(__rtype__ angleInDegrees)
        {
            return Rotation(Conversion.RadiansFromDegrees(angleInDegrees));
        }

        #endregion

        #region Operators

        public static bool operator ==(__type__ a, __type__ b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        public static bool operator !=(__type__ a, __type__ b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of __type__ multiplicaition is backward
        /// with respect to __m33t__ multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static __type__ operator *(__type__ t0, __type__ t1)
            => new __type__(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion
    }

    #endregion

    //# }
}
