using System;

namespace Aardvark.Base
{
    // xp     R00 R10 R20     (P.X      T.X)
    // yp  =  R01 R11 R21  *  (P.Y  -   T.Y)
    // zp     R02 R12 R22     (P.Z      T.X)

    [Obsolete]
    public struct CameraExtrinsics
    {
        /// <summary>
        /// Camera to world rotation.
        /// </summary>
        public readonly M33d Rotation;          // cam 2 world was chosen, so that it can be easily read
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
            if (cam2worldRotation.Determinant < 0.0)
                throw new ArgumentException("supplied rotation is improper (mirroring)");
            Rotation = cam2worldRotation;
            Translation = cam2worldTranslation;
        }

        public V3d CameraPointFromWorldPoint(V3d worldPoint)
            => Rotation.TransposedTransform(worldPoint - Translation);

        public V3d WorldPointFromCameraPoint(V3d cameraPoint)
            => Rotation.Transform(cameraPoint) + Translation;

        public M33d Camera2WorldRotation => Rotation;
        public V3d Camera2WorldTranslation => Translation;
        public M33d World2CameraRotation => Rotation.Transposed;
        public V3d World2CameraTranslation => -Rotation.TransposedTransform(Translation);
        public V3d World2CameraRotationAngleAxis => Rot3d.FromM33d(Rotation.Transposed).ToAngleAxis();

        public static CameraExtrinsics FromWorld2Camera(M33d rotation, V3d translation)
            => new CameraExtrinsics(rotation.Transposed, -rotation.TransposedTransform(translation));

        public static CameraExtrinsics FromWorld2Camera(V3d angleAxis, V3d translation)
            => FromWorld2Camera((M33d)Rot3d.FromAngleAxis(angleAxis), translation);

        public M44d Camera2World => new M44d(
            Rotation.M00, Rotation.M01, Rotation.M02, Translation.X,
            Rotation.M10, Rotation.M11, Rotation.M12, Translation.Y,
            Rotation.M20, Rotation.M21, Rotation.M22, Translation.Z,
            0.0, 0.0, 0.0, 1.0);

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

    [Obsolete]
    public struct CameraIntrinsics
    {
        public readonly V2d FocalLength;                     // 2    (fx, fy)    1.0 == Fun.Max(size.X, size.Y)
        public readonly V2d PrincipalPoint;                  // 2    (cx, cy)    [0..1], [0..1]
        public readonly double Skew;                         // 1    1.0 == Fun.Max(size.X, size.Y)
        public readonly double K1, K2, K3;                   // 3    radial distortion coefficients r^2, r^4, r^6
        public readonly double P1, P2;                       // 2    tangential distortion coefficients

        /// <summary>
        /// Construct the camera instinsics.
        /// </summary>
        /// <param name="focalLength">measured in pixels</param>
        /// <param name="principalPoint">measured in pixels from [0,0] at the left upper corner of the image</param>
        /// <param name="skew"></param>
        /// <param name="k1">radial distortion parameter coefficient of r^2</param>
        /// <param name="k2">radial distortion parameter coefficient of r^4</param>
        /// <param name="k3">radial distortion parameter coefficient of r^6</param>
        /// <param name="p1">tagnential distortion parameter 1</param>
        /// <param name="p2">tagnential distortion parameter 2</param>
        public CameraIntrinsics(
            V2d focalLength,
            V2d principalPoint,
            double skew,
            double k1, double k2, double k3,
            double p1, double p2)
        {
            FocalLength = focalLength;
            PrincipalPoint = principalPoint;
            Skew = skew;
            K1 = k1; K2 = k2; K3 = k3;
            P1 = p1; P2 = p2;
        }

        /// <summary>
        /// Project a point from camera space into image space according to the intrinsic camera parameters.
        /// This documents the camera model.
        /// </summary>
        public V2d ProjectToImage(V3d p, V2d imageSize)
        {
            // projection
            double x = p.X / p.Z;
            double y = p.Y / p.Z;

            // undo distortion here
            ComputeUndistortionParamsForCameraPoint(x, y, out double rd, out double tx, out double ty);

            // undistorted point:
            double xd = x * rd + tx;
            double yd = y * rd + ty;

            double maxSize = Fun.Max(imageSize.X, imageSize.Y);

            // point in image:
            return new V2d(
                FocalLength.X * maxSize * xd + Skew * maxSize * yd + PrincipalPoint.X * imageSize.X,
                FocalLength.Y * maxSize * yd                       + PrincipalPoint.Y * imageSize.Y);
        }

        public void ComputeUndistortionParamsForCameraPoint(double x, double y,
            out double rd, out double tx, out double ty)
        {
            // radial distortion
            double x2 = x * x, y2 = y * y, r2 = x2 + y2;
            rd = 1 + r2 * (K1 + r2 * (K2 + r2 * K3));

            // tantential distortion:
            var xy = x * y;
            tx = 2 * P1 * xy + P2 * (r2 + 2 * x2);
            ty = 2 * P2 * xy + P1 * (r2 + 2 * y2);
        }

        /// <summary>
        /// UnDistortion of pixel coordinates.
        /// </summary>
        public V2d UndistortPixel(V2d p2, V2i imageSize)
        {
            var maxSize = Fun.Max(imageSize.X, imageSize.Y);
            var y = (p2.Y - PrincipalPoint.Y * imageSize.Y) / (FocalLength.Y * maxSize);
            var x = (p2.X - PrincipalPoint.X * imageSize.X - Skew * maxSize * y) / (FocalLength.X * maxSize);

            ComputeUndistortionParamsForCameraPoint(x, y, out double rd, out double tx, out double ty);
            double xd = x * rd + tx;
            double yd = y * rd + ty;

            return new V2d(
                FocalLength.X * maxSize * xd + Skew * maxSize * yd + PrincipalPoint.X * imageSize.X,
                FocalLength.Y * maxSize * yd + PrincipalPoint.Y * imageSize.Y);
        }

        ///// <summary> TODO 51: where to put this?
        ///// Computes an undistorted Grid with (count.X + 1, count.Y + 1) vertices
        ///// the coordinates are roughly in the range [0..imageSize.X, 0..imageSize.Y].
        ///// These can be used for a piecewise linear undistortion using the graphics
        ///// card. If count is not given, it is set to iamgeSize / 8, for one grid
        ///// point each 8 pixels.
        ///// </summary>
        //Matrix<V2f> ComputeUndistortedGrid(V2i imageSize, V2i count = default(V2i))
        //{
        //    if (count == default(V2i)) count = imageSize / 8;
        //    var grid = new Matrix<V2f>(count + V2i.II);
        //    var delta = imageSize.ToV2d() / count.ToV2d();
        //    var self = this;
        //    grid.SetByCoord((x, y) => self.UndistortPixel(new V2d(x * delta.X, y * delta.Y), imageSize).ToV2f());
        //    return grid;
        //}
    }
}
