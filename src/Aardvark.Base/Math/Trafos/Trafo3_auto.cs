using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Trafo3f

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Trafo3f
    {
        [DataMember]
        public readonly M44f Forward;
        [DataMember]
        public readonly M44f Backward;

        #region Constructors

        public Trafo3f(M44f forward, M44f backward)
        {
            Forward = forward;
            Backward = backward;
        }

        public Trafo3f(Trafo3f trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        public Trafo3f(Euclidean3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        public Trafo3f(Similarity3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        public Trafo3f(Rot3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        #endregion

        #region Constants

        public static readonly Trafo3f Identity =
            new Trafo3f(M44f.Identity, M44f.Identity);

        #endregion

        #region Properties

        public Trafo3f Inverse => new Trafo3f(Backward, Forward);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        public override bool Equals(object other)
            => (other is Trafo3f) ? (this == (Trafo3f)other) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        #endregion

        #region Static Creators

        public static Trafo3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Trafo3f(
                M44f.Parse(x[0].ToString()),
                M44f.Parse(x[1].ToString())
            );
        }

        public static Trafo3f Translation(V3f v)
            => new Trafo3f(M44f.Translation(v), M44f.Translation(-v));

        public static Trafo3f Translation(float dx, float dy, float dz)
            => new Trafo3f(M44f.Translation(dx, dy, dz),
                           M44f.Translation(-dx, -dy, -dz));

        public static Trafo3f Scale(V3f v) 
            => new Trafo3f(M44f.Scale(v), M44f.Scale(1 / v));

        public static Trafo3f Scale(float sx, float sy, float sz)
            => new Trafo3f(M44f.Scale(sx, sy, sz),
                           M44f.Scale(1 / sx, 1 / sy, 1 / sz));

        public static Trafo3f Scale(float s)
            => new Trafo3f(M44f.Scale(s), M44f.Scale(1 / s));

        public static Trafo3f Rotation(V3f axis, float angleInRadians)
            => new Trafo3f(M44f.Rotation(axis, angleInRadians),
                           M44f.Rotation(axis, -angleInRadians));

        public static Trafo3f RotationInDegrees(V3f axis, float angleInDegrees)
            => Rotation(axis, Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3f RotationX(float angleInRadians)
            => new Trafo3f(M44f.RotationX(angleInRadians),
                           M44f.RotationX(-angleInRadians));

        public static Trafo3f RotationXInDegrees(float angleInDegrees)
            => RotationX(Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3f RotationY(float angleInRadians)
            => new Trafo3f(M44f.RotationY(angleInRadians),
                           M44f.RotationY(-angleInRadians));

        public static Trafo3f RotationYInDegrees(float angleInDegrees)
            => RotationY(Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3f RotationZ(float angleInRadians)
            => new Trafo3f(M44f.RotationZ(angleInRadians),
                           M44f.RotationZ(-angleInRadians));

        public static Trafo3f RotationZInDegrees(float angleInDegrees)
            => RotationZ(Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3f Rotation(float yawInRadians, float pitchInRadians, float rollInRadians)
        {
            var m = M44f.Rotation(yawInRadians, pitchInRadians, rollInRadians);
            return new Trafo3f(m, m.Transposed); //transposed is equal but faster to inverted on orthonormal matrices like rotations.
        }

        public static Trafo3f RotationInDegrees(float yawInDegrees, float pitchInDegrees, float rollInDegrees)
            => Rotation(yawInDegrees.RadiansFromDegrees(), pitchInDegrees.RadiansFromDegrees(), rollInDegrees.RadiansFromDegrees());

        public static Trafo3f Rotation(V3f yaw_pitch_roll_inRadians)
            => Rotation(yaw_pitch_roll_inRadians.X, yaw_pitch_roll_inRadians.Y, yaw_pitch_roll_inRadians.Z);

        public static Trafo3f RotationInDegrees(V3f yaw_pitch_roll_inDegrees)
            => RotationInDegrees(yaw_pitch_roll_inDegrees.X, yaw_pitch_roll_inDegrees.Y, yaw_pitch_roll_inDegrees.Z);

        public static Trafo3f RotateInto(V3f from, V3f into)
        {
            var rot = new Rot3f(from, into);
            var inv = rot.Inverse;
            return new Trafo3f((M44f)rot, (M44f)inv);
        }

        public static Trafo3f FromNormalFrame(V3f origin, V3f normal)
        {
            M44f.NormalFrame(origin, normal, out M44f forward, out M44f backward);
            return new Trafo3f(forward, backward);
        }

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion. 
        ///       The rotation is in Euler-Angles (yaw, pitch, roll).
        /// </summary>
        public static Trafo3f FromComponents(V3f scale, V3f rotation, V3f translation)
            => Scale(scale) * Rotation(rotation) * Translation(translation);

        public static Trafo3f ShearYZ(float factorY, float factorZ)
            => new Trafo3f(M44f.ShearYZ(factorY, factorZ),
                           M44f.ShearYZ(-factorY, -factorZ));


        public static Trafo3f ShearXZ(float factorX, float factorZ)
            => new Trafo3f(M44f.ShearXZ(factorX, factorZ),
                           M44f.ShearXZ(-factorX, -factorZ));

        public static Trafo3f ShearXY(float factorX, float factorY)
            => new Trafo3f(M44f.ShearXY(factorX, factorY),
                           M44f.ShearXY(-factorX, -factorY));

        /// <summary>
        /// Returns the trafo that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        public static Trafo3f FromBasis(V3f xAxis, V3f yAxis, V3f zAxis, V3f orign)
        {
            var mat = new M44f(
                            xAxis.X, yAxis.X, zAxis.X, orign.X,
                            xAxis.Y, yAxis.Y, zAxis.Y, orign.Y,
                            xAxis.Z, yAxis.Z, zAxis.Z, orign.Z,
                            0, 0, 0, 1);

            return new Trafo3f(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the trafo that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// NOTE that the axes MUST be normalized and normal to each other.
        /// </summary>
        public static Trafo3f FromOrthoNormalBasis(
                V3f xAxis, V3f yAxis, V3f zAxis)
        {
            return new Trafo3f(
                        new M44f(
                            xAxis.X, yAxis.X, zAxis.X, 0,
                            xAxis.Y, yAxis.Y, zAxis.Y, 0,
                            xAxis.Z, yAxis.Z, zAxis.Z, 0,
                            0, 0, 0, 1),
                        new M44f(
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
        public static Trafo3f ViewTrafo(V3f location, V3f u, V3f v, V3f z)
        {
            return new Trafo3f(
                new M44f(
                    u.X, u.Y, u.Z, -Vec.Dot(u, location),
                    v.X, v.Y, v.Z, -Vec.Dot(v, location),
                    z.X, z.Y, z.Z, -Vec.Dot(z, location),
                    0, 0, 0, 1
                ),
                new M44f(
                    u.X, v.X, z.X, location.X,
                    u.Y, v.Y, z.Y, location.Y,
                    u.Z, v.Z, z.Z, location.Z,
                    0, 0, 0, 1
                ));
        }

        /// <summary>
        /// Creates a right-handed view trafo, where z-negative points into the scene.
        /// </summary>
        public static Trafo3f ViewTrafoRH(V3f location, V3f up, V3f forward)
            => ViewTrafo(location, forward.Cross(up), up, -forward);

        /// <summary>
        /// Creates a left-handed view trafo, where z-positive points into the scene.
        /// </summary>
        public static Trafo3f ViewTrafoLH(V3f location, V3f up, V3f forward)
            => ViewTrafo(location, up.Cross(forward), up, forward);

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        public static Trafo3f PerspectiveProjectionRH(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,           f / (n - f),     (f * n) / (n - f),
                                    0,                     0,                    -1,                     0
                    ),                                                     
                                                                       
                new M44f(                                      
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
        public static Trafo3f PerspectiveProjectionOpenGl(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,     (f + n) / (n - f),  (2 * f * n) / (n - f),
                                    0,                     0,                    -1,                      0
                    ),

                new M44f(
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
        public static Trafo3f PerspectiveProjectionLH(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,                     0,                     0,
                                    0,     (2 * n) / (t - b),                     0,                     0,
                    (l + r) / (l - r),     (b + t) / (b - t),           f / (f - n),                     1,
                                    0,                     0,     (n * f) / (n - f),                     0
                    ),                                                     
                                                                       
                new M44f(                                      
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
        public static Trafo3f OrthoProjectionRH(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     1 / (n - f),           n / (n - f),
                              0,               0,               0,                     1
                    ),

                new M44f(
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
        public static Trafo3f OrthoProjectionOpenGl(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     2 / (n - f),     (f + n) / (n - f),
                              0,               0,               0,                     1
                    ),

                new M44f(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,     (n - f) / 2,          -(f + n) / 2,
                              0,               0,               0,                     1
                    )
                );
        }

        #endregion

        #region Operators

        public static bool operator ==(Trafo3f a, Trafo3f b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        public static bool operator !=(Trafo3f a, Trafo3f b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of Trafo3f multiplicaition is backward
        /// with respect to M44f multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo3f operator *(Trafo3f t0, Trafo3f t1)
            => new Trafo3f(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 

    }

    public static class Trafo3fExtensions
    {
        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static float GetScale(this M44f trafo)
            => (trafo.C0.XYZ.Length + trafo.C1.XYZ.Length + trafo.C2.XYZ.Length) / 3;

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors.
        /// NOTE: The extraction only gives absolute value (negative scale will be ignored)
        /// </summary>
        public static V3f GetScaleVector(this M44f trafo)
            => new V3f(trafo.C0.XYZ.Length, trafo.C1.XYZ.Length, trafo.C2.XYZ.Length);

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static float GetScale(this Trafo3f trafo)
            => trafo.Forward.GetScale();

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors. 
        /// </summary>
        public static V3f GetScaleVector(this Trafo3f trafo)
            => trafo.Forward.GetScaleVector();

        /// <summary>
        /// Extracts the inverse/backward translation component of the given transformation, which when given 
        /// a view transformation represents the location of the camera in world space.
        /// </summary>
        public static V3f GetViewPosition(this Trafo3f trafo)
            => trafo.Backward.C3.XYZ;

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A left-handed coordinates system transformation is expected, 
        /// where the view-space z-axis points in forward direction.
        /// </summary>
        public static V3f GetViewDirectionLH(this Trafo3f trafo)
            => trafo.Forward.R2.XYZ.Normalized;

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A right-handed coordinates system transformation is expected, where 
        /// the view-space z-axis points opposit the forward vector.
        /// </summary>
        public static V3f GetViewDirectionRH(this Trafo3f trafo)
            => -trafo.Forward.R2.XYZ.Normalized;

        /// <summary>
        /// Extracts the translation component of the given transformation, which when given 
        /// a model transformation represents the model origin in world position.
        /// </summary>
        public static V3f GetModelOrigin(this Trafo3f trafo)
            => trafo.Forward.C3.XYZ;

        /// <summary>
        /// Builds a hull from the given view-projection transformation (left, right, bottom, top, near, far).
        /// The view volume is assumed to be [-1, -1, -1] [1, 1, 1].
        /// The normals of the hull planes point to the outside and are normalized. 
        /// A point inside the visual hull will has negative height to all planes.
        /// </summary>
        public static Hull3d GetVisualHull(this M44f viewProj)
        {
            var r0 = viewProj.R0;
            var r1 = viewProj.R1;
            var r2 = viewProj.R2;
            var r3 = viewProj.R3;

            return new Hull3d(new[]
            {
                new Plane3d((V4d)(-(r3 + r0))).Normalized, // left
                new Plane3d((V4d)(-(r3 - r0))).Normalized, // right
                new Plane3d((V4d)(-(r3 + r1))).Normalized, // bottom
                new Plane3d((V4d)(-(r3 - r1))).Normalized, // top
                new Plane3d((V4d)(-(r3 + r2))).Normalized, // near
                new Plane3d((V4d)(-(r3 - r2))).Normalized, // far
            });
        }

        public static Hull3d GetVisualHull(this Trafo3f viewProj)
        {
            return viewProj.Forward.GetVisualHull();
        }

        /// <summary>
        /// Builds an ortho-normal orientation transformation form the given transform.
        /// Scale and Translation will be removed and basis vectors will be ortho-normalized.
        /// NOTE: The X-Axis is untouched and Y/Z are forced to a normal-angle.
        /// </summary>
        public static Trafo3f GetOrthoNormalOrientation(this Trafo3f trafo)
        {
            var x = trafo.Forward.C0.XYZ.Normalized; // TransformDir(V3f.XAxis)
            var y = trafo.Forward.C1.XYZ.Normalized; // TransformDir(V3f.YAxis)
            var z = trafo.Forward.C2.XYZ.Normalized; // TransformDir(V3f.ZAxis)

            y = z.Cross(x).Normalized;
            z = x.Cross(y).Normalized;

            return Trafo3f.FromBasis(x, y, z, V3f.Zero);
        }

        /// <summary>
        /// Decomposes a transformation into a scale, rotation and translation component.
        /// NOTE: The input is assumed to be a valid affine transformation.
        ///       The rotation output is in Euler-Angles (yaw, pitch, roll).
        /// </summary>
        public static void Decompose(this Trafo3f trafo, out V3f scale, out V3f rotation, out V3f translation)
        {
            translation = trafo.GetModelOrigin();
            
            var rt = trafo.GetOrthoNormalOrientation();
            if (rt.Forward.Det.IsTiny())
            {
                rotation = V3f.Zero;
            }
            else
            {
                var rot = Rot3f.FromFrame(rt.Forward.C0.XYZ, rt.Forward.C1.XYZ, rt.Forward.C2.XYZ);
                rotation = rot.GetEulerAngles();
            }

            scale = trafo.GetScaleVector();

            // if matrix is left-handed there must be some negative scale
            // since rotation remains the x-axis, the y-axis must be flipped
            if (trafo.Forward.Det < 0)
                scale.Y = -scale.Y;
        }
    }

    #endregion

    #region Trafo3d

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Trafo3d
    {
        [DataMember]
        public readonly M44d Forward;
        [DataMember]
        public readonly M44d Backward;

        #region Constructors

        public Trafo3d(M44d forward, M44d backward)
        {
            Forward = forward;
            Backward = backward;
        }

        public Trafo3d(Trafo3d trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        public Trafo3d(Euclidean3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        public Trafo3d(Similarity3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        public Trafo3d(Rot3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        #endregion

        #region Constants

        public static readonly Trafo3d Identity =
            new Trafo3d(M44d.Identity, M44d.Identity);

        #endregion

        #region Properties

        public Trafo3d Inverse => new Trafo3d(Backward, Forward);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        public override bool Equals(object other)
            => (other is Trafo3d) ? (this == (Trafo3d)other) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        #endregion

        #region Static Creators

        public static Trafo3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Trafo3d(
                M44d.Parse(x[0].ToString()),
                M44d.Parse(x[1].ToString())
            );
        }

        public static Trafo3d Translation(V3d v)
            => new Trafo3d(M44d.Translation(v), M44d.Translation(-v));

        public static Trafo3d Translation(double dx, double dy, double dz)
            => new Trafo3d(M44d.Translation(dx, dy, dz),
                           M44d.Translation(-dx, -dy, -dz));

        public static Trafo3d Scale(V3d v) 
            => new Trafo3d(M44d.Scale(v), M44d.Scale(1 / v));

        public static Trafo3d Scale(double sx, double sy, double sz)
            => new Trafo3d(M44d.Scale(sx, sy, sz),
                           M44d.Scale(1 / sx, 1 / sy, 1 / sz));

        public static Trafo3d Scale(double s)
            => new Trafo3d(M44d.Scale(s), M44d.Scale(1 / s));

        public static Trafo3d Rotation(V3d axis, double angleInRadians)
            => new Trafo3d(M44d.Rotation(axis, angleInRadians),
                           M44d.Rotation(axis, -angleInRadians));

        public static Trafo3d RotationInDegrees(V3d axis, double angleInDegrees)
            => Rotation(axis, Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3d RotationX(double angleInRadians)
            => new Trafo3d(M44d.RotationX(angleInRadians),
                           M44d.RotationX(-angleInRadians));

        public static Trafo3d RotationXInDegrees(double angleInDegrees)
            => RotationX(Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3d RotationY(double angleInRadians)
            => new Trafo3d(M44d.RotationY(angleInRadians),
                           M44d.RotationY(-angleInRadians));

        public static Trafo3d RotationYInDegrees(double angleInDegrees)
            => RotationY(Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3d RotationZ(double angleInRadians)
            => new Trafo3d(M44d.RotationZ(angleInRadians),
                           M44d.RotationZ(-angleInRadians));

        public static Trafo3d RotationZInDegrees(double angleInDegrees)
            => RotationZ(Conversion.RadiansFromDegrees(angleInDegrees));

        public static Trafo3d Rotation(double yawInRadians, double pitchInRadians, double rollInRadians)
        {
            var m = M44d.Rotation(yawInRadians, pitchInRadians, rollInRadians);
            return new Trafo3d(m, m.Transposed); //transposed is equal but faster to inverted on orthonormal matrices like rotations.
        }

        public static Trafo3d RotationInDegrees(double yawInDegrees, double pitchInDegrees, double rollInDegrees)
            => Rotation(yawInDegrees.RadiansFromDegrees(), pitchInDegrees.RadiansFromDegrees(), rollInDegrees.RadiansFromDegrees());

        public static Trafo3d Rotation(V3d yaw_pitch_roll_inRadians)
            => Rotation(yaw_pitch_roll_inRadians.X, yaw_pitch_roll_inRadians.Y, yaw_pitch_roll_inRadians.Z);

        public static Trafo3d RotationInDegrees(V3d yaw_pitch_roll_inDegrees)
            => RotationInDegrees(yaw_pitch_roll_inDegrees.X, yaw_pitch_roll_inDegrees.Y, yaw_pitch_roll_inDegrees.Z);

        public static Trafo3d RotateInto(V3d from, V3d into)
        {
            var rot = new Rot3d(from, into);
            var inv = rot.Inverse;
            return new Trafo3d((M44d)rot, (M44d)inv);
        }

        public static Trafo3d FromNormalFrame(V3d origin, V3d normal)
        {
            M44d.NormalFrame(origin, normal, out M44d forward, out M44d backward);
            return new Trafo3d(forward, backward);
        }

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion. 
        ///       The rotation is in Euler-Angles (yaw, pitch, roll).
        /// </summary>
        public static Trafo3d FromComponents(V3d scale, V3d rotation, V3d translation)
            => Scale(scale) * Rotation(rotation) * Translation(translation);

        public static Trafo3d ShearYZ(double factorY, double factorZ)
            => new Trafo3d(M44d.ShearYZ(factorY, factorZ),
                           M44d.ShearYZ(-factorY, -factorZ));


        public static Trafo3d ShearXZ(double factorX, double factorZ)
            => new Trafo3d(M44d.ShearXZ(factorX, factorZ),
                           M44d.ShearXZ(-factorX, -factorZ));

        public static Trafo3d ShearXY(double factorX, double factorY)
            => new Trafo3d(M44d.ShearXY(factorX, factorY),
                           M44d.ShearXY(-factorX, -factorY));

        /// <summary>
        /// Returns the trafo that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        public static Trafo3d FromBasis(V3d xAxis, V3d yAxis, V3d zAxis, V3d orign)
        {
            var mat = new M44d(
                            xAxis.X, yAxis.X, zAxis.X, orign.X,
                            xAxis.Y, yAxis.Y, zAxis.Y, orign.Y,
                            xAxis.Z, yAxis.Z, zAxis.Z, orign.Z,
                            0, 0, 0, 1);

            return new Trafo3d(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the trafo that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// NOTE that the axes MUST be normalized and normal to each other.
        /// </summary>
        public static Trafo3d FromOrthoNormalBasis(
                V3d xAxis, V3d yAxis, V3d zAxis)
        {
            return new Trafo3d(
                        new M44d(
                            xAxis.X, yAxis.X, zAxis.X, 0,
                            xAxis.Y, yAxis.Y, zAxis.Y, 0,
                            xAxis.Z, yAxis.Z, zAxis.Z, 0,
                            0, 0, 0, 1),
                        new M44d(
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
        public static Trafo3d ViewTrafo(V3d location, V3d u, V3d v, V3d z)
        {
            return new Trafo3d(
                new M44d(
                    u.X, u.Y, u.Z, -Vec.Dot(u, location),
                    v.X, v.Y, v.Z, -Vec.Dot(v, location),
                    z.X, z.Y, z.Z, -Vec.Dot(z, location),
                    0, 0, 0, 1
                ),
                new M44d(
                    u.X, v.X, z.X, location.X,
                    u.Y, v.Y, z.Y, location.Y,
                    u.Z, v.Z, z.Z, location.Z,
                    0, 0, 0, 1
                ));
        }

        /// <summary>
        /// Creates a right-handed view trafo, where z-negative points into the scene.
        /// </summary>
        public static Trafo3d ViewTrafoRH(V3d location, V3d up, V3d forward)
            => ViewTrafo(location, forward.Cross(up), up, -forward);

        /// <summary>
        /// Creates a left-handed view trafo, where z-positive points into the scene.
        /// </summary>
        public static Trafo3d ViewTrafoLH(V3d location, V3d up, V3d forward)
            => ViewTrafo(location, up.Cross(forward), up, forward);

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        public static Trafo3d PerspectiveProjectionRH(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,           f / (n - f),     (f * n) / (n - f),
                                    0,                     0,                    -1,                     0
                    ),                                                     
                                                                       
                new M44d(                                      
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
        public static Trafo3d PerspectiveProjectionOpenGl(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,     (f + n) / (n - f),  (2 * f * n) / (n - f),
                                    0,                     0,                    -1,                      0
                    ),

                new M44d(
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
        public static Trafo3d PerspectiveProjectionLH(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,                     0,                     0,
                                    0,     (2 * n) / (t - b),                     0,                     0,
                    (l + r) / (l - r),     (b + t) / (b - t),           f / (f - n),                     1,
                                    0,                     0,     (n * f) / (n - f),                     0
                    ),                                                     
                                                                       
                new M44d(                                      
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
        public static Trafo3d OrthoProjectionRH(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     1 / (n - f),           n / (n - f),
                              0,               0,               0,                     1
                    ),

                new M44d(
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
        public static Trafo3d OrthoProjectionOpenGl(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     2 / (n - f),     (f + n) / (n - f),
                              0,               0,               0,                     1
                    ),

                new M44d(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,     (n - f) / 2,          -(f + n) / 2,
                              0,               0,               0,                     1
                    )
                );
        }

        #endregion

        #region Operators

        public static bool operator ==(Trafo3d a, Trafo3d b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        public static bool operator !=(Trafo3d a, Trafo3d b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of Trafo3d multiplicaition is backward
        /// with respect to M44d multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo3d operator *(Trafo3d t0, Trafo3d t1)
            => new Trafo3d(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 

    }

    public static class Trafo3dExtensions
    {
        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static double GetScale(this M44d trafo)
            => (trafo.C0.XYZ.Length + trafo.C1.XYZ.Length + trafo.C2.XYZ.Length) / 3;

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors.
        /// NOTE: The extraction only gives absolute value (negative scale will be ignored)
        /// </summary>
        public static V3d GetScaleVector(this M44d trafo)
            => new V3d(trafo.C0.XYZ.Length, trafo.C1.XYZ.Length, trafo.C2.XYZ.Length);

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static double GetScale(this Trafo3d trafo)
            => trafo.Forward.GetScale();

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors. 
        /// </summary>
        public static V3d GetScaleVector(this Trafo3d trafo)
            => trafo.Forward.GetScaleVector();

        /// <summary>
        /// Extracts the inverse/backward translation component of the given transformation, which when given 
        /// a view transformation represents the location of the camera in world space.
        /// </summary>
        public static V3d GetViewPosition(this Trafo3d trafo)
            => trafo.Backward.C3.XYZ;

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A left-handed coordinates system transformation is expected, 
        /// where the view-space z-axis points in forward direction.
        /// </summary>
        public static V3d GetViewDirectionLH(this Trafo3d trafo)
            => trafo.Forward.R2.XYZ.Normalized;

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A right-handed coordinates system transformation is expected, where 
        /// the view-space z-axis points opposit the forward vector.
        /// </summary>
        public static V3d GetViewDirectionRH(this Trafo3d trafo)
            => -trafo.Forward.R2.XYZ.Normalized;

        /// <summary>
        /// Extracts the translation component of the given transformation, which when given 
        /// a model transformation represents the model origin in world position.
        /// </summary>
        public static V3d GetModelOrigin(this Trafo3d trafo)
            => trafo.Forward.C3.XYZ;

        /// <summary>
        /// Builds a hull from the given view-projection transformation (left, right, bottom, top, near, far).
        /// The view volume is assumed to be [-1, -1, -1] [1, 1, 1].
        /// The normals of the hull planes point to the outside and are normalized. 
        /// A point inside the visual hull will has negative height to all planes.
        /// </summary>
        public static Hull3d GetVisualHull(this M44d viewProj)
        {
            var r0 = viewProj.R0;
            var r1 = viewProj.R1;
            var r2 = viewProj.R2;
            var r3 = viewProj.R3;

            return new Hull3d(new[]
            {
                new Plane3d((-(r3 + r0))).Normalized, // left
                new Plane3d((-(r3 - r0))).Normalized, // right
                new Plane3d((-(r3 + r1))).Normalized, // bottom
                new Plane3d((-(r3 - r1))).Normalized, // top
                new Plane3d((-(r3 + r2))).Normalized, // near
                new Plane3d((-(r3 - r2))).Normalized, // far
            });
        }

        public static Hull3d GetVisualHull(this Trafo3d viewProj)
        {
            return viewProj.Forward.GetVisualHull();
        }

        /// <summary>
        /// Builds an ortho-normal orientation transformation form the given transform.
        /// Scale and Translation will be removed and basis vectors will be ortho-normalized.
        /// NOTE: The X-Axis is untouched and Y/Z are forced to a normal-angle.
        /// </summary>
        public static Trafo3d GetOrthoNormalOrientation(this Trafo3d trafo)
        {
            var x = trafo.Forward.C0.XYZ.Normalized; // TransformDir(V3d.XAxis)
            var y = trafo.Forward.C1.XYZ.Normalized; // TransformDir(V3d.YAxis)
            var z = trafo.Forward.C2.XYZ.Normalized; // TransformDir(V3d.ZAxis)

            y = z.Cross(x).Normalized;
            z = x.Cross(y).Normalized;

            return Trafo3d.FromBasis(x, y, z, V3d.Zero);
        }

        /// <summary>
        /// Decomposes a transformation into a scale, rotation and translation component.
        /// NOTE: The input is assumed to be a valid affine transformation.
        ///       The rotation output is in Euler-Angles (yaw, pitch, roll).
        /// </summary>
        public static void Decompose(this Trafo3d trafo, out V3d scale, out V3d rotation, out V3d translation)
        {
            translation = trafo.GetModelOrigin();
            
            var rt = trafo.GetOrthoNormalOrientation();
            if (rt.Forward.Det.IsTiny())
            {
                rotation = V3d.Zero;
            }
            else
            {
                var rot = Rot3d.FromFrame(rt.Forward.C0.XYZ, rt.Forward.C1.XYZ, rt.Forward.C2.XYZ);
                rotation = rot.GetEulerAngles();
            }

            scale = trafo.GetScaleVector();

            // if matrix is left-handed there must be some negative scale
            // since rotation remains the x-axis, the y-axis must be flipped
            if (trafo.Forward.Det < 0)
                scale.Y = -scale.Y;
        }
    }

    #endregion

}
