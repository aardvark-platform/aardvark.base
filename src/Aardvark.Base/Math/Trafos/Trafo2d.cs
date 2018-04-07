using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Trafo2d
    {
        [DataMember]
        public readonly M33d Forward;
        [DataMember]
        public readonly M33d Backward;

        #region Constructors

        public Trafo2d(M33d forward, M33d backward)
        {
            Forward = forward;
            Backward = backward;
        }

        public Trafo2d(Trafo2d trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        #endregion

        #region Constants

        public static readonly Trafo2d Identity =
            new Trafo2d(M33d.Identity, M33d.Identity);

        #endregion

        #region Properties

        public Trafo2d Inverse => new Trafo2d(Backward, Forward);

        #endregion

        #region Overrides

        public override int GetHashCode()
            => HashCode.GetCombined(Forward, Backward);

        public override bool Equals(object other)
            => (other is Trafo2d) ? (this == (Trafo2d)other) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        #endregion

        #region Static Creator Functions

        public static Trafo2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Trafo2d(
                M33d.Parse(x[0]),
                M33d.Parse(x[1])
            );
        }

        public static Trafo2d Translation(V2d v)
        {
            return new Trafo2d(M33d.Translation(v),
                               M33d.Translation(-v));
        }

        public static Trafo2d Translation(double dx, double dy)
        {
            return new Trafo2d(M33d.Translation(dx, dy),
                               M33d.Translation(-dx, -dy));
        }

        public static Trafo2d Scale(V2d sv)
        {
            return new Trafo2d(M33d.Scale(sv),
                               M33d.Scale(1 / sv));
        }

        public static Trafo2d Scale(double sx, double sy)
        {
            return new Trafo2d(M33d.Scale(sx, sy),
                               M33d.Scale(1 / sx, 1 / sy));
        }

        public static Trafo2d Scale(double s)
        {
            return new Trafo2d(M33d.Scale(s),
                               M33d.Scale(1 / s));
        }

        public static Trafo2d Rotation(double angleInRadians)
        {
            return new Trafo2d(M33d.Rotation(angleInRadians),
                               M33d.Rotation(-angleInRadians));
        }

        public static Trafo2d RotationInDegrees(double angleInDegrees)
        {
            return Rotation(Conversion.RadiansFromDegrees(angleInDegrees));
        }

        #endregion

        #region Operators

        public static bool operator ==(Trafo2d a, Trafo2d b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        public static bool operator !=(Trafo2d a, Trafo2d b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of Trafo2d multiplicaition is backward
        /// with respect to M44d multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo2d operator *(Trafo2d t0, Trafo2d t1)
            => new Trafo2d(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion
    }
}
