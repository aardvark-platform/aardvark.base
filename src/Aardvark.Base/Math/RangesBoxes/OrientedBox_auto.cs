using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    [DataContract]
    public struct OrientedBox2f
    {
        [DataMember]
        public Box2f Box;
        [DataMember]
        public Euclidean2f Trafo;

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

        public Box2f AxisAlignedBox => Box.Transformed((M33f)Trafo);

        public V2f[] Corners
        {
            get
            {
                var m = (M33f)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }

        public V2f[] CornersCCW
        {
            get
            {
                var m = (M33f)Trafo;
                return Box.ComputeCornersCCW().Map(p => m.TransformPos(p));
            }
        }
    }

    [DataContract]
    public struct OrientedBox3f
    {
        [DataMember]
        public Box3f Box;
        [DataMember]
        public Euclidean3f Trafo;

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

        public Box3f AxisAlignedBox => Box.Transformed((M44f)Trafo);

        public V3f[] Corners
        {
            get
            {
                var m = (M44f)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }
    }

    [DataContract]
    public struct OrientedBox2d
    {
        [DataMember]
        public Box2d Box;
        [DataMember]
        public Euclidean2d Trafo;

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

        public Box2d AxisAlignedBox => Box.Transformed((M33d)Trafo);

        public V2d[] Corners
        {
            get
            {
                var m = (M33d)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }

        public V2d[] CornersCCW
        {
            get
            {
                var m = (M33d)Trafo;
                return Box.ComputeCornersCCW().Map(p => m.TransformPos(p));
            }
        }
    }

    [DataContract]
    public struct OrientedBox3d
    {
        [DataMember]
        public Box3d Box;
        [DataMember]
        public Euclidean3d Trafo;

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

        public Box3d AxisAlignedBox => Box.Transformed((M44d)Trafo);

        public V3d[] Corners
        {
            get
            {
                var m = (M44d)Trafo;
                return Box.ComputeCorners().Map(p => m.TransformPos(p));
            }
        }
    }

}
