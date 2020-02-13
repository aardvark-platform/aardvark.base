using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var rt in Meta.RealTypes) {
    //#     var tc = rt.Char;
    //#     var type = "Trafo3" + tc;
    //#     var rtype = rt.Name;
    //#     var m44t = "M44" + tc;
    //#     var rot3t = "Rot3" + tc;
    //#     var euclidean3t = "Euclidean3" + tc;
    //#     var similarity3t = "Similarity3" + tc;
    //#     var v3t = "V3" + tc;
    //#     var v4t = "V4" + tc;
    #region __type__

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __type__
    {
        [DataMember]
        public readonly __m44t__ Forward;
        [DataMember]
        public readonly __m44t__ Backward;

        #region Constructors

        public __type__(__m44t__ forward, __m44t__ backward)
        {
            Forward = forward;
            Backward = backward;
        }

        public __type__(__type__ trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        public __type__(__euclidean3t__ trafo)
        {
            Forward = (__m44t__)trafo;
            Backward = (__m44t__)trafo.Inverse;
        }

        public __type__(__similarity3t__ trafo)
        {
            Forward = (__m44t__)trafo;
            Backward = (__m44t__)trafo.Inverse;
        }

        public __type__(__rot3t__ trafo)
        {
            Forward = (__m44t__)trafo;
            Backward = (__m44t__)trafo.Inverse;
        }

        #endregion

        #region Constants

        public static __type__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__m44t__.Identity, __m44t__.Identity);
        }

        #endregion

        #region Properties

        public __type__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(Backward, Forward);
        }

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        public override bool Equals(object other)
            => (other is __type__) ? (this == (__type__)other) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        #endregion

        #region Static Creators

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(
                __m44t__.Parse(x[0].ToString()),
                __m44t__.Parse(x[1].ToString())
            );
        }

        public static __type__ Translation(__v3t__ v)
            => new __type__(__m44t__.Translation(v), __m44t__.Translation(-v));

        public static __type__ Translation(__rtype__ dx, __rtype__ dy, __rtype__ dz)
            => new __type__(__m44t__.Translation(dx, dy, dz),
                           __m44t__.Translation(-dx, -dy, -dz));

        public static __type__ Scale(__v3t__ v) 
            => new __type__(__m44t__.Scale(v), __m44t__.Scale(1 / v));

        public static __type__ Scale(__rtype__ sx, __rtype__ sy, __rtype__ sz)
            => new __type__(__m44t__.Scale(sx, sy, sz),
                           __m44t__.Scale(1 / sx, 1 / sy, 1 / sz));

        public static __type__ Scale(__rtype__ s)
        {
            var t = 1 / s;
            return new __type__(__m44t__.Scale(s, s, s), __m44t__.Scale(t, t, t));
        }

        public static __type__ Rotation(__v3t__ normalizedAxis, __rtype__ angleInRadians)
            => new __type__(__m44t__.Rotation(normalizedAxis, angleInRadians),
                           __m44t__.Rotation(normalizedAxis, -angleInRadians));

        public static __type__ RotationInDegrees(__v3t__ normalizedAxis, __rtype__ angleInDegrees)
            => Rotation(normalizedAxis, Conversion.RadiansFromDegrees(angleInDegrees));

        public static __type__ RotationX(__rtype__ angleInRadians)
            => new __type__(__m44t__.RotationX(angleInRadians),
                           __m44t__.RotationX(-angleInRadians));

        public static __type__ RotationXInDegrees(__rtype__ angleInDegrees)
            => RotationX(Conversion.RadiansFromDegrees(angleInDegrees));

        public static __type__ RotationY(__rtype__ angleInRadians)
            => new __type__(__m44t__.RotationY(angleInRadians),
                           __m44t__.RotationY(-angleInRadians));

        public static __type__ RotationYInDegrees(__rtype__ angleInDegrees)
            => RotationY(Conversion.RadiansFromDegrees(angleInDegrees));

        public static __type__ RotationZ(__rtype__ angleInRadians)
            => new __type__(__m44t__.RotationZ(angleInRadians),
                           __m44t__.RotationZ(-angleInRadians));

        public static __type__ RotationZInDegrees(__rtype__ angleInDegrees)
            => RotationZ(Conversion.RadiansFromDegrees(angleInDegrees));

        public static __type__ Rotation(__rtype__ rollInRadians, __rtype__ pitchInRadians, __rtype__ yawInRadians)
        {
            var m = __m44t__.Rotation(rollInRadians, pitchInRadians, yawInRadians);
            return new __type__(m, m.Transposed); //transposed is equal but faster to inverted on orthonormal matrices like rotations.
        }

        public static __type__ RotationInDegrees(__rtype__ rollInDegrees, __rtype__ pitchInDegrees, __rtype__ yawInDegrees)
            => Rotation(rollInDegrees.RadiansFromDegrees(), pitchInDegrees.RadiansFromDegrees(), yawInDegrees.RadiansFromDegrees());

        public static __type__ Rotation(__v3t__ roll_pitch_yaw_inRadians)
            => Rotation(roll_pitch_yaw_inRadians.X, roll_pitch_yaw_inRadians.Y, roll_pitch_yaw_inRadians.Z);

        public static __type__ RotationInDegrees(__v3t__ roll_pitch_yaw_inDegrees)
            => RotationInDegrees(roll_pitch_yaw_inDegrees.X, roll_pitch_yaw_inDegrees.Y, roll_pitch_yaw_inDegrees.Z);

        public static __type__ RotateInto(__v3t__ from, __v3t__ into)
        {
            var rot = new __rot3t__(from, into);
            var inv = rot.Inverse;
            return new __type__((__m44t__)rot, (__m44t__)inv);
        }

        public static __type__ FromNormalFrame(__v3t__ origin, __v3t__ normal)
        {
            __m44t__.NormalFrame(origin, normal, out __m44t__ forward, out __m44t__ backward);
            return new __type__(forward, backward);
        }

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion. 
        ///       The rotation is in Euler-Angles (yaw, pitch, roll).
        /// </summary>
        public static __type__ FromComponents(__v3t__ scale, __v3t__ rotation, __v3t__ translation)
            => Scale(scale) * Rotation(rotation) * Translation(translation);

        public static __type__ ShearYZ(__rtype__ factorY, __rtype__ factorZ)
            => new __type__(__m44t__.ShearYZ(factorY, factorZ),
                           __m44t__.ShearYZ(-factorY, -factorZ));


        public static __type__ ShearXZ(__rtype__ factorX, __rtype__ factorZ)
            => new __type__(__m44t__.ShearXZ(factorX, factorZ),
                           __m44t__.ShearXZ(-factorX, -factorZ));

        public static __type__ ShearXY(__rtype__ factorX, __rtype__ factorY)
            => new __type__(__m44t__.ShearXY(factorX, factorY),
                           __m44t__.ShearXY(-factorX, -factorY));

        /// <summary>
        /// Returns the trafo that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        public static __type__ FromBasis(__v3t__ xAxis, __v3t__ yAxis, __v3t__ zAxis, __v3t__ orign)
        {
            var mat = new __m44t__(
                            xAxis.X, yAxis.X, zAxis.X, orign.X,
                            xAxis.Y, yAxis.Y, zAxis.Y, orign.Y,
                            xAxis.Z, yAxis.Z, zAxis.Z, orign.Z,
                            0, 0, 0, 1);

            return new __type__(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the trafo that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// NOTE that the axes MUST be normalized and normal to each other.
        /// </summary>
        public static __type__ FromOrthoNormalBasis(
                __v3t__ xAxis, __v3t__ yAxis, __v3t__ zAxis)
        {
            return new __type__(
                        new __m44t__(
                            xAxis.X, yAxis.X, zAxis.X, 0,
                            xAxis.Y, yAxis.Y, zAxis.Y, 0,
                            xAxis.Z, yAxis.Z, zAxis.Z, 0,
                            0, 0, 0, 1),
                        new __m44t__(
                            xAxis.X, xAxis.Y, xAxis.Z, 0,
                            yAxis.X, yAxis.Y, yAxis.Z, 0,
                            zAxis.X, zAxis.Y, zAxis.Z, 0,
                            0, 0, 0, 1)
                            );
        }

        /// <summary>
        /// Creates a view transformation from the given vectors.
        /// </summary>
        /// <param name="location">Origin of the view</param>
        /// <param name="u">Right vector of the view-plane</param>
        /// <param name="v">Up vector of the view-plane</param>
        /// <param name="z">Normal vector of the view-plane. This vector is supposed to point in view-direction for a left-handed view transformation and in opposite direction in the right-handed case.</param>
        /// <returns>The view transformation</returns>
        public static __type__ ViewTrafo(__v3t__ location, __v3t__ u, __v3t__ v, __v3t__ z)
        {
            return new __type__(
                new __m44t__(
                    u.X, u.Y, u.Z, -Vec.Dot(u, location),
                    v.X, v.Y, v.Z, -Vec.Dot(v, location),
                    z.X, z.Y, z.Z, -Vec.Dot(z, location),
                    0, 0, 0, 1
                ),
                new __m44t__(
                    u.X, v.X, z.X, location.X,
                    u.Y, v.Y, z.Y, location.Y,
                    u.Z, v.Z, z.Z, location.Z,
                    0, 0, 0, 1
                ));
        }

        /// <summary>
        /// Creates a right-handed view trafo, where z-negative points into the scene.
        /// </summary>
        public static __type__ ViewTrafoRH(__v3t__ location, __v3t__ up, __v3t__ forward)
            => ViewTrafo(location, forward.Cross(up), up, -forward);

        /// <summary>
        /// Creates a left-handed view trafo, where z-positive points into the scene.
        /// </summary>
        public static __type__ ViewTrafoLH(__v3t__ location, __v3t__ up, __v3t__ forward)
            => ViewTrafo(location, up.Cross(forward), up, forward);

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        public static __type__ PerspectiveProjectionRH(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __m44t__(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,           f / (n - f),     (f * n) / (n - f),
                                    0,                     0,                    -1,                     0
                    ),                                                     
                                                                       
                new __m44t__(                                      
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,     (n - f) / (f * n),                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)].
        /// </summary>
        public static __type__ PerspectiveProjectionOpenGl(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __m44t__(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,     (f + n) / (n - f),  (2 * f * n) / (n - f),
                                    0,                     0,                    -1,                      0
                    ),

                new __m44t__(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0, (n - f) / (2 * f * n),  (f + n) / (2 * f * n)
                    )
                );
        }

        /// <summary>
        /// Creates a left-handed perspective projection transform, where z-positive points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        public static __type__ PerspectiveProjectionLH(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __m44t__(
                    (2 * n) / (r - l),                     0,                     0,                     0,
                                    0,     (2 * n) / (t - b),                     0,                     0,
                    (l + r) / (l - r),     (b + t) / (b - t),           f / (f - n),                     1,
                                    0,                     0,     (n * f) / (n - f),                     0
                    ),                                                     
                                                                       
                new __m44t__(                                      
                    (r - l) / (2 * n),                     0,                     0,                     0,
                                    0,     (t - b) / (2 * n),                     0,                     0,
                                    0,                     0,                     0,     (n - f) / (f * n),
                    (r + l) / (2 * n),     (t + b) / (2 * n),                     1,                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed ortho projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        public static __type__ OrthoProjectionRH(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __m44t__(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     1 / (n - f),           n / (n - f),
                              0,               0,               0,                     1
                    ),

                new __m44t__(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,           n - f,                    -n,
                              0,               0,               0,                     1
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed ortho projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 1), (+1, +1, +1)].
        /// </summary>
        public static __type__ OrthoProjectionOpenGl(__rtype__ l, __rtype__ r, __rtype__ b, __rtype__ t, __rtype__ n, __rtype__ f)
        {
            return new __type__(
                new __m44t__(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     2 / (n - f),     (f + n) / (n - f),
                              0,               0,               0,                     1
                    ),

                new __m44t__(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,     (n - f) / 2,          -(f + n) / 2,
                              0,               0,               0,                     1
                    )
                );
        }

        #endregion

        #region Operators

        public static bool operator ==(__type__ a, __type__ b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        public static bool operator !=(__type__ a, __type__ b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of __type__ multiplicaition is backward
        /// with respect to __m44t__ multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static __type__ operator *(__type__ t0, __type__ t1)
            => new __type__(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 

    }

    public static partial class Trafo
    {
        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static __rtype__ GetScale(this __m44t__ trafo)
            => (trafo.C0.XYZ.Length + trafo.C1.XYZ.Length + trafo.C2.XYZ.Length) / 3;

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors.
        /// NOTE: The extraction only gives absolute value (negative scale will be ignored)
        /// </summary>
        public static __v3t__ GetScaleVector(this __m44t__ trafo)
            => new __v3t__(trafo.C0.XYZ.Length, trafo.C1.XYZ.Length, trafo.C2.XYZ.Length);

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static __rtype__ GetScale(this __type__ trafo)
            => trafo.Forward.GetScale();

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors. 
        /// </summary>
        public static __v3t__ GetScaleVector(this __type__ trafo)
            => trafo.Forward.GetScaleVector();

        /// <summary>
        /// Extracts the inverse/backward translation component of the given transformation, which when given 
        /// a view transformation represents the location of the camera in world space.
        /// </summary>
        public static __v3t__ GetViewPosition(this __type__ trafo)
            => trafo.Backward.C3.XYZ;

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A left-handed coordinates system transformation is expected, 
        /// where the view-space z-axis points in forward direction.
        /// </summary>
        public static __v3t__ GetViewDirectionLH(this __type__ trafo)
            => trafo.Forward.R2.XYZ.Normalized;

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A right-handed coordinates system transformation is expected, where 
        /// the view-space z-axis points opposit the forward vector.
        /// </summary>
        public static __v3t__ GetViewDirectionRH(this __type__ trafo)
            => -trafo.Forward.R2.XYZ.Normalized;

        /// <summary>
        /// Extracts the translation component of the given transformation, which when given 
        /// a model transformation represents the model origin in world position.
        /// </summary>
        public static __v3t__ GetModelOrigin(this __type__ trafo)
            => trafo.Forward.C3.XYZ;

        /// <summary>
        /// Builds a hull from the given view-projection transformation (left, right, bottom, top, near, far).
        /// The view volume is assumed to be [-1, -1, -1] [1, 1, 1].
        /// The normals of the hull planes point to the outside and are normalized. 
        /// A point inside the visual hull will has negative height to all planes.
        /// </summary>
        public static Hull3d GetVisualHull(this __m44t__ viewProj)
        {
            var r0 = viewProj.R0;
            var r1 = viewProj.R1;
            var r2 = viewProj.R2;
            var r3 = viewProj.R3;

            return new Hull3d(new[]
            {
                new Plane3d(/*# if (v4t != "V4d") {*/(V4d)/*# }*/(-(r3 + r0))).Normalized, // left
                new Plane3d(/*# if (v4t != "V4d") {*/(V4d)/*# }*/(-(r3 - r0))).Normalized, // right
                new Plane3d(/*# if (v4t != "V4d") {*/(V4d)/*# }*/(-(r3 + r1))).Normalized, // bottom
                new Plane3d(/*# if (v4t != "V4d") {*/(V4d)/*# }*/(-(r3 - r1))).Normalized, // top
                new Plane3d(/*# if (v4t != "V4d") {*/(V4d)/*# }*/(-(r3 + r2))).Normalized, // near
                new Plane3d(/*# if (v4t != "V4d") {*/(V4d)/*# }*/(-(r3 - r2))).Normalized, // far
            });
        }

        public static Hull3d GetVisualHull(this __type__ viewProj)
        {
            return viewProj.Forward.GetVisualHull();
        }

        /// <summary>
        /// Builds an ortho-normal orientation transformation form the given transform.
        /// Scale and Translation will be removed and basis vectors will be ortho-normalized.
        /// NOTE: The X-Axis is untouched and Y/Z are forced to a normal-angle.
        /// </summary>
        public static __type__ GetOrthoNormalOrientation(this __type__ trafo)
        {
            var x = trafo.Forward.C0.XYZ.Normalized; // TransformDir(__v3t__.XAxis)
            var y = trafo.Forward.C1.XYZ.Normalized; // TransformDir(__v3t__.YAxis)
            var z = trafo.Forward.C2.XYZ.Normalized; // TransformDir(__v3t__.ZAxis)

            y = z.Cross(x).Normalized;
            z = x.Cross(y).Normalized;

            return __type__.FromBasis(x, y, z, __v3t__.Zero);
        }

        /// <summary>
        /// Decomposes a transformation into a scale, rotation and translation component.
        /// NOTE: The input is assumed to be a valid affine transformation.
        ///       The rotation output is a vector with Euler-Angles [roll (X), pitch (Y), yaw (Z)] of rotation order Z, Y, X.
        /// </summary>
        public static void Decompose(this __type__ trafo, out __v3t__ scale, out __v3t__ rotation, out __v3t__ translation)
        {
            translation = trafo.GetModelOrigin();
            
            var rt = trafo.GetOrthoNormalOrientation();
            if (rt.Forward.Determinant.IsTiny())
            {
                rotation = __v3t__.Zero;
            }
            else
            {
                var rot = __rot3t__.FromFrame(rt.Forward.C0.XYZ, rt.Forward.C1.XYZ, rt.Forward.C2.XYZ);
                rotation = rot.GetEulerAngles();
            }

            scale = trafo.GetScaleVector();

            // if matrix is left-handed there must be some negative scale
            // since rotation remains the x-axis, the y-axis must be flipped
            if (trafo.Forward.Determinant < 0)
                scale.Y = -scale.Y;
        }
    }

    #endregion

    //# }
}
