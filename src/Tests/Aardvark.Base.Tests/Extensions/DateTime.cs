using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Tests.Extensions
{
    [TestFixture]
    public class DateTimeTests : TestSuite
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static double ComputeJulianDayInt(DateTime date)
        {
            var y = date.Year;
            var m = date.Month;
            var d = date.Day;

            var jd = ComputeJulianDay(y, m, d) - 0.5; // julian day with fraction 0 represents 12:00 noon UT -> subtract 0.5 to start at 00:00

            return jd + date.TimeOfDay.TotalDays; 
        }

        static int ComputeJulianDay(int y, int m, int d)
        {
            if (m < 3)
            {
                m += 13;
                y--;
            }
            else
            {
                m += 1;
            }

            int c = 2 - y / 100 + y / 400;

            return (1461 * (y + 4716) / 4) + (153 * m / 5) + d + c - 1524; // NOTE: subtract 1524.5 to get proper floating point julian day
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static double ComputeJulianDayNREL(DateTime date)
        {
            // algorithm from NREL spa.c
            // https://www.nrel.gov/docs/fy08osti/34302.pdf

            var y = date.Year;
            var m = date.Month; // between 1 and 12
            var d = date.Day;
            var dt = date.TimeOfDay.TotalDays;

            // in case of feb or jan, overflow months m => 13 or 14 (y = y - 1)
            if (m < 3)
            {
                m = m + 12;
                y--;
            }
            
            // 30.6001 is the days per month approximation (365-31-28)/10 (ie av month days without jan & feb)
            var jd = Fun.Floor(365.25 * (y + 4716.0)) + Fun.Floor(30.6001 * (m + 1)) + d + dt - 1524.5;
            //if (jd > 2299160.0) // for gregorian calendar ?
            {
                var a = y / 100;
                jd += 2 - a + (a / 4);
            }

            // http://radixpro.com/a4a-start/julian-day-and-julian-day-number/

            return jd;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static double ComputeJulianDayInt2(DateTime date)
        {
            var y = date.Year;
            var m = date.Month;
            var d = date.Day;
            var dt = date.TimeOfDay.TotalDays;

            if (m < 3)
            {
                m = m + 12;
                y = y - 1;
            }

            int jd = d + (153 * m - 457) / 5 + 365 * y + (y / 4) - (y / 100) + (y / 400);

            // JD 1721119 is about year 0 ?
            return jd + 1721118.5 + dt;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static double ComputeJulianDayMeeus(DateTime date)
        {
            var y = date.Year;
            var m = date.Month; // between 1 and 12
            var d = date.Day;
            var dt = date.TimeOfDay.TotalDays;

            // http://mathforum.org/library/drmath/view/62338.html
            //The following algorithm is adapted from Jean Meeus' "Astronomical 
            //Formulae for Calculators." 

            //Given a year YYYY, a month MM, and a day D, if the month is January
            //or February(M = 1 or 2), replace let y = YYYY - 1 and let m = M + 13.
            //Otherwise, let y = YYYY and m = M + 1.

            //Compute A = int(y / 100), and let B = 2 - A + int(A / 4).

            //Then the Julian Day number JD of this date is
            //JD = int(365.25 * y) + int(30.6001 * m) + B + D + 1720995.

            if (m < 3)
            {
                m = m + 13;
                y--;
            }
            else
            {
                m += 1;
            }

            // 30.6001 is the days per month approximation (365-31-28)/10 (ie av month days without jan & feb)
            var A = y / 100;
            var B = 2 - A + A / 4;
            var jd = Fun.Floor(365.25 * y) + Fun.Floor(30.6001 * m) + B + d + dt + 1720995;

            return jd - 0.5;
        }

        static double ComputeGregorianDay(DateTime date)
        {
            var J = date.ComputeJulianDay();

            // the following calculates the day of the month of a JD -> date.Day + date.TimeOfDay.TotalDays 1?
            int p = (int)Fun.Floor(J + 0.5);
            int s1 = p + 68569;
            int n = (int)Fun.Floor(4 * s1 / (double)146097);
            int s2 = s1 - (int)Fun.Floor(((double)146097 * n + 3) / 4);
            int i = (int)Fun.Floor((double)4000 * (s2 + 1) / 1461001);
            int s3 = s2 - (int)Fun.Floor((double)1461 * i / 4) + 31;
            int q = (int)Fun.Floor((double)80 * s3 / 2447);
            int e = s3 - (int)Fun.Floor((double)2447 * q / 80);
            // int s4 = (int)System.Math.Floor((double)q / 11);
            // int m = q + 2 - 12 * s4;
            // int y = 100 * (n - 49) + i + s4;
            double d = e + J - p + 0.5;
            return d;
        }

        [Test]
        public static void JulianDay()
        {
            var rnd = new RandomSystem(123);

            TestDate(new DateTime(2019, 3, 5, 12, 0, 0, 0));
            TestDate(new DateTime(2019, 3, 5, 12, 1, 0, 0));
            TestDate(new DateTime(2019, 3, 5, 11, 59, 0, 0));

            // https://en.wikipedia.org/wiki/Julian_day
            // 00:30:00.0 UT January 1, 2013, is 2 456 293.520 833
            var test2 = new DateTime(2013, 1, 1, 0, 30, 0);
            TestDate(test2);

            for (int i = 0; i < 10000000; i++)
            {
                var lg = rnd.UniformLong();
                if (lg > DateTime.MaxValue.Ticks || lg < DateTime.MinValue.Ticks)
                    continue;

                var dt = new DateTime(lg);

                TestDate(dt);
            }

            Report.Line("Times: (NOTE benchmark meaningless, measurement of first algorithm will always have x3 time)");
            Report.Line("GetDatePart: {0}ms", sw0.Elapsed.TotalMilliseconds);
            Report.Line("ComputeJulianDayNREL: {0}ms", sw1.Elapsed.TotalMilliseconds);
            Report.Line("ComputeJulianDayInt2: {0}ms", sw2.Elapsed.TotalMilliseconds);
            Report.Line("ComputeJulianDayMeeus: {0}ms", sw3.Elapsed.TotalMilliseconds);
            Report.Line("ComputeJulianDayInt: {0}ms", sw4.Elapsed.TotalMilliseconds);
            Report.Line("ComputeJulianDayRaw: {0}ms", sw5.Elapsed.TotalMilliseconds);
        }

        static Stopwatch sw0 = new Stopwatch();
        static Stopwatch sw1 = new Stopwatch();
        static Stopwatch sw2 = new Stopwatch();
        static Stopwatch sw3 = new Stopwatch();
        static Stopwatch sw4 = new Stopwatch();
        static Stopwatch sw5 = new Stopwatch();
        static int warpup = 10000;

        static void TestDate(DateTime date)
        {
            warpup--;

            // NOTE: benchmark meaningless, first algorithm will have x3 time
            if (warpup < 0) sw1.Start();
            var jd1 = ComputeJulianDayNREL(date); // has strange condition for gregorian date < year 1582
            if (warpup < 0) sw1.Stop();

            if (warpup < 0) sw2.Start();
            var jd2 = ComputeJulianDayInt2(date);
            if (warpup < 0) sw2.Stop();

            if (warpup < 0) sw3.Start();
            var jd3 = ComputeJulianDayMeeus(date);
            if (warpup < 0) sw3.Stop();

            if (warpup < 0) sw4.Start();
            var jd4 = ComputeJulianDayInt(date);
            if (warpup < 0) sw4.Stop();

            // NOTE: there would be any internal void GetDatePart(out int year, out int month, out int day) hidden in the .net framework
            if (warpup < 0) sw0.Start();
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            if (warpup < 0) sw0.Stop();

            if (warpup < 0) sw5.Start();
            var jd5 = (double)ComputeJulianDay(year, month, day);
            if (warpup < 0) sw5.Stop();

            jd5 += date.TimeOfDay.TotalDays - 0.5;

            var jdAard = date.ComputeJulianDay();

            //Report.Line("Date={0} Julian Day NREL: {1} ALT:{2}", date, (int)jd1, (int)jd2);

            if (!jd1.ApproximateEquals(jd2, 1e-07) || !jd1.ApproximateEquals(jd3, 1e-07) || !jd1.ApproximateEquals(jd4, 1e-07) || !jd1.ApproximateEquals(jdAard, 1e-7))
            {
                Report.Line("Date={0} Julian Day NREL: {1} ALT:{2}", date, (int)jd1, (int)jd2);
                Report.Line("FAIL");
            }

            var gd = ComputeGregorianDay(date);
            var gd1 = date.Day + date.TimeOfDay.TotalDays;

            var gdAard = DateTimeExtensions.GregorianDayOfMonthFromJulianDay(jd1);

            if (!gd.ApproximateEquals(gd1, 1e-5) || !gd.ApproximateEquals(gdAard, 1e-5))
            {
                Report.Line("Date={0} Julian Day: {1} Gregorian Day: {2} {3} {4}", date, jd1, gd, gd1, gdAard);
                Report.Line("FAIL");
            }


            var date2 = DateTimeExtensions.ComputeDateFromJulianDay(jd1);
            var dt = date - date2;
            if ((int)dt.TotalSeconds != 0)
            {
                Report.Line("DateIn={0} Julian Day={1} DateOut={2}", date, jd1, date2);
                Report.Line("FAIL");
            }

            if (year > 1)
            {
                var date3 = DateTimeExtensions.ComputeDateFromJulianDay((int)jd1);
                var tmp = new DateTime(year, month, day, 12, 0, 0); // 12h noon
                if (jd1.Frac() > 0.5)
                    tmp = tmp.AddDays(-1);
                var dt2 = tmp - date3;
                if ((int)dt2.TotalSeconds != 0)
                {
                    Report.Line("DateIn={0} Julian Day={1} DateOut={2}", tmp, jd1, date3);
                    Report.Line("FAIL");
                }
            }
        }
    }
}
