using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule3d : IEquatable<Capsule3d>, IValidity, IBoundingBox3d
    {
        [DataMember]
        public V3d P0;
        [DataMember]
        public V3d P1;
        [DataMember]
        public double Radius;

        #region Constructors

        public Capsule3d(V3d p0, V3d p1, double radius)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
        }

        public Capsule3d(Line3d axis, double radius)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
        }

        #endregion

        #region Constants

        public static readonly Capsule3d Invalid = new Capsule3d(V3d.NaN, V3d.NaN, -1.0);

        #endregion

        #region Properties

        public bool IsValid => Radius >= 0.0;
        public bool IsInvalid => Radius < 0.0;

        public Line3d Axis => new Line3d(P0, P1);

        public Cylinder3d Cylider => new Cylinder3d(P0, P1, Radius);

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d => new Box3d(
            new Sphere3d(P0, Radius).BoundingBox3d,
            new Sphere3d(P1, Radius).BoundingBox3d);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Capsule3d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && Radius.Equals(other.Radius);

        public override bool Equals(object other)
            => (other is ObliqueCone3d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

        public static Capsule3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Capsule3d(V3d.Parse(x[0]), V3d.Parse(x[1]), double.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

    }
}
