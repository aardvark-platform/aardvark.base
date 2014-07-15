using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class TensorArrayExtensions
    {
        #region Vector Extensions

        public static double DotProduct(this Vector<double> v0, Vector<double> v1)
        {
            double result = 0.0;
            double[] a0 = v0.Data, a1 = v1.Data;
            for (long i0 = v0.Origin, i1 = v1.Origin, e0 = i0 + v0.S, d0 = v0.D, d1 = v1.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result += a0[i0] * a1[i1];
            return result;
        }

        public static double NormSquared(this Vector<double> v)
        {
            double result = 0.0;
            double[] a = v.Data;
            for (long i = v.Origin, e = i + v.S, d = v.D; i != e; i += d)
                result += a[i] * a[i];
            return result;
        }

        #endregion

        #region Bit Reversed Index

        public static void BitReverseIndex<T>(
                this T[] v, long start, long size)
        {
            if (size == 0 || !size.IsPowerOfTwo())
                throw new ArgumentException(
                        "specified size not a power of 2");
            long hiBit = size >> 1, hiBit1 = hiBit >> 1, hiBit2 = hiBit1 >> 1;
            long index = 0, rIndex = 0;

            while (index < size)
            {
                if (index < rIndex)
                {
                    T help = v[start + rIndex];
                    v[start + rIndex] = v[start + index];
                    v[start + index] = help;
                }
                index++;
                rIndex ^= hiBit;
                if ((index & 1) == 0)
                {
                    rIndex ^= hiBit1;
                    if ((index & 2) == 0)
                    {
                        long bit = 2, rBit = hiBit2;
                        do { rIndex ^= rBit; bit <<= 1; rBit >>= 1; } while ((index & bit) == 0);                            
                    }
                }
            }

        }

        public static T[] BitReversedIndexCopy<T>(
                this T[] v, long start, long size)
        {
            if (size == 0 || !size.IsPowerOfTwo())
                throw new ArgumentException(
                        "specified size not a power of 2");

            var r = new T[size];
            long hiBit = size >> 1, hiBit1 = hiBit >> 1, hiBit2 = hiBit1 >> 1;
            long index = 0, rIndex = 0;

            while (index < size)
            {
                r[rIndex] = v[start + index];
                index++;
                rIndex ^= hiBit;
                if ((index & 1) == 0)
                {
                    rIndex ^= hiBit1;
                    if ((index & 2) == 0)
                    {
                        long bit = 2, rBit = hiBit2;
                        do { rIndex ^= rBit; bit <<= 1; rBit >>= 1; } while ((index & bit) == 0);
                    }
                }
            }
            return r;
        }

        public static void BitReverseIndex<T>(
                this T[] v, long start, long size, long stride)
        {
            if (size == 0 || !size.IsPowerOfTwo())
                throw new ArgumentException(
                        "specified size not a power of 2");
            long hiBit = size >> 1, hiBit1 = hiBit >> 1, hiBit2 = hiBit1 >> 1;
            long index = 0, rIndex = 0, position = start;

            while (index < size)
            {
                if (index < rIndex) Fun.Swap(ref v[position], ref v[start + rIndex * stride]);
                position += stride;
                index++;
                rIndex ^= hiBit;
                if ((index & 1) == 0)
                {
                    rIndex ^= hiBit1;
                    if ((index & 2) == 0)
                    {
                        long bit = 2, rBit = hiBit2;
                        do { rIndex ^= rBit; bit <<= 1; rBit >>= 1; } while ((index & bit) == 0);
                    }
                }
            }
        }

        #endregion

        #region Fast Hartley Transform

        public static void FastHartleyTransform(
                this double[] v, long start, long size)
        {
            v.BitReverseIndex(start, size);

            /* ---------------------------------------------------------------
	            The transforming part of the function.  It assumes that all
                indices are already bit-reversed.  For successive evaluation
                of the trigonometric functions the following recurrence is
                used:
                
		            a = 2 sin(square(d/2))
		            b  = sin d
            	
		            cos(t + d) = cos t - [ a * cos t + b sin t ]
		            sin(t + d) = sin t - [ a * sin t - b cos t ]

	            The algorithm is based on the following recursion:

		            H[f] = Heven[f]
			            + cos(2 PI f / n) Hodd[f]
			            + sin(2 PI f / n) Hodd[n-f]	f in [0, n-1]

		            Heven[n/2 + g] = Heven[g]
		            Hodd [n/2 + g] = Hodd [g]		g in [0, n/2-1]
            --------------------------------------------------------------- */
            long end = start + size;
            for (long len = 1, oldLen = 0, newLen = 0;
                 len < size;
                 oldLen = len, len = newLen, newLen = 2 * len)
            {
                long i, j;
                double hi, hj;

                for (i = start; i < end; i += newLen)  // for all blocks
                {
                    j = i + len;				        // special case:
                    hi = v[i]; hj = v[j];
                    v[i] = hi + hj;				        // f = 0
                    v[j] = hi - hj;				        // f = PI
                }

                if (len < 2) continue;

                for (i = start + oldLen; i < end; i += newLen)
                {
                    j = i + len;				        // special case:
                    hi = v[i]; hj = v[j];
                    v[i] = hi + hj;				        // f = PI/2
                    v[j] = hi - hj;				        // f = 3 * PI/2
                }

                if (len < 4) continue;

                double d = Constant.PiTimesTwo / newLen;
                double a = (d * 0.5).Sin().Square() * 2.0;  // init trig.
                double b = d.Sin();				        // recurrence
                double ct = 1.0;
                double st = 0.0;

                /* -----------------------------------------------------------
                    To increase the locality of references, the following two
                    loops could be exchanged, but then the trigonometric
                    functions need to be tabulated.
                ----------------------------------------------------------- */
                for (long f = 1; f < oldLen; f++)       // for all freqs in
                {						                //   the first quad
                    double one = a * ct + b * st;	    // trig. recurrence
                    double two = a * st - b * ct;
                    ct -= one;				            //   cos (t + d)
                    st -= two;				            //   sin (t + d)

                    for (i = start + f, j = start + len - f; // for all blocks
                         i < end;
                         i += newLen, j += newLen)
                    {
                        long k = i + len;
                        long l = j + len;

                        one = ct * v[k] + st * v[l];
                        two = ct * v[l] - st * v[k];
                        hi = v[i]; hj = v[j];
                        v[i] = hi + one; v[k] = hi - one;   // all four quads
                        v[j] = hj - two; v[l] = hj + two;	// (i,j,k,l)
                    }
                }
            }
        }

        public static double[] UntestedFastHartleyTransformedCopy(
                this double[] v, long start, long size)
        {
            if (size < 2) return v.Copy(start, size);
            var h = v.BitReversedIndexCopy(start, size);
            h.FastHartleyTransformRaw(start, size);
            return h;
        }

        private static void FastHartleyTransformRaw(
                this double[] v, long start, long size)
        {
            if (size < 2) return;
            /* ---------------------------------------------------------------
	            The transforming part of the function.  It assumes that all
                indices are already bit-reversed.  For successive evaluation
                of the trigonometric functions the following recurrence is
                used:
                
		            a = 2 sin(square(d/2))
		            b  = sin d
            	
		            cos(t + d) = cos t - [ a * cos t + b sin t ]
		            sin(t + d) = sin t - [ a * sin t - b cos t ]

	            The algorithm is based on the following recursion:

		            H[f] = Heven[f]
			            + cos(2 PI f / n) Hodd[f]
			            + sin(2 PI f / n) Hodd[n-f]	f in [0, n-1]

		            Heven[n/2 + g] = Heven[g]
		            Hodd [n/2 + g] = Hodd [g]		g in [0, n/2-1]
            --------------------------------------------------------------- */
            long end = start + size;

            for (long i = start; i < end; i += 2)
            {
                double h0 = v[i], h1 = v[i + 1];
                v[i] = h0 + h1; v[i + 1] = h0 - h1;     // f = 0, PI
            }
            if (size < 4) return;

            for (long i = start; i < end; i += 4)
            {
                double h0 = v[i], h2 = v[i + 2];
                v[i] = h0 + h2; v[i + 2] = h0 - h2;     // f = 0, PI
                double h1 = v[i + 1], h3 = v[i + 3];
                v[i + 1] = h1 + h3; v[i + 3] = h1 - h3; // f = PI/2, 3 * PI/2
            }
            if (size < 8) return;

            for (long i = start; i < end; i += 8)
            {
                double h0 = v[i], h4 = v[i + 4];
                v[i] = h0 + h4; v[i + 4] = h0 - h4;     // f = 0, PI
                double one = Constant.Sqrt2Half * (v[i + 5] + v[i + 7]);
                double two = Constant.Sqrt2Half * (v[i + 7] - v[i + 5]);
                double h1 = v[i + 1], h3 = v[i + 3];
                v[i + 1] = h1 + one; v[i + 5] = h1 - one;
                v[i + 3] = h3 - two; v[i + 7] = h3 + two;
                double h2 = v[i + 2], h6 = v[i + 6];
                v[i + 2] = h2 + h6; v[i + 6] = h2 - h6; // f = PI/2, 3 * PI/2
            }
            if (size < 16) return;

            var sTable = new double[size / 4];
            var cTable = new double[size / 4];

            for (long len = 8, lenDiv2 = 4, lenMul2 = 16;
                 len < size;
                 lenDiv2 = len, len = lenMul2, lenMul2 = 2 * len)
            {
                double d = Constant.PiTimesTwo / lenMul2;
                double a = Fun.Sin(d * 0.5).Square() * 2.0;  // init trig. recurrence
                double b = Fun.Sin(d);
                double ct = 1.0, st = 0.0;

                for (long f = 1; f < lenDiv2; f++)  // all freqs in the first quadrant
                {
                    double dct = a * ct + b * st, dst = a * st - b * ct;    // trig. rec
                    ct -= dct; st -= dst;           // cos (t + d), sin (t + d)
                    cTable[f] = ct; sTable[f] = st;
                }

                for (long i0 = start; i0 < end; i0 += lenMul2)
                {
                    long i4 = i0 + len;
                    double h0 = v[i0], h4 = v[i4];
                    v[i0] = h0 + h4; v[i4] = h0 - h4;   // f = 0, PI

                    for (long f = 1; f < lenDiv2; f++)  // all freqs in the first quadrant
                    {
                        long i1 = i0 + f, i3 = i0 + len - f;
                        long i5 = i1 + len, i7 = i3 + len;
                        double one = cTable[f] * v[i5] + sTable[f] * v[i7];
                        double two = cTable[f] * v[i7] - sTable[f] * v[i5];
                        double h1 = v[i1], h3 = v[i3];
                        v[i1] = h1 + one; v[i5] = h1 - one; // all four quadrants
                        v[i3] = h3 - two; v[i7] = h3 + two;	// 
                    }

                    long i2 = i0 + lenDiv2, i6 = i4 + lenDiv2;
                    double h2 = v[i2], h6 = v[i6];
                    v[i2] = h2 + h6; v[i6] = h2 - h6;       // f = PI/2, 3 * PI/2
                }
            }
        }

        public static void FastHartleyTransform(
	            this double[] v, long start, long size, long stride)
        {
            v.BitReverseIndex(start, size, stride);

            long strideSize = stride * size;
            long end = start + strideSize;

            for (long len = stride, oldLen = 0, newLen = 0;
                 len != strideSize;
                 oldLen = len, len = newLen, newLen = 2 * len)
            {
	            long i, j;
	            double hi, hj;

                for (i = start; i != end; i += newLen)		// for all blocks
	            {
	                j = i + len;				            // special case:
	                hi = v[i]; hj = v[j];
	                v[i] = hi + hj;				            // f = 0
	                v[j] = hi - hj;				            // f = PI
	            }
	            if (len < 2 * stride) continue;

                for (i = start + oldLen; i != end + oldLen; i += newLen)
	            {
	                j = i + len;				            // special case:
	                hi = v[i]; hj = v[j];
	                v[i] = hi + hj;				            // f = PI/2
	                v[j] = hi - hj;				            // f = 3 * PI/2
	            }

            	if (len < 4 * stride) continue;

                double d = Constant.PiTimesTwo / newLen;
                double a = (d * 0.5).Sin().Square() * 2.0;  // init trig.
	            double b = d.Sin();				            //  recurrence
   	            double ct = 1.0, st = 0.0;

                for (long f = stride; f != oldLen; f += stride) // all freqs in the first quad
	            {
		            double dct = a * ct + b * st, dst = a * st - b * ct;    // trig. recurrence
		            ct -= dct; st -= dst;			        //  cos (t + d), sin (t + d)

                    for (i = start + f, j = start + len - f; // for all blocks
                         i != end + f;
                         i += newLen, j += newLen)
                    {
                        long k = i + len;
                        long l = j + len;
                        double one = ct * v[k] + st * v[l];
                        double two = ct * v[l] - st * v[k];
                        hi = v[i]; hj = v[j];
                        v[i] = hi + one; v[k] = hi - one;	// all four quads
                        v[j] = hj - two; v[l] = hj + two;	// (i,j,k,l)
                    }
	            }
            }
        }

        #endregion
    }
}
