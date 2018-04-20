using System;

namespace Aardvark.Base
{
    /// <summary>
    /// Perlin noise generation class.
    /// Contains functions for 1D, 2D and 3D perlin noise generation.
    /// </summary>
    public class PerlinNoise
	{
		private int m_PrimeOne;
		private int m_PrimeTwo;
		private int m_PrimeThree;
		private int m_maxX;
		private int m_maxY;
		// private int m_maxZ;
		float[] m_lookup;

		private Func<float,float,float,float> m_interpolate;

		#region Constructors
		/// <summary>
		/// InitialintegerZes a new instance of the <see cref="PerlinNoise"/> class.
		/// </summary>
		public PerlinNoise()
		{
			m_PrimeOne   = 15731;
			m_PrimeTwo   = 789221;
			m_PrimeThree = 1376312589;
			m_lookup = null;
			m_maxX = 0;
			m_maxY = 0;
			// m_maxZ = 0;

            m_interpolate = (a, b, x) =>
            {
                x = (1 - (x * Constant.PiF).Cos()) * 0.5f;
                return a * (1 - x) + b * x;
            };
		}

		#endregion

		/// <summary>
		/// Generates a pseudo-random number based upon one value(dimension).
		/// </summary>
		/// <param name="x">An integer value.</param>
		/// <returns>A single-precision floating point value between -1 and 1.</returns>
		public float Noise(int x)
		{
			if (m_lookup != null)
				return m_lookup[x];

			x  = (x << 13) ^ x;
			double d = (double)((x * (x*x*m_PrimeOne+m_PrimeTwo)+m_PrimeThree) & 0x7fffffff);
			return ( 1.0f - (float)( d / 1073741824.0 ) );
		}

		/// <summary>
		/// Generates a pseudo-random number based upon two value(dimensions).
		/// </summary>
		/// <param name="x">An integer value.</param>
		/// <param name="y">An integer value.</param>
		/// <returns>A single-precision floating point value between -1 and 1.</returns>
		public float Noise(int x, int y)
		{
			if (m_lookup != null)
				return m_lookup[x+y*m_maxX];

			uint N = (uint)(x+y * 57);
			N = (N << 13) ^ N;
			double d = (double)((N * (N * N * m_PrimeOne+m_PrimeTwo )+m_PrimeThree) & 0x7fffffff);
			return(1.0f - (float)(d / 1073741824.0));
		}

		/// <summary>
		/// Generates a pseudo-random number based upon three value(dimensions).
		/// </summary>
		/// <param name="x">An integer value.</param>
		/// <param name="y">An integer value.</param>
		/// <param name="z">An integer value.</param>
		/// <returns>A single-precision floating point value between -1 and 1.</returns>
		public float Noise(int x, int y, int z)
		{
			if (m_lookup != null)
				return m_lookup[x+(z*m_maxY+y) * m_maxX];

			int L = (x+y * 57);
			int M = (y+z * 57);
			uint N = (uint)(L+M * 57);
			N = (N << 13) ^ N;
			double d = (double)((N * (N * N * m_PrimeOne+m_PrimeTwo )+m_PrimeThree ) & 0x7fffffff);
			return(1.0f - (float)(d / 1073741824.0));
		}
        
		public float SmoothNoise(int x)
            => (Noise(x)/2.0f)+(Noise(x-1)/4.0f)+(Noise(x+1)/4.0f);

		public float SmoothNoise(int x, int y)
		{
			float corners = (Noise(x-1, y-1) + Noise(x+1, y-1) + Noise(x-1, y+1) + Noise(x+1, y+1)) / 16.0f;
			float sides   = (Noise(x-1, y  ) + Noise(x+1, y  ) + Noise(x  , y-1) + Noise(x  , y+1)) / 8.0f;
			float center  =  Noise(x, y)/4.0f;
			return(corners+sides+center);
		}

