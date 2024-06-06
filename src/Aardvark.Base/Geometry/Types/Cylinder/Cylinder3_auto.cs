using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Cylinder3f

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Cylinder3f : IEquatable<Cylinder3f>, IBoundingBox3f, IValidity
    {
        [DataMember]
        public V3f P0;
        [DataMember]
        public V3f P1;
        [DataMember]
        public float Radius;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3f(V3f p0, V3f p1, float radius)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3f(Line3f axis, float radius)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3f(Cylinder3f c)
        {
            P0 = c.P0;
            P1 = c.P1;
            Radius = c.Radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3f(Cylinder3d c)
        {
            P0 = (V3f)c.P0;
            P1 = (V3f)c.P1;
            Radius = (float)c.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Cylinder3f(Cylinder3d c)
            => new Cylinder3f(c);

        #endregion

        #region Constants

        public static Cylinder3f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Cylinder3f(V3f.NaN, V3f.NaN, -1);
        }

        #endregion

        #region Properties

        public readonly float Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P0 - P1).Length;
        }

        public readonly V3f Center
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P0 + P1) * 0.5f;
        }

        public readonly Line3f Axis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Line3f(P0, P1);
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0;
        }

        public readonly Circle3f Circle0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3f(P0, (P0 - P1).Normalized, Radius);
        }

        public readonly Circle3f Circle1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3f(P1, (P1 - P0).Normalized, Radius);
        }

        public readonly float Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * ConstantF.PiTimesTwo * (Radius + Height);
        }

        public readonly float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius * ConstantF.Pi * Height;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cylinder3f a, Cylinder3f b)
            => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cylinder3f a, Cylinder3f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.GetCombined(P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Cylinder3f other) =>
            P0.Equals(other.P0) &&
            P1.Equals(other.P1) &&
            Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Cylinder3f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cylinder3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Cylinder3f(
                V3f.Parse(x[0]),
                V3f.Parse(x[1]),
                float.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion

        #region Operations

        /// <summary>
        /// P0 has height 0.0, P1 has height 1.0
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetHeight(V3f p)
        {
            var dir = (P1 - P0).Normalized;
            var pp = p.GetClosestPointOn(new Ray3f(P0, dir));
            return (pp - P0).Dot(dir);
        }
        /// <summary>
        /// Get circle at a specific height
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Circle3f GetCircle(float height)
        {
            var dir = (P1 - P0).Normalized;
            return new Circle3f(P0 + height * dir, dir, Radius);
        }

        #endregion

        #region IBoundingBox3f Members

        public readonly Box3f BoundingBox3f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box3f(Circle0.BoundingBox3f, Circle1.BoundingBox3f);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Cylinder3f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cylinder3f a, Cylinder3f b, float tolerance) =>
            ApproximateEquals(a.P0, b.P0, tolerance) &&
            ApproximateEquals(a.P1, b.P1, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Cylinder3f"/> are equal within
        /// ConstantF&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cylinder3f a, Cylinder3f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Cylinder3d

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Cylinder3d : IEquatable<Cylinder3d>, IBoundingBox3d, IValidity
    {
        [DataMember]
        public V3d P0;
        [DataMember]
        public V3d P1;
        [DataMember]
        public double Radius;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3d(V3d p0, V3d p1, double radius)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3d(Line3d axis, double radius)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3d(Cylinder3d c)
        {
            P0 = c.P0;
            P1 = c.P1;
            Radius = c.Radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Cylinder3d(Cylinder3f c)
        {
            P0 = (V3d)c.P0;
            P1 = (V3d)c.P1;
            Radius = (double)c.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Cylinder3d(Cylinder3f c)
            => new Cylinder3d(c);

        #endregion

        #region Constants

        public static Cylinder3d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Cylinder3d(V3d.NaN, V3d.NaN, -1);
        }

        #endregion

        #region Properties

        public readonly double Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P0 - P1).Length;
        }

        public readonly V3d Center
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P0 + P1) * 0.5;
        }

        public readonly Line3d Axis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Line3d(P0, P1);
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0;
        }

        public readonly Circle3d Circle0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3d(P0, (P0 - P1).Normalized, Radius);
        }

        public readonly Circle3d Circle1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle3d(P1, (P1 - P0).Normalized, Radius);
        }

        public readonly double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Constant.PiTimesTwo * (Radius + Height);
        }

        public readonly double Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius * Constant.Pi * Height;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Cylinder3d a, Cylinder3d b)
            => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Cylinder3d a, Cylinder3d b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.GetCombined(P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Cylinder3d other) =>
            P0.Equals(other.P0) &&
            P1.Equals(other.P1) &&
            Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Cylinder3d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cylinder3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Cylinder3d(
                V3d.Parse(x[0]),
                V3d.Parse(x[1]),
                double.Parse(x[2], CultureInfo.InvariantCulture)
            );
        }

        #endregion

        #region Operations

        /// <summary>
        /// P0 has height 0.0, P1 has height 1.0
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double GetHeight(V3d p)
        {
            var dir = (P1 - P0).Normalized;
            var pp = p.GetClosestPointOn(new Ray3d(P0, dir));
            return (pp - P0).Dot(dir);
        }
        /// <summary>
        /// Get circle at a specific height
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Circle3d GetCircle(double height)
        {
            var dir = (P1 - P0).Normalized;
            return new Circle3d(P0 + height * dir, dir, Radius);
        }

        #endregion

        #region IBoundingBox3d Members

        public readonly Box3d BoundingBox3d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box3d(Circle0.BoundingBox3d, Circle1.BoundingBox3d);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Cylinder3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cylinder3d a, Cylinder3d b, double tolerance) =>
            ApproximateEquals(a.P0, b.P0, tolerance) &&
            ApproximateEquals(a.P1, b.P1, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Cylinder3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Cylinder3d a, Cylinder3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

}
