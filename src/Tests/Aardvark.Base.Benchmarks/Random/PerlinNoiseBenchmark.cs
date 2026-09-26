using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*PerlinNoiseBenchmark*'
    [MemoryDiagnoser]
    public class PerlinNoiseBenchmark
    {
        private const int Count = 1024;

        private readonly PerlinNoise m_noise = new PerlinNoise();
        private readonly float[] m_positiveX = new float[Count];
        private readonly float[] m_positiveY = new float[Count];
        private readonly float[] m_positiveZ = new float[Count];
        private readonly float[] m_mixedX = new float[Count];
        private readonly float[] m_mixedY = new float[Count];
        private readonly float[] m_mixedZ = new float[Count];

        [GlobalSetup]
        public void Setup()
        {
            for (int i = 0; i < Count; i++)
            {
                float x = 0.125f + (i * 37 & 255) * 0.125f;
                float y = 0.375f + (i * 53 & 255) * 0.125f;
                float z = 0.625f + (i * 71 & 255) * 0.125f;

                m_positiveX[i] = x;
                m_positiveY[i] = y;
                m_positiveZ[i] = z;
                m_mixedX[i] = (i & 1) == 0 ? -x : x;
                m_mixedY[i] = i % 3 == 0 ? -y : y;
                m_mixedZ[i] = i % 4 == 0 ? z : -z;
            }
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float Positive1D()
        {
            float result = 0.0f;
            for (int i = 0; i < Count; i++)
                result += m_noise.InterpolateNoise(m_positiveX[i]);
            return result;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float MixedSign1D()
        {
            float result = 0.0f;
            for (int i = 0; i < Count; i++)
                result += m_noise.InterpolateNoise(m_mixedX[i]);
            return result;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float Positive2D()
        {
            float result = 0.0f;
            for (int i = 0; i < Count; i++)
                result += m_noise.InterpolateNoise(m_positiveX[i], m_positiveY[i]);
            return result;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float MixedSign2D()
        {
            float result = 0.0f;
            for (int i = 0; i < Count; i++)
                result += m_noise.InterpolateNoise(m_mixedX[i], m_mixedY[i]);
            return result;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float Positive3D()
        {
            float result = 0.0f;
            for (int i = 0; i < Count; i++)
                result += m_noise.InterpolateNoise(m_positiveX[i], m_positiveY[i], m_positiveZ[i]);
            return result;
        }

        [Benchmark(OperationsPerInvoke = Count)]
        public float MixedSign3D()
        {
            float result = 0.0f;
            for (int i = 0; i < Count; i++)
                result += m_noise.InterpolateNoise(m_mixedX[i], m_mixedY[i], m_mixedZ[i]);
            return result;
        }
    }
}
