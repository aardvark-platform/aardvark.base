using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Capsule3f

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Capsule3f : IEquatable<Capsule3f>, IValidity, IBoundingBox3f
    {
        [DataMember]
        public V3f P0;
        [DataMember]
        public V3f P1;
        [DataMember]
        public float Radius;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3f(V3f p0, V3f p1, float radius)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3f(Line3f axis, float radius)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3f(Capsule3f capsule)
        {
            P0 = capsule.P0;
            P1 = capsule.P1;
            Radius = capsule.Radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3f(Capsule3d capsule)
        {
            P0 = (V3f)capsule.P0;
            P1 = (V3f)capsule.P1;
            Radius = (float)capsule.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Capsule3f(Capsule3d c)
            => new Capsule3f(c);

        #endregion

        #region Constants

        public static readonly Capsule3f Invalid = new Capsule3f(V3f.NaN, V3f.NaN, -1);

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

        public Line3f Axis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Line3f(P0, P1);
        }

        public Cylinder3f Cylider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Cylinder3f(P0, P1, Radius);
        }

        #endregion

        #region IBoundingBox3f Members

        public Box3f BoundingBox3f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box3f(new Sphere3f(P0, Radius).BoundingBox3f, new Sphere3f(P1, Radius).BoundingBox3f);
        }

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => HashCode.GetCombined(P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Capsule3f other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Capsule3f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Capsule3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Capsule3f(V3f.Parse(x[0]), V3f.Parse(x[1]), float.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

    }

    #endregion

    #region Capsule3d

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Capsule3d : IEquatable<Capsule3d>, IValidity, IBoundingBox3d
    {
        [DataMember]
        public V3d P0;
        [DataMember]
        public V3d P1;
        [DataMember]
        public double Radius;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3d(V3d p0, V3d p1, double radius)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3d(Line3d axis, double radius)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3d(Capsule3d capsule)
        {
            P0 = capsule.P0;
            P1 = capsule.P1;
            Radius = capsule.Radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Capsule3d(Capsule3f capsule)
        {
            P0 = (V3d)capsule.P0;
            P1 = (V3d)capsule.P1;
            Radius = (double)capsule.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Capsule3d(Capsule3f c)
            => new Capsule3d(c);

        #endregion

        #region Constants

        public static readonly Capsule3d Invalid = new Capsule3d(V3d.NaN, V3d.NaN, -1);

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

        public Line3d Axis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Line3d(P0, P1);
        }

        public Cylinder3d Cylider
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Cylinder3d(P0, P1, Radius);
        }

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box3d(new Sphere3d(P0, Radius).BoundingBox3d, new Sphere3d(P1, Radius).BoundingBox3d);
        }

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => HashCode.GetCombined(P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Capsule3d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Capsule3d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Capsule3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Capsule3d(V3d.Parse(x[0]), V3d.Parse(x[1]), double.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

    }

    #endregion

}
