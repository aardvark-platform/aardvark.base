using System;
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
    //# Action mul = () => Out(" * ");
    //# Action and = () => Out(" && ");
    //# Action or = () => Out(" || ");
    //# Action andLit = () => Out(" and ");
    //# var fields = new[] {"X", "Y", "Z", "W"};
    //# foreach (var isDouble in new[] { false, true }) {
    //# for (int n = 2; n <= 3; n++) {
    //#   var m = n + 1;
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var one = isDouble ? "1.0" : "1.0f";
    //#   var xyz = "XYZW".Substring(0, n);
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Similarity" + n + tc;
    //#   var type2 = "Similarity" + n + tc2;
    //#   var vnt = "V" + n + tc;
    //#   var vmt = "V" + m + tc;
    //#   var mnnt = "M" + n + n + tc;
    //#   var mmmt = "M" + m + m + tc;
    //#   var mnmt = "M" + n + m + tc;
    //#   var rotnt = "Rot" + n + tc;
    //#   var trafont = "Trafo" + n + tc;
    //#   var affinent = "Affine" + n + tc;
    //#   var scalent = "Scale" + n + tc;
    //#   var shiftnt = "Shift" + n + tc;
    //#   var euclideannt = "Euclidean" + n + tc;
    //#   var euclideannt2 = "Euclidean" + n + tc2;
    //#   var nfields = fields.Take(n).ToArray();
    //#   var mfields = fields.Take(m).ToArray();
    //#   var fn = fields[n];
    //#   var eps = isDouble ? "1e-12" : "1e-5f";
    #region __type__

    /// <summary>
    /// Represents a Similarity Transformation in __n__D that is composed of a 
    /// Uniform Scale and a subsequent Euclidean transformation (__n__D rotation Rot and a subsequent translation by a __n__D vector Trans).
    /// This is an angle preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>
    {
        [DataMember]
        public __ftype__ Scale;
        [DataMember]
        public __euclideannt__ Euclidean;

        /// <summary>
        /// Gets the rotational component of this <see cref="__type__"/> transformation.
        /// </summary>
        public __rotnt__ Rot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Rot; }
        }

        /// <summary>
        /// Gets the translational component of this <see cref="__type__"/> transformation.
        /// </summary>
        public __vnt__ Trans
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Trans; }
        }

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ s)
        {
            Scale = s.Scale;
            Euclidean = s.Euclidean;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation from a <see cref="__type2__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ s)
        {
            Scale = (__ftype__)s.Scale;
            Euclidean = (__euclideannt__)s.Euclidean;
        }

        /// <summary>
        /// Creates a similarity transformation from an uniform scale by factor <paramref name="scale"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ scale)
        {
            Scale = scale;
            Euclidean = __euclideannt__.Identity;
        }

        /// <summary>
        /// Creates a similarity transformation from a rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__euclideannt__ euclideanTransformation)
        {
            Scale = 1;
            Euclidean = euclideanTransformation;
        }

        /// <summary>
        /// Constructs a similarity transformation from a rotation <paramref name="rotation"/> and translation <paramref name="translation"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__rotnt__ rotation, __vnt__ translation)
        {
            Scale = 1;
            Euclidean = new __euclideannt__(rotation, translation);
        }

        /// <summary>
        /// Constructs a similarity transformation from a rotation <paramref name="rotation"/> and translation by (/*# nfields.ForEach(f => { */<paramref name="t__f__"/>/*# }, comma);*/).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__rotnt__ rotation, /*# nfields.ForEach(f => { */__ftype__ t__f__/*# }, comma);*/)
        {
            Scale = 1;
            Euclidean = new __euclideannt__(rotation, /*# nfields.ForEach(f => { */t__f__/*# }, comma);*/);
        }

        /// <summary>
        /// Creates a similarity transformation from a uniform scale by factor <paramref name="scale"/>, and a (subsequent) rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ scale, __euclideannt__ euclideanTransformation)
        {
            Scale = scale;
            Euclidean = euclideanTransformation;
        }

        /// <summary>
        /// Constructs a similarity transformation from a uniform scale by factor <paramref name="scale"/>, and a (subsequent) rotation <paramref name="rotation"/> and translation <paramref name="translation"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ scale, __rotnt__ rotation, __vnt__ translation)
        {
            Scale = scale;
            Euclidean = new __euclideannt__(rotation, translation);
        }

        /// <summary>
        /// Constructs a similarity transformation from a uniform scale by factor <paramref name="scale"/>, and a (subsequent) rotation <paramref name="rotation"/> and and translation by (/*# nfields.ForEach(f => { */<paramref name="t__f__"/>/*# }, comma);*/).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ scale, __rotnt__ rotation, /*# nfields.ForEach(f => { */__ftype__ t__f__/*# }, comma);*/)
        {
            Scale = scale;
            Euclidean = new __euclideannt__(rotation, /*# nfields.ForEach(f => { */t__f__/*# }, comma);*/);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static __type__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(1, __euclideannt__.Identity);
        }

        #endregion

        #region Similarity Transformation Arithmetics

        //# if (n > 2) {
        /// <summary>
        /// Returns a new version of this Similarity transformation with a normalized rotation quaternion.
        /// </summary>
        public __type__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(Scale, Euclidean.Normalized);
        }

        //# }
        /// <summary>
        /// Gets the (multiplicative) inverse of this Similarity transformation.
        /// [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        public __type__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newS = 1 / Scale;
                var newR = Euclidean.Inverse;
                newR.Trans *= newS;
                return new __type__(newS, newR);
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Similarity transformations.
        /// This concatenates the two similarity transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ a, __type__ b)
        {
            //a.Scale * b.Scale, a.Rot * b.Rot, a.Trans + a.Rot * a.Scale * b.Trans
            return new __type__(a.Scale * b.Scale, new __euclideannt__(
                a.Rot * b.Rot,
                a.Trans + a.Rot.Transform(a.Scale * b.Trans))
                );
        }

        /// <summary>
        /// Transforms a <see cref="__vmt__"/> vector by a <see cref="__type__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ operator *(__type__ s, __vmt__ v)
            => s.Euclidean * new __vmt__(/*# nfields.ForEach(f => {*/s.Scale * v.__f__/*#}, comma);*/, v.__fn__);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation (as a __m__x__m__ matrix) with a <see cref="__mmmt__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mmmt__ operator *(__type__ s, __mmmt__ m)
        {
            var t = (__mnmt__)s;
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
        public static __mmmt__ operator *(__mmmt__ m, __type__ s)
        {
            var t = (__mnmt__)s;
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
        public static __mnmt__ operator *(__type__ s, __mnmt__ m)
        {
            var t = (__mnmt__)s;
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
        public static __mnmt__ operator *(__mnmt__ m, __type__ s)
        {
            var t = (__mnmt__)s;
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
        public static __mnmt__ operator *(__type__ s, __mnnt__ m)
            => new __mnmt__(s.Rot * m * s.Scale, s.Trans);

        /// <summary>
        /// Multiplies a <see cref="__mnnt__"/> and a <see cref="__type__"/> (as a __n__x__m__ matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mnmt__ operator *(__mnnt__ m, __type__ s)
            => new __mnmt__(s.Scale * m * s.Rot, m * s.Trans);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> and a <see cref="__rotnt__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ s, __rotnt__ r)
            => new __type__(s.Scale, s.Euclidean * r);

        /// <summary>
        /// Multiplies a <see cref="__rotnt__"/> and a <see cref="__type__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__rotnt__ r, __type__ s)
               => new __type__(s.Scale, r * s.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> and a <see cref="__shiftnt__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ a, __shiftnt__ b)
            => new __type__(a.Scale, a.Euclidean * (a.Scale * b));

        /// <summary>
        /// Multiplies a <see cref="__shiftnt__"/> and a <see cref="__type__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__shiftnt__ a, __type__ b)
            => new __type__(b.Scale, a * b.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> and a <see cref="__scalent__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __affinent__ operator *(__type__ a, __scalent__ b)
        {
            var t = (__mnnt__)a;
            return new __affinent__(new __mnnt__(/*# nfields.ForEach((fi, i) => { */
                /*# nfields.ForEach((fj, j) => { */t.M__i____j__ * b.__fj__/*# }, comma); }, comma);*/),
                a.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="__scalent__"/> and a <see cref="__type__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __affinent__ operator *(__scalent__ a, __type__ b)
        {
            var t = (__mnnt__)b;
            return new __affinent__(new __mnnt__(/*# nfields.ForEach((fi, i) => { */
                /*# nfields.ForEach((fj, j) => { */t.M__i____j__ * a.__fi__/*# }, comma); }, comma);*/),
                b.Trans * a.V);
        }

        /// <summary>
        /// Multiplies an Euclidean transformation by a Similarity transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__euclideannt__ a, __type__ b)
        {
            return new __type__(b.Scale, a * b.Euclidean);
            //return (__type__)a * b;
        }

        /// <summary>
        /// Multiplies a Similarity transformation by an Euclidean transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ a, __euclideannt__ b)
        {
            return a * (__type__)b;
        }

        #endregion

        #region Comparison Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ t0, __type__ t1)
        {
            return t0.Scale == t1.Scale && t0.Euclidean == t1.Euclidean;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ t0, __type__ t1)
        {
            return !(t0 == t1);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__mnnt__"/> matrix and a translation <see cref="__vnt__"/>.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__mnnt__And__vnt__(__mnnt__ m, __vnt__ trans, __ftype__ epsilon = __eps__)
        {
            //# n.ForEach(i => {
            var s__i__ = m.C__i__.Norm2;
            //# });
            var s = (/*# n.ForEach(i => {*/s__i__/*# }, mul);*/).Pow(__one__ / __n__); //geometric mean of scale

            if (!(/*#n.ForEach(i => {*/(s__i__ / s - 1).IsTiny(epsilon)/*# }, and);*/))
                throw new ArgumentException("Matrix features non-uniform scaling");

            m /= s;
            return new __type__(s, __euclideannt__.From__mnnt__And__vnt__(m, trans/*# if (n > 2) {*/, epsilon/*# }*/));
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__mnmt__"/> matrix.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__mnmt__(__mnmt__ m, __ftype__ epsilon = __eps__)
            => From__mnnt__And__vnt__((__mnnt__)m, m.C__n__/*# if (n > 2) {*/, epsilon/*# }*/);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__mmmt__"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components or
        /// a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__mmmt__(__mmmt__ m, __ftype__ epsilon = __eps__)
        {
            if (!(/*#n.ForEach(j => {*/m.M__n____j__.IsTiny(epsilon)/*# }, and);*/))
                throw new ArgumentException("Matrix contains perspective components.");

            if (m.M__n____n__.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return From__mnnt__And__vnt__(((__mnnt__)m) / m.M__n____n__, m.C__n__.__xyz__/*# if (n > 2) {*/, epsilon/*# }*/);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from an <see cref="__scalent__"/>.
        /// The transformation <paramref name="scale"/> must represent a uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__scalent__(__scalent__ scale, __ftype__ epsilon = __eps__)
        {
            var s = (/*# nfields.ForEach(f => {*/scale.__f__/*# }, mul);*/).Pow(__one__ / __n__);

            if (!scale.ApproximateEquals(new __scalent__(s), epsilon))
                throw new ArgumentException("Matrix features non-uniform scaling");

            return new __type__(s, __euclideannt__.Identity);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from an <see cref="__affinent__"/>.
        /// The transformation <paramref name="affine"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__affinent__(__affinent__ affine, __ftype__ epsilon = __eps__)
            => From__mmmt__((__mmmt__)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__trafont__"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__trafont__(__trafont__ trafo, __ftype__ epsilon = __eps__)
            => From__mmmt__(trafo.Forward, epsilon);

        /// <summary>
        /// Creates a scaling transformation using a uniform scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Scaling(__ftype__ scaleFactor)
            => new __type__(scaleFactor);

        #region Translation

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation with the translational component given by __n__ scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(/*# nfields.ForEach(f => { */__ftype__ t__f__/*# }, comma); */)
            => new __type__(__rotnt__.Identity, /*# nfields.ForEach(f => { */t__f__/*# }, comma); */);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation with the translational component given by a <see cref="__vnt__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(__vnt__ vector)
            => new __type__(__rotnt__.Identity, vector);

        /// <summary>
        /// Creates an <see cref="__type__"/> transformation with the translational component given by a <see cref="__shiftnt__"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Translation(__shiftnt__ shift)
            => new __type__(__rotnt__.Identity, shift.V);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="__rotnt__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__rotnt__ rot)
            => new __type__(rot, __vnt__.Zero);

        //# if (n == 2) {
        /// <summary>
        /// Creates a rotation transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__ftype__ angleInRadians)
            => new __type__(new __rotnt__(angleInRadians), __vnt__.Zero);

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
            => new __type__(__rotnt__.Rotation(normalizedAxis, angleRadians), __vnt__.Zero);

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
            => new __type__(__rotnt__.RotationEuler(rollInRadians, pitchInRadians, yawInRadians), __vnt__.Zero);

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
            => new __type__(__rotnt__.RotateInto(from, into), __vnt__.Zero);

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationX(__ftype__ angleRadians)
            => new __type__(__rotnt__.RotationX(angleRadians), __vnt__.Zero);

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
            => new __type__(__rotnt__.RotationY(angleRadians), __vnt__.Zero);

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
            => new __type__(__rotnt__.RotationZ(angleRadians), __vnt__.Zero);

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
        public static explicit operator __mnmt__(__type__ s)
        {
            __mnmt__ rv = (__mnmt__)s.Euclidean;
            //# n.ForEach(i => {
            /*# n.ForEach(j => {*/rv.M__i____j__ *= s.Scale; /*# });*/
            //# });
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __mnnt__(__type__ s)
        {
            __mnnt__ rv = (__mnnt__)s.Euclidean;
            //# n.ForEach(i => {
            /*# n.ForEach(j => {*/rv.M__i____j__ *= s.Scale; /*# });*/
            //# });
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __mmmt__(__type__ s)
        {
            __mmmt__ rv = (__mmmt__)s.Euclidean;
            //# n.ForEach(i => {
            /*# n.ForEach(j => {*/rv.M__i____j__ *= s.Scale; /*# });*/
            //# });
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __affinent__(__type__ s)
        {
            var m = (__mnmt__)s;
            return new __affinent__((__mnnt__)m, m.C__n__);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __trafont__(__type__ s)
            => new __trafont__((__mmmt__)s, (__mmmt__)s.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type2__(__type__ s)
            => new __type2__((__ftype2__)s.Scale, (__euclideannt2__)s.Euclidean);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Scale, Euclidean);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Scale.Equals(other.Scale) && Euclidean.Equals(other.Euclidean);

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Scale, Euclidean);
        }

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__ftype__.Parse(x[0], CultureInfo.InvariantCulture), __euclideannt__.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Similarity
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Inverse(__type__ s)
            => s.Inverse;

        /// <summary>
        /// Inverts the similarity transformation (multiplicative inverse).
        /// this = [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref __type__ t)
        {
            t.Scale = 1 / t.Scale;
            t.Euclidean.Invert();
            t.Euclidean.Trans *= t.Scale;
        }

        #endregion

        //# if (n > 2) {
        #region Normalize

        /// <summary>
        /// Returns a copy of a <see cref="__type__"/> with its rotation quaternion normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Normalized(__type__ s)
            => s.Normalized;

        /// <summary>
        /// Normalizes the rotation quaternion of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref __type__ t)
        {
            t.Euclidean.Normalize();
        }

        #endregion

        //# }
        #region Transform

        /// <summary>
        /// Transforms a <see cref="__vmt__"/> by a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vmt__ Transform(this __type__ s, __vmt__ v)
            => s * v;

        /// <summary>
        /// Transforms direction vector v (v.__fn__ is presumed 0.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformDir(this __type__ t, __vnt__ v)
        {
            return t.Euclidean.TransformDir(t.Scale * v);
        }

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ TransformPos(this __type__ t, __vnt__ p)
        {
            return t.Euclidean.TransformPos(t.Scale * p);
        }

        /// <summary>
        /// Transforms direction vector v (v.__fn__ is presumed 0.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformDir(this __type__ t, __vnt__ v)
        {
            return t.Euclidean.InvTransformDir(v) / t.Scale;
        }

        /// <summary>
        /// Transforms point p (p.__fn__ is presumed 1.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __vnt__ InvTransformPos(this __type__ t, __vnt__ p)
        {
            return t.Euclidean.InvTransformPos(p) / t.Scale;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ t0, __type__ t1)
        {
            return ApproximateEquals(t0, t1, Constant<__ftype__>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ t0, __type__ t1, __ftype__ tol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, tol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ t0, __type__ t1, __ftype__ angleTol, __ftype__ posTol, __ftype__ scaleTol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, scaleTol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, angleTol, posTol);
        }

        #endregion
    }

    #endregion

    //# } }
}
