using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Aardvark.Base
{

    #region IPixOp<Tr>

    public interface IPixOp<Tr>
    {
        Tr FileImage(FileImage fi);
        Tr PixImageInfo(PixImageInfo pii);
        Tr PixCubeMap(PixCubeMap pcm, Tr[] subArray);
        Tr PixStack(PixStack ps, Tr[] subArray);

        Tr PixImage(PixImage pi);
        Tr PixImageMipMap(PixImageMipMap pimm);

        Tr FileVolume(FileVolume fv);
        Tr PixVolumeInfo(PixVolumeInfo pvi);

        Tr PixVolume(PixVolume pv);
    }

    #endregion

    #region IPixProductOp<Tr>

    public interface IPixProductOp<Tr>
    {
        Tr FileImage(FileImage fi0, FileImage fi1);
        Tr FileVolume(FileVolume fv0, FileVolume fv1);
        Tr PixImage(PixImage pi0, PixImage pi1);
        Tr PixVolume(PixVolume pv0, PixVolume pv1);
        Tr PixImageMipMap(PixImageMipMap pimm0, PixImageMipMap pimm1);
        Tr PixImageInfo(PixImageInfo pii0, PixImageInfo pii1);
        Tr PixVolumeInfo(PixVolumeInfo pvi0, PixVolumeInfo pvi1);

        Tr PixCubeMap(PixCubeMap pcm0, PixCubeMap pcm1, Tr[] subArray);
        Tr PixStack(PixStack ps0, PixStack ps1, Tr[] subArray);
    }

    #endregion

    #region IPix

    public interface IPix
    {
        Tr Op<Tr>(IPixOp<Tr> op);
    }

    #endregion 

    // IPix Types:

    #region FileImage

    //[RegisterTypeInfo]
    public class FileImage : IPix
    {
        public string Path;

        public FileImage() { }
        public FileImage(string path) { Path = path; }

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.FileImage(this); }

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "Path", (c, o) => c.CodeString(ref ((FileImage)o).Path));
        //}
    }

    #endregion

    #region FileVolume

    //[RegisterTypeInfo]
    public class FileVolume : IPix
    {
        public string Path;

        public FileVolume() { }
        public FileVolume(string path) { Path = path; }

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.FileVolume(this); }

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "Path", (c, o) => c.CodeString(ref ((FileVolume)o).Path));
        //}
    }

    #endregion

    #region PixImageInfo

    //[RegisterTypeInfo]
    public class PixImageInfo: IPix
    {
        public PixFormat Format;
        public V2i Size;

        public PixImageInfo() { }
        public PixImageInfo(PixFormat format, V2i size) { Format = format; Size = size; }

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixImageInfo(this); }

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "PixFormat", (c, o) => c.CodePixFormat(ref ((PixVolumeInfo)o).Format));
        //    yield return new FieldCoder(1, "Size", (c, o) => c.CodeV2i(ref ((PixImageInfo)o).Size));
        //}
    }

    #endregion

    #region PixVolumeInfo

    //[RegisterTypeInfo]
    public class PixVolumeInfo : IPix
    {
        public PixFormat Format;
        public V3i Size;

        public PixVolumeInfo() { }
        public PixVolumeInfo(PixFormat format, V3i size) { Format = format; Size = size; }

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixVolumeInfo(this); }

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "PixFormat", (c, o) => c.CodePixFormat(ref ((PixVolumeInfo)o).Format));
        //    yield return new FieldCoder(1, "Size", (c, o) => c.CodeV3i(ref ((PixVolumeInfo)o).Size));
        //}
    }

    #endregion

    #region PixCubeMap

    //[RegisterTypeInfo]
    public class PixCubeMap : IPix
    {
        public IPix[] PixArray;

        public PixCubeMap() { }
        public PixCubeMap(params IPix[] pixArray)
        {
            if (pixArray.Length != 6) throw new ArgumentException("CubeMap needs exactly 6 images");
            var descriptors = pixArray[0].GetDescriptors();
            for (int i = 1; i < 6; i++)
                if (!pixArray[i].GetDescriptors().IsEqualTo(descriptors, null, null, (a,b) => true))
                    throw new ArgumentException("CubeMap images must be of same type and size");

            PixArray = pixArray;
        }

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixCubeMap(this, PixArray.Copy(p => p.Op(op))); }

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "PixArray", (c, o) => c.CodeT(ref ((PixCubeMap)o).PixArray));
        //}
    }

    #endregion

    #region PixStack

    //[RegisterTypeInfo]
    public class PixStack : IPix
    {
        public IPix[] PixArray;

        public PixStack() { }
        public PixStack(params IPix[] pixArray) { PixArray = pixArray; }

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixStack(this, PixArray.Copy(p => p.Op(op))); }

        //public IEnumerable<FieldCoder> GetFieldCoders(int coderVersion)
        //{
        //    yield return new FieldCoder(0, "PixArray", (c, o) => c.CodeT(ref ((PixStack)o).PixArray));
        //}
    }

    #endregion

    // IPix Ops:

    #region PixCopyOp

    public class PixCopyOp : IPixOp<IPix>
    {
        public virtual IPix FileImage(FileImage fi) { return fi; }
        public virtual IPix FileVolume(FileVolume fv) { return fv; }
        public virtual IPix PixImage(PixImage pi) { return pi; }
        public virtual IPix PixVolume(PixVolume pv) { return pv; }
        public virtual IPix PixImageMipMap(PixImageMipMap pimm) { return pimm; }
        public virtual IPix PixImageInfo(PixImageInfo pii) { return pii; }
        public virtual IPix PixVolumeInfo(PixVolumeInfo pvi) { return pvi; }

        public virtual IPix PixCubeMap(PixCubeMap pcm, params IPix[] subArray)
        {
            return subArray.AllEqual(pcm.PixArray, (a, b) => a == b)
                            ? pcm : new PixCubeMap(subArray);
        }

        public virtual IPix PixStack(PixStack ps, params IPix[] subArray)
        {
            return subArray.AllEqual(ps.PixArray, (a, b) => a == b)
                            ? ps : new PixStack(subArray);
        }

    }

    #endregion

    #region PixInnerProductOp<Tr>

    public class PixInnerProductOp<Tr> : IPixOp<Func<Tr>>
    {
        public IPix Other;
        public IPixProductOp<Tr> ProductOp;

        public PixInnerProductOp(IPixProductOp<Tr> productOp, IPix other)
        {
            ProductOp = productOp;
            Other = other;
        }

        public virtual Func<Tr> FileImage(FileImage fi)
        {
            return () =>
            {
                var typedOther = Other as FileImage;
                if (typedOther == null) throw new ArgumentException();
                return ProductOp.FileImage(fi, typedOther);
            };
        }

        public virtual Func<Tr> FileVolume(FileVolume fv)
        {
            return () =>
            {
                var typedOther = Other as FileVolume;
                if (typedOther == null) throw new ArgumentException();
                return ProductOp.FileVolume(fv, typedOther);
            };
        }

        public virtual Func<Tr> PixImage(PixImage pi)
        {
            return () =>
            {
                var typedOther = Other as PixImage;
                if (typedOther == null) throw new ArgumentException();
                return ProductOp.PixImage(pi, typedOther);
            };
        }

        public virtual Func<Tr> PixVolume(PixVolume pv)
        {
            return () =>
            {
                var typedOther = Other as PixVolume;
                if (typedOther == null) throw new ArgumentException();
                return ProductOp.PixVolume(pv, typedOther);
            };
        }

        public virtual Func<Tr> PixImageMipMap(PixImageMipMap pimm)
        {
            return () =>
            {
                var typedOther = Other as PixImageMipMap;
                if (typedOther == null) throw new ArgumentException();
                return ProductOp.PixImageMipMap(pimm, typedOther);
            };
        }

        public virtual Func<Tr> PixImageInfo(PixImageInfo pii)
        {
            return () =>
            {
                var typedOther = Other as PixImageInfo;
                if (typedOther == null) throw new ArgumentException();
                return ProductOp.PixImageInfo(pii, typedOther);
            };
        }

        public virtual Func<Tr> PixVolumeInfo(PixVolumeInfo pvi)
        {
            return () =>
            {
                var typedOther = Other as PixVolumeInfo;
                if (typedOther == null) throw new ArgumentException();
                return ProductOp.PixVolumeInfo(pvi, typedOther);
            };
        }

        public virtual Func<Tr> PixCubeMap(PixCubeMap pcm, Func<Tr>[] subArray)
        {
            return () =>
            {
                var typedOther = Other as PixCubeMap;
                if (typedOther == null) throw new ArgumentException();
                var length = subArray.Length;
                if (length != typedOther.PixArray.Length) throw new ArgumentException();
                var result = ProductOp.PixCubeMap(pcm, typedOther,
                                               subArray.ProductArray(typedOther.PixArray, length,
                                                    (fp0, p1) => { Other = p1; return fp0(); }));
                Other = typedOther;
                return result;
            };
        }

        public virtual Func<Tr> PixStack(PixStack ps, Func<Tr>[] subArray)
        {
            return () =>
            {
                var typedOther = Other as PixStack;
                if (typedOther == null) throw new ArgumentException();
                var length = subArray.Length;
                if (length != typedOther.PixArray.Length)  throw new ArgumentException();
                var result = ProductOp.PixStack(ps, typedOther,
                                                  subArray.ProductArray(typedOther.PixArray, length,
                                                            (fp0, p1) => { Other = p1; return fp0(); }));
                Other = typedOther;
                return result;
            };
        }
    }

    #endregion

    #region PixCompare

    public class PixEqualOp : IPixProductOp<bool>
    {
        public Func<PixImage, PixImage, bool> ImageEqualFun;
        public Func<PixVolume, PixVolume, bool> VolumeEqualFun;
        public Func<string, string, bool> StringEqualFun;

        public PixEqualOp(
                Func<PixImage, PixImage, bool> imageEqualFun,
                Func<PixVolume, PixVolume, bool> volumeEqualFun,
                Func<string, string, bool> stringEqualFun)
        {
            ImageEqualFun = imageEqualFun; VolumeEqualFun = volumeEqualFun; StringEqualFun = stringEqualFun;
        }

        public bool FileImage(FileImage fi0, FileImage fi1)
        {
            return StringEqualFun(fi0.Path, fi1.Path);
        }

        public bool FileVolume(FileVolume fv0, FileVolume fv1)
        {
            return StringEqualFun(fv0.Path, fv1.Path);
        }

        public bool PixImage(PixImage pi0, PixImage pi1)
        {
            return ImageEqualFun(pi0, pi1);
        }

        public bool PixVolume(PixVolume pv0, PixVolume pv1)
        {
            return VolumeEqualFun(pv0, pv1);
        }

        public bool PixImageMipMap(PixImageMipMap pimm0, PixImageMipMap pimm1)
        {
            return pimm0.ImageArray.AllEqual(pimm1.ImageArray, ImageEqualFun);
        }

        public bool PixImageInfo(PixImageInfo pii0, PixImageInfo pii1)
        {
            return pii0.Format == pii1.Format && pii0.Size == pii1.Size;
        }

        public bool PixVolumeInfo(PixVolumeInfo pvi0, PixVolumeInfo pvi1)
        {
            return pvi0.Format == pvi1.Format && pvi0.Size == pvi1.Size;
        }

        public bool PixCubeMap(PixCubeMap pcm0, PixCubeMap pcm1, bool[] subArray)
        {
            return subArray.FoldLeft(true, (s, b) => s && b);
        }

        public bool PixStack(PixStack ps0, PixStack ps1, bool[] subArray)
        {
            return subArray.FoldLeft(true, (s, b) => s && b);
        }
    }


    #endregion

    #region PixCountPixImagesInMemory

    public class PixCountPixImagesInMemory : IPixOp<int>
    {
        public int FileImage(FileImage fi) { return 0; }
        public int FileVolume(FileVolume fv) { return 0; }
        public int PixImage(PixImage pi) { return 1; }
        public int PixVolume(PixVolume pv) { return 1; }
        public int PixImageMipMap(PixImageMipMap pimm) { return pimm.ImageArray.Length; }
        public int PixImageInfo(PixImageInfo pii) { return 0; }
        public int PixVolumeInfo(PixVolumeInfo pvi) { return 0; }

        public int PixCubeMap(PixCubeMap pcm, int[] subArray) { return subArray.Sum(); }
        public int PixStack(PixStack ps, int[] subArray) { return subArray.Sum(); }
    }

    #endregion

    #region PixLoadOp

    public class PixLoadOp : PixCopyOp, IPixOp<IPix>
    {
        public string PathPrefix;

        public PixLoadOp(string pathPrefix) { PathPrefix = pathPrefix; }

        public override IPix FileImage(FileImage fi)
        {
            return Base.PixImage.Create(PathPrefix + fi.Path);
        }
    }

    #endregion

    #region PixSaveOp

    public class PixIntSumProductOp : IPixProductOp<int>
    {
        public virtual int FileImage(FileImage fi0, FileImage fi1) { return 0; }
        public virtual int FileVolume(FileVolume fv0, FileVolume fv1) { return 0; }
        public virtual int PixImage(PixImage pi0, PixImage pi1) { return 0; }
        public virtual int PixVolume(PixVolume pv0, PixVolume pv1) { return 0; }
        public virtual int PixImageMipMap(PixImageMipMap mm0, PixImageMipMap mm1) { return 0; }
        public virtual int PixImageInfo(PixImageInfo ii0, PixImageInfo ii1) { return 0; }
        public virtual int PixVolumeInfo(PixVolumeInfo vi0, PixVolumeInfo vi1) { return 0; }
        public virtual int PixCubeMap(PixCubeMap cm0, PixCubeMap cm1, int[] subArray) { return subArray.Sum(); }
        public virtual int PixStack(PixStack is0, PixStack is1, int[] subArray) { return subArray.Sum(); }
    }

    public class PixSaveOp : PixInnerProductOp<int>
    {
        public string PathPrefix;

        public PixSaveOp(IPix other, string pathPrefix)
            : base(new PixIntSumProductOp(), other)
        { PathPrefix = pathPrefix; }

        public override Func<int> PixImage(PixImage pi)
        {
            return () =>
            {
                var typedOther = Other as FileImage;
                if (typedOther == null) throw new ArgumentException();
                pi.SaveAsImage(PathPrefix + typedOther.Path);
                return 1;
            };
        }

        public override Func<int> PixImageMipMap(PixImageMipMap pimm)
        {
            return () =>
            {
                var typedOther = Other as FileImage;
                if (typedOther == null) throw new ArgumentException();
                pimm.ImageArray[0].SaveAsImage(PathPrefix + typedOther.Path);
                return 1;
            };
        }

    }

    #endregion

    #region PixToPixImage<T>

    public class PixToPixImage<T> : PixCopyOp, IPixOp<IPix>
    {
        public override IPix PixImage(PixImage pi)
        {
            var newImage = pi.ToPixImage<T>();
            return pi == newImage ? pi : newImage;
        }

        public override IPix PixImageMipMap(PixImageMipMap pimm)
        {
            var newImageArray = pimm.ImageArray.Copy(p => p.ToPixImage<T>());
            return newImageArray.AllEqual(pimm.ImageArray, (a, b) => a == b)
                                ? pimm : new PixImageMipMap(newImageArray);
        }

        public override IPix PixImageInfo(PixImageInfo pii)
        {
            if (typeof(T) == pii.Format.Type) return pii;
            return new PixImageInfo(new PixFormat(typeof(T), pii.Format.Format), pii.Size);
        }
    }

    #endregion

    #region PixGetInfo

    public class PixGetInfo : PixCopyOp, IPixOp<IPix>
    {
        public override IPix PixImage(PixImage pi) { return pi.Info; }
        public override IPix PixImageMipMap(PixImageMipMap pimm) { return pimm.ImageArray[0].Info; }
    }

    #endregion

    #region PixInfoToZeroSize

    public class PixInfoToZeroSize : PixCopyOp, IPixOp<IPix>
    {
        public override IPix PixImageInfo(PixImageInfo pii)
        {
            return new PixImageInfo(pii.Format, V2i.Zero);
        }

        public override IPix PixVolumeInfo(PixVolumeInfo pvi)
        {
            return new PixVolumeInfo(pvi.Format, V3i.Zero);
        }
    }

    #endregion

    #region PixRemoveMipMaps

    public class PixRemoveMipMaps : PixCopyOp, IPixOp<IPix>
    {
        public override IPix PixImageMipMap(PixImageMipMap pimm) { return pimm.ImageArray[0]; }
    }

    #endregion

    #region PixCreateMipMaps

    public class PixCreateMipMaps : PixCopyOp, IPixOp<IPix>
    {
        public PixImageMipMap.MipMapOptions Options;

        public PixCreateMipMaps(PixImageMipMap.MipMapOptions options) { Options = options; }

        public override IPix PixImage(PixImage pi)
        {
            return Base.PixImageMipMap.Create(pi, Options);
        }
    }

    #endregion

    #region PixGetPixImageArray

    public class PixGetPixImageArray : IPixOp<Func<PixImage[]>>
    {
        public int Level;

        public PixGetPixImageArray(int level) { Level = level; }

        public Func<PixImage[]> FileImage(FileImage fi) { return () => new PixImage[0]; }
        public Func<PixImage[]> FileVolume(FileVolume fv) { return () => new PixImage[0]; }

        public Func<PixImage[]> PixImage(PixImage pi)
        {
            return () => Level == 0 ? pi.IntoArray() : new PixImage[0];
        }

        public Func<PixImage[]> PixVolume(PixVolume pv) { return () => new PixImage[0]; }
        public Func<PixImage[]> PixImageInfo(PixImageInfo pii) { return () => new PixImage[0]; }
        public Func<PixImage[]> PixVolumeInfo(PixVolumeInfo pvi) { return () => new PixImage[0]; }

        public Func<PixImage[]> PixImageMipMap(PixImageMipMap pimm)
        {
            return () => Level == 0 ? pimm.ImageArray[0].IntoArray() : new PixImage[0];
        }

        public Func<PixImage[]> PixCubeMap(PixCubeMap pcm, Func<PixImage[]>[] subArray)
        {
            return () => { --Level; var subImages = subArray.Copy(f => f()); ++Level; return subImages.FlatCopy(); };
        }

        public Func<PixImage[]> PixStack(PixStack ps, Func<PixImage[]>[] subArray)
        {
            return () => { ++Level; var subImages = subArray.Copy(f => f()); --Level; return subImages.FlatCopy(); };
        }

    }

    #endregion

    #region IPix Extensions

    public static class IPixExtensions
    {

        public static IPix WithRemovedMipMaps(this IPix pix)
        {
            return pix.Op(new PixRemoveMipMaps());
        }

        public static IPix WithLoadedImages(this IPix pix, string pathPrefix = null)
        {
            return pix.Op(new PixLoadOp(pathPrefix == null ? "" : pathPrefix));
        }

        /// <summary>
        /// </summary>
        /// <returns>The number of saved PixImages.</returns>
        public static int SaveImages(this IPix pix, IPix names, string pathPrefix = null)
        {
            return pix.Op(new PixSaveOp(names, pathPrefix == null ? "" : pathPrefix))();
        }

        public static IPix WithMipMaps(
                this IPix pix, PixImageMipMap.MipMapOptions options = null)
        {
            return pix.Op(new PixCreateMipMaps(options == null
                            ? PixImageMipMap.MipMapOptions.Default : options));
        }

        public static IPix ToPixImage<T>(this IPix pix)
        {
            return pix.Op(new PixToPixImage<T>());
        }

        public static IPix GetDescriptors(this IPix pix)
        {
            return pix.Op(new PixGetInfo());
        }

        public static PixImage[] GetPixImageArray(this IPix pix, int level = 0)
        {
            return pix.Op(new PixGetPixImageArray(level))();
        }

        public static bool IsEqualTo(this IPix pix, IPix other,
                Func<PixImage, PixImage, bool> imageEqualFun = null,
                Func<PixVolume, PixVolume, bool> volumeEqualFun = null,
                Func<string, string, bool> stringEqualFun = null)
        {
            if (imageEqualFun == null) imageEqualFun = (a, b) => a == b;
            if (volumeEqualFun == null) volumeEqualFun = (a, b) => a == b;
            if (stringEqualFun == null) stringEqualFun = (a, b) => a == b;
            try
            {
                return pix.Op(new PixInnerProductOp<bool>(
                                    new PixEqualOp(imageEqualFun, volumeEqualFun, stringEqualFun), other))();
            }
            catch
            {
                return false;
            }
        }
    }
    
    #endregion

}
