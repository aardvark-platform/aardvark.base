using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomSampleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // This initialization is required to be able to use DeviL for loading and saving PixImages.
            Aardvark.Base.Aardvark.Init();
            
            var rnd = new RandomSystem(1);
            var halton = new HaltonRandomSeries(2, rnd);
            var pseudo = new PseudoRandomSeries(new RandomSystem(2));
            var forced = new ForcedRandomSeries(new V2i[0], V2i.OO);
            
            DrawLambertian(30000, 1024, pseudo).
                SaveAsImage("C:\\Debug\\RandomLambert_Pseudo.bmp");

            DrawLambertian(30000, 1024, halton).
                SaveAsImage("C:\\Debug\\RandomLambert_Halton.bmp");
            
            DrawDiskSamples(30000, 1024, halton).
                SaveAsImage("C:\\Debug\\RandomDisk_Halton.bmp");

            DrawDiskSamples(30000, 1024, pseudo).
                SaveAsImage("C:\\Debug\\RandomDisk_Psuedo.bmp");
        }

        static PixImage DrawDiskSamples(int cnt, int imgSize, IRandomSeries rnd)
        {
            var img = new PixImage<byte>(imgSize, imgSize, 4);
            var imgMat = img.GetMatrix<C4b>();

            for (int i = 0; i < cnt; i++)
            {
                var p = RandomSample.Disk(rnd, 0) * 0.5 + 0.5;
                var crd = new V2i(p * new V2d(img.Size - 1));
                imgMat[crd.X, crd.Y] = C4b.White;
            }

            return img;
        }

        static PixImage DrawLambertian(int cnt, int imgSize, IRandomSeries rnd)
        {
            var img = new PixImage<byte>(imgSize, imgSize, 4);
            var imgMat = img.GetMatrix<C4b>();

            for (int i = 0; i < cnt; i++)
            {
                var vec = RandomSample.Lambertian(V3d.OOI, rnd, 0);
                vec = vec * 0.5 + 0.5;
                var crd = new V2i(vec.XY * new V2d(img.Size - 1));
                imgMat[crd.X, crd.Y] = C4b.White;
            }

            return img;
        }
    }
}
