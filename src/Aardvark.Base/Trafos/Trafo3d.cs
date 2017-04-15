using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Trafo3d
    {
        public readonly M44d Forward;
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

        public Trafo3d Inverse
        {
            get { return new Trafo3d(Backward, Forward); }
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Forward, Backward);
        }

        public override bool Equals(object other)
        {
            return (other is Trafo3d) ? (this == (Trafo3d)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "[{0}, {1}]", Forward, Backward);
        }

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
        {
            return new Trafo3d(M44d.Translation(v),
                               M44d.Translation(-v));
        }

        public static Trafo3d Translation(double dx, double dy, double dz)
        {
            return new Trafo3d(M44d.Translation(dx, dy, dz),
                               M44d.Translation(-dx, -dy, -dz));
        }

        public static Trafo3d Scale(V3d v)
        {
            return new Trafo3d(M44d.Scale(v),
                               M44d.Scale(1 / v));
        }

        public static Trafo3d Scale(double sx, double sy, double sz)
        {
            return new Trafo3d(M44d.Scale(sx, sy, sz),
                               M44d.Scale(1 / sx, 1 / sy, 1 / sz));
        }

        public static Trafo3d Scale(double s)
        {
            return new Trafo3d(M44d.Scale(s),
                               M44d.Scale(1 / s));
        }

        public static Trafo3d Rotation(V3d axis, double angleInRadians)
        {
            return new Trafo3d(M44d.Rotation(axis, angleInRadians),
                               M44d.Rotation(axis, -angleInRadians));
        }

        public static Trafo3d RotationInDegrees(V3d axis, double angleInDegrees)
        {
            return Rotation(axis, Conversion.RadiansFromDegrees(angleInDegrees));
        }

        public static Trafo3d RotationX(double angleInRadians)
        {
            return new Trafo3d(M44d.RotationX(angleInRadians),
                               M44d.RotationX(-angleInRadians));
        }

        public static Trafo3d RotationXInDegrees(double angleInDegrees)
        {
            return RotationX(Conversion.RadiansFromDegrees(angleInDegrees));
        }

        public static Trafo3d RotationY(double angleInRadians)
        {
            return new Trafo3d(M44d.RotationY(angleInRadians),
                               M44d.RotationY(-angleInRadians));
        }

        public static Trafo3d RotationYInDegrees(double angleInDegrees)
        {
            return RotationY(Conversion.RadiansFromDegrees(angleInDegrees));
        }

        public static Trafo3d RotationZ(double angleInRadians)
        {
            return new Trafo3d(M44d.RotationZ(angleInRadians),
                               M44d.RotationZ(-angleInRadians));
        }

        public static Trafo3d RotationZInDegrees(double angleInDegrees)
        {
            return RotationZ(Conversion.RadiansFromDegrees(angleInDegrees));
        }

        public static Trafo3d Rotation(double yawInRadians, double pitchInRadians, double rollInRadians)
        {
            var m = M44d.Rotation(yawInRadians, pitchInRadians, rollInRadians);
            return new Trafo3d(m, m.Transposed); //transposed is equal but faster to inverted on orthonormal matrices like rotations.
        }

        public static Trafo3d RotationInDegrees(double yawInDegrees, double pitchInDegrees, double rollInDegrees)
        {
            return Rotation(yawInDegrees.RadiansFromDegrees(), pitchInDegrees.RadiansFromDegrees(), rollInDegrees.RadiansFromDegrees());
        }

        public static Trafo3d Rotation(V3d yaw_pitch_roll_inRadians)
        {
            return Rotation(yaw_pitch_roll_inRadians.X, yaw_pitch_roll_inRadians.Y, yaw_pitch_roll_inRadians.Z);
        }

        public static Trafo3d RotationInDegrees(V3d yaw_pitch_roll_inDegrees)
        {
            return RotationInDegrees(yaw_pitch_roll_inDegrees.X, yaw_pitch_roll_inDegrees.Y, yaw_pitch_roll_inDegrees.Z);
        }

        public static Trafo3d RotateInto(V3d from, V3d into)
        {
            var rot = new Rot3d(from, into);
            var inv = rot.Inverse;
            return new Trafo3d((M44d)rot, (M44d)inv);
        }

        public static Trafo3d FromNormalFrame(V3d origin, V3d normal)
        {
            M44d forward, backward;
            M44d.NormalFrame(origin, normal, out forward, out backward);
            return new Trafo3d(forward, backward);
        }

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation and translation componets.
        /// NOTE: Uses the Scale * Rotation * Translation notion. 
        ///       The rotation is in Euler-Angles (yaw, pitch, roll).
        /// </summary>
        public static Trafo3d FromComponents(V3d scale, V3d rotation, V3d translation)
        {
            return Trafo3d.Scale(scale) * Trafo3d.Rotation(rotation) * Trafo3d.Translation(translation);
        }

        public static Trafo3d ShearYZ(double factorY, double factorZ)
        {
            return new Trafo3d(M44d.ShearYZ(factorY, factorZ),
                               M44d.ShearYZ(-factorY, -factorZ));
        }

        public static Trafo3d ShearXZ(double factorX, double factorZ)
        {
            return new Trafo3d(M44d.ShearXZ(factorX, factorZ),
                               M44d.ShearXZ(-factorX, -factorZ));
        }

        public static Trafo3d ShearXY(double factorX, double factorY)
        {
            return new Trafo3d(M44d.ShearXY(factorX, factorY),
                               M44d.ShearXY(-factorX, -factorY));
        }

        /// <summary>
        /// Returns the trafo that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        public static Trafo3d FromBasis(V3f xAxis, V3f yAxis, V3f zAxis, V3f orign)
        {
            return Trafo3d.FromBasis((V3d)xAxis, (V3d)yAxis, (V3d)zAxis, (V3d)orign);
        }

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

        public static Trafo3d ViewTrafo(V3d location, V3d u, V3d v, V3d z)
        {
            return new Trafo3d(
                new M44d(
                    u.X, u.Y, u.Z, -V3d.Dot(u, location),
                    v.X, v.Y, v.Z, -V3d.Dot(v, location),
                    z.X, z.Y, z.Z, -V3d.Dot(z, location),
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
        {
            return Trafo3d.ViewTrafo(location, forward.Cross(up), up, -forward);
        }

        /// <summary>
        /// Creates a left-handed view trafo, where z-positive points into the scene.
        /// </summary>
        public static Trafo3d ViewTrafoLH(V3d location, V3d up, V3d forward)
        {
            return Trafo3d.ViewTrafo(location, up.Cross(forward), up, forward);
        }

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

        #endregion

        #region Operators

        public static bool operator ==(Trafo3d a, Trafo3d b)
        {
            return
                a.Forward == b.Forward &&
                a.Backward == b.Backward;
        }

        public static bool operator !=(Trafo3d a, Trafo3d b)
        {
            return
                a.Forward != b.Forward ||
                a.Backward != b.Backward;
        }

        /// <summary>
        /// The order of operation of Trafo3d multiplicaition is backward
        /// with respect to M44d multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo3d operator *(Trafo3d t0, Trafo3d t1)
        {
            return new Trafo3d(t1.Forward * t0.Forward,
                               t0.Backward * t1.Backward);
        }

        #endregion 

    }

    public static class Trafo3dExtensions
    {
        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static double GetScale(this M44d trafo)
        {
            return (trafo.C0.XYZ.Length + trafo.C1.XYZ.Length + trafo.C2.XYZ.Length) / 3;
        }

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors.
        /// NOTE: The extraction only gives absolute value (negative scale will be ignored)
        /// </summary>
        public static V3d GetScaleVector(this M44d trafo)
        {
            return new V3d(trafo.C0.XYZ.Length, trafo.C1.XYZ.Length, trafo.C2.XYZ.Length);
        }

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        public static double GetScale(this Trafo3d trafo)
        {
            return trafo.Forward.GetScale();
        }

        /// <summary>
        /// Extracts a scale vector from the given matrix by calculating the lengths of the basis vectors. 
        /// </summary>
        public static V3d GetScaleVector(this Trafo3d trafo)
        {
            return trafo.Forward.GetScaleVector();
        }

        /// <summary>
        /// Extracts the inverse/backward translation component of the given transformation, which when given 
        /// a view transformation represents the location of the camera in world space.
        /// </summary>
        public static V3d GetViewPosition(this Trafo3d trafo)
        {
            return trafo.Backward.C3.XYZ;
        }

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A left-handed coordinates system transformation is expected, 
        /// where the view-space z-axis points in forward direction.
        /// </summary>
        public static V3d GetViewDirectionLH(this Trafo3d trafo)
        {
            return trafo.Forward.R2.XYZ.Normalized;
        }

        /// <summary>
        /// Extracts the Z-Axis from the given transformation.
        /// NOTE: A right-handed coordinates system transformation is expected, where 
        /// the view-space z-axis points opposit the forward vector.
        /// </summary>
        public static V3d GetViewDirectionRH(this Trafo3d trafo)
        {
            return -trafo.Forward.R2.XYZ.Normalized;
        }

        /// <summary>
        /// Extracts the translation component of the given transformation, which when given 
        /// a model transformation represents the model origin in world position.
        /// </summary>
        public static V3d GetModelOrigin(this Trafo3d trafo)
        {
            return trafo.Forward.C3.XYZ;
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
}
