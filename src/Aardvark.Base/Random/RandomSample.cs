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
namespace Aardvark.Base;

public static class RandomSample
{
    /// <summary>
    /// Uses the 2 random series (seriesIndex, seriesIndex+1) to generate a random point on a sphere.
    /// </summary>
    public static V3d Spherical(IRandomSeries rnds, int seriesIndex)
    {
        return Spherical(rnds.UniformDouble(seriesIndex), rnds.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Uses the 2 random variables x1 and x2 to generate a random point on a sphere.
    /// </summary>
    public static V3d Spherical(double x1, double x2)
    {
        var phi = Constant.PiTimesTwo * x1;
        var z = 1 - 2 * x2;
        var r = Fun.Max(1 - z * z, 0).Sqrt();
        return new V3d(r * phi.Cos(), r * phi.Sin(), z);
    }

    /// <summary>
    /// Uses the 2 random variables x1 and x2 to generate a random point on a sphere.
    /// </summary>
    public static V3f Spherical(float x1, float x2)
    {
        var phi = ConstantF.PiTimesTwo * x1;
        var z = 1 - 2 * x2;
        var r = Fun.Max(1 - z * z, 0).Sqrt();
        return new V3f(r * phi.Cos(), r * phi.Sin(), z);
    }

    /// <summary>
    /// Generate a cosine weighted random sample oriented in the supplied normal direction 
    /// using two random series (seriesIndex, seriesIndex+1).
    /// The normal is expected to be normalized.
    /// </summary>
    public static V3d Lambertian(V3d normal, IRandomSeries rnds, int seriesIndex)
    {
        return Lambertian(normal, 
            rnds.UniformDouble(seriesIndex),
            rnds.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generate a cosine weighted random sample oriented in the supplied normal direction 
    /// using two random series (seriesIndex, seriesIndex+1).
    /// The normal is expected to be normalized.
    /// </summary>
    public static V3f Lambertian(V3f normal, IRandomSeries rnds, int seriesIndex)
    {
        return Lambertian(normal,
            (float)rnds.UniformDouble(seriesIndex),
            (float)rnds.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generate a cosine weighted random sample oriented in the supplied normal direction 
    /// using the 2 random variables x1 and x2.
    /// The normal is expected to be normalized.
    /// </summary>
    public static V3d Lambertian(V3d normal, double x1, double x2)
    {
        // random point on cylinder barrel
        var phi = Constant.PiTimesTwo * x1;
        var z = 1 - 2 * x2;
        // project to sphere
        var r = Fun.Max(1 - z * z, 0).Sqrt();
        var vec = new V3d(r * phi.Cos(), r * phi.Sin(), z) + normal;
        var squareLen = vec.LengthSquared;

        // check if random sphere point was perfectly opposite of the normal direction (in case x2 ~= 1)
        if (squareLen < 1e-9)
            return normal;

        var norm = 1 / squareLen.Sqrt();
        return vec * norm;
    }

    /// <summary>
    /// Generate a cosine weighted random sample oriented in the supplied normal direction 
    /// using the 2 random variables x1 and x2.
    /// The normal is expected to be normalized.
    /// </summary>
    public static V3f Lambertian(V3f normal, float x1, float x2)
    {
        // random point on cylinder barrel
        var phi = ConstantF.PiTimesTwo * x1;
        var z = 1 - 2 * x2;
        // project to sphere
        var r = Fun.Max(1 - z * z, 0).Sqrt();
        var vec = new V3f(r * phi.Cos(), r * phi.Sin(), z) + normal;
        var squareLen = vec.LengthSquared;

        // check if random sphere point was perfectly opposite of the normal direction (in case x2 ~= 1)
        if (squareLen < 1e-9f)
            return normal;

        var norm = 1 / squareLen.Sqrt();
        return vec * norm;
    }

    /// <summary>
    /// Generates a cosine weighted random direction using the 2 random series (seriesIndex, seriesIndex+1).
    /// See Global Illuminatin Compendium, Dutre 2003, (35)
    /// PDF = cos(theta)/PI
    /// </summary>
    public static V3d Lambertian(IRandomSeries rnds, int seriesIndex)
    {
        return Lambertian(
            rnds.UniformDouble(seriesIndex),
            rnds.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a cosine weighted random direction using the 2 random varables x1 and x2.
    /// See Global Illuminatin Compendium, Dutré 2003, (35)
    /// PDF = cos(theta)/PI
    /// </summary>
    public static V3d Lambertian(double x1, double x2)
    {
        // random point on disk
        var r = Fun.Sqrt(x1);
        var phi = Constant.PiTimesTwo * x2;
        var x = r * Fun.Cos(phi);
        var y = r * Fun.Sin(phi);
        // project to hemisphere
        var z = Fun.Sqrt(1 - x1); // x1 = r^2
        return new V3d(x, y, z);
    }

    /// <summary>
    /// Generates a cosine weighted random direction using the 2 random varables x1 and x2.
    /// See Global Illuminatin Compendium, Dutré 2003, (35)
    /// PDF = cos(theta)/PI
    /// </summary>
    public static V3f Lambertian(float x1, float x2)
    {
        // random point on disk
        var r = Fun.Sqrt(x1);
        var phi = ConstantF.PiTimesTwo * x2;
        var x = r * Fun.Cos(phi);
        var y = r * Fun.Sin(phi);
        // project to hemisphere
        var z = Fun.Sqrt(1 - x1); // x1 = r^2
        return new V3f(x, y, z);
    }

    /// <summary>
    /// Generates a uniform distributed 2d random sample on a disk with radius 1.
    /// It uses two random series (seriesIndex, seriesIndex+1).
    /// </summary>
    public static V2d Disk(IRandomSeries rnd, int seriesIndex)
    {
        return Disk(rnd.UniformDouble(seriesIndex), rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed 2d random sample on a disk with radius 1 
    /// using the 2 random variables x1 and x2.
    /// </summary>
    public static V2d Disk(double x1, double x2)
    {
        // random direction
        var phi = x1 * Constant.PiTimesTwo;
        // random radius transformed by sqrt to result in area equivalent samples distribution
        var r = x2.Sqrt();
        return new V2d(r * Fun.Cos(phi),
                       r * Fun.Sin(phi));
    }

    /// <summary>
    /// Generates a uniform distributed 2d random sample on a disk with radius 1 
    /// using the 2 random variables x1 and x2.
    /// </summary>
    public static V2f Disk(float x1, float x2)
    {
        // random direction
        var phi = x1 * ConstantF.PiTimesTwo;
        // random radius transformed by sqrt to result in area equivalent samples distribution
        var r = x2.Sqrt();
        return new V2f(r * Fun.Cos(phi),
                       r * Fun.Sin(phi));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V2d Triangle(Triangle2d t, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(t.P0, t.P1, t.P2,
                    rnd.UniformDouble(seriesIndex),
                    rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V2f Triangle(Triangle2f t, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(t.P0, t.P1, t.P2,
                    (float)rnd.UniformDouble(seriesIndex),
                    (float)rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V2d Triangle(Triangle2d t, double x1, double x2)
    {
        return Triangle(t.P0, t.P1, t.P2, x1, x2);
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V2f Triangle(Triangle2f t, float x1, float x2)
    {
        return Triangle(t.P0, t.P1, t.P2, x1, x2);
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V2d Triangle(V2d p0, V2d p1, V2d p2, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(p0, p1, p2,
                    rnd.UniformDouble(seriesIndex),
                    rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V2f Triangle(V2f p0, V2f p1, V2f p2, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(p0, p1, p2,
                    (float)rnd.UniformDouble(seriesIndex),
                    (float)rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V2d Triangle(V2d p0, V2d p1, V2d p2, double x1, double x2)
    {
        var x1sq = x1.Sqrt();
        return (1 - x1sq) * p0 + (x1sq * (1 - x2)) * p1 + (x1sq * x2) * p2;
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V2f Triangle(V2f p0, V2f p1, V2f p2, float x1, float x2)
    {
        var x1sq = x1.Sqrt();
        return (1 - x1sq) * p0 + (x1sq * (1 - x2)) * p1 + (x1sq * x2) * p2;
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V3d Triangle(Triangle3d t, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(t.P0, t.P1, t.P2,
                    rnd.UniformDouble(seriesIndex),
                    rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V3f Triangle(Triangle3f t, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(t.P0, t.P1, t.P2,
                    (float)rnd.UniformDouble(seriesIndex),
                    (float)rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V3d Triangle(Triangle3d t, double x1, double x2)
    {
        return Triangle(t.P0, t.P1, t.P2, x1, x2);
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V3f Triangle(Triangle3f t, float x1, float x2)
    {
        return Triangle(t.P0, t.P1, t.P2, x1, x2);
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V3d Triangle(V3d p0, V3d p1, V3d p2, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(p0, p1, p2, 
                    rnd.UniformDouble(seriesIndex),
                    rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random series (seriesIndex,  seriesIndex + 1).
    /// </summary>
    public static V3f Triangle(V3f p0, V3f p1, V3f p2, IRandomSeries rnd, int seriesIndex)
    {
        return Triangle(p0, p1, p2,
                    (float)rnd.UniformDouble(seriesIndex),
                    (float)rnd.UniformDouble(seriesIndex + 1));
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V3d Triangle(V3d p0, V3d p1, V3d p2, double x1, double x2)
    {
        var x1sq = x1.Sqrt();
        return (1 - x1sq) * p0 + (x1sq * (1 - x2)) * p1 + (x1sq * x2) * p2;
    }

    /// <summary>
    /// Generates a uniform distributed random sample on the given triangle using 
    /// two random variables x1 and x2.
    /// </summary>
    public static V3f Triangle(V3f p0, V3f p1, V3f p2, float x1, float x2)
    {
        var x1sq = x1.Sqrt();
        return (1 - x1sq) * p0 + (x1sq * (1 - x2)) * p1 + (x1sq * x2) * p2;
    }
}
