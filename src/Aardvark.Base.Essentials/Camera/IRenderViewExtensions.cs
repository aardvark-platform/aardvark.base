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
        /// Returns the multiplied View-Projection transformation.
        /// </summary>
        public static Trafo3d ViewProjTrafo(this IViewProjection vp)
            => vp.View.ViewTrafo * vp.Projection.ProjectionTrafo;

        /// <summary>
        /// Builds a hull from the given view-projection (left, right, top, bottom, near, far).
        /// The normals of the hull planes point to the outside. 
        /// A point inside the visual hull will have positive height to all planes.
        /// </summary>
        public static Hull3d GetVisualHull(this IViewProjection vp)
            => vp.ViewProjTrafo().GetVisualHull();

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
