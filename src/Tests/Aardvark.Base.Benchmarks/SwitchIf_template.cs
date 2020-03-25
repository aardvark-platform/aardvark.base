using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks
{
    // Testing two variants for the Column(int) and Row(int) and element indexer methods for matrix types.
    // The old implementation used a "switch" statement, whereas here we want to prove that using "if" is more efficient.
    // Using "switch" seems to be inferior for Column and Row because it is never actually inlined. However, there are noticable differences
    // between different dimensions, i.e. the number of branches.
    // In the case of element indexers, the "if" variant actually isn't always better. Note that here we may not want to put
    // AggressiveInlining because for 4x4 matrices there would be 16 branches in total.
    
    //# string[] methods = new[] { "Switch", "If" }; 
    //# Action comma = () => Out(", ");
    //# Action el = () => Out("else ");
    //# for (int d = 2; d <= 4; d++) {
    //# var dd = d * d;
    //# var type = "SwitchIf" + d;
    //# var vtype = "V" + d + "d";
    //# var mtype = "M" + d + "x" + d;
    #region __type__

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class __type__
    {
        private struct __mtype__
        {
            //# d.ForEach(j => {
            public double /*# d.ForEach(k => { */M__j____k__/*# }, comma); */;
            //# });

            //# d.ForEach(k => {
            public __vtype__ C__k__
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new __vtype__(/*# d.ForEach(f => {*/ M__f____k__/*#}, comma); */); }
            }

            //# });
            public __mtype__(double[] elems)
            {
                //# d.ForEach(i => {
                /*# d.ForEach(j => { var idx = i * d + j; */M__i____j__ = elems[__idx__]; /*# });*/
                //# });
            }

            #region Elem

            public double ElemSwitch(int row, int column)
            {
                switch (row)
                {
                    //# d.ForEach(r => { 
                    case __r__:
                        switch (column)
                        {
                            //# d.ForEach(c => {
                            case __c__: return M__r____c__;
                            //# });
                            default: throw new IndexOutOfRangeException();
                        }
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }

            public double ElemIf(int row, int column)
            {
                /*# d.ForEach(i => { */if (row == __i__)
                {
                    /*# d.ForEach(j => { */if (column == __j__) return M__i____j__;
                    /*# }, el); */else throw new IndexOutOfRangeException();
                }
                /*# }, el); */else throw new IndexOutOfRangeException();
            }

            #endregion

            #region Col

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __vtype__ ColSwitch(int index)
            {
                switch (index)
                {
                    //# d.ForEach(i => {
                    case __i__: return C__i__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __vtype__ ColIf(int index)
            {
                /*# d.ForEach(i => { */if (index == __i__) return C__i__;
                /*# }, el); */else throw new IndexOutOfRangeException();
            }

            #endregion
        }

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly __mtype__[] matrices = new __mtype__[count];

        public __type__()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(__d__));
            matrices.SetByIndex(i => new __mtype__(rnd.CreateUniformDoubleArray(__dd__)));
        }

        //# methods.ForEach(m => {
        #region Benchmarks for __m__

        [Benchmark]
        public double Elem__m__()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].Elem__m__(idx.X, idx.Y);
            }

            return accum;
        }

        [Benchmark]
        public __vtype__ Col__m__()
        {
            __vtype__ accum = __vtype__.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].Col__m__(indices[i].X);
            }

            return accum;
        }

        #endregion

        //# });
    }

    #endregion

    //# }
}