		public float SmoothNoise(int x, int y, int z)
		{
			float corners, sides, center;
			float averageZM1, averageZ, averageZP1;

			// average of neighbours in z-1
			corners = (Noise(x-1, y-1, z-1) + Noise(x+1, y-1, z-1) + Noise(x-1, y+1, z-1) + Noise(x+1, y+1, z-1)) / 16.0f;
			sides   = (Noise(x-1, y  , z-1) + Noise(x+1, y  , z-1) + Noise(x  , y-1, z-1) + Noise(x  , y+1, z-1)) / 8.0f;
			center  =  Noise(x  , y  , z-1) / 4.0f;
			averageZM1     = corners+sides+center;

			// average of neighbours in z
			corners = (Noise(x-1, y-1, z) + Noise(x+1, y-1, z) + Noise(x-1, y+1, z) + Noise(x+1, y+1, z)) / 16.0f;
			sides   = (Noise(x-1, y  , z) + Noise(x+1, y  , z) + Noise(x  , y-1, z) + Noise(x  , y+1, z)) / 8.0f;
			center  =  Noise(x, y, z ) / 4.0f;
			averageZ       = corners+sides+center;

			// average of neighbours in z+1
			corners = (Noise(x-1, y-1, z+1) + Noise(x+1, y-1, z+1) + Noise(x-1, y+1, z+1) + Noise(x+1, y+1, z+1)) / 16.0f;
			sides   = (Noise(x-1, y  , z+1) + Noise(x+1, y  , z+1) + Noise(x  , y-1, z+1) + Noise(x  , y+1, z+1)) / 8.0f;
			center  =  Noise(x  , y  , z+1) / 4.0f;
			averageZP1     = corners+sides+center;

			return((averageZM1 / 4.0f)+(averageZ / 2.0f)+(averageZP1 / 4.0f));
		}
        
		public float InterpolateNoise(float x)
		{
			int integerX = (int)x;
			float fracX  = x - (float)integerX;

			float v1 = SmoothNoise(integerX);
			float v2 = SmoothNoise(integerX + 1);

			return m_interpolate(v1, v2, fracX);
		}

		public float InterpolateNoise(float x, float y)
		{
			int integerX = (int)x;
			float fracX  = x - (float)integerX;

			int integerY = (int)y;
			float fracY  = y - (float)integerY;

			float v1 = SmoothNoise(integerX   , integerY);
			float v2 = SmoothNoise(integerX+1 , integerY);
			float v3 = SmoothNoise(integerX   , integerY+1);
			float v4 = SmoothNoise(integerX+1 , integerY+1);

			float i1 = m_interpolate(v1, v2, fracX);
			float i2 = m_interpolate(v3, v4, fracX);

			return m_interpolate(i1, i2, fracY);

		}

		public float InterpolateNoise(float x, float y, float z)
		{
			int integerX = (int)x;
			float fracX  = x - (float)integerX;

			int integerY = (int)y;
			float fracY  = y - (float)integerY;
			
			int integerZ = (int)z;
			float fracZ  = z - (float)integerZ;	

			float v1 = SmoothNoise(integerX   , integerY   , integerZ);
			float v2 = SmoothNoise(integerX+1 , integerY   , integerZ);
			float v3 = SmoothNoise(integerX   , integerY+1 , integerZ);
			float v4 = SmoothNoise(integerX+1 , integerY+1 , integerZ);
			float v5 = SmoothNoise(integerX   , integerY   , integerZ+1);
			float v6 = SmoothNoise(integerX+1 , integerY   , integerZ+1);
			float v7 = SmoothNoise(integerX   , integerY+1 , integerZ+1);
			float v8 = SmoothNoise(integerX+1 , integerY+1 , integerZ+1);

			float i1 = m_interpolate(v1, v2, fracX);
			float i2 = m_interpolate(v3, v4, fracX);
			float i3 = m_interpolate(v5, v6, fracX);
			float i4 = m_interpolate(v7, v8, fracX);

			float i5 = m_interpolate(i1, i2, fracY);
			float i6 = m_interpolate(i3, i4, fracY);

			return m_interpolate(i5, i6, fracZ);
		}
        
		public float PerlinNoise1F(float x, float amplitude, float frequencyX)
		{
			return InterpolateNoise(x*frequencyX) * amplitude;
		}

		public float PerlinNoise2F(float x, float y, float amplitude, float frequencyX, float frequencyY)
		{
			return InterpolateNoise(x*frequencyX, y*frequencyY ) * amplitude;
		}

		public float PerlinNoise3F(float x, float y, float z, float amplitude, float frequencyX, float frequencyY, float frequencyZ)
		{
			return InterpolateNoise(x*frequencyX, y*frequencyY, z*frequencyZ) * amplitude;
        }
        
        /// <summary>
        /// Generates the 1d noise and stores it in the lookup table(for faster processing).
        /// </summary>
        /// <param name="maxX">max x value (generates noise from 0 to uiMaxX)</param>
        void GenerateLookup(int maxX)
		{
			m_maxX = maxX;
			m_lookup = new float[maxX];

			for (int x = 0; x < maxX; x++)
				m_lookup[x] = Noise(x);
        }
	
        /// <summary>
        /// Generates the 2d noise and stores it in the lookup table (for faster processing).
        /// </summary>
        /// <param name="maxX">max x value (generates noise from 0 to uiMaxX)</param>
        /// <param name="maxY">max y value (generates noise from 0 to uiMaxY)</param>
        void GenerateLookup(int maxX, int maxY)
		{
			m_maxX = maxX;
			m_maxY = maxY;
			m_lookup = new float[maxX * maxY];

			int offsetY = 0;

			for (int y = 0; y < maxY; y++)
			{
				for (int x = 0; x < maxX; x++)
					m_lookup[x + offsetY] = Noise(x,y);

				offsetY += maxX;
			}
		}

