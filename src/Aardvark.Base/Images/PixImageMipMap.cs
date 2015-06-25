using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Aardvark.Base
{
    //[RegisterTypeInfo]
    public class PixImageMipMap : IPix, IPixMipMap2d
    {
        public PixImage[] ImageArray;

        #region MipMapOptions

        public class MipMapOptions
        {
            public int Count;
            public ImageInterpolation Interpolation;
            public bool PowerOfTwo;
            // extend further - filter type...

            public static MipMapOptions Default = new MipMapOptions()
            {
                Count = 0,
                Interpolation = ImageInterpolation.Cubic,
                PowerOfTwo = false,
            };
        }

        #endregion

        #region Constructors

        public PixImageMipMap() { }

        public PixImageMipMap(params PixImage[] images) { ImageArray = images; }

        #endregion

        #region Properties

        public int ImageCount { get { return ImageArray.Length; } }

        public IEnumerable<PixImage> Images { get { return ImageArray; } }
        
        #endregion

        #region Static Creators

        //static PixImageMipMap()
        //{
        //    FileTexture2d.RegisterLoadFun(Create);
        //}

        static Dictionary<Type, Func<PixImage, MipMapOptions, PixImageMipMap>> convertTable = new Dictionary<Type, Func<PixImage, MipMapOptions, PixImageMipMap>>()
            {
                {typeof(PixImage<byte>), (pix, opt) => new PixImageMipMap<byte>((PixImage<byte>)pix, opt)},
                {typeof(PixImage<ushort>), (pix, opt) => new PixImageMipMap<ushort>((PixImage<ushort>)pix, opt)},
                {typeof(PixImage<uint>), (pix, opt) => new PixImageMipMap<uint>((PixImage<uint>)pix, opt)},
                {typeof(PixImage<float>), (pix, opt) => new PixImageMipMap<float>((PixImage<float>)pix, opt)},
                {typeof(PixImage<double>), (pix, opt) => new PixImageMipMap<double>((PixImage<double>)pix, opt)}
            };

        public static IEnumerable<PixImageMipMap> Create(IEnumerable<PixImage> pixies, MipMapOptions options)
        {
            try
            {
                return from pixie in pixies
                       select convertTable[pixie.GetType()](pixie, options);
            }
            catch (KeyNotFoundException)
            { 
                throw new NotImplementedException("PixImage Type not implemented");
            }
        }

        public static IEnumerable<PixImageMipMap> Create(IEnumerable<PixImage> pixies)
        {
            return Create(pixies, MipMapOptions.Default);
        }

        public static PixImageMipMap Create(string filename)
        {
            return new PixImageMipMap(PixImage.Create(filename, PixLoadOptions.Default));
        }

        public static PixImageMipMap Create(string filename, PixLoadOptions options)
        {
            return new PixImageMipMap(PixImage.Create(filename, options));
        }

        public static IEnumerable<PixImageMipMap> Create(IEnumerable<string> filenames)
        {
            return Create(filenames, MipMapOptions.Default);
        }

        public static IEnumerable<PixImageMipMap> Create(IEnumerable<string> filenames, MipMapOptions options)
        {
            return Create(from filename in filenames select PixImage.Create(filename), options);
        }

        public static PixImageMipMap Create(PixImage pix)
        {
            return Create(pix.IntoIEnumerable(), MipMapOptions.Default).Single();
        }

        public static PixImageMipMap Create(PixImage pix, MipMapOptions options)
        {
            return Create(pix.IntoIEnumerable(), options).Single();
        }

        #endregion

        #region IPix

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixImageMipMap(this); }

        #endregion

        #region IPixMipMap2d

        public int LevelCount
        {
            get { return ImageArray.Length; }
        }

        IPixImage2d IPixMipMap2d.this[int level]
        {
            get
            {
                return ImageArray[level];
            }
        }

        public PixImage this[int level]
        {
            get { return ImageArray[level]; }
        }

        #endregion

        //#region IFieldCodeable

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "images", (c, o) => c.CodeTArray(ref ((PixImageMipMap)o).ImageArray));
        //}

        //#endregion

        public PixFormat PixFormat
        {
            get { if (ImageArray.IsEmptyOrNull()) new Exception("PixMipMap is empty"); return ImageArray[0].PixFormat; }
        }

    }

    public class PixImageMipMap<T> : PixImageMipMap
    {
        #region Properties

        new public IEnumerable<PixImage<T>> Images { get { return (PixImage<T>[])ImageArray; } }
        public IEnumerable<V2i> ImageSizes { get { return from image in ImageArray select image.Size; } }
        public V2i BaseImageSize { get { return ImageArray[0].Size; } }

        #endregion

        #region Constructors
        
        public PixImageMipMap(PixImage<T> singleImage)
        {
            ImageArray = new PixImage<T>[] { singleImage };
            BuildMipMaps(MipMapOptions.Default);
        }
        
        public PixImageMipMap(PixImage<T> singleImage, MipMapOptions options)
        {
            ImageArray = new PixImage<T>[] { singleImage };
            BuildMipMaps(options);
        }

        #endregion

        /// <summary>
        /// Builds mipmap pyramid using supplied MipMapOptions.
        /// Throws an exception if mips are already present (this function ensures correct MipMapOptions)
        /// </summary>
        private void BuildMipMaps(MipMapOptions options)
        {
            if (ImageArray == null || ImageArray.Length == 0)
                throw new ArgumentException("No valid PixImage available.");

            if (ImageArray.Length > 1)
                throw new ArgumentException("PixImageMipMap already contains image pyramid");

            var size = ImageArray[0].Size;

            if (options.PowerOfTwo && (!Fun.IsPowerOfTwo(size.X) || !Fun.IsPowerOfTwo(size.Y)))
            {
                var powerOfTwoSize = new V2i(size.X.NextPowerOfTwo(), size.Y.NextPowerOfTwo());
                ImageArray[0] = ((PixImage<T>)ImageArray[0]).Resized(powerOfTwoSize);
                Report.Line(2, "Resizing PixImage to size of power of two");
                size = powerOfTwoSize;
                //throw new ArgumentException("Can only build mip maps for PixImages with size of power of two");
            }

            var count = options.Count == 0
                ? (int)Fun.Max(Fun.Log(size.X, 2).Ceiling(), Fun.Log(size.Y, 2).Ceiling())
                : options.Count;

            var mipMaps = new PixImage<T>[count];
            mipMaps[0] = (PixImage<T>)ImageArray[0];

            for (int i = 1; i < count; i++)
            {
                //size = (V2i)((V2d)size * 0.5);   // hardware mipmaps implemented in dx/gl also use round down.
                size = new V2i(Fun.Max(1, size.X >> 1), Fun.Max(1, size.Y >> 1));
                mipMaps[i] = mipMaps[i - 1].Resized(size, options.Interpolation);
            }

            ImageArray = mipMaps;
            //ImagesContainer.FromPixImageArray(m_images);
            //InfoOfSet = new Info(Kind.MipMapPyramid, m_images.Length, m_images.Select(x => x.Size).ToArray());
        }
    }
}
