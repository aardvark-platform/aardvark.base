using System;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    #region Box3dAndFlags

    [DataContract]
    public struct Box3dAndFlags
    {
        [DataMember]
        public Box.Flags BFlags;
        [DataMember]
        public Box3d BBox;

        public Box3dAndFlags(Box3d union, Box3d box0, Box3d box1)
        {
            BFlags = 0;
            BBox = union;
            if (box0.Min.X > union.Min.X) { BBox.Min.X = box0.Min.X; BFlags |= Box.Flags.MinX0; }
            if (box0.Min.Y > union.Min.Y) { BBox.Min.Y = box0.Min.Y; BFlags |= Box.Flags.MinY0; }
            if (box0.Min.Z > union.Min.Z) { BBox.Min.Z = box0.Min.Z; BFlags |= Box.Flags.MinZ0; }
            if (box0.Max.X < union.Max.X) { BBox.Max.X = box0.Max.X; BFlags |= Box.Flags.MaxX0; }
            if (box0.Max.Y < union.Max.Y) { BBox.Max.Y = box0.Max.Y; BFlags |= Box.Flags.MaxY0; }
            if (box0.Max.Z < union.Max.Z) { BBox.Max.Z = box0.Max.Z; BFlags |= Box.Flags.MaxZ0; }
            if (box1.Min.X > union.Min.X) { BBox.Min.X = box1.Min.X; BFlags |= Box.Flags.MinX1; }
            if (box1.Min.Y > union.Min.Y) { BBox.Min.Y = box1.Min.Y; BFlags |= Box.Flags.MinY1; }
            if (box1.Min.Z > union.Min.Z) { BBox.Min.Z = box1.Min.Z; BFlags |= Box.Flags.MinZ1; }
            if (box1.Max.X < union.Max.X) { BBox.Max.X = box1.Max.X; BFlags |= Box.Flags.MaxX1; }
            if (box1.Max.Y < union.Max.Y) { BBox.Max.Y = box1.Max.Y; BFlags |= Box.Flags.MaxY1; }
            if (box1.Max.Z < union.Max.Z) { BBox.Max.Z = box1.Max.Z; BFlags |= Box.Flags.MaxZ1; }
        }
    }

    #endregion

    #region OctoBox2d

    [DataContract]
    public struct OctoBox2d
    {
        [DataMember]
        public double PX, PY, NX, NY, PXPY, PXNY, NXPY, NXNY;

        #region Constructors

        public OctoBox2d(
                double px, double py, double nx, double ny,
                double pxpy, double pxny, double nxpy, double nxny)
        {
            PX = px; PY = py; NX = nx; NY = ny;
            PXPY = pxpy; PXNY = pxny; NXPY = nxpy; NXNY = nxny;
        }

        #endregion

        #region Constants

        public static readonly OctoBox2d Invalid =
                new OctoBox2d(double.MinValue, double.MinValue, double.MinValue, double.MinValue,
                              double.MinValue, double.MinValue, double.MinValue, double.MinValue);

        public static readonly OctoBox2d Zero =
                new OctoBox2d(0, 0, 0, 0, 0, 0, 0, 0);

        #endregion

        #region Properties

        public double Area
        {
            get
            {
                return (PX + NX) * (PY + NY)
                        - 0.5 * (Fun.Square(PX + PY - PXPY) + Fun.Square(PX + NY - PXNY)
                                 + Fun.Square(NX + PY - NXPY) + Fun.Square(NX + NY - NXNY));
            }
        }

        #endregion

        #region Manipulation

        public void ExtendBy(V2d p)
        {
            ExtendBy(p.X, p.Y);
        }

        public void ExtendBy(double x, double y)
        {
            if (x > PX) PX = x;
            if (y > PY) PY = y;
            var nx = -x; if (nx > NX) NX = nx;
            var ny = -y; if (ny > NY) NY = ny;
            var pxpy = x + y; if (pxpy > PXPY) PXPY = pxpy;
            var pxny = x - y; if (pxny > PXNY) PXNY = pxny;
            var nxpy = -x + y; if (nxpy > NXPY) NXPY = nxpy;
            var nxny = -x - y; if (nxny > NXNY) NXNY = nxny;
        }

        #endregion
    }

    #endregion
}