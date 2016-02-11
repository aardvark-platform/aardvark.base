using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    // xp     R00 R10 R20     (P.X      T.X)
    // yp  =  R01 R11 R21  *  (P.Y  -   T.Y)
    // zp     R02 R12 R22     (P.Z      T.X)

    public struct CameraExtrinsics
    {
        /// <summary>
        /// Camera to world rotation.
        /// </summary>
        public readonly M33d Rotation;          // cam 2 world was chose, so that it can be easily read
                                                // while debugging

        /// <summary>
        /// Camera to World translation.
        /// </summary>
        public readonly V3d Translation;        // cam 2 world was chosen, so that it can be easily read
                                                // while debugging

        public CameraExtrinsics(M33d cam2worldRotation, V3d cam2worldTranslation)
        {
            var zero = cam2worldRotation * cam2worldRotation.Transposed - M33d.Identity;
            if (zero.NormMax > 1e6)
                throw new ArgumentException("supplied matrix is not a rotation");
            if (cam2worldRotation.Determinant() < 0.0)
                throw new ArgumentException("supplied rotation is improper (mirroring)");
            Rotation = cam2worldRotation;
            Translation = cam2worldTranslation;
        }

        public M33d Camera2WorldRotation => Rotation;
        public V3d Camera2WorldTranslation => Translation;
        public M33d World2CameraRotation => Rotation.Transposed;
        public V3d World2CameraTranslation => -Rotation.TransposedTransform(Translation);

        public CameraExtrinsics FromWorld2Camera(M33d rotation, V3d translation)
        {
            return new CameraExtrinsics(rotation.Transposed, -rotation.TransposedTransform(translation));
        }

        public M44d Camera2World
        {
            get
            {
                return new M44d(Rotation.M00, Rotation.M01, Rotation.M02, Translation.X,
                                Rotation.M10, Rotation.M11, Rotation.M12, Translation.Y,
                                Rotation.M20, Rotation.M21, Rotation.M22, Translation.Z,
                                0.0, 0.0, 0.0, 1.0);
            }
        }

        public M44d World2Camera
        {
            get
            {
                var shift = Rotation.TransposedTransform(Translation);
                return new M44d(Rotation.M00, Rotation.M10, Rotation.M20, -shift.X,
                                Rotation.M01, Rotation.M11, Rotation.M21, -shift.Y,
                                Rotation.M02, Rotation.M12, Rotation.M22, -shift.Z,
                                0.0, 0.0, 0.0, 1.0);
            }
        }

    }

    public struct CameraIntrinsics
    {
        public V2d FocalLength;                     // 2    (fx, fy)    in pixels
        public V2d PrincipalPoint;                  // 2    (cx, cy)    in pixels
        public double Skew;                         // 1    sk
        public Tup3<double> RadialDistortion;       // 3    (k1, k2, k3)
        public Tup2<double> TangentialDistortion;   // 2    (t1, t2) assumed 0.0 if not available


        public CameraIntrinsics(
            V2d focalLength,
            V2d principalPoint,
            double skew,
            Tup3<double> radialDistortion,
            Tup2<double> tangentialDistortion)
        {
            FocalLength = focalLength;
            PrincipalPoint = principalPoint;
            Skew = skew;
            RadialDistortion = radialDistortion;
            TangentialDistortion = tangentialDistortion;
        }

        /// <summary>
        /// Project a point into image space according to the intrinsic camera parameters.
        /// This documents the camera model.
        /// </summary>
        public V2d ProjectToImage(V3d p)
        {
            // projection
            double x = p.X / p.Z;
            double y = p.Y / p.Z;

            // undo distortion here
            double rd, tx, ty;
            ComputeUnDistortionParamsForPoint(x, y, out rd, out tx, out ty);
            
            // undistorted point:
            double xd = x * rd + tx;
            double yd = y * rd + ty;

            // point in image:
            return new V2d(
                FocalLength.X * xd + Skew * yd + PrincipalPoint.X,
                FocalLength.Y * yd             + PrincipalPoint.Y);
        }

        public void ComputeUnDistortionParamsForPoint(double x, double y, out double rd, out double tx, out double ty)
        {
            // radial distortion
            double x2 = x * x, y2 = y * y, r2 = x2 + y2;
            rd = 1 + r2 * (RadialDistortion.E0 + r2 * (RadialDistortion.E1 + r2 * RadialDistortion.E2));

            // tantential distortion:
            var xy = x * y;
            tx = 2 * TangentialDistortion.E0 * xy + TangentialDistortion.E1 * (r2 + 2 * x2);
            ty = 2 * TangentialDistortion.E1 * xy + TangentialDistortion.E0 * (r2 + 2 * y2);
        }


        public V2d UnDistortPixel(V2d p2)
        {
            var y = (p2.Y - PrincipalPoint.Y) / FocalLength.Y;
            var x = (p2.X - PrincipalPoint.X - Skew * y) / FocalLength.X;

            double rd, tx, ty;
            ComputeUnDistortionParamsForPoint(x, y, out rd, out tx, out ty);
            double xd = x * rd + tx;
            double yd = y * rd + ty;

            return new V2d(
                FocalLength.X * xd + Skew * yd + PrincipalPoint.X,
                FocalLength.Y * yd + PrincipalPoint.Y);
        }

        /// <summary>
        /// Computes an undistorted Grid with (count.X + 1, count.Y + 1) vertices
        /// the coordinates are roughly in the range [0..imageSize.X, 0..imageSize.Y].
        /// These can be used for a piecewise linear undistortion using the graphics
        /// card. If count is not given, it is set to iamgeSize / 8, for one grid
        /// point each 8 pixels. 
        /// </summary>
        Matrix<V2f> ComputeUnDistortedGrid(V2i imageSize, V2i count = default(V2i))
        {
            if (count == default(V2i)) count = imageSize / 8;
            var grid = new Matrix<V2f>(count + V2i.II);
            var delta = imageSize.ToV2d() / count.ToV2d();
            var self = this;
            grid.SetByCoord((x, y) => self.UnDistortPixel(new V2d(x * delta.X, y * delta.Y)).ToV2f());
            return grid;
        }

    }





}
