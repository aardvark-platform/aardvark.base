using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    public enum ImageScalingScenario
    {
        Integer,
        Fractional,
        Anisotropic
    }

    [MemoryDiagnoser]
    public class ImageScalingBenchmark
    {
        private Volume<byte> m_source;
        private V2d m_scaleFactor;

        [ParamsAllValues]
        public ImageScalingScenario Scenario { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            V2l size;
            switch (Scenario)
            {
                case ImageScalingScenario.Integer:
                    size = new V2l(640, 480);
                    m_scaleFactor = new V2d(0.5, 0.5);
                    break;

                case ImageScalingScenario.Fractional:
                    size = new V2l(641, 479);
                    m_scaleFactor = new V2d(0.63, 0.57);
                    break;

                default:
                    size = new V2l(640, 480);
                    m_scaleFactor = new V2d(0.5, 1.0);
                    break;
            }

            m_source = ImageTensors.CreateImageVolume<byte>(new V3l(size, 4));
            var data = m_source.Data;
            for (int i = 0; i < data.Length; i++)
                data[i] = (byte)(i * 31 + i / 17);
        }

        [Benchmark, BenchmarkCategory("Existing")]
        public Volume<byte> Near()
            => m_source.Scaled(m_scaleFactor, ImageInterpolation.Near);

        [Benchmark, BenchmarkCategory("Existing")]
        public Volume<byte> Linear()
            => m_source.Scaled(m_scaleFactor, ImageInterpolation.Linear);

        [Benchmark, BenchmarkCategory("Existing")]
        public Volume<byte> Cubic()
            => m_source.Scaled(m_scaleFactor, ImageInterpolation.Cubic);

        [Benchmark, BenchmarkCategory("SuperSample")]
        public Volume<byte> SuperSample()
            => m_source.Scaled(m_scaleFactor, ImageInterpolation.SuperSample);
    }
}
