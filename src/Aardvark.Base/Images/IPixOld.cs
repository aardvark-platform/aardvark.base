using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Aardvark.Base
{
    public interface IPixOld //: INode
    {
        PixInfoOld PixInfo { get; }
    }

    interface IPixOld<T> : IPixOld
    {
    }

    #region Pix

    public enum PixTopology
    {
        Unknown,
        SingleImage,
        ImageStack,
        CubeMap
    }

    //[RegisterTypeInfo]
    public class Pix : IPixOld
    {
        private IPixOld[] m_pixArray;
        private PixTopology m_topology;

        #region Constructors

        public Pix() { }

        public Pix(PixImage singleImage)
        {
            m_pixArray = singleImage.IntoArray();
            m_topology = PixTopology.SingleImage;
        }

        public Pix(PixImageMipMap singleImage)
        {
            m_pixArray = singleImage.IntoArray();
            m_topology = PixTopology.SingleImage;
        }

        public Pix(IEnumerable<PixImage> images)
            : this(images, PixTopology.ImageStack)
        { }

        public Pix(IEnumerable<PixImageMipMap> images)
            : this(images, PixTopology.ImageStack)
        { }

        public Pix(IEnumerable<IPixOld> pixes, PixTopology kindOfSet)
        {
            if (pixes == null || pixes.Count() == 0)
                throw new ArgumentException("at least one pix must be supplied");

            //if( kindOfSet==Kind.CubeMap && pixies.Count() != 6 )
            //    throw new ArgumentException("CubeMaps need exactly 6 Images");

            m_pixArray = pixes.ToArray();

            m_topology = kindOfSet;
        }

        #endregion

        #region Properties

        public PixTopology Topology
        {
            get { return m_topology; }
            set { m_topology = value; }
        }

        public IEnumerable<IPixOld> PixArray
        {
            get { return m_pixArray; }
            set { m_pixArray = value.ToArray(); }
        }

        public PixInfoOld PixInfo
        {
            get
            {
                return new PixInfoOld
                {
                    { PixInfoOld.Property.Topology, Topology },
                    { PixInfoOld.Property.SubInfoArray, (from p in PixArray select p.PixInfo).ToArray() },
                };
            }
        }

        // public IEnumerable<INode> SubNodes { get { return m_pixArray; } }

        public bool IsEmpty()
        {
            return m_pixArray.Length == 0;
        }

        #endregion

        #region Creators

        public static Pix CreateCubeMap(IEnumerable<PixImage> images)
        {
            if (images.Count() != 6)
                throw new ArgumentException("CubeMap does need exactly 6 images");

            return new Pix(images, PixTopology.CubeMap);
        }
        public static Pix CreateCubeMap(IEnumerable<PixImageMipMap> images)
        {
            if (images.Count() != 6)
                throw new ArgumentException("CubeMap does need exactly 6 images");

            return new Pix(images, PixTopology.CubeMap);
        }

        public static Pix CreateCubeMap(PixImage i1, PixImage i2, PixImage i3, PixImage i4, PixImage i5, PixImage i6)
        {
            return new Pix(new PixImage[] { i1, i2, i3, i4, i5, i6 }, PixTopology.CubeMap);
        }
        public static Pix CreateCubeMap(PixImageMipMap i1, PixImageMipMap i2, PixImageMipMap i3, PixImageMipMap i4, PixImageMipMap i5, PixImageMipMap i6)
        {
            return new Pix(new PixImageMipMap[] { i1, i2, i3, i4, i5, i6 }, PixTopology.CubeMap);
        }


        #endregion

        //#region IFieldCodeable Members

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "Topology", (c, o) => c.CodeT(ref ((Pix)o).m_topology));
        //    yield return new FieldCoder(0, "PixArray", (c, o) => c.CodeT(ref ((Pix)o).m_pixArray));
        //}

        //#endregion
    }

    #endregion

    #region PixInfoOld

    public class PixInfoOld : Dictionary<string, object> //, INode
    {
        public static class Property
        {
            public const string Kind = "Kind";
            public const string ColFormat = "ColFormat";
            public const string Type = "Type";
            public const string Topology = "Topology";
            public const string SubInfoArray = "SubInfoArray";
        }

        public T Get<T>(string propertyName)
        {
            return (T)this[propertyName];
        }

        //#region INode<PixInfo> Members

        //public IEnumerable<INode> SubNodes
        //{
        //    get
        //    {
        //        object subInfoArray;
        //        if (TryGetValue(Property.SubInfoArray, out subInfoArray))
        //            foreach (var p in (PixInfoOld[])subInfoArray) yield return p;
        //    }
        //}

        //#endregion
    }

    #endregion

}
