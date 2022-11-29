/*
    Copyright 2006-2020. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Circle2f

    /// <summary>
    /// A two dimensional circle represented by center and radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Circle2f : IEquatable<Circle2f>, IBoundingBox2f, IValidity
    {
        [DataMember]
        public V2f Center;

        [DataMember]
        public float Radius;

        #region Constructors

        /// <summary>
        /// Creates circle from center and radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2f(V2f center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Creates circle from center and point on circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2f(V2f center, V2f pointOnCircle)
        {
            Center = center;
            Radius = (pointOnCircle - center).Length;
        }

        /// <summary>
        /// Creates circle from three points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2f(V2f a, V2f b, V2f c)
        {
            var l0 = new Plane2f((b - a).Normalized, (a + b) * 0.5f);
            var l1 = new Plane2f((c - b).Normalized, (b + c) * 0.5f);
            if (l0.Intersects(l1, out Center))
            {
                Radius = (a - Center).Length;
            }
            else
            {
                throw new Exception("Cannot construct circle because given points are collinear.");
            }
        }

        /// <summary>
        /// Creates a <see cref="Circle2f"/> from another <see cref="Circle2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2f(Circle2f o)
        {
            Center = o.Center;
            Radius = o.Radius;
        }

        /// <summary>
        /// Creates a <see cref="Circle2f"/> from a <see cref="Circle2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2f(Circle2d o)
        {
            Center = (V2f)o.Center;
            Radius = (float)o.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Circle2f(Circle2d c)
            => new Circle2f(c);

        #endregion

        #region Constants

        public static Circle2f Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle2f(V2f.Zero, 0);
        }

        public static Circle2f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle2f(V2f.NaN, -1);
        }

        public static Circle2f Unit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle2f(V2f.Zero, 1);
        }

        #endregion

        #region Properties

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0.0 || float.IsNaN(Radius) || Center.IsNaN;
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0.0 && !Center.IsNaN;
        }

        public float RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius;
        }

        public float Circumference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2 * Radius * ConstantF.Pi;
        }

        public float Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius * ConstantF.Pi;
        }

        public Box2f InscribedSquare
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = Fun.Sqrt(RadiusSquared * 0.5f);
                return new Box2f(new V2f(-a), new V2f(a));
            }
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Circle2f a, Circle2f b)
            => (a.Center == b.Center) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Circle2f a, Circle2f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Circle2f other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Circle2f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        /// <summary>
        /// Parses Circle2f from a string created with Circle2f.ToString().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Circle2f(V2f.Parse(x[0]), float.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox2f Members

        public Box2f BoundingBox2f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box2f(
                        new V2f(Center.X - Radius, Center.Y - Radius),
                        new V2f(Center.X + Radius, Center.Y + Radius)
                   );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Circle2f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle2f a, Circle2f b, float tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Circle2f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle2f a, Circle2f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Circle2d

    /// <summary>
    /// A two dimensional circle represented by center and radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Circle2d : IEquatable<Circle2d>, IBoundingBox2d, IValidity
    {
        [DataMember]
        public V2d Center;

        [DataMember]
        public double Radius;

        #region Constructors

        /// <summary>
        /// Creates circle from center and radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2d(V2d center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Creates circle from center and point on circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2d(V2d center, V2d pointOnCircle)
        {
            Center = center;
            Radius = (pointOnCircle - center).Length;
        }

        /// <summary>
        /// Creates circle from three points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2d(V2d a, V2d b, V2d c)
        {
            var l0 = new Plane2d((b - a).Normalized, (a + b) * 0.5);
            var l1 = new Plane2d((c - b).Normalized, (b + c) * 0.5);
            if (l0.Intersects(l1, out Center))
            {
                Radius = (a - Center).Length;
            }
            else
            {
                throw new Exception("Cannot construct circle because given points are collinear.");
            }
        }

        /// <summary>
        /// Creates a <see cref="Circle2d"/> from another <see cref="Circle2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2d(Circle2d o)
        {
            Center = o.Center;
            Radius = o.Radius;
        }

        /// <summary>
        /// Creates a <see cref="Circle2d"/> from a <see cref="Circle2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Circle2d(Circle2f o)
        {
            Center = (V2d)o.Center;
            Radius = (double)o.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Circle2d(Circle2f c)
            => new Circle2d(c);

        #endregion

        #region Constants

        public static Circle2d Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle2d(V2d.Zero, 0);
        }

        public static Circle2d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle2d(V2d.NaN, -1);
        }

        public static Circle2d Unit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Circle2d(V2d.Zero, 1);
        }

        #endregion

        #region Properties

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0.0 || double.IsNaN(Radius) || Center.IsNaN;
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0.0 && !Center.IsNaN;
        }

        public double RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius;
        }

        public double Circumference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2 * Radius * Constant.Pi;
        }

        public double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius * Constant.Pi;
        }

        public Box2d InscribedSquare
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = Fun.Sqrt(RadiusSquared * 0.5);
                return new Box2d(new V2d(-a), new V2d(a));
            }
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Circle2d a, Circle2d b)
            => (a.Center == b.Center) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Circle2d a, Circle2d b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Circle2d other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Circle2d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        /// <summary>
        /// Parses Circle2d from a string created with Circle2d.ToString().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Circle2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Circle2d(V2d.Parse(x[0]), double.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox2d Members

        public Box2d BoundingBox2d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box2d(
                        new V2d(Center.X - Radius, Center.Y - Radius),
                        new V2d(Center.X + Radius, Center.Y + Radius)
                   );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Circle2d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle2d a, Circle2d b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Circle2d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Circle2d a, Circle2d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

}
