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

            var series = new(string, IRandomSeries)[]
            {
                ("Pseudo", new PseudoRandomSeries(new RandomSystem(2))),
                ("Halton", new HaltonRandomSeries(2, rnd)),
                ("Forced", new ForcedRandomSeries("C:\\Debug\\forced-1024-sq.bin", rnd)),
            };

            var size = 1024;
            var sampleCount = 30000;

            var sampleImages = series.Map(rndSeries =>
            {
                var img = new PixImage<byte>(size, size, 4);
                var samples = GenerateRectSamples(sampleCount, rndSeries.Item2);
                DrawSamples(samples, img);
                img.SaveAsImage(String.Format("C:\\Debug\\Samples_{0}.bmp", rndSeries.Item1));
                return img;
            });

            SaveStiched(sampleImages, "C:\\Debug\\Compare_Samples.bmp");

            var diskImages = series.Map(rndSeries =>
            {
                var img = new PixImage<byte>(size, size, 4);
                var samples = GenerateDiskSamples(sampleCount, rndSeries.Item2);
                DrawSamples(samples, img);
                img.SaveAsImage(String.Format("C:\\Debug\\DiskSamples_{0}.bmp", rndSeries.Item1));
                return img;
            });

            SaveStiched(diskImages, "C:\\Debug\\Compare_DiskSamples.bmp");

            var lambertImages = series.Map(rndSeries =>
            {
                var img = new PixImage<byte>(size, size, 4);
                var samples = GenerateLambertianSamples(sampleCount, rndSeries.Item2);
                DrawSamples(samples, img);
                img.SaveAsImage(String.Format("C:\\Debug\\LambertSamples_{0}.bmp", rndSeries.Item1));
                return img;
            });

            SaveStiched(diskImages, "C:\\Debug\\Compare_LambertSamples.bmp");

            var triangles = GenerateTriangles(10, rnd);
            var trianglesImage = new PixImage<byte>(size, size, 4);
            DrawTriangles(triangles, trianglesImage);
            trianglesImage.SaveAsImage("C:\\Debug\\Triangles.bmp");

            var triangleImages = series.Map(rndSeries =>
            {
                var img = new PixImage<byte>(size, size, 4);
                DrawTriangles(triangles, img);
                var samples = GenerateTriangleSamples2(triangles, sampleCount / 3, rnd, rndSeries.Item2);
                DrawSamples(samples, img);
                img.SaveAsImage(String.Format("C:\\Debug\\TriangleSamples_{0}.bmp", rndSeries.Item1));
                return img;
            });

            SaveStiched(triangleImages, "C:\\Debug\\Compare_TriangleSamples.bmp");


            var cdfSampleTestCount = 1000000;
            var pdfSize = 255; // uneven values with give symmetrical pdf
            var gaussStretch = pdfSize / 10.0;
            var pdf = new double[pdfSize].SetByIndex(i =>
            {
                var x = (i - pdfSize / 2) / gaussStretch;
                return Gauss(x, 1.0);
            });

            var cdf = new DiscreteFunction(pdf);
            var test = cdf.Norm.ApproximateEquals(gaussStretch, 1e-4);
            Report.Line("TEST: Gauss PDF Normalized = {0}", test);

            rnd.ReSeed(0);
            var sc1 = new int[pdf.Length];
            Report.BeginTimed("CDF.Sample (Binary Search)");
            for (int i = 0; i < cdfSampleTestCount; i++)
            {
                var ind = cdf.Sample(rnd.UniformDouble());
                sc1[ind]++;
            }
            Report.End();
            var cdfSampling1 = DrawSampleDistributionCounts(sc1, 1024);
            DrawGauss(cdfSampling1, gaussStretch);
            cdfSampling1.SaveAsImage("C:\\Debug\\CDFSampling1.bmp");

            var sc2 = new int[pdf.Length];

            rnd.ReSeed(0);
            Report.BeginTimed("CDF.Sample (Linear Search)");
            for (int i = 0; i < cdfSampleTestCount; i++)
            {
                var ind = cdf.SampleLinear(rnd.UniformDouble());
                sc2[ind]++;
            }
            Report.End();
            var cdfSampling2 = DrawSampleDistributionCounts(sc2, 1024);
            DrawGauss(cdfSampling2, gaussStretch);
            cdfSampling2.SaveAsImage("C:\\Debug\\CDFSampling2.bmp");

            var test2 = 0.UpToExclusive(sc1.Length).All(i => sc1[i] == sc2[i]);
            Report.Line("TEST: Sampling Equal = {0}", test2);

            SaveStiched(new[] { cdfSampling1, cdfSampling1 }, "C:\\Debug\\Compare_CDFSampling.bmp");

            var rndSampleTestCount = 10000000;
            var counts = new int[size];
            var stdRnd = new RandomGaussian(rnd);
            counts.Set(0);

            Report.BeginTimed("RandomGaussian");
            for (int i = 0; i < rndSampleTestCount; i++)
            {
                var s = (int)Fun.Round((stdRnd.GetDouble() * 0.25 + 0.5) * size);
                if (s >= 0 && s < size)
                    counts[s]++;
            }
            Report.End();

            var rndGaussImg = DrawSampleDistributionCounts(counts, 1024);
            rndGaussImg.SaveAsImage("C:\\Debug\\RandomGaussian.bmp");

            counts.Set(0);
            Report.BeginTimed("RandomUniform.Normal");
            for (int i = 0; i < rndSampleTestCount; i++)
            {
                var s = (int)Fun.Round((rnd.Gaussian(0, 1) * 0.25 + 0.5) * size);
                if (s >= 0 && s < size)
                    counts[s]++;
            }
            Report.End();

            var rndGaussImg2 = DrawSampleDistributionCounts(counts, 1024);
            rndGaussImg2.SaveAsImage("C:\\Debug\\RandomUniform.Normal.bmp");

            SaveStiched(new [] { rndGaussImg, rndGaussImg2 }, "C:\\Debug\\Compare_RandomGaussian.bmp");
        }

        static double Gauss(double x, double s)
        {
            return 1 / (s * Constant.SqrtPiTimesTwo) * Fun.Exp(-0.5 * (x / s).Square());
        }

        static double Gauss(double x, double y, double sx, double sy)
        {
            return 1 / (sx * sy * Constant.PiTimesTwo) * Fun.Exp(-0.5 * ((x / sx).Square() + (y / sy).Square()));
        }

        static void DrawGauss(PixImage<byte> img, double gaussStretch)
        {
            var imgMat = img.GetMatrix<C4b>();
            var max = Gauss(0, 1.0);
            var lastY = 0;
            for(int x = 0; x < img.Size.X; x++)
            {
                var gx = (x - (img.Size.X - 1) / 2.0) / gaussStretch * 0.25;
                var gy = (int)(Gauss(gx, 1.0) / max * (img.Size.Y - 1));
                if (x > 0)
                {
                    var last = new V2i(x - 1, img.Size.Y - 1 - lastY);
                    var curr = new V2i(x - 1, img.Size.Y - 1 - gy);
                    imgMat.SetLine(last, curr, C4b.Green);
                }
                lastY = gy;
            }
        }

        static PixImage<byte> DrawSampleDistributionCounts(int[] counts, int size)
        {
            var maxCount = counts.Max();
            var img = new PixImage<byte>(size, size, 4);
            var imgMat = img.GetMatrix<C4b>();
            var currentLine = 0;
            counts.ForEach((c, i) =>
            {
                var h = (int)((c / (maxCount + 1.0)) * size);
                var nextLine = ((i + 1) * size / counts.Length);
                while (currentLine < nextLine)
                {
                    imgMat.SetLine(new V2i(currentLine, size - 1), new V2i(currentLine, size - 1 - h), C4b.White);
                    currentLine++;
                }
            });
            return img;
        }

        static void SaveStiched(IEnumerable<PixImage<byte>> images, string filename)
        {
            var size = images.First().Size.Y;
            var offsetImage= new PixImage<byte>(size / 50, size, 4);

            // interleave the sample images with an image for empty space
            // -> Zip with stitch images + remove last
            var cnt = images.Count();
            var array = images.Zip(offsetImage.Repeat(cnt)).Take(cnt * 2 - 1).ToArray().IntoArray();
            array.Stitch().SaveAsImage(filename);
        }

        static void SaveStichedSquare(PixImage<byte>[] images, string filename)
        {
            images.StitchSquare().SaveAsImage(filename);
        }

        static double Normal(IRandomUniform rnd, double mean, double stdDev)
        {
            // Box-Muller Transformation
            var u1 = 1.0 - rnd.UniformDouble();   // uniform (0,1] -> log requires > 0
            var u2 = rnd.UniformDouble();         // uniform [0,1)
            var randStdNormal = Fun.Sqrt(-2.0 * Fun.Log(u1)) *
                                Fun.Sin(Constant.PiTimesTwo * u2);
            return mean + stdDev * randStdNormal; 
        }

        static Triangle2d[] GenerateTriangles(int cnt, IRandomUniform rnd)
        {
            // generate triangles in 2d area [0, 1]
            
            var ta = 1.5 / cnt; // area of triangle
            var tr = Fun.Sqrt(ta * 0.5); // edge length of right angled triangle with area ta

            var xx = new Random(1);

            var triangles = new List<Triangle2d>(cnt);
            const int maxFails = 1000;
            var fails = 0;
            while(triangles.Count < cnt && fails < maxFails)
            {
                var p0 = rnd.UniformV2d();
                var r1 = Normal(rnd, tr, tr * 0.5);
                var p1 = p0 + rnd.UniformV2dDirection() * r1;
                if (p1.AnySmaller(0.0) || p1.AnyGreater(1.0))
                {
                    fails++;
                    continue;
                }
                var r2 = Normal(rnd, tr, tr * 0.5);
                var p2 = p0 + rnd.UniformV2dDirection() * r2;
                if (p2.AnySmaller(0.0) || p2.AnyGreater(1.0))
                {
                    fails++;
                    continue;
                }

                var t = new Triangle2d(p0, p1, p2);

                if (t.Area < ta * 0.2)
                {
                    fails++;
                    continue;
                }

                if (triangles.Any(x => x.Intersects(t)))
                {
                    fails++;
                }
                else
                {
                    triangles.Add(t);
                    fails = 0;
                }
            }

            return triangles.ToArray();
        }

        static PixImage<byte> DrawDiskSamples(int cnt, int imgSize, IRandomSeries rnd)
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

        static V2d[] GenerateDiskSamples(int cnt, IRandomSeries rnd)
        {
            return new V2d[cnt].SetByIndex(i => RandomSample.Disk(rnd, 0).XY * 0.5 + 0.5);
        }

        static V2d[] GenerateRectSamples(int cnt, IRandomSeries rnd)
        {
            return new V2d[cnt].SetByIndex(i => new V2d(rnd.UniformDouble(0), rnd.UniformDouble(1)));
        }

        static V2d[] GenerateLambertianSamples(int cnt, IRandomSeries rnd)
        {
            return new V2d[cnt].SetByIndex(i => RandomSample.Lambertian(V3d.OOI, rnd, 0).XY * 0.5 + 0.5);
        }

        static void DrawTriangles(IEnumerable<Triangle2d> ta, PixImage<byte> img)
        {
            var imgMat = img.GetMatrix<C4b>();
            var sz = new V2d(img.Size - 1);
            foreach (var t in ta)
            {
                foreach (var e in t.EdgeLines)
                    imgMat.SetLine(e.P0 * sz, e.P1 * sz, new C4b(0.5, 0.5, 1));
            }
        }

        static void DrawSamples(IEnumerable<V2d> samples, PixImage<byte> img)
        {
            var imgMat = img.GetMatrix<C4b>();
            var sz = new V2d(img.Size - 1);
            foreach (var s in samples)
                imgMat[new V2i(s * sz)] = C4b.White;
        }

        static V2d[] GenerateTriangleSamples(Triangle2d[] ta, int count, IRandomUniform rnd, IRandomSeries rndSeries)
        {
            var cdf = new DiscreteFunction(ta.Select(t => t.Area).ToArray());
            return new V2d[count].SetByIndex(i =>
            {
                var ti = cdf.Sample(rnd.UniformDouble());
                var t = ta[ti];
                var s = RandomSample.Triangle(t, rndSeries, 0);
                return s.XY;
            });
        }

        static V2d[] GenerateTriangleSamples2(Triangle2d[] ta, int count, IRandomUniform rnd, IRandomSeries rndSeries)
        {
            var areas = ta.Map(t => t.Area);
            var total = areas.Sum();
            var samples = new List<V2d>(count);
            ta.ForEach((t, i) =>
            {
                var tsc = (int)Fun.Round(areas[i] / total * count);
                for(int s = 0; s < tsc; s++)
                    samples.Add(RandomSample.Triangle(t, rndSeries, 0));
            });
            return samples.ToArray();
        }

        /// <summary>
        /// Represents a discrete function that is represented by a probability density function (PDF)
        /// and a cumulative density function (CDF).
        /// </summary>
        public class DiscreteFunction
        {
            double[] m_pdf;
            double[] m_cdf;

            /// <summary>
            /// Gets the input probability density function.
            /// It is not necessarily normalized.
            /// </summary>
            public double[] PDF { get { return m_pdf; } }

            /// <summary>
            /// Gets the calculated cumulative density function with +1 elements than the PDF.
            /// It is not necessarily normalized.
            /// </summary>
            public double[] CDF { get { return m_cdf; } }

            /// <summary>
            /// Returns the normalization factor of the PDF and CDF.
            /// In case the supplied PDF is not normalized this factor will be != 1.0 and
            /// needs to be considered when interpreting the raw PDF or CDF values.
            /// </summary>
            public double Norm { get { return m_cdf[m_cdf.Length - 1]; } }
            
            /// <summary>
            /// Create distribution from discrete probability distribution function (PDF).
            /// The PDF does not need to be normalized.
            /// </summary>
            public DiscreteFunction(double[] pdf)
            {
                m_pdf = pdf;
                m_cdf = m_pdf.Integrated().ToArray();
            }

            /// <summary>
            /// O(log n) binary search
            /// </summary>
            public int Sample(double rnd)
            {
                var valueToFind = rnd * m_cdf.Last();

                var i0 = 0;
                var range = m_cdf.Length;
                // binary search
                while (range > 0)
                {
                    var halfRange = range >> 1;
                    var center = i0 + halfRange;

                    // check if value is left or right
                    if (m_cdf[center] <= valueToFind)
                    {
                        // right half
                        i0 = center + 1;
                        range -= halfRange + 1;
                    }
                    else
                    {
                        // left half
                        range = halfRange;
                    }
                }
                return Fun.Clamp(i0 - 1, 0, m_cdf.Length - 2);
            }

            /// <summary>
            /// O(n) linear search
            /// </summary>
            public int SampleLinear(double rnd)
            {
                var sum = m_cdf.Last();
                var valueToFind = rnd * sum;
                int index = 0;
                double aggregate = 0;
                // Linear Search
                for (; index < m_pdf.Length; index++)
                {
                    aggregate += m_pdf[index];
                    if (valueToFind <= aggregate)
                        break;
                }
                return index;
            }

            /// <summary>
            /// Gets the normalized probability density function value at the supplied index.
            /// The function is normalized to integrate to a value of 1.
            /// </summary>
            public double PDFValue(int index)
            {
                return m_pdf[index] / this.Norm;
            }

            /// <summary>
            /// Gets the normalized cumulative distribution function value at the supplied index.
            /// The function is normalized to integrate to a value of 1.
            /// </summary>
            public double CDFValue(int index)
            {
                return m_cdf[index] / this.Norm;
            }
        }
    }
}
