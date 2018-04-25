using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Aardvark.Base
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Torus3d : IBoundingBox3d
    {
        public V3d Position;
        public V3d Direction;
        public double MajorRadius;
        public double MinorRadius;

        #region Constructor

        public Torus3d(V3d position, V3d direction, double majorRadius, double minorRadius)
        {
            Position = position;
            Direction = direction;
            MinorRadius = minorRadius;
            MajorRadius = majorRadius;
        }

        #endregion

        #region Properties

        public Circle3d MajorCircle => GetMajorCircle();
        public double Area => 4 * Constant.PiSquared * MajorRadius * MinorRadius;
        public double Volume => 2 * Constant.PiSquared * MajorRadius * MinorRadius * MinorRadius;

        #endregion

        #region Operations

        public Circle3d GetMajorCircle() => new Circle3d(Position, Direction, MajorRadius); 

        public Circle3d GetMinorCircle(double angle)
        {
            var c = GetMajorCircle();
            var p = c.GetPoint(angle);
            var dir = (p - Position).Normalized.Cross(Direction).Normalized;
            return new Circle3d(p, dir, MinorRadius);
        }

        public double GetMinimalDistance(V3d p) => GetMinimalDistance(p, Position, Direction, MajorRadius, MinorRadius);

        public static double GetMinimalDistance(V3d p, V3d position, V3d direction, double majorRadius, double minorRadius)
        {
            var plane = new Plane3d(direction, position);
            var planePoint = p.GetClosestPointOn(plane);
            var distanceOnPlane = (V3d.Distance(planePoint, position) - majorRadius).Abs();
            var distanceToCircle = (V3d.DistanceSquared(planePoint, p) + distanceOnPlane.Square()).Sqrt();
            return (distanceToCircle - minorRadius).Abs();
        }

        #endregion

        #region IBoundingBox3d Members

        Box3d IBoundingBox3d.BoundingBox3d => GetMajorCircle().BoundingBox3d.EnlargedBy(MinorRadius);

        #endregion
    }
}