        /// <summary>
        /// Generates the 3d noise and stores it in the lookup table (for faster processing).
        /// </summary>
        /// <param name="maxX">max x value (generates noise from 0 to uiMaxX)</param>
        /// <param name="maxY">max y value (generates noise from 0 to uiMaxY)</param>
        /// <param name="maxZ">max z value (generates noise from 0 to uiMaxZ)</param>
        void GenerateLookup(int maxX, int maxY, int maxZ)
		{
			m_maxX = maxX;
			m_maxY = maxY;
			// m_maxZ = maxZ;
			m_lookup = new float[maxX * maxY * maxZ];

			int offsetY = 0;
			int offsetZ = 0;
			int stepZ = maxX * maxY;

			for (int z = 0; z < maxZ; z++)
			{
				for (int y = 0; y < maxY; y++)
				{
					for (int x = 0; x < maxX; x++)
						m_lookup[x + offsetY + offsetZ] = Noise(x,y,z);

					offsetY += maxX;
				}

				offsetY = 0;
				offsetZ += stepZ;
			}
		}
	}

    public static class ImprovedNoise
    {
        public static double Noise(double x, double y, double z)
        {
            int xc = (int)Fun.Floor(x) & 0xff,          // Find unit cube that
                yc = (int)Fun.Floor(y) & 0xff,          // contains point.
                zc = (int)Fun.Floor(z) & 0xff;
            x -= Fun.Floor(x);                          // Find relative x,y,z
            y -= Fun.Floor(y);                          // of point in cube.
            z -= Fun.Floor(z);
            double u = Fade(x), v = Fade(y), w = Fade(z);
                    
            int a = p[xc  ]+yc, aa = p[a]+zc, ab = p[a+1]+zc, // hash of cube 
                b = p[xc+1]+yc, ba = p[b]+zc, bb = p[b+1]+zc; // corners

            return Lerp(w, Lerp(v, Lerp(u, Grad(p[aa  ], x  , y  , z  ),
                                           Grad(p[ba  ], x-1, y  , z  )),
                                   Lerp(u, Grad(p[ab  ], x  , y-1, z  ),
                                           Grad(p[bb  ], x-1, y-1, z  ))),
                           Lerp(v, Lerp(u, Grad(p[aa+1], x  , y  , z-1),
                                           Grad(p[ba+1], x-1, y  , z-1)),
                                   Lerp(u, Grad(p[ab+1], x  , y-1, z-1),
                                           Grad(p[bb+1], x-1, y-1, z-1))));
        }

        static double Fade(double t) => t * t * t * (t * (t * 6 - 15) + 10);

        static double Lerp(double t, double a, double b) => a + t * (b - a);

        static double Grad(int hash, double x, double y, double z)
        {
            int h = hash & 0xf;             // Convert lo 4 bits of hash code
            double  u = h < 8 ? x : y,      // into 12 gradient directions.
                    v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        static readonly int[] permutation = {
            151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,
            30,69,142,8,99,37,240,21,10,23,190,6,148,247,120,234,75, 0,26,197,
            62,94,252,219,203,117,35, 11,32,57,177,33,88,237,149,56,87,174,20,
            125,136,171,168,68,175,74,165,71,134,139,48,27,166,77,146,158,231,
            83,111,229,122,60,211,133,230,220,105,92, 41,55,46,245,40,244,102,
            143,54,65,25,63,161, 1,216,80,73,209,76,132,187,208,89,18,169,200,
            196,135,130,116,188,159, 86,164,100,109,198,173,186, 3,64, 52,217,
            226,250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,
            47, 16,58,17,182,189, 28,42,223,183,170,213,119,248,152, 2,44,154,
            163,70,221,153,101,155,167,43,172, 9,129, 22,39,253,19,98,108,110,
            79,113,224,232,178,185,112,104,218,246,97,228,251, 34,242,193,238,
            210,144, 12,191,179,162,241,81,51,145,235,249, 14,239,107, 49,192,
            214,31,181,199,106,157,184,84,204,176,115,121,50,45,127,4,150,254,
            138,236,205,93,222,114,67,29, 24,72,243,141,128,195,78, 66,215,61,
            156,180
        };

        static readonly int[] p;

        static ImprovedNoise()
        {
            p = new int[512];
            for (int i = 0; i < 256 ; i++) p[256+i] = p[i] = permutation[i];
        }
    }
}

