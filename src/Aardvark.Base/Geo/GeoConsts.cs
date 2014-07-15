using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    /// <summary>
    /// Holds data for a geodesic transform from one datum to another. 
    /// </summary>
    public class GeoDatum
    {
        private double m_dx;
        private double m_dy;
        private double m_dz;

        private double m_rx;
        private double m_ry;
        private double m_rz;
        private double m_ds;

        /// <summary>
        /// Constructs geodetic datum.
        /// Deltas and Scaling should be provided in meters.
        /// Rotation has to be given in arc-seconds.
        /// </summary>
        /// <param name="dx">Delta x</param>
        /// <param name="dy">Delta y</param>
        /// <param name="dz">Delta z</param>
        /// <param name="rx">Rotation x</param>
        /// <param name="ry">Rotation y</param>
        /// <param name="rz">Rotation z</param>
        /// <param name="ds">Scaling</param>
        public GeoDatum(
                double dx, double dy, double dz,
                double rx, double ry, double rz,
                double ds )
        {
            m_dx = dx;
            m_dy = dy;
            m_dz = dy;
            m_rx = rx;
            m_ry = ry;
            m_rz = rz;

            m_ds = ds;
        }

        /// <summary>
        /// Delta X
        /// </summary>
        public double dX { get { return m_dx; } }

        /// <summary>
        /// Delta Y
        /// </summary>
        public double dY { get { return m_dy; } }

        /// <summary>
        /// Delta Z
        /// </summary>
        public double dZ { get { return m_dz; } }

        /// <summary>
        /// Rotation X in arc-seconds.
        /// </summary>
        public double rXsec { get { return m_rx; } }

        /// <summary>
        /// Rotation Y in arc-seconds.
        /// </summary>
        public double rYsec { get { return m_ry; } }

        /// <summary>
        /// Rotation Z in arc seconds.
        /// </summary>
        public double rZsec { get { return m_rz; } }

        /// <summary>
        /// Rotation X in radians.
        /// </summary>
        public double rXrad { get { return m_rx * Constant.ArcSecond; } }

        /// <summary>
        /// Rotation Y in radians.
        /// </summary>
        public double rYrad { get { return m_ry * Constant.ArcSecond; } }

        /// <summary>
        /// Rotation Z in radians.
        /// </summary>
        public double rZrad { get { return m_rz * Constant.ArcSecond; } }
        
        /// <summary>
        /// Scaling in meters.
        /// </summary>
        public double dS { get { return m_ds; } }


        /// <summary>
        /// Conversion from WGS84 to Austria NS (MGI 1).
        /// </summary>
        public static readonly GeoDatum Wgs84toAustriaNs1 =
            new GeoDatum(-575.0, -93.0, -466.0, 5.1, 1.6, 5.2, -2.5);
        /// <summary>
        /// Cconversion from WGS84 to Austria NS (MGI 2).
        /// </summary>
        public static readonly GeoDatum Wgs84toAustriaNs2 =
            new GeoDatum(-577.326, -90.129, -463.919, 5.14, 1.47, 5.30, -2.4232);
        /// <summary>
        /// Conversion from Austria NS1 (MGI) to WGS84.
        /// </summary>
        public static readonly GeoDatum AustriaNs1toWgs84 =
            new GeoDatum(575.0, 93.0, 466.0, -5.1, -1.6, -5.2, 2.5);
        /// <summary>
        /// Conversion FROM Austria NS2 (MGI) TO WGS84.
        /// </summary>
        public static readonly GeoDatum AustriaNs2toWgs84 =
            new GeoDatum(577.326, 90.129, 463.919, -5.14, -1.47, -5.30, 2.4232);
    }

    /// <summary>
    /// This struct provides properties for a particular World-Ellipsoid. 
    /// It is defined by ist two radii: "a" and "b".
    /// </summary>
    public struct GeoEllipsoid
    {
        public readonly double A;
        public readonly double B;
        public readonly double F;

        /// <summary>
        /// Construct a geo-ellipsoid from semi-major axis a (bigger radius),
        /// semi-minor axis b (smaller radius) and flattening f. Since only
        /// two of these are necessary for specification, use the static
        /// functions <see cref="FromAB"/> and <see cref="FromAF"/> to construct
        /// ellipsoids given their defining parameters.
        /// </summary>
        public GeoEllipsoid(double a, double b, double f)
        {
            A = a; B = b; F = f;
        }

        public static GeoEllipsoid FromAB(double a, double b)
        {
            return new GeoEllipsoid(a, b, (a - b) / a);
        }

        public static GeoEllipsoid FromAF(double a, double f)
        {
            return new GeoEllipsoid(a, a * (1.0 - f), f);
        }

        /// <summary>
        /// Returns the inverse of the flattening: 1/f.
        /// </summary>
        public double InvF { get { return (1.0 / F); } }

        /// <summary>
        /// Returns the first eccentricity squared: e^2 = (a^2-b^2)/a^2.
        /// </summary>
        public double EQ { get { return (A * A - B * B) / (A * A); } }//{ return F * (2.0 - F); } }

        /// <summary>
        /// Returns the second eccentricity squared: e2^2 = (a^2-b^2)/b^2.
        /// </summary>
        public double E2Q { get { return (A * A - B * B) / (B * B); } }

        public static readonly GeoEllipsoid Airy1830 = FromAB(6377563.396, 6356256.909);
        public static readonly GeoEllipsoid Bessel1841 = FromAB(6377397.15508, 6356078.96290);
        public static readonly GeoEllipsoid Grs80 = FromAF(6378137.0, 1.0 / 298.257222101);
        public static readonly GeoEllipsoid International1924 = FromAF(6378388.0, 1.0 / 297.0);
        public static readonly GeoEllipsoid Clarke1880 = FromAF(6378249.145, 1.0 / 293.465);
        public static readonly GeoEllipsoid Wgs84 = FromAF(6378137.0, 1.0 / 298.257223563);
    }

    /// <summary>
    /// Static class. Provides constants for Geodesic Conversions.
    /// </summary>
    public static class GeoConstant
    {
        /// <summary>
        /// Returns the M28 meridian used in austrian Gauss-Krueger System (MGI) -> GK-reference meridian for West-Austria.
        /// (28 deg from Ferro = 10 deg + 20 min from Greenwich).
        /// </summary>
        public const double AustriaM28 = 10 + 1 / 3.0;
        /// <summary>
        /// Returns the M31 meridian used in austrian Gauss-Krueger System (MGI) -> GK-reference meridian for Mid-Austria.
        /// (31 deg from Ferro = 13 deg + 20 min from Greenwich).
        /// </summary>
        public const double AustriaM31 = 13 + 1 / 3.0;
        /// <summary>
        /// Returns the M34 meridian used in austrian Gauss-Krueger System (MGI) -> GK-reference meridian for East-Austria.
        /// (34 deg from Ferro = 16 deg + 20 min from Greenwich).
        /// </summary>
        public const double AustriaM34 = 16 + 1 / 3.0;
        
    }
}
