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

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Circle2" + tc;
    //#   var type2 = "Circle2" + tc2;
    //#   var v2t = "V2" + tc;
    //#   var box2t = "Box2" + tc;
    //#   var plane2t = "Plane2" + tc;
    //#   var iboundingbox = "IBoundingBox2" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    #region __type__

    /// <summary>
    /// A two dimensional circle represented by center and radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>, __iboundingbox__, IValidity
    {
        [DataMember]
        public __v2t__ Center;

        [DataMember]
        public __ftype__ Radius;

        #region Constructors

        /// <summary>
        /// Creates circle from center and radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v2t__ center, __ftype__ radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Creates circle from center and point on circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v2t__ center, __v2t__ pointOnCircle)
        {
            Center = center;
            Radius = (pointOnCircle - center).Length;
        }

        /// <summary>
        /// Creates circle from three points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v2t__ a, __v2t__ b, __v2t__ c)
        {
            var l0 = new __plane2t__((b - a).Normalized, (a + b) * __half__);
            var l1 = new __plane2t__((c - b).Normalized, (b + c) * __half__);
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
        /// Creates a <see cref="__type__"/> from another <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ o)
        {
            Center = o.Center;
            Radius = o.Radius;
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> from a <see cref="__type2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ o)
        {
            Center = (__v2t__)o.Center;
            Radius = (__ftype__)o.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type2__ c)
            => new __type__(c);

        #endregion

        #region Constants

        public static __type__ Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v2t__.Zero, 0);
        }

        public static __type__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v2t__.NaN, -1);
        }

        public static __type__ Unit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v2t__.Zero, 1);
        }

        #endregion

        #region Properties

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0.0 || __ftype__.IsNaN(Radius) || Center.IsNaN;
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0.0 && !Center.IsNaN;
        }

        public __ftype__ RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius;
        }

        public __ftype__ Circumference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2 * Radius * __pi__;
        }

        public __ftype__ Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius * __pi__;
        }

        public __box2t__ InscribedSquare
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var a = Fun.Sqrt(RadiusSquared * __half__);
                return new __box2t__(new __v2t__(-a), new __v2t__(a));
            }
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
            => (a.Center == b.Center) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        /// <summary>
        /// Parses __type__ from a string created with __type__.ToString().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__v2t__.Parse(x[0]), __ftype__.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region __iboundingbox__ Members

        public __box2t__ BoundingBox2__tc__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __box2t__(
                        new __v2t__(Center.X - Radius, Center.Y - Radius),
                        new __v2t__(Center.X + Radius, Center.Y + Radius)
                   );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    //# }
}
