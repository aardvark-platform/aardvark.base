using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# Action comma = () => Out(", ");
    //# Action commaln = () => Out("," + Environment.NewLine);
    //# Action add = () => Out(" + ");
    //# Action and = () => Out(" && ");
    //# Action or = () => Out(" || ");
    //# Action andLit = () => Out(" and ");
    //# var fields = new[] {"X", "Y", "Z", "W"};
    //# foreach (var isDouble in new[] { false, true }) {
    //# for (int n = 2; n <= 3; n++) {
    //#   var m = n + 1;
    //#   var ftype = isDouble ? "double" : "float";
    //#   var xyz = "XYZW".Substring(0, n);
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Euclidean" + n + tc;
    //#   var type2 = "Euclidean" + n + tc2;
    //#   var vnt = "V" + n + tc;
    //#   var vnt2 = "V" + n + tc2;
    //#   var vmt = "V" + m + tc;
    //#   var mnnt = "M" + n + n + tc;
    //#   var mmmt = "M" + m + m + tc;
    //#   var mnmt = "M" + n + m + tc;
    //#   var rotnt = "Rot" + n + tc;
    //#   var rotnt2 = "Rot" + n + tc2;
    //#   var trafont = "Trafo" + n + tc;
    //#   var affinent = "Affine" + n + tc;
    //#   var scalent = "Scale" + n + tc;
    //#   var shiftnt = "Shift" + n + tc;
    //#   var similaritynt = "Similarity" + n + tc;
    //#   var nfields = fields.Take(n).ToArray();
    //#   var mfields = fields.Take(m).ToArray();
    //#   var fn = fields[n];
    //#   var eps = isDouble ? "1e-12" : "1e-5f";
    #region __type__

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in __n__D that is composed of a 
    /// __n__D rotation Rot and a subsequent translation by a __n__D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __type__
    {
        [DataMember]
        public __rotnt__ Rot;
        [DataMember]
        public __vnt__ Trans;

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ e)
        {
            Rot = e.Rot;
            Trans = e.Trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__rotnt__ rot)
        {
            Rot = rot;
            Trans = __vnt__.Zero;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__vnt__ trans)
        {
            Rot = __rotnt__.Identity;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation by (/*# nfields.ForEach(f => { */<paramref name="t__f__"/>/*# }, comma);*/).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(/*# nfields.ForEach(f => { */__ftype__ t__f__/*# }, comma);*/)
        {
            Rot = __rotnt__.Identity;
            Trans = new __vnt__(/*# nfields.ForEach(f => { */t__f__/*# }, comma);*/);
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__rotnt__ rot, __vnt__ trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation by (/*# nfields.ForEach(f => { */<paramref name="t__f__"/>/*# }, comma);*/)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__rotnt__ rot, /*# nfields.ForEach(f => { */__ftype__ t__f__/*# }, comma);*/)
        {
            Rot = rot;
            Trans = new __vnt__(/*# nfields.ForEach(f => { */t__f__/*# }, comma);*/);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static __type__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__rotnt__.Identity, __vnt__.Zero);
        }

        #endregion

        #region Properties

        //# if (n > 2) {
        /// <summary>
        /// Returns a new version of this Euclidean transformation with a normalized rotation quaternion.
        /// </summary>
        public __type__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(Rot.Normalized, Trans);
        }
        //# }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public __type__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newR = Rot.Inverse;
                return new __type__(newR, -newR.Transform(Trans));
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ a, __type__ b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new __type__(a.Rot * b.Rot, a.Trans + a.Rot.Transform(b.Trans));
        }

        /// <summary>
        /// Transforms a <see cref="__vmt__"/> vector by a <see cref="__type__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ operator *(__type__ e, __vmt__ v)
        {
            var rot = (__mnnt__)e.Rot;
            return new __vmt__(/*# nfields.ForEach((fi, i) => { */
                /*# mfields.ForEach((fj, j) => {
                var aij = (j < n) ? "rot.M" + i + j : "e.Trans." + fi;
                */__aij__ * v.__fj__/*# }, add); }, comma);*/,
                v.__fn__);
        }

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation (as a __m__x__m__ matrix) with a <see cref="__mmmt__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mmmt__ operator *(__type__ e, __mmmt__ m)
        {
            var t = (__mnmt__)e;
            return new __mmmt__(/*# n.ForEach(i => { m.ForEach(j => { */
                /*# m.ForEach(k => {
                */t.M__i____k__ * m.M__k____j__/*# }, add); }, comma); }, commaln);*/,

                /*# m.ForEach(i => { */m.M__n____i__/*# }, comma);*/);
        }

        /// <summary>
        /// Multiplies a <see cref="__mmmt__"/> with a <see cref="__type__"/> transformation (as a __m__x__m__ matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mmmt__ operator *(__mmmt__ m, __type__ e)
        {
            var t = (__mnmt__)e;
            return new __mmmt__(/*# m.ForEach(i => { m.ForEach(j => { */
                /*# n.ForEach(k => {
                */m.M__i____k__ * t.M__k____j__/*# }, add);
                 if (j == n) {*/ + m.M__i____n__/*# } }, comma); }, commaln);*/);
        }

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation (as a __n__x__m__ matrix) with a <see cref="__mnmt__"/> (as a __m__x__m__ matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mnmt__ operator *(__type__ e, __mnmt__ m)
        {
            var t = (__mnmt__)e;
            return new __mnmt__(/*# n.ForEach(i => { m.ForEach(j => { */
                /*# n.ForEach(k => {
                */t.M__i____k__ * m.M__k____j__/*# }, add);
                 if (j == n) {*/ + t.M__i____n__/*# } }, comma); }, commaln);*/);
        }

        /// <summary>
        /// Multiplies a <see cref="__mnmt__"/> with a <see cref="__type__"/> transformation (as a __m__x__m__ matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mnmt__ operator *(__mnmt__ m, __type__ e)
        {
            var t = (__mnmt__)e;
            return new __mnmt__(/*# n.ForEach(i => { m.ForEach(j => { */
                /*# n.ForEach(k => {
                */m.M__i____k__ * t.M__k____j__/*# }, add);
                 if (j == n) {*/ + m.M__i____n__/*# } }, comma); }, commaln);*/);
        }

        /// <summary>
        /// Multiplies a <see cref="__type__"/> (as a __n__x__m__ matrix) and a <see cref="__mnnt__"/> (as a __m__x__m__ matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mnmt__ operator *(__type__ a, __mnnt__ m)
            => new __mnmt__(a.Rot * m, a.Trans);

        /// <summary>
        /// Multiplies a <see cref="__mnnt__"/> and a <see cref="__type__"/> (as a __n__x__m__ matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mnmt__ operator *(__mnnt__ m, __type__ a)
            => new __mnmt__(m * a.Rot, m * a.Trans);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> and a <see cref="__rotnt__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ e, __rotnt__ r)
            => new __type__(e.Rot * r, e.Trans);

        /// <summary>
        /// Multiplies a <see cref="__rotnt__"/> and a <see cref="__type__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__rotnt__ r, __type__ e)
            => new __type__(r * e.Rot, r * e.Trans);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> and a <see cref="__shiftnt__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ e, __shiftnt__ s)
        {
            return new __type__(e.Rot, e.Rot * s.V + e.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="__shiftnt__"/> and a <see cref="__type__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__shiftnt__ s, __type__ e)
        {
            return new __type__(e.Rot, e.Trans + s.V);
        }

        /// <summary>
        /// Multiplies a <see cref="__type__"/> and a <see cref="__scalent__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __affinent__ operator *(__type__ r, __scalent__ s)
        {
            var t = (__mnnt__)r.Rot;
            return new __affinent__(new __mnnt__(/*# nfields.ForEach((fi, i) => { */
                /*# nfields.ForEach((fj, j) => { */t.M__i____j__ * s.__fj__/*# }, comma); }, comma);*/),
                r.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="__scalent__"/> and a <see cref="__type__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __affinent__ operator *(__scalent__ s, __type__ r)
        {
            var t = (__mnnt__)r.Rot;
            return new __affinent__(new __mnnt__(/*# nfields.ForEach((fi, i) => { */
                /*# nfields.ForEach((fj, j) => { */t.M__i____j__ * s.__fi__/*# }, comma); }, comma);*/),
                r.Trans * s.V);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(__type__ r0, __type__ r1)
        {
            return r0.Rot == r1.Rot && r0.Trans == r1.Trans;
        }

        public static bool operator !=(__type__ r0, __type__ r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// The matrix <paramref name="rot"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //# if (n == 2) {
        public static __type__ From__mnnt__And__vnt__(__mnnt__ rot, __vnt__ trans)
            => new __type__(__rotnt__.From__mnnt__(rot), trans);

        //# } else {
        public static __type__ From__mnnt__And__vnt__(__mnnt__ rot, __vnt__ trans, __ftype__ epsilon = __eps__)
            => new __type__(__rotnt__.From__mnnt__(rot, epsilon), trans);

        //# }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__mmmt__"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components and its upper left __n__x__n__ submatrix must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__mmmt__(__mmmt__ m, __ftype__ epsilon = __eps__) 
        {
            if (!(/*#n.ForEach(j => {*/m.M__n____j__.IsTiny(epsilon)/*# }, and);*/))
                throw new ArgumentException("Matrix contains perspective components.");
            if (m.M__n____n__.IsTiny(epsilon)) throw new ArgumentException("Matrix is not homogeneous.");

            return From__mnnt__And__vnt__(((__mnnt__)m) / m.M__n____n__,
                    m.C__n__.__xyz__ / m.M__n____n__/*# if (n > 2) {*/,
                    epsilon/*# }*/);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__mnmt__"/> matrix.
        /// The left __n__x__n__ submatrix of <paramref name="m"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__mnmt__(__mnmt__ m, __ftype__ epsilon = __eps__)
        {
            return From__mnnt__And__vnt__(((__mnnt__)m),
                    m.C__n__.__xyz__/*# if (n > 2) {*/,
                    epsilon/*# }*/);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__similaritynt__"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__similaritynt__(__similaritynt__ similarity, __ftype__ epsilon = __eps__)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            return similarity.Euclidean;
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from an <see cref="__affinent__"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__affinent__(__affinent__ affine, __ftype__ epsilon = __eps__)
            => From__mmmt__((__mmmt__)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__trafont__"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__trafont__(__trafont__ trafo, __ftype__ epsilon = __eps__)
            => From__mmmt__(trafo.Forward, epsilon);

        #region Translation

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation with the translational component given by __n__ scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(/*# nfields.ForEach(f => { */__ftype__ t__f__/*# }, comma); */)
            => new __type__(/*# nfields.ForEach(f => { */t__f__/*# }, comma); */);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation with the translational component given by a <see cref="__vnt__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(__vnt__ vector)
            => new __type__(vector);

        /// <summary>
        /// Creates an <see cref="__type__"/> transformation with the translational component given by a <see cref="__shiftnt__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(__shiftnt__ shift)
            => new __type__(shift.V);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="__rotnt__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__rotnt__ rot)
            => new __type__(rot);

        //# if (n == 2) {
        /// <summary>
        /// Creates a rotation transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__ftype__ angleInRadians)
            => new __type__(new __rotnt__(angleInRadians));

        /// <summary>
        /// Creates a rotation transformation with the specified angle in degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationInDegrees(__ftype__ angleInDegrees)
            => Rotation(angleInDegrees.RadiansFromDegrees());

        //# } else if (n == 3) {
        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__vnt__ normalizedAxis, __ftype__ angleRadians)
            => new __type__(__rotnt__.Rotation(normalizedAxis, angleRadians));

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationInDegrees(__vnt__ normalizedAxis, __ftype__ angleDegrees)
            => Rotation(normalizedAxis, angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEuler(__ftype__ rollInRadians, __ftype__ pitchInRadians, __ftype__ yawInRadians)
            => new __type__(__rotnt__.RotationEuler(rollInRadians, pitchInRadians, yawInRadians));

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEulerInDegrees(__ftype__ rollInDegrees, __ftype__ pitchInDegrees, __ftype__ yawInDegrees)
            => RotationEuler(
                rollInDegrees.RadiansFromDegrees(),
                pitchInDegrees.RadiansFromDegrees(),
                yawInDegrees.RadiansFromDegrees());

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
            => RotationEulerInDegrees(
                rollPitchYawInDegrees.X,
                rollPitchYawInDegrees.Y,
                rollPitchYawInDegrees.Z);

        /// <summary>
        /// Creates a rotation transformation which rotates one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotateInto(__vnt__ from, __vnt__ into)
            => new __type__(__rotnt__.RotateInto(from, into));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationX(__ftype__ angleRadians)
            => new __type__(__rotnt__.RotationX(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationXInDegrees(__ftype__ angleDegrees)
            => RotationX(angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationY(__ftype__ angleRadians)
            => new __type__(__rotnt__.RotationY(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationYInDegrees(__ftype__ angleDegrees)
            => RotationY(angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationZ(__ftype__ angleRadians)
            => new __type__(__rotnt__.RotationZ(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationZInDegrees(__ftype__ angleDegrees)
            => RotationZ(angleDegrees.RadiansFromDegrees());

        //# }
        #endregion

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __mnmt__(__type__ e)
            => new __mnmt__((__mnnt__)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __mnnt__(__type__ e)
            => (__mnnt__)e.Rot;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __mmmt__(__type__ e)
        {
            __mmmt__ rv = (__mmmt__)e.Rot;
            rv.C__n__ = e.Trans.__xyz__I;
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __similaritynt__(__type__ e)
            => new __similaritynt__(1, e);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __affinent__(__type__ e)
            => new __affinent__((__mnnt__)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __trafont__(__type__ e)
            => new __trafont__((__mmmt__)e, (__mmmt__)e.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type2__(__type__ e)
            => new __type2__((__rotnt2__)e.Rot, (__vnt2__)e.Trans);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Rot.Equals(other.Rot) && Trans.Equals(other.Trans);

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Rot, Trans);
        }

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__rotnt__.Parse(x[0]), __vnt__.Parse(x[1]));
        }

        #endregion

    }

    public static partial class Euclidean
    {
        //# if (n > 2) {
        #region Normalize

        /// <summary>
        /// Returns a copy of a <see cref="__type__"/> with its rotation quaternion normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Normalized(__type__ r)
            => r.Normalized;

        /// <summary>
        /// Normalizes the rotation quaternion of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref __type__ r)
        {
            r.Rot.Normalize();
        }

        #endregion

        //# }
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Inverse(__type__ r)
            => r.Inverse;

        /// <summary>
        /// Inverts this rigid transformation (multiplicative inverse).
        /// this = [Rot^T,-Rot^T Trans]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref __type__ r)
        {
            r.Rot.Invert();
            r.Trans = -r.Rot.Transform(r.Trans);
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="__vmt__"/> by an <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ Transform(this __type__ a, __vmt__ v)
            => a * v;

        /// <summary>
        /// Transforms direction vector v (v.__fn__ is presumed 0.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformDir(this __type__ r, __vnt__ v)
        {
            return r.Rot.Transform(v);
        }

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformPos(this __type__ r, __vnt__ p)
        {
            return r.Rot.Transform(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.__fn__ is presumed 0.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformDir(this __type__ r, __vnt__ v)
        {
            return r.Rot.InvTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformPos(this __type__ r, __vnt__ p)
        {
            return r.Rot.InvTransform(p - r.Trans);
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ r0, __type__ r1)
        {
            return ApproximateEquals(r0, r1, Constant<__ftype__>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ r0, __type__ r1, __ftype__ tol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, tol) && r0.Rot.ApproximateEquals(r1.Rot, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ r0, __type__ r1, __ftype__ angleTol, __ftype__ posTol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, posTol) && r0.Rot.ApproximateEquals(r1.Rot, angleTol);
        }

        #endregion
    }

    #endregion

    //# } }
}
