using System;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    #region Box (static)

    public static class Box
    {
        [Flags]
        public enum Flags
        {
            None = 0x00000000,

            /* ---------------------------------------------------------------
                flags that mark the faces of a box
            --------------------------------------------------------------- */
            MinX0 = 0x00000001,
            MinY0 = 0x00000002,
            MinZ0 = 0x00000004,

            MaxX0 = 0x00000008,
            MaxY0 = 0x00000010,
            MaxZ0 = 0x00000020,

            Min0 = MinX0 | MinY0 | MinZ0,
            Max0 = MaxX0 | MaxY0 | MaxZ0,

            X0 = MinX0 | MaxX0,
            Y0 = MinY0 | MaxY0,
            Z0 = MinZ0 | MaxZ0,

            All0 = Min0 | Max0,

            /* ---------------------------------------------------------------
                flags that mark the faces of a second, independent box
            --------------------------------------------------------------- */
            MinX1 = MinX0 << 6,
            MinY1 = MinY0 << 6,
            MinZ1 = MinZ0 << 6,
            MaxX1 = MaxX0 << 6,
            MaxY1 = MaxY0 << 6,
            MaxZ1 = MaxZ0 << 6,
            Min1 = Min0 << 6,
            Max1 = Max0 << 6,
            X1 = X0 << 6,
            Y1 = Y0 << 6,
            Z1 = Z0 << 6,
            All1 = All0 << 6,

            /* ---------------------------------------------------------------
                flags that operate on both face bits together
            --------------------------------------------------------------- */
            MinX = MinX0 | MinX1,
            MinY = MinY0 | MinY1,
            MinZ = MinZ0 | MinZ1,
            MaxX = MaxX0 | MaxX1,
            MaxY = MaxY0 | MaxY1,
            MaxZ = MaxZ0 | MaxZ1,
            Min = Min0 | Min1,
            Max = Max0 | Max1,
            MinXMinY = MinX | MinY,
            MinXMaxY = MinX | MaxY,
            MaxXMinY = MaxX | MinY,
            MaxXMaxY = MaxX | MaxY,
            X = X0 | X1,
            Y = Y0 | Y1,
            Z = Z0 | Z1,
            All = All0 | All1,

            /* ---------------------------------------------------------------
                flags that mark the edges of the box
            --------------------------------------------------------------- */
            Edge01 = 0x00001000,
            Edge23 = 0x00002000,
            Edge45 = 0x00004000,
            Edge67 = 0x00008000,
            Edge02 = 0x00010000,
            Edge13 = 0x00020000,
            Edge46 = 0x00040000,
            Edge57 = 0x00080000,
            Edge04 = 0x00100000,
            Edge15 = 0x00200000,
            Edge26 = 0x00400000,
            Edge37 = 0x00800000,

            /* ---------------------------------------------------------------
                flags that mark the corners of the box
            --------------------------------------------------------------- */
            Corner0 = 0x01000000,
            Corner1 = 0x02000000,
            Corner2 = 0x04000000,
            Corner3 = 0x08000000,
            Corner4 = 0x10000000,
            Corner5 = 0x20000000,
            Corner6 = 0x40000000,
            Corner7 = (int)-0x80000000,
        }
    }

    #endregion

    #region Box2i

    public partial struct Box2i
    {
    }

    #endregion

    #region Box2l

    public partial struct Box2l
    {
    }

    #endregion

    #region Box2f

    public partial struct Box2f
    {
        public Box2f(Box2d box)
        {
            Min = (V2f)box.Min;
            Max = (V2f)box.Max;
        }

        public static explicit operator Box2f(Box2d box)
        {
            return new Box2f(box);
        }
    }

    #endregion

    #region Box2d

    public partial struct Box2d
    {
        public Box2d(Box2f box)
        {
            Min = (V2d)box.Min;
            Max = (V2d)box.Max;
        }

        public static explicit operator Box2d(Box2f box)
        {
            return new Box2d(box);
        }

        public V2d[] ComputeCornersCCW()
        {
            return new V2d[] {
                Min,
                new V2d(Max.X, Min.Y),
                Max,
                new V2d(Min.X, Max.Y),
            };
        }

        /// <summary>
        /// Same as Min.X.
        /// </summary>
        public double Left
        {
            get { return Min.X; }
            set { Min.X = value; }
        }

        /// <summary>
        /// Same as Min.Y.
        /// </summary>
        public double Top
        {
            get { return Min.Y; }
            set { Min.Y = value; }
        }

        /// <summary>
        /// Same as Max.X.
        /// </summary>
        public double Right
        {
            get { return Max.X; }
            set { Max.X = value; }
        }

        /// <summary>
        /// Same as Max.Y.
        /// </summary>
        public double Bottom
        {
            get { return Max.Y; }
            set { Max.Y = value; }
        }
    }

    #endregion

    #region Box3i

    public partial struct Box3i
    {
    }

    #endregion

    #region Box3l

    public partial struct Box3l
    {
    }

    #endregion

    #region Box3f

    public partial struct Box3f
    {
        public Box3f(Box3d box)
        {
            Min = (V3f)box.Min;
            Max = (V3f)box.Max;
        }

        public static explicit operator Box3d(Box3f box)
        {
            return new Box3d(box);
        }
    }

    #endregion

    #region Box3d

    public partial struct Box3d
    {
        public Box3d(Box3f box)
        {
            Min = (V3d)box.Min;
            Max = (V3d)box.Max;
        }

        public static explicit operator Box3f(Box3d box)
        {
            return new Box3f(box);
        }

        public Range1d X { get { return new Range1d(Min.X, Max.X); } }
        public Range1d Y { get { return new Range1d(Min.Y, Max.Y); } }
        public Range1d Z { get { return new Range1d(Min.Z, Max.Z); } }
        public Box2d XY { get { return new Box2d(Min.XY, Max.XY); } }
        public Box2d XZ { get { return new Box2d(Min.XZ, Max.XZ); } }
        public Box2d YX { get { return new Box2d(Min.YX, Max.YX); } }
        public Box2d YZ { get { return new Box2d(Min.YZ, Max.YZ); } }
        public Box2d ZX { get { return new Box2d(Min.ZX, Max.ZX); } }
        public Box2d ZY { get { return new Box2d(Min.ZY, Max.ZY); } }
    }

    #endregion

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