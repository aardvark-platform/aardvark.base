using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aardvark.Base.Benchmarks
{
    // Testing variants for indexer methods for matrix and vector types.
    // There are three variants:
    // (*) Flow control with switch (original method)
    // (*) Flow control flow with if
    // (*) Unsafe with pointers
    // Using "switch" is problematic because it is never actually inlined (despite AggressiveInlining).
    // Flow control with "if" seems strictly better for this reason, however there are noticable differences
    // between different dimensions, i.e. the number of branches. In the case of element indexers, the "if" variant actually isn't always better.
    // The final method to be tested uses unsafe and direct access with pointers. As expect this method is much more efficient than the other
    // two in most cases. An exception to this are the row and column indexers for 4x4 matrices. Here, the flow control variants actually perform
    // slightly better.

    //# string[] methods = new[] { "Switch", "If", "Unsafe" };
    //# Action comma = () => Out(", ");
    //# Action el = () => Out("else ");
    //# for (int d = 2; d <= 4; d++) {
    //# var n = d;
    //# var m = d;
    //# var nm = n * m;
    //# var type = "Indexers" + d;
    //# var vtype = "Vector";
    //# var nmtype = "Matrix";
    //# var vntype = "V" + n + "d";
    //# var vmtype = "V" + m + "d";
    //# var fields = Meta.VecFields.Take(d).ToArray();
    //# var vgetptr = "&" + fields[0];
    //# var mgetptr = "&M00";
    #region __type__

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class __type__
    {
        #region __vtype__

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct __vtype__
        {
            #region Fields

            public readonly double /*# fields.ForEach(f => { */__f__/*# }, comma); */;

            #endregion

            #region Constructor

            public __vtype__(double[] elems)
            {
                //# fields.ForEach((f, i) => {
                __f__ = elems[__i__];
                //# });
            }

            #endregion

            #region Indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int index)
            {
                switch (index)
                {
                    //# fields.ForEach((f, i) => {
                    case __i__: return __f__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int index)
            {
                /*# fields.ForEach((f, i) => { */if (index == __i__) return __f__;
                /*# }, el); */else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int index)
            {
                fixed (double* ptr = __vgetptr__) { return ptr[index]; }
            }

            #endregion
        }

        #endregion

        #region __nmtype__

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct __nmtype__
        {
            #region Fields

            //# n.ForEach(i => {
            public readonly double /*# d.ForEach(j => { */M__i____j__/*# }, comma); */;
            //# });

            #endregion

            #region Rows

            //# n.ForEach(i => {
            public __vmtype__ R__i__
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new __vmtype__(/*# m.ForEach(j => {*/ M__i____j__/*#}, comma); */); }
            }

            //# });
            #endregion

            #region Columns

            //# n.ForEach(j => {
            public __vntype__ C__j__
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new __vntype__(/*# n.ForEach(i => {*/ M__i____j__/*#}, comma); */); }
            }

            //# });
            #endregion

            #region Constructor

            public __nmtype__(double[] elems)
            {
                //# n.ForEach(i => {
                /*# m.ForEach(j => { var idx = i * d + j; */M__i____j__ = elems[__idx__]; /*# });*/
                //# });
            }

            #endregion

            #region Element indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int row, int column)
            {
                switch (row)
                {
                    //# n.ForEach(r => {
                    case __r__:
                        switch (column)
                        {
                            //# m.ForEach(c => {
                            case __c__: return M__r____c__;
                            //# });
                            default: throw new IndexOutOfRangeException();
                        }
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int row, int column)
            {
                /*# n.ForEach(i => { */if (row == __i__)
                {
                    /*# m.ForEach(j => { */if (column == __j__) return M__i____j__;
                    /*# }, el); */else throw new IndexOutOfRangeException();
                }
                /*# }, el); */else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int row, int column)
            {
                fixed (double* ptr = __mgetptr__)
                {
                    return ptr[row * __d__ + column];
                }
            }

            #endregion

            #region Row indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __vmtype__ RowSwitch(int index)
            {
                switch (index)
                {
                    //# n.ForEach(i => {
                    case __i__: return R__i__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __vmtype__ RowIf(int index)
            {
                /*# n.ForEach(i => { */if (index == __i__) return R__i__;
                /*# }, el); */else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe __vmtype__ RowUnsafe(int index)
            {
                fixed (double* ptr = __mgetptr__)
                {
                    return new __vmtype__(/*# m.ForEach(j => {
                        */ptr[index * __d__/*#
                        if (j > 0) {*/ + __j__/*#
                        }*/]/*# }, comma);*/);
                }
            }

            #endregion

            #region Column indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __vntype__ ColSwitch(int index)
            {
                switch (index)
                {
                    //# m.ForEach(i => {
                    case __i__: return C__i__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __vntype__ ColIf(int index)
            {
                /*# m.ForEach(i => { */if (index == __i__) return C__i__;
                /*# }, el); */else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe __vntype__ ColUnsafe(int index)
            {
                fixed (double* ptr = __mgetptr__)
                {
                    return new __vntype__(/*# n.ForEach(i => {
                        var offset = i * d;
                        */ptr[index/*#
                        if (offset > 0) {*/ + __offset__/*#
                        }*/]/*# }, comma);*/);
                }
            }

            #endregion
        }

        #endregion

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly __vtype__[] vectors = new __vtype__[count];
        readonly __nmtype__[] matrices = new __nmtype__[count];

        public __type__()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(__n__));
            vectors.SetByIndex(i => new __vtype__(rnd.CreateUniformDoubleArray(__n__)));
            matrices.SetByIndex(i => new __nmtype__(rnd.CreateUniformDoubleArray(__nm__)));
        }

        #region Tests

        [Test]
        public void VectorElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var vec = vectors[i];

                var mif = vec.ElemIf(idx.X);
                var msw = vec.ElemSwitch(idx.X);
                var mus = vec.ElemUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ElemIf(idx.X, idx.Y);
                var msw = mat.ElemSwitch(idx.X, idx.Y);
                var mus = mat.ElemUnsafe(idx.X, idx.Y);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixRowIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.RowIf(idx.X);
                var msw = mat.RowSwitch(idx.X);
                var mus = mat.RowUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixColumnIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ColIf(idx.X);
                var msw = mat.ColSwitch(idx.X);
                var mus = mat.ColUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        #endregion

        #region Benchmarks for Vector Element Indexers

        //# methods.ForEach(mth => {
        [Benchmark]
        public double VectorElementIndexer__mth__()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].Elem__mth__(idx);
            }

            return accum;
        }

        //# });
        #endregion

        #region Benchmarks for Matrix Element Indexers

        //# methods.ForEach(mth => {
        [Benchmark]
        public double MatrixElementIndexer__mth__()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].Elem__mth__(idx.X, idx.Y);
            }

            return accum;
        }

        //# });
        #endregion

        #region Benchmarks for Matrix Row Indexers
        //# methods.ForEach(mth => {

        [Benchmark]
        public __vmtype__ MatrixRowIndexer__mth__()
        {
            __vmtype__ accum = __vmtype__.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].Row__mth__(indices[i].X);
            }

            return accum;
        }

        //# });
        #endregion

        #region Benchmarks for Matrix Column Indexers
        //# methods.ForEach(mth => {

        [Benchmark]
        public __vntype__ MatrixColumnIndexer__mth__()
        {
            __vntype__ accum = __vntype__.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].Col__mth__(indices[i].X);
            }

            return accum;
        }

        //# });
        #endregion
    }

    #endregion

    //# }
}