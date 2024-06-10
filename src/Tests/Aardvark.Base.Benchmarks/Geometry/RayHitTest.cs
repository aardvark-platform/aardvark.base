using BenchmarkDotNet.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{

    //|               Method |     Mean |     Error |    StdDev |   Median | Gen 0 | Gen 1 | Gen 2 | Allocated |
    //|--------------------- |---------:|----------:|----------:|---------:|------:|------:|------:|----------:|
    //|         RaySphereHit | 3.228 ms | 0.0176 ms | 0.0156 ms | 3.227 ms |     - |     - |     - |       2 B |
    //|      RaySphereHit_V2 | 2.300 ms | 0.0457 ms | 0.1205 ms | 2.273 ms |     - |     - |     - |         - |
    //|    RaySphereHit_Slim | 1.759 ms | 0.0346 ms | 0.0862 ms | 1.715 ms |     - |     - |     - |         - |
    //| RaySphereHit_Slim_V2 | 1.546 ms | 0.0086 ms | 0.0072 ms | 1.547 ms |     - |     - |     - |       3 B |

    [PlainExporter, MemoryDiagnoser]
    public class RayHitTest
    {
        public Ray3d[] rays;

        public RayHitTest()
        {
            var rnd = new RandomSystem(1);
            // generate random rays within box[-1, 1]
            rays = new Ray3d[100000].SetByIndex(i => new Ray3d(rnd.UniformV3d() * 2 - 1, rnd.UniformV3dDirection()));
        }
        
        public V3d GetRaySphereHit(Ray3d ray, Sphere3d sphere)
        {
            var hit = RayHit3d.MaxRange;
            if (ray.Hits(sphere, ref hit))
                return hit.Point;
            return V3d.Zero;
        }

        public V3d GetRaySphereHit_Slim(Ray3d ray, Sphere3d sphere)
        {
            if (ray.Hits(sphere, out var hitT))
                return ray.GetPointOnRay(hitT);
            return V3d.Zero;
        }

        [Benchmark]
        public V3d RaySphereHit()
        {
            var a = rays;
            var s = V3d.Zero;
            var sphere = new Sphere3d(V3d.OOO, 0.8);
            for (int i = 0; i < a.Length; i++)
                s += GetRaySphereHit(a[i], sphere);
            return s;
        }

        [Benchmark]
        public V3d RaySphereHit_V2()
        {
            var a = rays;
            var s = V3d.Zero;
            var sphere = new Sphere3d(V3d.OOO, 0.8);
            var hit = RayHit3d.MaxRange;
            for (int i = 0; i < a.Length; i++)
            {
                // reset hit
                hit.T = double.MaxValue;
                if (a[i].Hits(sphere, ref hit))
                    s += hit.Point;
            }
            return s;
        }

        [Benchmark]
        public V3d RaySphereHit_Slim()
        {
            var a = rays;
            var s = V3d.Zero;
            var sphere = new Sphere3d(V3d.OOO, 0.8);
            for (int i = 0; i < a.Length; i++)
                s += GetRaySphereHit_Slim(a[i], sphere);
            return s;
        }

        [Benchmark]
        public V3d RaySphereHit_Slim_V2()
        {
            var a = rays;
            var s = V3d.Zero;
            var sphere = new Sphere3d(V3d.OOO, 0.8);
            for (int i = 0; i < a.Length; i++)
                if (a[i].Hits(sphere, out var hitT))
                    s += a[i].GetPointOnRay(hitT);
            return s;
        }

        [Test]
        public void RaySphereHit_Test()
        {
            var a = RaySphereHit();
            var b = RaySphereHit_Slim();
            if (a != b) // results must be identical
                throw new InvalidOperationException();
        }
    }
}
