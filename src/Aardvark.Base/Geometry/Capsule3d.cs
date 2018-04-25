using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule3d : IValidity, IBoundingBox3d
    {
        public V3d P0;
        public V3d P1;
        public double Radius;

        #region Constructors

        public Capsule3d(V3d p0, V3d p1, double radius)
        {
            P0 = p0;
            P1 = p1;
            Radius = radius;
        }

        public Capsule3d(Line3d axis, double radius)
        {
            P0 = axis.P0;
            P1 = axis.P1;
            Radius = radius;
        }

        #endregion

        #region Constants

        public static readonly Capsule3d Invalid = new Capsule3d(V3d.NaN, V3d.NaN, -1.0);

        #endregion

        #region Properties

        public bool IsValid => Radius >= 0.0;
        public bool IsInvalid => Radius < 0.0;

        public Line3d Axis => new Line3d(P0, P1);

        public Cylinder3d Cylider => new Cylinder3d(P0, P1, Radius);

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d => new Box3d(
            new Sphere3d(P0, Radius).BoundingBox3d,
            new Sphere3d(P1, Radius).BoundingBox3d);

        #endregion
    }
}
