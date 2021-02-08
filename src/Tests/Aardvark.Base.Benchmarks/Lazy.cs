using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;
using Microsoft.FSharp.Core;

namespace Aardvark.Base.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class Lazy
    {
        #region CameraView implementations

        private readonly struct CameraViewParams
        {
            public CameraViewParams(V3d l, V3d f, V3d u, V3d r)
            {
                Location = l;
                Forward = f.Normalized;
                Up = u.Normalized;
                Right = r.Normalized;
            }

            public readonly V3d Location;
            public readonly V3d Forward;
            public readonly V3d Up;
            public readonly V3d Right;
        }

        private class CameraViewLazy
        {
            private readonly V3d location;
            private readonly V3d forward;
            private readonly V3d up;
            private readonly V3d right;
            private readonly Lazy<Trafo3d> viewTrafo;

            public CameraViewLazy(CameraViewParams p)
            {
                location = p.Location; forward = p.Forward; up = p.Up; right = p.Right;
                viewTrafo = new Lazy<Trafo3d>(() => Trafo3d.ViewTrafo(location, right, up, -forward));
            }

            public V3d Location => location;
            public V3d Forward => forward;
            public V3d Up => up;
            public V3d Right => right;
            public Trafo3d ViewTrafo
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => viewTrafo.Value;
            }
        }

        private class CameraViewNullable
        {
            private readonly V3d location;
            private readonly V3d forward;
            private readonly V3d up;
            private readonly V3d right;
            private Trafo3d? viewTrafo;

            public CameraViewNullable(CameraViewParams p)
            {
                location = p.Location; forward = p.Forward; up = p.Up; right = p.Right;
                viewTrafo = null;
            }

            public V3d Location => location;
            public V3d Forward => forward;
            public V3d Up => up;
            public V3d Right => right;
            public Trafo3d ViewTrafo
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (viewTrafo == null)
                    {
                        viewTrafo = Trafo3d.ViewTrafo(location, right, up, -forward);
                    }

                    return viewTrafo.Value;
                }
            }
        }

        private class CameraViewOption
        {
            private readonly V3d location;
            private readonly V3d forward;
            private readonly V3d up;
            private readonly V3d right;
            private FSharpOption<Trafo3d> viewTrafo;

            public CameraViewOption(CameraViewParams p)
            {
                location = p.Location; forward = p.Forward; up = p.Up; right = p.Right;
                viewTrafo = FSharpOption<Trafo3d>.None;
            }

            public V3d Location => location;
            public V3d Forward => forward;
            public V3d Up => up;
            public V3d Right => right;
            public Trafo3d ViewTrafo
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (viewTrafo == null)
                    {
                        viewTrafo = Trafo3d.ViewTrafo(location, right, up, -forward);
                    }

                    return viewTrafo.Value;
                }
            }
        }

        #endregion

        const int count = 10000;
        const int subcount = 100;
        readonly CameraViewLazy[] lazy = new CameraViewLazy[count];
        readonly CameraViewNullable[] nullable = new CameraViewNullable[count];
        readonly CameraViewOption[] option = new CameraViewOption[count];

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new RandomSystem(1);

            var arr = new CameraViewParams[count];
            arr.SetByIndex(i => new CameraViewParams(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d()));
            lazy.SetByIndex(i => new CameraViewLazy(arr[i]));
            nullable.SetByIndex(i => new CameraViewNullable(arr[i]));
            option.SetByIndex(i => new CameraViewOption(arr[i]));
        }

        [Benchmark]
        public M44d LazyField()
        {
            var result = M44d.Zero;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < subcount; j++)
                {
                    result += lazy[i].ViewTrafo.Forward;
                }
            }
            return result;
        }

        [Benchmark]
        public M44d NullableField()
        {
            var result = M44d.Zero;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < subcount; j++)
                {
                    result += nullable[i].ViewTrafo.Forward;
                }
            }
            return result;
        }

        [Benchmark]
        public M44d OptionField()
        {
            var result = M44d.Zero;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < subcount; j++)
                {
                    result += option[i].ViewTrafo.Forward;
                }
            }
            return result;
        }
    }
}
