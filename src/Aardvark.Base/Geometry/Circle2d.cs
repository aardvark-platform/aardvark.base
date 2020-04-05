/*
    Copyright 2006-2018. The Aardvark Platform Team.
    
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

namespace Aardvark.Base
{
    /// <summary>
    /// A two dimensional circle represented by center and radius.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle2d : IBoundingBox2d, IValidity
    {
        public V2d Center;
        public double Radius;

        #region Constructors

        /// <summary>
        /// Creates circle from center and radius.
        /// </summary>
        public Circle2d(V2d center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Creates circle from center and point on circle.
        /// </summary>
        public Circle2d(V2d center, V2d pointOnCircle)
        {
            Center = center;
            Radius = (pointOnCircle - center).Length;
        }

        /// <summary>
        /// Creates circle from three points.
        /// </summary>
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

        #endregion

        #region Constants

        public static readonly Circle2d Zero = new Circle2d(V2d.Zero, 0.0);
        public static readonly Circle2d Invalid = new Circle2d(V2d.NaN, -1.0);
        public static readonly Circle2d Unit = new Circle2d(V2d.Zero, 1.0);

        #endregion

        #region Properties

        public bool IsInvalid => Radius < 0.0 || double.IsNaN(Radius) || Center.IsNaN;

        public bool IsValid => Radius >= 0.0 && !Center.IsNaN;

        public double RadiusSquared => Radius * Radius;

        public double Circumference => 2.0 * Radius * Constant.Pi;

        public double Area => Radius * Radius * Constant.Pi;

        public Box2d InscribedSquare
        {
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

        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Circle2d other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        public override bool Equals(object other)
            => (other is Circle2d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        /// <summary>
        /// Parses Circle2d from a string created with Circle2d.ToString().
        /// </summary>
        public static Circle2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Circle2d(V2d.Parse(x[0]), double.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox2d Members

        public Box2d BoundingBox2d => new Box2d(
            new V2d(Center.X - Radius, Center.Y - Radius),
            new V2d(Center.X + Radius, Center.Y + Radius)
            );

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

    public static class Box2dExtensions
    {
        /// <summary>
        /// Computes bounding circle of box.
        /// </summary>
        public static Circle2d GetBoundingCircle2d(this Box2d box)
            => box.IsInvalid ? Circle2d.Invalid : new Circle2d(box.Center, 0.5 * box.Size.Length);
    }
}
