using Aardvark.Base;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace PixImageDemo
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            // require a directory of the following name inside the Aardvark workdir.
            string dir = WorkDir.FindDir("PixImageDemo");

            PolygonDemo(dir);
            HowManyColorsIllusion(dir);
            ResampleDemo(dir);
            LinearRampDemo(dir);
            VariousDemos(dir);
        }


        public static void PolygonDemo(string dir)
        {
            // This code produces tiny tiff images that demonstrate the pixel-
            // level precision of SetMonotonePolygonFilledRaw.

            var tris = new[] {
                new { Tri = new V2d[] { new V2d(7.7, 2.5), new V2d(11.7, 2.5), new V2d(9.8, 0.8) }, Col = C3b.DarkGreen },
                new { Tri = new V2d[] { new V2d(11.7, 2.5), new V2d(7.7, 2.5), new V2d(9.5, 5.2) }, Col = C3b.Green },
                new { Tri = new V2d[] { new V2d(7, 4), new V2d(5, 6), new V2d(8, 7) }, Col = C3b.Green },
                new { Tri = new V2d[] { new V2d(1, 1), new V2d(2, 4), new V2d(6, 2) }, Col = C3b.Green },
                new { Tri = new V2d[] { new V2d(7, 4), new V2d(1, 6), new V2d(5, 6) }, Col = C3b.DarkGreen },
                new { Tri = new V2d[] { new V2d(7, 4), new V2d(8, 7), new V2d(9.5, 5.5) }, Col = C3b.DarkGreen },
                new { Tri = new V2d[] { new V2d(13.5, 5.5), new V2d(13.5, 7.5), new V2d(15.5, 5.5) }, Col = C3b.Green },
                new { Tri = new V2d[] { new V2d(15.5, 5.5), new V2d(13.5, 7.5), new V2d(15.5, 7.5) }, Col = C3b.DarkGreen },
                new { Tri = new V2d[] { new V2d(15, 0), new V2d(13.5, 1.5), new V2d(14.5, 2.5) }, Col = C3b.Green },
                new { Tri = new V2d[] { new V2d(14.5, 2.5), new V2d(13.5, 1.5), new V2d(14.5, 2.5) }, Col = C3b.Red },
                new { Tri = new V2d[] { new V2d(7.5, 0.5), new V2d(6.5, 1.5), new V2d(7.5, 1.5) }, Col = C3b.Red },
                new { Tri = new V2d[] { new V2d(6.3, 0.3), new V2d(5.3, 1.3), new V2d(6.3, 1.3) }, Col = C3b.Red },
                new { Tri = new V2d[] { new V2d(11.5, 4.5), new V2d(11.5, 6.5), new V2d(12.5, 5.5) }, Col = C3b.DarkGreen },
            };

            var triImg = new PixImage<byte>(16, 8, 3);
            var tMat = triImg.GetMatrix<C3b>();
            Report.BeginTimed("polygon demo triangles");
            foreach (var t in tris)
                tMat.SetMonotonePolygonFilledRaw(t.Tri, t.Col);
            triImg.SaveAsImage(Path.Combine(dir, "polygon-triangles.tiff"));
            Report.End();

            var polyImg = new PixImage<byte>(10, 10, 3);

            var pMat = polyImg.GetMatrix<C3b>();

            V2d[] poly = new V2d[] {
                new V2d( 4,  1),
                new V2d( 1,  3),
                new V2d( 2,  6),
                new V2d( 2.1,  6.1),
                new V2d( 5,  7),
                new V2d( 8,  5),
                new V2d( 9,  3),
            };
            Report.BeginTimed("polygon demo polygon");
            pMat.SetMonotonePolygonFilledRaw(poly, C3b.Red);
            polyImg.SaveAsImage(Path.Combine(dir, "polygon-polygon.tiff"));
            Report.End();
        }

        /// <summary>
        /// Creates Images of an optical illusion that tricks the mind into
        /// seeing more different colors (4) than are actually present in the
        /// image (3).
        /// </summary>
        public static PixImage<byte> CreateHowManyColorsIllusion(int size, bool parallel = true)
        {
            var scale = 1024.0 / size;
            var delta = 0.5 * (double)(size - 1);

            var pixImage = new PixImage<byte>(size, size, 3);

            var colorMatrix = pixImage.GetMatrix<C3b>();

            var orange = new C3b(255, 150, 0);
            var magenta = new C3b(255, 0, 255);
            var bluegreen = new C3b(0, 255, 150);
            Func<long, long, C3b> pixelFun = (x, y) =>
            {
                var xd = scale * (x - delta); var yd = scale * (y - delta);
                var r = Fun.Sqrt(xd * xd + yd * yd);
                var phi = Fun.Atan2(yd, xd);
                var lp1 = phi / Constant.PiTimesFour;
                var lp2 = phi / Constant.Pi; // TimesTwo;
                var lr = Fun.Log(r) / Constant.E;
                var p1 = Fun.Frac(0.05 + 4 * (lr - lp1));
                var p2 = Fun.Frac(96 * (lr + lp2)); // 64
                return p2 < 0.5
                    ? (p1 >= 0.0 && p1 < 0.25 ? bluegreen : orange)
                    : (p1 >= 0.5 && p1 < 0.75 ? bluegreen : magenta);
            };

            if (parallel)
                colorMatrix.SetByCoordParallelY(pixelFun);
            else
                colorMatrix.SetByCoord(pixelFun);

            return pixImage;
        }


        public static void HowManyColorsIllusion(string dir)
        {
            bool parallel = true;

            var sizes = new[] { 512, 576, 648, 768, 864, 1024, 1152, 1296, 1536, 1728 };

            Report.BeginTimed("how-many-colors illusions");
            foreach (var size in sizes)
            {
                Report.BeginTimed("size: {0}", size);
                var pixImg = CreateHowManyColorsIllusion(size, parallel);
                pixImg.SaveAsImage(Path.Combine(dir, "how-many-colors-" + size.ToString() + ".png"));
                Report.End();
            }
            Report.End();

        }

        /// <summary>
        /// Perform image resampling in software. Shows how to use higher order
        /// resampling (e.g. Lanczos or Bicubic) on matrices. This is not a very
        /// fast implementation, but it works on Matrices of arbitrary type!
        /// </summary>
        public static void ResampleDemo(string dir)
        {
            Report.BeginTimed("resample example");
            var inImg = CreateHowManyColorsIllusion(300);
            var inMat = inImg.GetMatrix<C3b>();

            // enlarge by a factor of Pi to see what happens
            double scale = 1.0 / Constant.Pi;
            double shift = -13.0;

            var outImg0 = new PixImage<byte>(1024, 1024, 3);
            var outMat0 = outImg0.GetMatrix<C3b>();
            var outImg1 = new PixImage<byte>(1024, 1024, 3);
            var outMat1 = outImg1.GetMatrix<C3b>();
            var outImg2 = new PixImage<byte>(1024, 1024, 3);
            var outMat2 = outImg2.GetMatrix<C3b>();
            var outImg3 = new PixImage<byte>(1024, 1024, 3);
            var outMat3 = outImg3.GetMatrix<C3b>();

            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(-0.5);

            outMat0.ForeachIndex((x, y, i) =>
                {
                    /// Note: LinComRawC3f in x direction results in a byte color (range 0-255) stored
                    /// in a C3f. The second C3f.LinCom for the y direction does not perform any additional
                    /// scaling, thus we need to copy the "ByteInFloat" color back to a byte color at the
                    /// end (this perfoms clamping). Tensor.Tensor.Index6SamplesClamped clamps to the border
                    /// region and allows any double pixel address.
                    outMat0[i] = inMat.Sample36(x * scale + shift, y * scale + shift,
                                               Fun.Lanczos3f, Fun.Lanczos3f,
                                               C3b.LinComRawC3f, C3f.LinCom,
                                               Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped)
                                                .Copy(Col.ByteFromByteInFloatClamped);

                    /// Note: LinComRawC3f in x direction results in a byte color (range 0-255) stored
                    /// in a C3f. The second C3f.LinCom for the y direction does not perform any additional
                    /// scaling, thus we need to copy the "ByteInFloat" color back to a byte color at the
                    /// end (this perfoms clamping). Tensor.Index4SamplesClamped clamps to the border
                    /// region and allows any double pixel address.
                    outMat1[i] = inMat.Sample16(x * scale + shift, y * scale + shift,
                                               hermiteSpline, hermiteSpline,
                                               C3b.LinComRawC3f, C3f.LinCom,
                                               Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped)
                                                .Copy(Col.ByteFromByteInFloatClamped);


                    /// Note here the two C3b.LinCom calls perform the clamping immediately. Thus we have
                    /// Five clamping calls on each sample: 4 on each x-line, and one in the final
                    /// y-interpolation.
                    /// Here we have cyclic border handling. Note that Tensor.Index4SamplesCyclic1 only
                    /// handles one cycle at each side (minus some border pixels), so the addressable
                    /// range is not quite 3x3 times the size of the original image.
                    outMat2[i] = inMat.Sample16(x * scale + shift, y * scale + shift,
                                               hermiteSpline, hermiteSpline,
                                               C3b.LinCom, C3b.LinCom,
                                               Tensor.Index4SamplesCyclic1, Tensor.Index4SamplesCyclic1);

                    // outMat3[i] = inMat.Sample4Clamped(x * scale + shift, y * scale + shift, ColFun.Lerp, ColFun.Lerp);
                    outMat3[i] = inMat.Sample4Clamped(x * scale + shift, y * scale + shift, ColFun.LerpC3f, ColFun.Lerp).Copy(Col.ByteFromByteInFloatClamped);
                });

            outImg0.SaveAsImage(Path.Combine(dir, "resample-36clamped.tif"));
            outImg1.SaveAsImage(Path.Combine(dir, "resample-16clamped.tif"));
            outImg2.SaveAsImage(Path.Combine(dir, "resample-d16cyclic.tif"));
            outImg3.SaveAsImage(Path.Combine(dir, "resample-4clamped.tif"));
            Report.End();
        }


        public static void VariousDemos(string dir)
        {
            bool alternative = true;

            // WriteLinearRampImage();
            // Interpolation();

            Report.BeginTimed("various PixImage demos");
            // NOTE: in the following comments a byte image is an image that uses a
            // byte for each channel of each pixel, a float image is an image that uses
            // a float for each channel of each pixel.

            var colorImage = CreateHowManyColorsIllusion(1024);

            // scaling an image
            var scaledColorImage = new PixImage<byte>(1280, 800, 3);
            scaledColorImage.GetMatrix<C4b>().SetScaledCubic(colorImage.GetMatrix<C4b>());

            scaledColorImage.SaveAsImage(Path.Combine(dir, "v-scaled-image.png"));


            // writing a color png image
            colorImage.SaveAsImage(Path.Combine(dir, "v-color-image.png"));

            var grayImage = colorImage.ToGrayscalePixImage();


            // scaling a grayscale image
            var scaledGrayImage = new PixImage<byte>(1280, 800, 1);
            scaledGrayImage.Matrix.SetScaledLanczos(grayImage.Matrix);

            scaledGrayImage.SaveAsImage(Path.Combine(dir, "v-scaled-gray-image.png"));

            // for grayscale and black/white images the Matrix property works
            grayImage.Matrix.SetLineY(16, 0, 100, 0);

            // writing a grayscale png image
            grayImage.SaveAsImage(Path.Combine(dir, "v-gray-image.png"));


            var gray2colorImage = grayImage.ToPixImage<byte>(Col.Format.BGR);
            // writing grayxcal image as a color image
            gray2colorImage.SaveAsImage(Path.Combine(dir, "v-gray2color-image.png"));

            // loading a 8-bit per channel color image
            var byteImg = new PixImage<byte>(Path.Combine(dir, "v-color-image.png"));

            //var byteImg2 = byteImg.Scaled(0.5);
            //byteImg2.SaveAsImage(Path.Combine(dir, "v-color-2.png"));
            //var byteImg4 = byteImg2.Scaled(0.5);
            //byteImg4.SaveAsImage(Path.Combine(dir, "v-color-4.png"));
            //var byteImg8 = byteImg4.Scaled(0.5);
            //byteImg8.SaveAsImage(Path.Combine(dir, "r-color-8.png"));

            // retrieving channel matrices from an rgb image
            var rc = byteImg.GetChannel(Col.Channel.Red);
            var gc = byteImg.GetChannel(Col.Channel.Green);
            var bc = byteImg.GetChannel(Col.Channel.Blue);

            // convert 8bit/channel rgb image to 16bit/channel image
            // var ushortImage = byteImg.ToPixImage<ushort>();

            // ushortImage.Rotated(30 * Constant.RadiansPerDegree, false)
            //            .SaveAsImage(Path.Combine(odir, "rotated_30_rgb16.png"));

            // save 16bit/channel rgb image.
            // ushortImage.SaveAsImage(Path.Combine(odir, "rgb8_to_rgb16.tif"));

            // load 16bit/channel rgb image
            // var ushortImage2 = new PixImage<ushort>(Path.Combine(odir, "rgb8_to_rgb16.tif"));

            // save again as 8bit/channel rgb image
            // ushortImage2.ToPixImage<byte>().SaveAsImage(Path.Combine(odir, "rgb8_to_rgb16_to_rgb8.tif"));

            // building a new rgb image from channel matrices
            var newImg = new PixImage<byte>(rc, gc, bc);

            // writing an 8-bit per channel png image
            newImg.SaveAsImage(Path.Combine(dir, "v-recombined-color.png"), PixFileFormat.Png,
                               options: PixSaveOptions.Default
                                        | PixSaveOptions.UseStorageService
                                        | PixSaveOptions.UseChunkedStream);

            //byteImg.Rotated(60.0 * Constant.RadiansPerDegree)
            //        .SaveAsImage(Path.Combine(dir, "v-rotated-60-resized.png"));

            //byteImg.Rotated(90.0 * Constant.RadiansPerDegree)
            //        .SaveAsImage(Path.Combine(dir, "v-rotated-90-resized.png"));

            //byteImg.Volume.Transformed(ImageTrafo.Rot90).ToPixImage()
            //        .SaveAsImage(Path.Combine(odir, "rotated_90_csharp_rgb8.png"));

            //byteImg.Rotated(180.0 * Constant.RadiansPerDegree)
            //        .SaveAsImage(Path.Combine(dir, "v-rotated-180-resized.png"));

            //byteImg.Volume.Transformed(ImageTrafo.Rot180).ToPixImage()
            //        .SaveAsImage(Path.Combine(odir, "rotated_180_csharp_rgb8.png"));

            //byteImg.Rotated(270.0 * Constant.RadiansPerDegree)
            //        .SaveAsImage(Path.Combine(dir, "v-rotated-270-resized.png"));

            //byteImg.Volume.Transformed(ImageTrafo.Rot270).ToPixImage()
            //        .SaveAsImage(Path.Combine(odir, "rotated_270_csharp_rgb8.png"));

            // loading an 8-bit per channel rgb image directly as a float image
            var floatImg = new PixImage<float>(Path.Combine(dir, "v-color-image.png"));

            // converting a float image to a byte image
            var floatToByteImg = floatImg.ToPixImage<byte>();

            // saving the converted image in png format
            floatToByteImg.SaveAsImage(Path.Combine(dir, "v-byte2float2byte-color.png"));

            // color conversion to linear response
            var linearFloatImg = floatImg.Copy<C3f>(Col.LinearSRGBFromSRGB);

            // converting the linear float image to a byte image and saving it in png format
            linearFloatImg.ToPixImage<byte>().SaveAsImage(Path.Combine(dir, "v-linear-color.png"));

            // loading a byte image
            var bImg = new PixImage<byte>(Path.Combine(dir, "v-color-image.png"));

            byte threshold = 2;

            var isSame = byteImg.Volume.InnerProduct(
                                bImg.Volume, (b1, b2) => Fun.Abs(b2-b1) < threshold, 
                                true, (equal, pixEqual) => equal && pixEqual, equal => !equal);



            // replacing a border of 50 pixels by replicating the 1-pixel frame inside
            // the border outwards
            bImg.Volume.ReplicateBorder(new Border2l(50));

            // acessing pixels of a byte image as C3b's  
            var c3bmatrix = bImg.GetMatrix<C3b>();

            // var copiedMatrix = c3bmatrix.Copy();

            var newC3fImage = c3bmatrix.ToPixImage<float>();

            // setting a region in the matrix
            c3bmatrix.SetRectangleFilled(48, 48, 52, 52, C3b.Black); // min x, min y, max x, max y

            if (alternative)
            {
                // this is equivalent to:
                c3bmatrix.SubMatrix(48, 48, 5, 5).Set(C3b.Black); // start x, start y, size x, size y
            }

            // accessing a single pixel of the matrix
            c3bmatrix[50, 50] = C3b.VRVisGreen;

            var size = c3bmatrix.Size;

            // draw a bresenham line
            c3bmatrix.SetLine(size.X - 50, 50, 50, size.Y - 50, C3b.Blue);

            // draw a bresenham circle
            c3bmatrix.SetCircle(size.X / 2, size.Y / 2, 50, C3b.Yellow);

            c3bmatrix.SetCircleFilled((size.X * 3) / 4, (size.Y * 3) / 4, 50, C3b.Yellow);
            c3bmatrix.SetCircleFilled(25, 25, 75, C3b.Yellow);


            var cx = size.X / 2; var cy = size.Y / 2;

            for (int i = 0; i < 36; i++)
            {
                var alpha = i * 2 * Constant.Pi / 36;
                var dx = Fun.Cos(alpha);
                var dy = Fun.Sin(alpha);

                c3bmatrix.SetLineAllTouchedRaw(cx + 64 * dx, cy + 64 * dy, cx + 128 * dx, cy + 128 * dy,
                                               C3b.Yellow);
            }




            // writing the image with the replicated border as a png
            bImg.SaveAsImage(Path.Combine(dir, "v-border-drawing.png"));

            Report.End();

        }

        public static void Difference(string dir)
        {
            var idir = Path.Combine(dir, "in");
            var odir = Path.Combine(dir, "out");
            var hiliteImg = new PixImage<float>(Path.Combine(idir, "ref_hilite.png"));
            var luxImg = new PixImage<float>(Path.Combine(idir, "ref_lux.png"));

            var diffImg = new PixImage<float>(Col.Format.RGB, hiliteImg.Size);

            diffImg.Volume.SetMap2(hiliteImg.Volume, luxImg.Volume, (a, b) => Fun.Abs(b - a));

            var outImg = diffImg.ToPixImage<ushort>();

            outImg.SaveAsImage(Path.Combine(odir, "difference.tiff"));
        }


        public static void CopyTest(string dir)
        {
            var idir = Path.Combine(dir, "in");
            var odir = Path.Combine(dir, "out");
            var src = new PixImage<byte>(Path.Combine(idir, "rgb8.jpg")); //.ToGrayscalePixImage();
            var image = new PixImage<byte>(src.Format, src.Size);
            var srcvolume = src.Volume;
            // var copyvol = srcvolume.Copy(); // does not work, creates default volume layout!
            var copyvol = srcvolume.CopyToImage(); // works since it creates default image layout!
            image.Volume = copyvol;
            image.SaveAsImage(Path.Combine(idir, "rgb8-copied.jpg"));
        }

        public static void LinearRampDemo(string dir)
        {
            Report.BeginTimed("linear ramp demo");
            var width = 1920;
            var height = 1080;
            var barHeight = height/4;

            var line = new byte[width].SetByIndex(
                        i => Col.ByteFromDouble((double)i / (double)(width - 1)));

            // write an image with linear red, green, blue, and gray ramps
            var linearImage = new PixImage<byte>(Col.Format.RGB, new V2i(width, height));
            var linVol = linearImage.Volume;

            linVol.SubVolume(0, 0, 0, width, barHeight, 1).AsMatrixWindow().SetByCoord((x, y) => line[x]);
            linVol.SubVolume(0, barHeight, 1, width, barHeight, 1).AsMatrixWindow().SetByCoord((x, y) => line[x]);
            linVol.SubVolume(0, 2 * barHeight, 2, width, barHeight, 1).AsMatrixWindow().SetByCoord((x, y) => line[x]);
            linVol.SubVolume(0, 3 * barHeight, 0, width, barHeight, 3).SetByCoord((x, y, c) => line[x]);

            linearImage.SaveAsImage(Path.Combine(dir, "linear-ramp.tiff"));
            Report.End();
        }
    }
}
