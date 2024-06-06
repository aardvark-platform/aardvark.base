using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region OrientedBox2f

    [DataContract]
    public struct OrientedBox2f
    {
        [DataMember]
        public Box2f Box;
        [DataMember]
        public Euclidean2f Trafo;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2f(OrientedBox2d box)
        {
            Box = (Box2f)box.Box;
            Trafo = (Euclidean2f)box.Trafo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2f(Box2f box)
        {
            Box = box;
            Trafo = Euclidean2f.Identity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2f(Box2f box, Rot2f rot)
        {
            Box = box;
            Trafo = new Euclidean2f(rot, V2f.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2f(Box2f box, V2f trans)
        {
            Box = box;
            Trafo = new Euclidean2f(Rot2f.Identity, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2f(Box2f box, Rot2f rot, V2f trans)
        {
            Box = box;
            Trafo = new Euclidean2f(rot, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2f(Box2f box, Euclidean2f trafo)
        {
            Box = box;
            Trafo = trafo;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator OrientedBox2f(OrientedBox2d b)
            => new OrientedBox2f(b);

        #endregion

        #region Properties

        public readonly Box2f AxisAlignedBox => Box.Transformed((M33f)Trafo);

        public readonly V2f[] Corners
        {
            get
            {
                var m = (M33f)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }

        public readonly V2f[] CornersCCW
        {
            get
            {
                var m = (M33f)Trafo;
                return Box.ComputeCornersCCW().Map(p => m.TransformPos(p));
            }
        }

        #endregion
    }

    #endregion

    #region OrientedBox3f

    [DataContract]
    public struct OrientedBox3f
    {
        [DataMember]
        public Box3f Box;
        [DataMember]
        public Euclidean3f Trafo;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3f(OrientedBox3d box)
        {
            Box = (Box3f)box.Box;
            Trafo = (Euclidean3f)box.Trafo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3f(Box3f box)
        {
            Box = box;
            Trafo = Euclidean3f.Identity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3f(Box3f box, Rot3f rot)
        {
            Box = box;
            Trafo = new Euclidean3f(rot, V3f.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3f(Box3f box, V3f trans)
        {
            Box = box;
            Trafo = new Euclidean3f(Rot3f.Identity, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3f(Box3f box, Rot3f rot, V3f trans)
        {
            Box = box;
            Trafo = new Euclidean3f(rot, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3f(Box3f box, Euclidean3f trafo)
        {
            Box = box;
            Trafo = trafo;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator OrientedBox3f(OrientedBox3d b)
            => new OrientedBox3f(b);

        #endregion

        #region Properties

        public readonly Box3f AxisAlignedBox => Box.Transformed((M44f)Trafo);

        public readonly V3f[] Corners
        {
            get
            {
                var m = (M44f)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }

        #endregion
    }

    #endregion

    #region OrientedBox2d

    [DataContract]
    public struct OrientedBox2d
    {
        [DataMember]
        public Box2d Box;
        [DataMember]
        public Euclidean2d Trafo;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2d(OrientedBox2f box)
        {
            Box = (Box2d)box.Box;
            Trafo = (Euclidean2d)box.Trafo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2d(Box2d box)
        {
            Box = box;
            Trafo = Euclidean2d.Identity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2d(Box2d box, Rot2d rot)
        {
            Box = box;
            Trafo = new Euclidean2d(rot, V2d.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2d(Box2d box, V2d trans)
        {
            Box = box;
            Trafo = new Euclidean2d(Rot2d.Identity, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2d(Box2d box, Rot2d rot, V2d trans)
        {
            Box = box;
            Trafo = new Euclidean2d(rot, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox2d(Box2d box, Euclidean2d trafo)
        {
            Box = box;
            Trafo = trafo;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator OrientedBox2d(OrientedBox2f b)
            => new OrientedBox2d(b);

        #endregion

        #region Properties

        public readonly Box2d AxisAlignedBox => Box.Transformed((M33d)Trafo);

        public readonly V2d[] Corners
        {
            get
            {
                var m = (M33d)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }

        public readonly V2d[] CornersCCW
        {
            get
            {
                var m = (M33d)Trafo;
                return Box.ComputeCornersCCW().Map(p => m.TransformPos(p));
            }
        }

        #endregion
    }

    #endregion

    #region OrientedBox3d

    [DataContract]
    public struct OrientedBox3d
    {
        [DataMember]
        public Box3d Box;
        [DataMember]
        public Euclidean3d Trafo;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3d(OrientedBox3f box)
        {
            Box = (Box3d)box.Box;
            Trafo = (Euclidean3d)box.Trafo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3d(Box3d box)
        {
            Box = box;
            Trafo = Euclidean3d.Identity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3d(Box3d box, Rot3d rot)
        {
            Box = box;
            Trafo = new Euclidean3d(rot, V3d.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3d(Box3d box, V3d trans)
        {
            Box = box;
            Trafo = new Euclidean3d(Rot3d.Identity, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3d(Box3d box, Rot3d rot, V3d trans)
        {
            Box = box;
            Trafo = new Euclidean3d(rot, trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OrientedBox3d(Box3d box, Euclidean3d trafo)
        {
            Box = box;
            Trafo = trafo;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator OrientedBox3d(OrientedBox3f b)
            => new OrientedBox3d(b);

        #endregion

        #region Properties

        public readonly Box3d AxisAlignedBox => Box.Transformed((M44d)Trafo);

        public readonly V3d[] Corners
        {
            get
            {
                var m = (M44d)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }

        #endregion
    }

    #endregion

}
