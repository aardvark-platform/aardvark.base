using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Cylinder3d : IBoundingBox3d, IValidity
    {
        public V3d P0;
        public V3d P1;
        public double Radius;
        public double DistanceScale;

        #region Constructors

        public Cylinder3d(V3d p0, V3d p1, double radius, double distanceScale = 0)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
            DistanceScale = distanceScale;
        }

        public Cylinder3d(Line3d axis, double radius, double distanceScale = 0)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
            DistanceScale = distanceScale;
        }

        #endregion

        #region Constants

        public static readonly Cylinder3d Invalid = new Cylinder3d(V3d.NaN, V3d.NaN, -1.0);

        #endregion

        #region Properties

        public double Height { get { return (P0 - P1).Length; } }

        public V3d Center { get { return (P0 + P1) * 0.5; } }

        public Line3d Axis { get { return new Line3d(P0, P1); } }

        public bool IsValid { get { return Radius >= 0.0; } }

        public bool IsInvalid { get { return Radius < 0.0; } }

        public Circle3d Circle0 { get { return new Circle3d(P0, (P0 - P1).Normalized, Radius); } }

        public Circle3d Circle1 { get { return new Circle3d(P1, (P1 - P0).Normalized, Radius); } }

        public double Area
        {
            get
            {
                return Radius * Constant.PiTimesTwo * (Radius + Height);
            }
        }

        public double Volume
        {
            get
            {
                return Radius * Radius * Constant.Pi * Height;
            }
        }

        #endregion

        #region Operations

        /// <summary>
        /// P0 has height 0.0, P1 has height 1.0
        /// </summary>
        public double GetHeight(V3d p)
        {
            var dir = (P1 - P0).Normalized;
            var pp = p.GetClosestPointOn(new Ray3d(P0, dir));
            return (pp - P0).Dot(dir);
        }
        /// <summary>
        /// Get circle at a specific height
        /// </summary>
        public Circle3d GetCircle(double height)
        {
            var dir = (P1 - P0).Normalized;
            return new Circle3d(P0 + height * dir, dir, Radius);
        }

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d
        {
            get 
            {
                return new Box3d(Circle0.BoundingBox3d, Circle1.BoundingBox3d);
            }
        }

        #endregion
    }
}
