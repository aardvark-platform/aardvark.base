using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ObliqueCone3d
    {
        public readonly V3d Origin;
        public readonly Circle3d Circle;

        #region Constructor

        public ObliqueCone3d(V3d o, Circle3d c)
        {
            Origin = o;
            Circle = c;
        }

        #endregion

        #region Constants

        public static readonly ObliqueCone3d Invalid = new ObliqueCone3d(V3d.NaN, Circle3d.Invalid);

        #endregion

        #region Properties

        public bool IsValid => Circle.Radius >= 0.0;
        public bool IsInvalid => Circle.Radius < 0.0;

        #endregion

        #region Operations

        /// <summary>
        /// get circle of oblique cone where distance between cone origin and circle origin equals distance
        /// </summary>
        public Circle3d GetCircle(double distance)
        {
            var dir = Circle.Center - Origin;
            var pDistance = dir.Length;
            dir.Normalize();
            var newCenter = Origin + dir * distance;
            var newRadius = distance / pDistance * Circle.Radius;

            return new Circle3d(newCenter, Circle.Normal, newRadius);
        }

        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Cone3d : IValidity
    {
        public V3d Origin;
        public V3d Direction; // axis of revolution
        public double Angle; // angle between axis and outer edge

        #region Constructor

        public Cone3d(V3d origin, V3d dir, double angle)
        {
            Origin = origin;
            Direction = dir;
            Angle = angle;
        }

        #endregion

        #region Constants

        public static readonly Cone3d Invalid = new Cone3d(V3d.NaN, V3d.Zero, 0.0);

        #endregion

        #region Properties

        public bool IsValid => Direction != V3d.Zero;
        public bool IsInvalid => Direction == V3d.Zero;

        #endregion

        #region Operations

        /// <summary>
        /// if zero, point is located on cone
        /// </summary>
        public double GetDistance(V3d point) => Vec.Distance(point, GetClosestPoint(point));

        public Circle3d GetCircle(double height)
        {
            //circle along axis
            var dirLength = height;
            var radius = GetRadius(height);
            var dir = Direction.Normalized * dirLength;
            return new Circle3d(Origin + dir, dir.Normalized, radius);
        }

        public Ray3d GetAxis() => new Ray3d(Origin, Direction);

        public double GetHeight(V3d position)
            => new Ray3d(Origin, Direction).GetTOfProjectedPoint(position);

        public double GetRadius(double height) => height * Angle.Sin() / Angle.Cos();

        public V3d GetClosestPoint(V3d point)
        {
            var ray = new Ray3d(Origin, Direction);
            var cp = point.GetClosestPointOn(ray);
            var radius = GetRadius(GetHeight(point));
            var dir = (point - cp).Normalized * radius;

            var p0 = cp + dir;
            var p1 = point.GetClosestPointOn(new Ray3d(Origin, (p0 - Origin).Normalized));

            return (Vec.Distance(point, p1) < Vec.Distance(point, p0)) ? p1 : p0;
        }

        #endregion
    }
}
