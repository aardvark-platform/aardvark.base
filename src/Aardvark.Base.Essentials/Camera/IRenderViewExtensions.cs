using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static class IRenderViewExtensions
    {
        /// <summary>
        /// Projects a point from world-space to normalized device coordinates.
        /// </summary>
        public static Ndc3d Project(this IRenderView rv, V3d point)
        {
            var viewPoint = rv.View.ViewTrafo.Forward.TransformPos(point);
            return rv.Projection.TransformPos(viewPoint);
        }

        /// <summary>
        /// Project a point from world-space to a pixel position.
        /// </summary>
        public static PixelPosition ProjectPixel(this IRenderView rv, V3d point)
        {
            var ndcPoint = rv.Project(point);
            return new PixelPosition(ndcPoint, rv.Region);
        }

        /// <summary>
        /// Returns the number of pixels per unit on the near plane. 
        /// ISSUE: Does not consider ClippingWindow of projection
        /// </summary>
        public static V2d PixelsPerUnitOnNearPlane(this IRenderView rv)
        {
            if (rv.Projection is ICameraProjectionPerspective)
            {
                var unitsOnNearPlane = Fun.Tan(((ICameraProjectionPerspective)rv.Projection).HorizontalFieldOfViewInDegrees.RadiansFromDegrees() * 0.5) * 2 * rv.Projection.Near;
                return (V2d)rv.Region.Size / new V2d(unitsOnNearPlane, unitsOnNearPlane / rv.Projection.AspectRatio);
            }
            else
            {
                return (V2d)rv.Region.Size / rv.Projection.ClippingWindow.Size; 
            }
        }

        /// <summary>
        /// Builds a hull from the given view-projection. The normals of the hull planes point to the inside. 
        /// A point inside the visual hull will have positive height to all planes.
        /// </summary>
        public static Hull3d GetVisualHull(this IViewProjection vp)
        {
            var mvp = vp.View.ViewTrafo * vp.Projection.ProjectionTrafo;

            var frustumCorners = new Box3d(-V3d.IIO, V3d.III).ComputeCorners();

            // use inverse view-projection to get vertices in world space
            frustumCorners.Apply(c => mvp.Backward.TransformPosProj(c));

            // hull planes should point inward, assume right-handed transformation to build planes
            var hull = new Hull3d(new[]
            {
                new Plane3d(frustumCorners[0], frustumCorners[2], frustumCorners[4]), // left
                new Plane3d(frustumCorners[1], frustumCorners[5], frustumCorners[3]), // right
                new Plane3d(frustumCorners[2], frustumCorners[3], frustumCorners[7]), // bottom
                new Plane3d(frustumCorners[0], frustumCorners[4], frustumCorners[1]), // top
                new Plane3d(frustumCorners[0], frustumCorners[1], frustumCorners[2]), // near
                new Plane3d(frustumCorners[4], frustumCorners[6], frustumCorners[5]), // far
            });

            // handedness of transformation -> flip planes in case of left-handed
            if (mvp.Forward.Det < 0)
                hull.PlaneArray.Apply(p => p.Reversed);
            
            return hull;
        }

        /// <summary>
        /// Returns the ray-direction towards the center of the view.
        /// </summary>
        public static V3d CentralDirection(this IViewProjection vp)
        {
            var trafo = vp.View.ViewTrafo.Backward * vp.Projection.ProjectionTrafo.Backward;

            var nearPoint = trafo.TransformPosProj(V3d.OOO);
            var farPoint = trafo.TransformPosProj(V3d.OOI);

            return (farPoint - nearPoint).Normalized;
        }
    }
}
