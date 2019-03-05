using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    public static class DateTimeExtensions
    {
        public static double ComputeJulianDay(this DateTime date)
        {
            var y = date.Year;
            var m = date.Month;
            var d = date.Day;

            while (m < 0)
            {
                m += 12; y--;
            }

            while (m > 12)
            {
                m -= 12; y++;
            }

            if (m < 2)
            {
                m = m + 12;
                y = y - 1;
            }

            int c = 2 - (int)System.Math.Floor((double)y / 100)
                      + (int)System.Math.Floor((double)y / 400);

            return (int)System.Math.Floor(1461.0 * (y + 4716.0) / 4.0) +
                    (int)System.Math.Floor(153.0 * (m + 1.0) / 5.0) + d +
                    c - 1524.5;
        }

        public static double ComputeGregorianDay(this DateTime date)
        {
            var J = date.ComputeJulianDay();

            int p = (int)System.Math.Floor(J + 0.5);
            int s1 = p + 68569;
            int n = (int)System.Math.Floor((double)4 * s1 / 146097);
            int s2 = s1 - (int)System.Math.Floor(((double)146097 * n + 3) / 4);
            int i = (int)System.Math.Floor((double)4000 * (s2 + 1) / 1461001);
            int s3 = s2 - (int)System.Math.Floor((double)1461 * i / 4) + 31;
            int q = (int)System.Math.Floor((double)80 * s3 / 2447);
            int e = s3 - (int)System.Math.Floor((double)2447 * q / 80);
            // int s4 = (int)System.Math.Floor((double)q / 11);
            // int m = q + 2 - 12 * s4;
            // int y = 100 * (n - 49) + i + s4;
            double d = e + J - p + 0.5;
            return d;
        }
    }
}
