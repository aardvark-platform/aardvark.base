using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    
    public static class RangeExtensions
    {
        /// <summary>
        /// Returns the int sequence 0 ... count-1.
        /// </summary>
        public static IEnumerable<int> Range(this int count)
        {
            for (int i = 0; i < count; i++) yield return i;
        }

        /// <summary>
        /// Returns the long sequence 0 ... count-1.
        /// </summary>
        public static IEnumerable<long> Range(this long count)
        {
            for (long i = 0; i < count; i++) yield return i;
        }

        public static IEnumerable<int> UpTo(this int self, int upToInclusive)
        {
            for (int i = self; i <= upToInclusive; i++) yield return i;
        }
        public static IEnumerable<long> UpTo(this int self, long upToInclusive)
        {
            for (long i = self; i <= upToInclusive; i++) yield return i;
        }
        public static IEnumerable<long> UpTo(this long self, long upToInclusive)
        {
            for (long i = self; i <= upToInclusive; i++) yield return i;
        }
        public static IEnumerable<int> UpTo(this int self, double upToInclusive)
        {
            return UpTo(self, (int)upToInclusive);
        }
        public static IEnumerable<int> UpToExclusive(this int self, int upToExclusive)
        {
            for (int i = self; i < upToExclusive; i++) yield return i;
        }
        public static IEnumerable<int> UpToExclusive(this int self, double upToExclusive)
        {
            return UpToExclusive(self, (int)upToExclusive);
        }

        public static IEnumerable<int> UpTo(this int self, int upToInclusive, int step)
        {
            for (int i = self; i <= upToInclusive; i += step) yield return i;
        }
        public static IEnumerable<int> UpTo(this int self, double upToInclusive, int step)
        {
            return UpTo(self, (int)upToInclusive, step);
        }
        public static IEnumerable<int> UpToExclusive(this int self, int upToExclusive, int step)
        {
            for (int i = self; i < upToExclusive; i += step) yield return i;
        }
        public static IEnumerable<int> UpToExclusive(this int self, double upToExclusive, int step)
        {
            return UpToExclusive(self, (int)upToExclusive, step);
        }

        public static IEnumerable<int> DownTo(this int self, int downToInclusive)
        {
            for (int i = self; i >= downToInclusive; i--) yield return i;
        }
        public static IEnumerable<int> DownTo(this int self, double downToInclusive)
        {
            return DownTo(self, (int)downToInclusive);
        }

        public static IEnumerable<int> DownTo(this int self, int downToInclusive, int step)
        {
            for (int i = self; i >= downToInclusive; i -= step) yield return i;
        }
        public static IEnumerable<int> DownTo(this int self, double downToInclusive, int step)
        {
            return DownTo(self, (int)downToInclusive, step);
        }


        //put here by haaser (Method needed in Intersections.cs: Line2d.Intersects(Line2d)
        //feel free to relocate it if this is a bad place for this Extension
        /// <summary>
        /// Checks if 2 ranges intersect each other with tolerance parameter.
        /// </summary>
        public static bool Intersects(this Range1d r0, Range1d range, double eps, out Range1d result)
        {
            result = Range1d.Invalid;
            if (r0.Min - eps > range.Max) return false;
            else if (r0.Max + eps < range.Min) return false;
            else
            {
                result = new Range1d(System.Math.Max(r0.Min, range.Min), System.Math.Min(r0.Max, range.Max));
                if (result.Size < eps)
                {
                    double center = result.Center;
                    result.Min = center - eps;
                    result.Max = center + eps;
                }
                return true;
            }
        }
    }

}
