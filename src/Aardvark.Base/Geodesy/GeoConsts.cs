/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
#pragma warning disable IDE1006 // Naming Styles

namespace Aardvark.Base;

/// <summary>
/// Holds data for a geodesic transform from one datum to another. 
/// </summary>
/// <remarks>
/// Constructs geodetic datum.
/// Deltas and Scaling should be provided in meters.
/// Rotation has to be given in arc-seconds.
/// </remarks>
/// <param name="dx">Delta x</param>
/// <param name="dy">Delta y</param>
/// <param name="dz">Delta z</param>
/// <param name="rx">Rotation x</param>
/// <param name="ry">Rotation y</param>
/// <param name="rz">Rotation z</param>
/// <param name="ds">Scaling</param>
public class GeoDatum(
    double dx, double dy, double dz,
    double rx, double ry, double rz,
    double ds
    )
{
    private readonly double m_dx = dx;
    private readonly double m_dy = dy;
    private readonly double m_dz = dz;

    private readonly double m_rx = rx;
    private readonly double m_ry = ry;
    private readonly double m_rz = rz;
    private readonly double m_ds = ds;

    /// <summary>
    /// Delta X
    /// </summary>
    public double dX  => m_dx;

    /// <summary>
    /// Delta Y
    /// </summary>
    public double dY => m_dy;

    /// <summary>
    /// Delta Z
    /// </summary>
    public double dZ => m_dz;

    /// <summary>
    /// Rotation X in arc-seconds.
    /// </summary>
    public double rXsec => m_rx;

    /// <summary>
    /// Rotation Y in arc-seconds.
    /// </summary>
    public double rYsec => m_ry;

    /// <summary>
    /// Rotation Z in arc seconds.
    /// </summary>
    public double rZsec => m_rz;

    /// <summary>
    /// Rotation X in radians.
    /// </summary>
    public double rXrad => m_rx * Constant.ArcSecond;

    /// <summary>
    /// Rotation Y in radians.
    /// </summary>
    public double rYrad => m_ry * Constant.ArcSecond;

    /// <summary>
    /// Rotation Z in radians.
    /// </summary>
    public double rZrad => m_rz * Constant.ArcSecond;

    /// <summary>
    /// Scaling in meters.
    /// </summary>
    public double dS => m_ds;


    /// <summary>
    /// Conversion from WGS84 to Austria NS (MGI 1).
    /// </summary>
    public static readonly GeoDatum Wgs84toAustriaNs1 =
        new(-575.0, -93.0, -466.0, 5.1, 1.6, 5.2, -2.5);

    /// <summary>
    /// Cconversion from WGS84 to Austria NS (MGI 2).
    /// </summary>
    public static readonly GeoDatum Wgs84toAustriaNs2 =
        new(-577.326, -90.129, -463.919, 5.14, 1.47, 5.30, -2.4232);

    /// <summary>
    /// Conversion from Austria NS1 (MGI) to WGS84.
    /// </summary>
    public static readonly GeoDatum AustriaNs1toWgs84 =
        new(575.0, 93.0, 466.0, -5.1, -1.6, -5.2, 2.5);

    /// <summary>
    /// Conversion FROM Austria NS2 (MGI) TO WGS84.
    /// </summary>
    public static readonly GeoDatum AustriaNs2toWgs84 =
        new(577.326, 90.129, 463.919, -5.14, -1.47, -5.30, 2.4232);
}

/// <summary>
/// This struct provides properties for a particular World-Ellipsoid. 
/// It is defined by ist two radii: "a" and "b".
/// </summary>
/// <remarks>
/// Construct a geo-ellipsoid from semi-major axis a (bigger radius),
/// semi-minor axis b (smaller radius) and flattening f. Since only
/// two of these are necessary for specification, use the static
/// functions <see cref="FromAB"/> and <see cref="FromAF"/> to construct
/// ellipsoids given their defining parameters.
/// </remarks>
public readonly struct GeoEllipsoid(double a, double b, double f)
{
    public readonly double A = a;
    public readonly double B = b;
    public readonly double F = f;

    public static GeoEllipsoid FromAB(double a, double b)
        => new(a, b, (a - b) / a);

    public static GeoEllipsoid FromAF(double a, double f)
        => new(a, a * (1.0 - f), f);

    /// <summary>
    /// Returns the inverse of the flattening: 1/f.
    /// </summary>
    public double InvF => (1.0 / F);

    /// <summary>
    /// Returns the first eccentricity squared: e^2 = (a^2-b^2)/a^2.
    /// </summary>
    public double EQ => (A * A - B * B) / (A * A);

    /// <summary>
    /// Returns the second eccentricity squared: e2^2 = (a^2-b^2)/b^2.
    /// </summary>
    public double E2Q => (A * A - B * B) / (B * B);

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
