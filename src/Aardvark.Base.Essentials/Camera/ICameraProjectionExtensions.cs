using Aardvark.Base;

namespace Aardvark.Base
{
    public static class ICameraProjectionExtensions
    {
        /// <summary>
        /// Gets line (in camera space) from point on near plane to corresponding point on far plane.
        /// </summary>
        public static Line3d Unproject(this ICameraProjection self, Ndc2d p)
        {
            return new Line3d(
                self.UnprojectPointOnNearPlane(p),
                self.UnprojectPointOnFarPlane(p)
                );
        }

        /// <summary>
        /// Gets point (in camera space) from point in normalized device coordinates.
        /// </summary>
        public static V3d Unproject(this ICameraProjection self, Ndc3d p)
        {
            return self.ProjectionTrafo.Backward.TransformPosProj(p.Position);
        }

        /// <summary>
        /// Gets point on near plane (in camera space) from point in normalized device coordinates.
        /// </summary>
        public static V3d UnprojectPointOnNearPlane(this ICameraProjection self, Ndc2d p)
        {
            return self.ProjectionTrafo.Backward.TransformPosProj(new V3d(p.Position, 0.0));
        }

        /// <summary>
        /// Gets point on far plane (in camera space) from point in normalized device coordinates.
        /// </summary>
        public static V3d UnprojectPointOnFarPlane(this ICameraProjection self, Ndc2d p)
        {
            return self.ProjectionTrafo.Backward.TransformPosProj(new V3d(p.Position, 1.0));
        }

        /// <summary>
        /// Gets normalized device coordinates from point in camera space.
        /// </summary>
        public static Ndc3d TransformPos(this ICameraProjection self, V3d posInCameraSpace)
        {
            return new Ndc3d(self.ProjectionTrafo.Forward.TransformPosProj(posInCameraSpace));
        }
    }
}
