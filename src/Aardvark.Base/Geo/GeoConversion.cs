using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Aardvark.Base
{
    public static class Geo
    {
        #region Conversion Methods

        /// <summary>
        /// Returns the XYZ-coordinates from longitude, latitude and height on a given reference ellipsoid.
        /// </summary>
        public static V3d XyzFromLonLatHeight(V3d lonLatHeight, GeoEllipsoid ellipsoid)
        {
            double lam = Conversion.RadiansFromDegrees(lonLatHeight.X);
            double phi = Conversion.RadiansFromDegrees(lonLatHeight.Y);
            double h = lonLatHeight.Z;

            double cos_lam = System.Math.Cos(lam);
            double cos_phi = System.Math.Cos(phi);
            double sin_lam = System.Math.Sin(lam);
            double sin_phi = System.Math.Sin(phi);

            /// eccentricity square
            double eq = ellipsoid.EQ;
    
            /// radius of the curvature in prime vertical
            double Rv = ellipsoid.A / (1.0 - eq * sin_phi * sin_phi).Sqrt();

            V3d result = new V3d();
            result.X = (Rv + h) * cos_phi * cos_lam;
            result.Y = (Rv + h) * cos_phi * sin_lam;
            result.Z = ((1.0 - eq) * Rv + h) * sin_phi;

            return result;
        }

        /// <summary>
        /// Returns the longitude, latitude and height from XYZ-coords on a given world reference ellipsoid. 
        /// </summary>
        /// <param name="xyz">Vector with xyz.</param>
        /// <param name="ellipsoid">GeoEllipsoid from the GeoConsts class.</param>
        /// <returns>Position vector with Lon,Lat,Hei</returns>
        public static V3d LonLatHeightFromXyz(V3d xyz, GeoEllipsoid ellipsoid)
        {
            /// this implementation follows the Bowring Method (85).
            /// It might be extended to the newer Toms Method (99), but is delivers suitable results.

            double a = ellipsoid.A;
            double b = ellipsoid.B;

            // Distance between x and y.
            double W = (xyz.X * xyz.X + xyz.Y * xyz.Y).Sqrt();

            // apprx phi
            double PHI = System.Math.Atan((xyz.Z * a) / (W * b));
            
            double eq = ellipsoid.EQ;
            double e2q = ellipsoid.E2Q;// (eq / (1.0 - eq)).Sqrt();

            double lam = System.Math.Atan(xyz.Y / xyz.X);

            double sinPhi = (PHI).Sin();
            double sinPhiTo3 = sinPhi * sinPhi * sinPhi;

            double cosPhi = (PHI).Cos();
            double cosPhiTo3 = cosPhi * cosPhi * cosPhi;

            double phi = System.Math.Atan((xyz.Z + e2q * b * sinPhiTo3) / (W - eq * a * cosPhiTo3));
            double sinphi = phi.Sin();

            // curvature in the prime vertical
            double Rv = a / (1.0 - eq * sinphi * sinphi).Sqrt();

            double cosphi = phi.Cos();
            double h = W / cosphi - Rv;

            double lat = Conversion.DegreesFromRadians(phi);// phi * 180 / pi;
            double lon = Conversion.DegreesFromRadians(lam); // lam * 180 / pi;
          
            return new V3d(lon, lat, h);
        }

        public const string MapsGoogleComWebServiceKey =
            "ABQIAAAAk4r0d6x0SCDbJHYjUrVJShSz1LOpcG8SAwxdzXiuGjdF4u3iExRpOB4S6dgElxAkq4aQ_QEUad74oQ";

        /// <summary>
        /// Returns WGS84 Longitude/Latitude/Height for given street address,
        /// or null if address not found. Uses maps.google.com webservice.
        /// </summary>
        public static V3d? Wgs84FromStreetAddress(string address)
        {
            var output = "csv";
            var url = @"http://maps.google.com/maps/geo?" 
                        + "q=" + Uri.EscapeDataString(address)
                        + "&output=" + output
                        + "&key=" + MapsGoogleComWebServiceKey;

            var resp = new WebClient().DownloadString(url);
            var tokens = resp.Split(',');
            if (tokens.Length != 4) throw new FormatException();
            if (tokens[0] != "200") return null;
            return new V3d(
                double.Parse(tokens[2], Localization.FormatEnUS),
                double.Parse(tokens[3], Localization.FormatEnUS),
                151.0);

            // sm's key - no more than 50000 queries per day!!!!!!!!!
            // ABQIAAAAk4r0d6x0SCDbJHYjUrVJShSz1LOpcG8SAwxdzXiuGjdF4u3iExRpOB4S6dgElxAkq4aQ_QEUad74oQ        
        }

        /// <summary>
        /// Transforms XYZ coord on one reference ellipsoid to XYZ coords on another
        /// ref ellipsoid. The ellipsoids-difference is defined in a geometric datum
        /// which is passes as the second argument.
        /// </summary>
        public static V3d HelmertsTransformXyzToXyz(V3d xyz, GeoDatum datum)
        {
            // transformation matrix
            var T = new M33d(1.0, datum.rZrad, -datum.rYrad,
                            -datum.rZrad, 1.0, datum.rXrad,
                             datum.rYrad, -datum.rXrad, 1.0);

            // delta in ellipsoid center positions
            var dP = new V3d(datum.dX, datum.dY, datum.dZ);

            // scale factor
            var mu = new Scale3d(datum.dS * 0.000001 + 1.0);

            var result = dP + (mu * T) * xyz;
            return result;
        }

        /// <summary>
        /// Gauss-Krueger projection from a reference ellipsoid datum (Lon/Lat/Height) to local GK-coordinates (in meters).
        /// </summary>
        /// <param name="LonLatHeight">V3d with Lon/Lat/Height.</param>
        public static V3d GaussKruegerEllipsoidToPlane(
            V3d lonLatHeight, GeoEllipsoid ellipsoid, double zeroMeridian
            )
        {
            double phi = Conversion.RadiansFromDegrees(lonLatHeight.Y);
            double lam = Conversion.RadiansFromDegrees(lonLatHeight.X - zeroMeridian);           

            double cosphi = phi.Cos();

            double a = ellipsoid.A;
            double b = ellipsoid.B;
            double n = (a - b) / (a + b);

            //n = (a-b)/(a+b);
            double nTo2 = n * n;
            double nTo3 = nTo2 * n;
            double nTo4 = nTo3 * n;
            double nTo5 = nTo4 * n;

            // arc length approx. polynom coefficients
            double c1 = (a + b) / 2.0 * (1.0 + nTo2 / 4.0 + nTo4 / 64.0);
            double c2 = -(3.0 / 2.0) * n + (9.0 / 16.0) * nTo3 - (3.0 / 32.0) * nTo5;
            double c3 = (15.0 / 16.0) * nTo2 - (15.0 / 32.0) * nTo4;
            double c4 = -(35.0 / 48.0) * nTo3 + (105.0 / 256.0) * nTo5;
            double c5 = (315.0 / 512.0) * nTo4;

            // sine of multiple of phi
            double sin2phi = (2.0 * phi).Sin();
            double sin4phi = (4.0 * phi).Sin();
            double sin6phi = (6.0 * phi).Sin();
            double sin8phi = (8.0 * phi).Sin();

            // arc lenght of the meridan
            double B = c1 * (phi + c2 * sin2phi + c3 * sin4phi + c4 * sin6phi + c5 * sin8phi);

            //% second nummerical eccentrencity
            double e2q = ellipsoid.E2Q;
            
            // cos(phi) to powers
            double cosphiTo2 = cosphi * cosphi;
            double cosphiTo3 = cosphiTo2 * cosphi;
            double cosphiTo4 = cosphiTo3 * cosphi;
            double cosphiTo5 = cosphiTo4 * cosphi;
            double cosphiTo6 = cosphiTo5 * cosphi;
            double cosphiTo7 = cosphiTo6 * cosphi;
            double cosphiTo8 = cosphiTo7 * cosphi;

            //% auxilary quantity 1
            double nuTo2 = e2q * cosphiTo2;

            //%radius of the curvature in prime vertical
            double N = (a * a) / (b * (1.0 + nuTo2).Sqrt());

            // lambda to powers
            double lamTo2 = lam * lam;
            double lamTo3 = lamTo2 * lam;
            double lamTo4 = lamTo3 * lam;
            double lamTo5 = lamTo4 * lam;
            double lamTo6 = lamTo5 * lam;
            double lamTo7 = lamTo6 * lam;
            double lamTo8 = lamTo7 * lam;

            //% auxilary quantity 2
            double t = phi.Tan();

            // t to powers
            double tTo2 = t * t;
            double tTo4 = tTo2 * tTo2;
            double tTo6 = tTo2 * tTo4;

            // nu^2 to powers
            double nuTo4 = nuTo2 * nuTo2;
            //double nuqTo22 = nuq.Pow(22);

            //% x-coord: breite
            double x1 = t / 2.0 * N * cosphiTo2 * lamTo2;
            double x2 = t / 24.0 * N * cosphiTo4 * (5.0 - tTo2 + 9.0 * nuTo2 + 4.0 * nuTo4) * lamTo4;
            double x3 = t / 720.0 * N * cosphiTo6 * (61.0 - 58.0 * tTo2 + tTo4 + 270.0 * nuTo2 - 330.0 * tTo2 * nuTo4) * lamTo6;
            double x4 = t / 403204.0 * N * cosphiTo8 * (1385.0 - 3111.0 * tTo2 + 543.0 * tTo4 - tTo6) * lamTo8;

            double nX = B + x1 +x2 + x3 + x4;
            nX = nX-5000000; // <-- im model 

            // y-coord: laenge
            double y1 = N * cosphi * lam;
            double y2 = N / 6.0 * cosphiTo3 * (1.0 - tTo2 + nuTo2) * lamTo3;
            double y3 = N / 120.0 * cosphiTo5 * (5.0 - 18.0 * tTo2 + tTo4 + 14 * nuTo2 - 58.0 * tTo2 * nuTo2) * lamTo5;
            double y4 = N / 5040.0 * cosphiTo7 * (61.0 - 479.0 * tTo2 + 179.0 * tTo4 - tTo6) * lamTo7;

            double nY = y1 +y2 + y3 + y4;
            //%nY = nY + 500000+L0/3*1000000; %<-- potsdam datum
            return new V3d(nY, nX, lonLatHeight.Z); //<-- Laenge, Breite, Hoehe

        }

        public static V3d GaussKruegerPlaneToEllipsoid(
            V3d rightHeightAlt, GeoEllipsoid ellipsoid, double referenceMeridan
            )
        {
            double a = ellipsoid.A;
            double b = ellipsoid.B;

            double Y = rightHeightAlt.X;
            double X = rightHeightAlt.Y + 5000000;            

            double n = (a - b) / (a + b);
            double nTo2 = n * n;
            double nTo3 = nTo2 * n;
            double nTo4 = nTo3 * n;
            double nTo5 = nTo4 * n;

            double c1 = (a + b) / 2.0 * (1.0 + 1.0 / 4.0 * nTo2 * 1.0 / 64.0 * nTo4);
            double c2 = 3.0 / 2.0 * n - 27.0 / 32.0 * nTo3 - 269.0 / 512.0 * nTo5;
            double c3 = 21.0 / 16.0 * nTo2 - 55.0 / 32 * nTo4;
            double c4 = 151.0 / 96.0 * nTo3 - 417.0 / 128.0 * nTo5;
            double c5 = 1097.0 / 512.0 * nTo4;

            double c0 = X / c1;

            //latitude footprint in radians
            double fPHI = c0 + c2 * (2.0 * c0).Sin() + c3 * (4.0 * c0).Sin() + c4 * (6.0 * c0).Sin() + c5 * (8.0 * c0).Sin();

            //% second nummerical eccentrencity
            double e2q = ellipsoid.E2Q;

            //% auxilary quantity 1
            double cosfPHI = fPHI.Cos();
            double nuTo2 = e2q * cosfPHI * cosfPHI;
            double nuTo4 = nuTo2 * nuTo2;

            //%radius of the curvature in prime vertical
            double N = (a * a) / (b * (1.0 + nuTo2).Sqrt());
            
            double Nto2 = N*N;
            double Nto3 = Nto2 * N;
            double Nto4 = Nto3 * N;
            double Nto5 = Nto4 * N;
            double Nto6 = Nto5 * N;
            double Nto7 = Nto6 * N;
            double Nto8 = Nto7 * N;

            //% auxilary quantity 2
            double t = fPHI.Tan();

            // t to powers
            double tTo2 = t * t;
            double tTo4 = tTo2 * tTo2;
            double tTo6 = tTo2 * tTo4;

            double Yto2 = Y * Y;
            double Yto3 = Yto2 * Y;
            double Yto4 = Yto3 * Y;
            double Yto5 = Yto4 * Y;
            double Yto6 = Yto5 * Y;
            double Yto7 = Yto6 * Y;
            double Yto8 = Yto7 * Y;


            double p1 = fPHI + (t / (2.0 * Nto2)) * (-1.0 - nuTo2) * Yto2;
            double p2 = (t / (24.0 * Nto4)) * (5.0 + 3.0 * tTo2 + 6.0 * nuTo2 - 6 * tTo2 * nuTo2 - 3.0 * nuTo4 - 9.0 * tTo2 * nuTo4) * Yto4;
            double p3 = (t / (720.0 * Nto6)) * (-61.0 - 90.0 * tTo2 - 45.0 * tTo4 - 107.0 * nuTo2 + 162.0 * tTo2 * nuTo2 + 45.0 * tTo4 * nuTo2) * Yto6;
            double p4 = (t / (40320.0 * Nto8)) * (1385.0 + 3633.0 * tTo2 + 4045 * tTo4 + 1575 * tTo6) * Yto8;

            double phi = p1+p2 + p3 + p4;

            double l1 = Y / (cosfPHI * N);
            double l2 = ((-1.0 -2.0 * tTo2 - nuTo2) * Yto3) / (6.0 * Nto3 * cosfPHI);
            double l3 = (5.0 + 28.0 * tTo2 + 24 * tTo4 + 6.0 * nuTo2 + 8.0 * tTo2 * nuTo2) * Yto5 / (120.0 * Nto5 * cosfPHI);
            double l4 = (-61.0 - 662 * tTo2 - 1320 * tTo4 - 720 * tTo6) * Yto7 / (5040.0 * Nto7 * cosfPHI);

            double lam = l1 +l2 + l3 + l4;

            double lat = Conversion.DegreesFromRadians(phi);
            double lon = Conversion.DegreesFromRadians(lam) +referenceMeridan;
            return new V3d(lon, lat, rightHeightAlt.Z);
        }

        #endregion

        #region Amap conversions

        // amap info for OK50 map
        // utm zone 33
        // easting 68500 - 682900 (low = east)
        // northing 5104320 - 5432000 (low = south)
        // no pixel offsets

        // 0/65535 ------------------------122880/65535
        //  |                                    |
        //  |                                    |
        //  |                                    |
        //  |----x/y                             |
        //  |     |                              |
        //  |     |                              |
        //  0/0------------------------------122880/0

        // conversion into pixel coordinates
        // map size: width 122880 px, height 65535 px
        // given: utm wgs84 coordinates of zone 33 (important, zone 32 does not work
        // and needs to be converted into zone 33 first; this is only required for parts
        // of western austria).
        // x = (easting - 68500.0) / 5.0
        // y = (northing - 5104320) / 5.0

        public static V2i OK50PixelCoordinatesFromWgs84(int easting, int northing)
        {
            return new V2i(
                (double)(easting - 68500) / 5.0,
                (double)(northing - 5104320) / 5.0);
        }

        public static V2i OK50PixelCoordinatesFromWgs84(V2i wgs84)
        {
            return OK50PixelCoordinatesFromWgs84(wgs84.X, wgs84.Y);
        }

        public static V2i Wgs84FromOK50PixelCoordinates(V2i pos)
        {
            return new V2i(
                pos.X * 5 + 68500,
                pos.Y * 5 + 5104320);
        }

        #endregion

        #region Distance

        public static double DistanceVincentyWgs84(
                V2d lonLat0, V2d lonLat1)
        {
            return DistanceVincenty(lonLat0, lonLat1, GeoEllipsoid.Wgs84);
        }

        /// <summary>
        /// Vincenty Inverse Solution of Geodesics on the Ellipsoid (c) Chris Veness 2002-2012
        /// from: Vincenty inverse formula - T Vincenty, "Direct and Inverse Solutions of Geodesics on the
        /// Ellipsoid with application of nested equations", Survey Review, vol XXII no 176, 1975
        /// http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf 
        /// Calculates geodetic distance between two points on the WGS 84
        /// ellipsoid specified by latitude/longitude using  Vincenty
        /// inverse formula for ellipsoids.
        /// </summary>
        public static double DistanceVincenty(
                V2d lonLat0, V2d lonLat1, GeoEllipsoid ellipsoid)
        {
            double a = ellipsoid.A, b = ellipsoid.B,  f = ellipsoid.F;
            double L = (lonLat1.X - lonLat0.X) * Constant.RadiansPerDegree;
            double U1 = Math.Atan((1-f) * Math.Tan(lonLat0.Y * Constant.RadiansPerDegree));
            double U2 = Math.Atan((1-f) * Math.Tan(lonLat1.Y * Constant.RadiansPerDegree));
            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);  
            double lambda = L, lambdaP;
            int iterLimit = 100;
            double sinSigma, cosSigma, sigma, cosSqAlpha, cos2SigmaM;
            do
            {
                double sinLambda = Math.Sin(lambda), cosLambda = Math.Cos(lambda);
                sinSigma = Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) + 
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda)
                    * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
                if (sinSigma == 0) return 0;  // co-incident points
                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);
                double sinAlpha = cosU1 * cosU2 * sinLambda/sinSigma;
                cosSqAlpha = 1 - sinAlpha * sinAlpha;
                cos2SigmaM = cosSigma - 2 * sinU1 * sinU2/cosSqAlpha;
                if (double.IsNaN(cos2SigmaM)) cos2SigmaM = 0;  // equatorial line: cosSqAlpha=0 (§6)
                double C = f/16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = L + (1 - C) * f * sinAlpha *
                    (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            }
            while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

            if (iterLimit == 0) return double.NaN;  // formula failed to converge

            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq/16384 * (4096 + uSq*(-768 + uSq * (320 - 175 * uSq)));
            double B = uSq/1024 * (256 + uSq*(-128 + uSq * (74 - 47 * uSq)));
            double deltaSigma = B * sinSigma * (cos2SigmaM + B/4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)-
                B/6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            double s = b * A * (sigma - deltaSigma);

            return s;
        }

        public static double DistanceVincentyWgs84(
                V2d lonLat0, V2d lonLat1,
                out double brng0, out double brng1)
        {
            return DistanceVincenty(lonLat0, lonLat1, GeoEllipsoid.Wgs84, out brng0, out brng1);
        }

        /// <summary>
        /// Vincenty Inverse Solution of Geodesics on the Ellipsoid (c) Chris Veness 2002-2012
        /// from: Vincenty inverse formula - T Vincenty, "Direct and Inverse Solutions of Geodesics on the
        /// Ellipsoid with application of nested equations", Survey Review, vol XXII no 176, 1975
        /// http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf 
        /// Calculates geodetic distance between two points on the WGS 84
        /// ellipsoid specified by latitude/longitude using  Vincenty
        /// inverse formula for ellipsoids.
        /// </summary>
        public static double DistanceVincenty(
                V2d lonLat0, V2d lonLat1, GeoEllipsoid ellipsoid,
                out double brng0, out double brng1)
        {
            double a = ellipsoid.A, b = ellipsoid.B,  f = ellipsoid.F;
            double L = (lonLat1.X - lonLat0.X) * Constant.RadiansPerDegree;
            double U1 = Math.Atan((1-f) * Math.Tan(lonLat0.Y * Constant.RadiansPerDegree));
            double U2 = Math.Atan((1-f) * Math.Tan(lonLat1.Y * Constant.RadiansPerDegree));
            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);  
            double lambda = L, lambdaP;
            int iterLimit = 100;
            double sinSigma, cosSigma, sinLambda, cosLambda, sigma, cosSqAlpha, cos2SigmaM;
            do
            {
                sinLambda = Math.Sin(lambda); cosLambda = Math.Cos(lambda);
                sinSigma = Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) + 
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda)
                    * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
                if (sinSigma == 0)
                {
                    brng0 = double.NaN;
                    brng1 = double.NaN;
                    return 0;  // co-incident points
                }
                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);
                double sinAlpha = cosU1 * cosU2 * sinLambda/sinSigma;
                cosSqAlpha = 1 - sinAlpha * sinAlpha;
                cos2SigmaM = cosSigma - 2 * sinU1 * sinU2/cosSqAlpha;
                if (double.IsNaN(cos2SigmaM)) cos2SigmaM = 0;  // equatorial line: cosSqAlpha=0 (§6)
                double C = f/16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = L + (1 - C) * f * sinAlpha *
                    (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            }
            while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

            if (iterLimit == 0)
            {
                brng0 = double.NaN;
                brng1 = double.NaN;
                return double.NaN;  // formula failed to converge
            }
            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq/16384 * (4096 + uSq*(-768 + uSq * (320 - 175 * uSq)));
            double B = uSq/1024 * (256 + uSq*(-128 + uSq * (74 - 47 * uSq)));
            double deltaSigma = B * sinSigma * (cos2SigmaM + B/4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)-
                B/6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            double s = b * A * (sigma - deltaSigma);

            var fwdAz = Math.Atan2(cosU2*sinLambda,  cosU1*sinU2-sinU1*cosU2*cosLambda);
            var revAz = Math.Atan2(cosU1*sinLambda, -sinU1*cosU2+cosU1*sinU2*cosLambda);

            brng0 = fwdAz * Constant.DegreesPerRadian;
            brng1 = revAz * Constant.DegreesPerRadian; 

            return s;
        }

        public static V2d DirectionVincentyWgs84(V2d lonLat, double brng, double dist)
        {
            double finalBrng;
            return DirectionVincenty(lonLat, brng, dist, GeoEllipsoid.Wgs84, out finalBrng);
        }

        /// <summary>
        /// Vincenty Direct Solution of Geodesics on the Ellipsoid (c) Chris Veness 2005-2012
        /// from: Vincenty direct formula - T Vincenty, "Direct and Inverse Solutions of Geodesics on the
        /// Ellipsoid with application of nested equations", Survey Review, vol XXII no 176, 1975
        /// http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf 
        /// </summary>
        public static V2d DirectionVincenty(V2d lonLat, double brng, double dist, GeoEllipsoid gel)
        {
            double finalBrng;
            return DirectionVincenty(lonLat, brng, dist, gel, out finalBrng);
        }

        public static V2d DirectionVincentyWgs84(
            V2d lonLat, double brng, double dist, out double finalBrng)
        {
            return DirectionVincenty(lonLat, brng, dist, GeoEllipsoid.Wgs84, out finalBrng);
        }

        /// <summary>
        /// Vincenty Direct Solution of Geodesics on the Ellipsoid (c) Chris Veness 2005-2012
        /// from: Vincenty direct formula - T Vincenty, "Direct and Inverse Solutions of Geodesics on the
        /// Ellipsoid with application of nested equations", Survey Review, vol XXII no 176, 1975
        /// http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf 
        /// </summary>
        public static V2d DirectionVincenty(V2d lonLat, double brng, double dist, GeoEllipsoid gel,
            out double finalBrng)
        {
            double a = gel.A, b = gel.B,  f = gel.F;  // WGS-84 ellipsiod
            var s = dist;
            var alpha1 = brng * Constant.RadiansPerDegree;
            var sinAlpha1 = Math.Sin(alpha1);
            var cosAlpha1 = Math.Cos(alpha1);

            var tanU1 = (1-f) * Math.Tan(lonLat.Y * Constant.RadiansPerDegree);
            double cosU1 = 1 / Math.Sqrt((1 + tanU1*tanU1)), sinU1 = tanU1*cosU1;
            var sigma1 = Math.Atan2(tanU1, cosAlpha1);
            var sinAlpha = cosU1 * sinAlpha1;
            var cosSqAlpha = 1 - sinAlpha*sinAlpha;
            var uSq = cosSqAlpha * (a*a - b*b) / (b*b);
            var A = 1 + uSq/16384*(4096+uSq*(-768+uSq*(320-175*uSq)));
            var B = uSq/1024 * (256+uSq*(-128+uSq*(74-47*uSq)));

            var sigma = s / (b * A); var sigmaP = 2*Constant.Pi;
            double sinSigma = 0, cosSigma = 0, cos2SigmaM = 0;
            while (Math.Abs(sigma-sigmaP) > 1e-12) {
                cos2SigmaM = Math.Cos(2*sigma1 + sigma);
                sinSigma = Math.Sin(sigma);
                cosSigma = Math.Cos(sigma);
                var deltaSigma = B*sinSigma*(cos2SigmaM+B/4*(cosSigma*(-1+2*cos2SigmaM*cos2SigmaM)-
                    B/6*cos2SigmaM*(-3+4*sinSigma*sinSigma)*(-3+4*cos2SigmaM*cos2SigmaM)));
                sigmaP = sigma;
                sigma = s / (b*A) + deltaSigma;
            }

            var tmp = sinU1*sinSigma - cosU1*cosSigma*cosAlpha1;
            var lat2 = Math.Atan2(sinU1*cosSigma + cosU1*sinSigma*cosAlpha1, 
                (1-f)*Math.Sqrt(sinAlpha*sinAlpha + tmp*tmp));
            var lambda = Math.Atan2(sinSigma*sinAlpha1, cosU1*cosSigma - sinU1*sinSigma*cosAlpha1);
            var C = f/16*cosSqAlpha*(4+f*(4-3*cosSqAlpha));
            var L = lambda - (1-C) * f * sinAlpha *
                (sigma + C*sinSigma*(cos2SigmaM+C*cosSigma*(-1+2*cos2SigmaM*cos2SigmaM)));
            var lon2 = (lonLat.X * Constant.RadiansPerDegree+L+3*Constant.Pi)%(2*Constant.Pi) - Constant.Pi;  // normalise to -180...+180

            var revAz = Math.Atan2(sinAlpha, -tmp);  // final bearing, if required

            finalBrng = revAz * Constant.DegreesPerRadian;
            return new V2d(lon2 * Constant.DegreesPerRadian, lat2 * Constant.DegreesPerRadian);

        }

        public static double ComputeLength(V2d[] lonLatArray, GeoEllipsoid gel, params int[] indexArray)
        {
            var pc = indexArray.Length;
            var p0 = lonLatArray[indexArray[0]];
            var d = 0.0;
            for (int i = 1; i < pc; i++)
            {
                var p1 = lonLatArray[indexArray[i]];
                d += DistanceVincenty(p0, p1, gel);
                p0 = p1;
            }
            return d;
        }

        public static double ComputeLengthWgs84(V2d[] lonLatArray, params int[] indexArray)
        {
            return ComputeLength(lonLatArray, GeoEllipsoid.Wgs84, indexArray);
        }

        public static double ComputePerimeter(V2d[] lonLatArray, GeoEllipsoid gel, params int[] indexArray)
        {
            var pc = indexArray.Length;
            var p0 = lonLatArray[indexArray[pc - 1]];
            var d = 0.0;
            for (int i = 0; i < pc; i++)
            {
                var p1 = lonLatArray[indexArray[i]];
                d += DistanceVincenty(p0, p1, gel);
                p0 = p1;
            }
            return d;
        }

        public static double ComputePerimeterWgs84(V2d[] lonLatArray, params int[] indexArray)
        {
            return ComputePerimeter(lonLatArray, GeoEllipsoid.Wgs84, indexArray);
        }

        #endregion
    }

}
