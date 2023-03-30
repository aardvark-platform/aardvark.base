using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# Action comma = () => Out(", ");
    //# Action commaln = () => Out("," + Environment.NewLine);
    //# Action add = () => Out(" + ");
    //# Action and = () => Out(" && ");
    //# var fields = new[] {"X", "Y", "Z", "W"};
    //# var fieldsL = new[] {"x", "y", "z", "w"};
    //# foreach (var rt in Meta.RealTypes) {
    //# for (int n = 2; n <= 3; n++) {
    //#     var m = n + 1;
    //#     var rt2 = (rt == Meta.DoubleType) ? Meta.FloatType : Meta.DoubleType;
    //#     var rtype = rt.Name;
    //#     var rtype2 = rt2.Name;
    //#     var tc = rt.Char;
    //#     var tc2 = rt2.Char;
    //#     var type = "Trafo" + n + tc;
    //#     var type2 = "Trafo" + n + tc2;
    //#     var mnnt = "M" + n + n + tc;
    //#     var mnnt2 = "M" + n + n + tc2;
    //#     var mmmt = "M" + m + m + tc;
    //#     var mmmt2 = "M" + m + m + tc2;
    //#     var rotnt = "Rot" + n + tc;
    //#     var euclideannt = "Euclidean" + n + tc;
    //#     var similaritynt = "Similarity" + n + tc;
    //#     var affinent = "Affine" + n + tc;
    //#     var shiftnt = "Shift" + n + tc;
    //#     var scalent = "Scale" + n + tc;
    //#     var vnt = "V" + n + tc;
    //#     var vmt = "V" + m + tc;
    //#     var nfields = fields.Take(n).ToArray();
    //#     var nfieldsL = fieldsL.Take(n).ToArray();
    //#     var fn = fields[n];
    //#     var isDouble = (rt == Meta.DoubleType);
    //#     var half = (rt != Meta.DoubleType) ? "0.5f" : "0.5";
    #region __type__

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>
    {
        [DataMember]
        public readonly __mmmt__ Forward;
        [DataMember]
        public readonly __mmmt__ Backward;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="__type__"/> from a forward and backward transformation <see cref="__mmmt__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__mmmt__ forward, __mmmt__ backward)
        {
            Forward = forward;
            Backward = backward;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from another <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from a <see cref="__type2__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ trafo)
        {
            Forward = (__mmmt__)trafo.Forward;
            Backward = (__mmmt__)trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from an <see cref="__affinent__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__affinent__ trafo)
        {
            Forward = (__mmmt__)trafo;
            Backward = (__mmmt__)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from a <see cref="__euclideannt__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__euclideannt__ trafo)
        {
            Forward = (__mmmt__)trafo;
            Backward = (__mmmt__)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from a <see cref="__similaritynt__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__similaritynt__ trafo)
        {
            Forward = (__mmmt__)trafo;
            Backward = (__mmmt__)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from a <see cref="__rotnt__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__rotnt__ trafo)
        {
            Forward = (__mmmt__)trafo;
            Backward = (__mmmt__)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from a <see cref="__scalent__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__scalent__ trafo)
        {
            Forward = (__mmmt__)trafo;
            Backward = (__mmmt__)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> from a <see cref="__shiftnt__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__shiftnt__ trafo)
        {
            Forward = (__mmmt__)trafo;
            Backward = (__mmmt__)trafo.Inverse;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static __type__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__mmmt__.Identity, __mmmt__.Identity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the inverse of this <see cref="__type__"/>.
        /// </summary>
        public __type__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(Backward, Forward);
        }

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Forward.Equals(other.Forward) && Backward.Equals(other.Backward);

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(
                __mmmt__.Parse(x[0].ToString()),
                __mmmt__.Parse(x[1].ToString())
            );
        }

        #endregion

        #region Static Creators

        //# if (n == 3) {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromNormalFrame(__vnt__ origin, __vnt__ normal)
        {
            __mmmt__.NormalFrame(origin, normal, out __mmmt__ forward, out __mmmt__ backward);
            return new __type__(forward, backward);
        }

        //# }
        //# if (n == 2) {
        /// <summary>
        /// Builds a transformation matrix using the scale, rotation (in radians) and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromComponents(__vnt__ scale, __rtype__ rotationInRadians, __vnt__ translation)
            => Scale(scale) * Rotation(rotationInRadians) * Translation(translation);

        //# } else {
        /// <summary>
        /// Builds a transformation matrix using the scale, rotation and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion. 
        ///       The rotation is in Euler-Angles (roll, pitch, yaw) in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromComponents(__vnt__ scale, __vnt__ rotationInRadians, __vnt__ translation)
            => Scale(scale) * RotationEuler(rotationInRadians) * Translation(translation);

        //# }
        /// <summary>
        /// Returns the <see cref="__type__"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromBasis(/*# nfieldsL.ForEach(f => {*/__vnt__ __f__Axis/*# }, comma);*/, __vnt__ origin)
        {
            var mat = new __mmmt__(/*# nfields.ForEach(fi => {*/
                            /*# nfieldsL.ForEach(fj => {*/__fj__Axis.__fi__/*# }, comma);*/, origin.__fi__/*#}, comma);*/,
                            /*# n.ForEach(j => {*/0/*# }, comma);*/, 1);

            return new __type__(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the <see cref="__type__"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// Note that the axes MUST be normalized and normal to each other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromOrthoNormalBasis(/*# nfieldsL.ForEach(f => {*/__vnt__ __f__Axis/*# }, comma);*/)
        {
            return new __type__(
                        new __mmmt__(/*# nfields.ForEach(fi => {*/
                            /*# nfieldsL.ForEach(fj => {*/__fj__Axis.__fi__/*# }, comma);*/, 0/*#}, comma);*/,
                            /*# n.ForEach(j => {*/0/*# }, comma);*/, 1),
                        new __mmmt__(/*# nfieldsL.ForEach(fi => {*/
                            /*# nfields.ForEach(fj => {*/__fi__Axis.__fj__/*# }, comma);*/, 0/*#}, comma);*/,
                            /*# n.ForEach(j => {*/0/*# }, comma);*/, 1)
                        );
        }

        #region Translation

        /// <summary>
        /// Creates an <see cref="__type__"/> transformation with the translational component given by a <see cref="__vnt__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(__vnt__ v)
            => new __type__(__mmmt__.Translation(v), __mmmt__.Translation(-v));

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation with the translational component given by __n__ scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(/*# nfields.ForEach(f => { */__rtype__ t__f__/*# }, comma); */)
            => Translation(new __vnt__(/*# nfields.ForEach(f => { */t__f__/*# }, comma); */));

        /// <summary>
        /// Creates an <see cref="__type__"/> transformation with the translational component given by a <see cref="__shiftnt__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(__shiftnt__ shift)
            => Translation(shift.V);

        #endregion

        #region Scale

        /// <summary>
        /// Creates a scaling transformation using a <see cref="__vnt__"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Scale(__vnt__ v) 
            => new __type__(__mmmt__.Scale(v), __mmmt__.Scale(1 / v));

        /// <summary>
        /// Creates a scaling transformation using __n__ scalars as scaling factors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Scale(/*# nfields.ForEach(f => { */__rtype__ s__f__/*# }, comma); */)
            => new __type__(__mmmt__.Scale(/*# nfields.ForEach(f => { */s__f__/*# }, comma); */),
                           __mmmt__.Scale(/*# nfields.ForEach(f => { */1 / s__f__/*# }, comma); */));

        /// <summary>
        /// Creates a scaling transformation using a uniform scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Scale(__rtype__ s)
        {
            var t = 1 / s;
            return new __type__(__mmmt__.Scale(s), __mmmt__.Scale(t));
        }

        /// <summary>
        /// Creates a scaling transformation using a <see cref="__scalent__"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Scale(__scalent__ scale)
            => new __type__(scale);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="__rotnt__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__rotnt__ rotation)
            => new __type__(rotation);

        //# if (n == 2) {
        /// <summary>
        /// Creates a rotation transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__rtype__ angleInRadians)
            => new __type__(__mmmt__.Rotation(angleInRadians), __mmmt__.Rotation(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation with the specified angle in degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationInDegrees(__rtype__ angleInDegrees)
            => Rotation(angleInDegrees.RadiansFromDegrees());

        //# } else if (n == 3) {
        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__vnt__ normalizedAxis, __rtype__ angleInRadians)
            => new __type__(__mmmt__.Rotation(normalizedAxis, angleInRadians),
                           __mmmt__.Rotation(normalizedAxis, -angleInRadians));

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationInDegrees(__vnt__ normalizedAxis, __rtype__ angleInDegrees)
            => Rotation(normalizedAxis, Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEuler(__rtype__ rollInRadians, __rtype__ pitchInRadians, __rtype__ yawInRadians)
        {
            var m = __mmmt__.RotationEuler(rollInRadians, pitchInRadians, yawInRadians);
            return new __type__(m, m.Transposed); //transposed is equal but faster to inverted on orthonormal matrices like rotations.
        }

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEulerInDegrees(__rtype__ rollInDegrees, __rtype__ pitchInDegrees, __rtype__ yawInDegrees)
            => RotationEuler(rollInDegrees.RadiansFromDegrees(), pitchInDegrees.RadiansFromDegrees(), yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEuler(__vnt__ rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEulerInDegrees(__vnt__ rollPitchYawInDegrees)
            => RotationEulerInDegrees(rollPitchYawInDegrees.X, rollPitchYawInDegrees.Y, rollPitchYawInDegrees.Z);

        /// <summary>
        /// Creates a rotation transformation which rotates one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotateInto(__vnt__ from, __vnt__ into)
        {
            var rot = __rotnt__.RotateInto(from, into);
            var inv = rot.Inverse;
            return new __type__((__mmmt__)rot, (__mmmt__)inv);
        }

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationX(__rtype__ angleInRadians)
            => new __type__(__mmmt__.RotationX(angleInRadians),
                           __mmmt__.RotationX(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationXInDegrees(__rtype__ angleInDegrees)
            => RotationX(Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationY(__rtype__ angleInRadians)
            => new __type__(__mmmt__.RotationY(angleInRadians),
                           __mmmt__.RotationY(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationYInDegrees(__rtype__ angleInDegrees)
            => RotationY(Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationZ(__rtype__ angleInRadians)
            => new __type__(__mmmt__.RotationZ(angleInRadians),
                           __mmmt__.RotationZ(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationZInDegrees(__rtype__ angleInDegrees)
            => RotationZ(Conversion.RadiansFromDegrees(angleInDegrees));

        //# }
        #endregion

        //# if (n == 3) {
        #region Shear

        /// <summary>
        /// Creates a shear transformation along the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ ShearYZ(__rtype__ factorY, __rtype__ factorZ)
            => new __type__(__mmmt__.ShearYZ(factorY, factorZ),
                           __mmmt__.ShearYZ(-factorY, -factorZ));

        /// <summary>
        /// Creates a shear transformation along the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ ShearXZ(__rtype__ factorX, __rtype__ factorZ)
            => new __type__(__mmmt__.ShearXZ(factorX, factorZ),
                           __mmmt__.ShearXZ(-factorX, -factorZ));

        /// <summary>
        /// Creates a shear transformation along the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ ShearXY(__rtype__ factorX, __rtype__ factorY)
            => new __type__(__mmmt__.ShearXY(factorX, factorY),
                           __mmmt__.ShearXY(-factorX, -factorY));

        #endregion

        #region View transformation

        /// <summary>
        /// Creates a view transformation from the given vectors.
        /// </summary>
        /// <param name="location">Origin of the view</param>
        /// <param name="u">Right vector of the view-plane</param>
        /// <param name="v">Up vector of the view-plane</param>
        /// <param name="z">Normal vector of the view-plane. This vector is supposed to point in view-direction for a left-handed view transformation and in opposite direction in the right-handed case.</param>
        /// <returns>The view transformation</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ ViewTrafo(__vnt__ location, __vnt__ u, __vnt__ v, __vnt__ z)
        {
            return new __type__(
                new __mmmt__(
                    u.X, u.Y, u.Z, -Vec.Dot(u, location),
                    v.X, v.Y, v.Z, -Vec.Dot(v, location),
                    z.X, z.Y, z.Z, -Vec.Dot(z, location),
                    0, 0, 0, 1
                ),
                new __mmmt__(
                    u.X, v.X, z.X, location.X,
                    u.Y, v.Y, z.Y, location.Y,
                    u.Z, v.Z, z.Z, location.Z,
                    0, 0, 0, 1
                ));
        }

        /// <summary>
        /// Creates a right-handed view trafo, where z-negative points into the scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ ViewTrafoRH(__vnt__ location, __vnt__ up, __vnt__ forward)
            => ViewTrafo(location, forward.Cross(up), up, -forward);

        /// <summary>
        /// Creates a left-handed view trafo, where z-positive points into the scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ ViewTrafoLH(__vnt__ location, __vnt__ up, __vnt__ forward)
            => ViewTrafo(location, up.Cross(forward), up, forward);

        #endregion

        #region Projection transformation

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ PerspectiveProjectionRH(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f = __rtype__.PositiveInfinity)
        {
            __rtype__ m22, m23, m32i;

            if (f.IsPositiveInfinity())
            {
                m22  = -1;
                m23  = -n;
                m32i = -1 / n;
            }
            else
            {
                m22  = f / (n - f);
                m23  = (f * n) / (n - f);
                m32i = (n - f) / (f * n);
            }

            return new __type__(
                new __mmmt__(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,                   m22,                   m23,
                                    0,                     0,                    -1,                     0
                    ),
                new __mmmt__(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                  m32i,                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ PerspectiveProjectionRH(__rtype__ horizontalFovInRadians, __rtype__ aspect, __rtype__ n, __rtype__ f = __rtype__.PositiveInfinity)
        {
            __rtype__ d = Fun.Tan(__half__ * horizontalFovInRadians) * n;
            return __type__.PerspectiveProjectionRH(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ PerspectiveProjectionGL(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f = __rtype__.PositiveInfinity)
        {
            __rtype__ m22, m23, m32i, m33i;

            if (f.IsPositiveInfinity())
            {
                m22  = -1;
                m23  = -2 * n;
                m32i = -1 / (2 * n);
                m33i = -m32i;
            }
            else
            {
                m22  = (f + n) / (n - f);
                m23  = (2 * f * n) / (n - f);
                m32i = (n - f) / (2 * f * n);
                m33i = (f + n) / (2 * f * n);
            }

            return new __type__(
                new __mmmt__(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,                   m22,                    m23,
                                    0,                     0,                    -1,                      0
                    ),
                new __mmmt__(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                   m32i,                 m33i
                    )
                );
        }

        [Obsolete("Use PerspectiveProjectionGL instead.")]
        public static __type__ PerspectiveProjectionOpenGl(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
            => __type__.PerspectiveProjectionGL(l, r, b, t, n, f);

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ PerspectiveProjectionGL(__rtype__ horizontalFovInRadians, __rtype__ aspect, __rtype__ n, __rtype__ f = __rtype__.PositiveInfinity)
        {
            __rtype__ d = Fun.Tan(__half__ * horizontalFovInRadians) * n;
            return __type__.PerspectiveProjectionGL(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a left-handed perspective projection transform, where z-positive points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        [Obsolete("Broken, do not use.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ PerspectiveProjectionLH(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __mmmt__(
                    (2 * n) / (r - l),                     0,                     0,                     0,
                                    0,     (2 * n) / (t - b),                     0,                     0,
                    (l + r) / (l - r),     (b + t) / (b - t),           f / (f - n),                     1,
                                    0,                     0,     (n * f) / (n - f),                     0
                    ),                                                     
                                                                       
                new __mmmt__(                                      
                    (r - l) / (2 * n),                     0,                     0,                     0,
                                    0,     (t - b) / (2 * n),                     0,                     0,
                                    0,                     0,                     0,     (n - f) / (f * n),
                    (r + l) / (2 * n),     (t + b) / (2 * n),                     1,                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed orthographic projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ OrthoProjectionRH(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __mmmt__(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     1 / (n - f),           n / (n - f),
                              0,               0,               0,                     1
                    ),
                new __mmmt__(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,           n - f,                    -n,
                              0,               0,               0,                     1
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed orthographic projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ OrthoProjectionGL(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __mmmt__(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     2 / (n - f),     (f + n) / (n - f),
                              0,               0,               0,                     1
                    ),
                new __mmmt__(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,     (n - f) / 2,          -(f + n) / 2,
                              0,               0,               0,                     1
                    )
                );
        }

        [Obsolete("Use OrthoProjectionGL instead.")]
        public static __type__ OrthoProjectionOpenGl(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
            => __type__.OrthoProjectionGL(l, r, b, t, n, f);

        #endregion

        //# }
        #endregion

        #region Conversion Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type2__(__type__ r)
            => new __type2__(r);

        #endregion

        #region Operators

        /// <summary>
        /// Returns whether two <see cref="__type__"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        /// <summary>
        /// Returns whether two <see cref="__type__"/> transformations are different.
        /// </summary>
        public static bool operator !=(__type__ a, __type__ b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of __type__ multiplicaition is backward
        /// with respect to __mmmt__ multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static __type__ operator *(__type__ t0, __type__ t1)
            => new __type__(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 
    }

    public static partial class Trafo
    {
        #region Operations

        /// <summary>
        /// Returns the inverse of the given <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Inverse(__type__ trafo)
            => trafo.Inverse;

        /// <summary>
        /// Returns the forward matrix the given <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mmmt__ Forward(__type__ trafo)
            => trafo.Forward;

        /// <summary>
        /// Returns the backward matrix the given <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mmmt__ Backward(__type__ trafo)
            => trafo.Backward;

        #endregion

        #region Transformation Extraction

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __rtype__ GetScale(this __type__ trafo)
            => trafo.Forward.GetScale__n__();

        /// <summary>
        /// Extracts a scale vector from the given transformation by calculating the lengths of the basis vectors. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ GetScaleVector(this __type__ trafo)
            => trafo.Forward.GetScaleVector__n__();

        //# if (n == 3) {
        /// <summary>
        /// Extracts the inverse/backward translation component of the given transformation, which when given 
        /// a view transformation represents the location of the camera in world space.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ GetViewPosition(this __type__ trafo)
            => trafo.Backward.C3.XYZ;

        /// <summary>
        /// Extracts the forward vector from the given view transformation.
        /// NOTE: A left-handed coordinates system transformation is expected, 
        /// where the view-space z-axis points in forward direction.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ GetViewDirectionLH(this __type__ trafo)
            => trafo.Forward.GetViewDirectionLH();

        /// <summary>
        /// Extracts the forward vector from the given view transformation.
        /// NOTE: A right-handed coordinates system transformation is expected, where 
        /// the view-space z-axis points opposit the forward vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ GetViewDirectionRH(this __type__ trafo)
            => trafo.Forward.GetViewDirectionRH();

        /// <summary>
        /// Extracts the translation component of the given transformation, which when given 
        /// a model transformation represents the model origin in world position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ GetModelOrigin(this __type__ trafo)
            => trafo.Forward.GetModelOrigin();

        //# if (isDouble) {
        /// <summary>
        /// Builds a hull from the given view-projection transformation (left, right, bottom, top, near, far).
        /// The view volume is assumed to be [-1, -1, -1] [1, 1, 1].
        /// The normals of the hull planes point to the outside and are normalized. 
        /// A point inside the visual hull will has negative height to all planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Hull3d GetVisualHull(this __type__ viewProj)
            => viewProj.Forward.GetVisualHull();

        //# }
        /// <summary>
        /// Builds an ortho-normal orientation transformation form the given transform.
        /// Scale and Translation will be removed and basis vectors will be ortho-normalized.
        /// NOTE: The x-axis is untouched and y/z are forced to a normal-angle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ GetOrthoNormalOrientation(this __type__ trafo)
        {
            var x = trafo.Forward.C0.XYZ.Normalized;
            var z = trafo.Forward.C2.XYZ;

            var y = z.Cross(x).Normalized;
            z = x.Cross(y).Normalized;

            return __type__.FromOrthoNormalBasis(x, y, z);
        }

        /// <summary>
        /// Decomposes a transformation into a scale, rotation and translation component.
        /// NOTE: The input is assumed to be a valid affine transformation.
        ///       The rotation output is a vector with Euler-Angles [roll (X), pitch (Y), yaw (Z)] in radians of rotation order Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decompose(this __type__ trafo, out __vnt__ scale, out __vnt__ rotationInRadians, out __vnt__ translation)
        {
            translation = trafo.GetModelOrigin();
            
            var rt = trafo.GetOrthoNormalOrientation();
            if (rt.Forward.Determinant.IsTiny())
            {
                rotationInRadians = __vnt__.Zero;
            }
            else
            {
                var rot = __rotnt__.FromFrame(rt.Forward.C0.XYZ, rt.Forward.C1.XYZ, rt.Forward.C2.XYZ);
                rotationInRadians = rot.GetEulerAngles();
            }

            scale = trafo.GetScaleVector();

            // if matrix is left-handed there must be some negative scale
            // since rotation remains the x-axis, the y-axis must be flipped
            if (trafo.Forward.Determinant < 0)
                scale.Y = -scale.Y;
        }
        //# }

        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="__vmt__"/> by a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ Transform(this __type__ r, __vmt__ v)
            => r.Forward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.__fn__ is presumed 0.0) by a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformDir(this __type__ r, __vnt__ v)
            => r.Forward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by a <see cref="__type__"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformPos(this __type__ r, __vnt__ p)
            => r.Forward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by a <see cref="__type__"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformPosProj(this __type__ r, __vnt__ p)
            => r.Forward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by a <see cref="__type__"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ TransformPosProjFull(this __type__ r, __vnt__ p)
            => r.Forward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.__fn__ is presumed 0.0) by a <see cref="__type__"/>
        /// (i.e. by its transposed backward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformNormal(this __type__ r, __vnt__ n)
            => r.Backward.TransposedTransformDir(n);

        /// <summary>
        /// Transforms a <see cref="__vmt__"/> by the inverse of a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ InvTransform(this __type__ r, __vmt__ v)
            => r.Backward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.__fn__ is presumed 0.0) by the inverse of a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformDir(this __type__ r, __vnt__ v)
            => r.Backward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by the inverse of a <see cref="__type__"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformPos(this __type__ r, __vnt__ p)
            => r.Backward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by the inverse of a <see cref="__type__"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformPosProj(this __type__ r, __vnt__ p)
            => r.Backward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by the inverse of a <see cref="__type__"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ InvTransformPosProjFull(this __type__ r, __vnt__ p)
            => r.Backward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.__fn__ is presumed 0.0) by the inverse of a <see cref="__type__"/>
        /// (i.e. by its transposed forward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformNormal(this __type__ r, __vnt__ n)
            => r.Forward.TransposedTransformDir(n);

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        /// <summary>
        /// Returns if two <see cref="__type__"/> transformations are equal with regard to a threshold <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, __rtype__ epsilon)
            => a.Forward.ApproximateEquals(b.Forward, epsilon) && a.Backward.ApproximateEquals(b.Backward, epsilon);

        #endregion
    }

    #endregion

    //# } }
}
