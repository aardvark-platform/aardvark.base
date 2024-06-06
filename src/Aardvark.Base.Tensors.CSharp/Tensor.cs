using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static partial class Tensor
    {
        /// <summary>
        /// Raw sample access without checks.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup2<long>>
            Index2SamplesRaw = (i, min, max, d) => new Tup2<long>(0L, d);

        /// <summary>
        /// Provides sample clamping to the border value.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup2<long>>
            Index2SamplesClamped = (i, first, end, d) =>
            {
                long i0 = i - first;
                if (i0 < 0) return new Tup2<long>(-i0 * d);
                long i1 = i - end + 1;
                if (i1 >= 0) return new Tup2<long>(-i1 * d);
                return new Tup2<long>(0L, d);
            };

        /// <summary>
        /// Provides cyclic handling within one cycle of the original.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup2<long>>
            Index2SamplesCyclic1 = (i, first, end, d) =>
            {
                long i0 = i - first;
                if (i0 < 0)
                {
                    var s = (end - first) * d;
                    if (i0 < -1) return new Tup2<long>(s, s + d);
                    return new Tup2<long>(s, d);
                }
                long i1 = i - end + 1;
                if (i1 >= 0)
                {
                    var s = (first - end) * d;
                    if (i1 >= 1) return new Tup2<long>(s, s + d);
                    return new Tup2<long>(0, s + d);
                }
                return new Tup2<long>(0L, d);
            };

        /// <summary>
        /// Raw sample access without checks.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup3<long>>
            Index3SamplesRaw = (i, first, end, d) => new Tup3<long>(-d, 0L, d);

        /// <summary>
        /// Provides sample clamping to the border value.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup3<long>>
            Index3SamplesClamped = (i, first, end, d) =>
            {
                long i0 = i - first, i1 = i - end + 1;
                return i0 > 0 ? (i1 < 0 ? new Tup3<long>(-d, 0L, d)
                                            : (i1 == 0 ? new Tup3<long>(-d, 0L, 0L)
                                                        : new Tup3<long>(-i1 * d)))
                                : (i0 == 0 ? (i1 < 0 ? new Tup3<long>(0L, 0L, d)
                                                        : new Tup3<long>(0L))
                                            : new Tup3<long>(-i0 * d));
            };

        /// <summary>
        /// Raw sample access without checks.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup4<long>>
            Index4SamplesRaw = (i, first, end, d) => new Tup4<long>(-d, 0L, d, d + d);

        /// <summary>
        /// Provides sample clamping to the border value.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup4<long>>
            Index4SamplesClamped = (i, first, end, d) =>
            {
                long i0 = i - first, i1 = i - end + 1;
                if (i0 < 1)
                {
                    if (i0 < 0)
                    {
                        if (i0 < -1) return new Tup4<long>(-i0 * d);
                        if (i1 >= -1) return new Tup4<long>(d);
                        return new Tup4<long>(d, d, d, d + d);
                    }
                    if (i1 >= -1)
                    {
                        if (i1 >= 0) return new Tup4<long>(0L);
                        return new Tup4<long>(0L, 0L, d, d);
                    }
                    return new Tup4<long>(0L, 0L, d, d + d);
                }
                if (i1 >= -1)
                {
                    if (i1 >= 0)
                    {
                        if (i1 >= 1) return new Tup4<long>(-i1 * d);
                        return new Tup4<long>(-d, 0L, 0L, 0L);
                    }
                    return new Tup4<long>(-d, 0L, d, d);
                }
                return new Tup4<long>(-d, 0L, d, d + d);
            };

        /// <summary>
        /// Provides cyclic handling within one cycle of the original.
        /// Note that this does not handle regions with less than four
        /// samples.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup4<long>>
            Index4SamplesCyclic1 = (i, first, end, d) =>
            {
                long i0 = i - first, i1 = i - end + 1;
                if (i0 < 1)
                {
                    var s = (end - first) * d;
                    if (i0 < -1)
                    {
                        if (i0 < -2) return new Tup4<long>(s - d, s, s + d, s + d + d);
                        return new Tup4<long>(s - d, s, s + d, d + d);
                    }
                    if (i0 < 0) return new Tup4<long>(s - d, s, d, d + d);
                    return new Tup4<long>(s - d, 0L, d, d + d);
                }
                if (i1 >= -1)
                {
                    var s = (first - end) * d;
                    if (i1 >= 1)
                    {
                        if (i1 >= 2) return new Tup4<long>(s - d, s, s + d, s + d + d);
                        return new Tup4<long>(-d, s, s + d, s + d + d);
                    }
                    if (i1 >= 0) return new Tup4<long>(-d, 0L, s + d, s + d + d);
                    return new Tup4<long>(-d, 0L, d, s + d + d);
                }
                return new Tup4<long>(-d, 0L, d, d + d);
            };

        /// <summary>
        /// Raw sample access without checks.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup6<long>>
            Index6SamplesRaw = (i, first, end, d) =>
            {
                var d2 = d + d; return new Tup6<long>(-d2, -d, 0L, d, d2, d2 + d);
            };


        /// <summary>
        /// Provides sample clamping to the border value. Note that this
        /// function currently requires that max - min >= 5! i.e. it 
        /// does not work for too small source tensors.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup5<long>>
            Index5SamplesClamped = (i, first, end, d) =>
            {
                long i0 = i - first, i1 = i - end + 1;
                long d2 = d + d;
                if (i0 < 2)
                {
                    switch (i0)
                    {
                        case 1: return new Tup5<long>(-d, -d, 0L, d, d2);
                        case 0: return new Tup5<long>(0L, 0L, 0L, d, d2);
                        case -1: return new Tup5<long>(d, d, d, d, d2);
                        default: return new Tup5<long>(-i0 * d);
                    }
                }
                if (i1 > -2)
                {
                    switch (i1)
                    {
                        case -1: return new Tup5<long>(-d2, d, 0L, d, d);
                        case 0: return new Tup5<long>(-d2, d, 0L, 0L, 0L);
                        case 1: return new Tup5<long>(-d2, -d, -d, -d, -d);
                        default: return new Tup5<long>(-i1 * d);
                    }
                }
                return new Tup5<long>(-d2, -d, 0L, d, d2);
            };


        /// <summary>
        /// Provides sample clamping to the border value. Note that this
        /// function currently requires that max - min >= 5! i.e. it 
        /// does not work for too small source tensors.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup6<long>>
            Index6SamplesClamped = (i, first, end, d) =>
            {
                long i0 = i - first, i1 = i - end + 1;
                long d2 = d + d;
                if (i0 < 2)
                {
                    switch (i0)
                    {
                        case 1: return new Tup6<long>(-d, -d, 0L, d, d2, d2 + d);
                        case 0: return new Tup6<long>(0L, 0L, 0L, d, d2, d2 + d);
                        case -1: return new Tup6<long>(d, d, d, d, d2, d2 + d);
                        case -2: return new Tup6<long>(d2, d2, d2, d2, d2, d2 + d);
                        default: return new Tup6<long>(-i0 * d);
                    }
                }
                if (i1 > -3)
                {
                    switch (i1)
                    {
                        case -2: return new Tup6<long>(-d2, -d, 0L, d, d2, d2);
                        case -1: return new Tup6<long>(-d2, -d, 0L, d, d, d);
                        case 0: return new Tup6<long>(-d2, -d, 0L, 0L, 0L, 0L);
                        case 1: return new Tup6<long>(-d2, -d, -d, -d, -d, -d);
                        default: return new Tup6<long>(-i1 * d);
                    }
                }
                return new Tup6<long>(-d2, -d, 0L, d, d2, d2 + d);
            };

        /// <summary>
        /// Provides sample clamping to the border value. Note that this
        /// function currently requires that max - min >= 5! i.e. it 
        /// does not work for too small source tensors.
        /// </summary>
        public static readonly Func<long, long, long, long, Tup7<long>>
            Index7SamplesClamped = (i, first, end, d) =>
            {
                long i0 = i - first, i1 = i - end + 1;
                long d2 = d + d;
                if (i0 < 3)
                {
                    switch (i0)
                    {
                        case 2: return new Tup7<long>(-d2, -d2, -d, 0L, d, d2, d2 + d);
                        case 1: return new Tup7<long>(-d, -d, -d, 0L, d, d2, d2 + d);
                        case 0: return new Tup7<long>(0L, 0L, 0L, 0L, d, d2, d2 + d);
                        case -1: return new Tup7<long>(d, d, d, d, d, d2, d2 + d);
                        case -2: return new Tup7<long>(d2, d2, d2, d2, d2, d2, d2 + d);
                        default: return new Tup7<long>(-i0 * d);
                    }
                }
                if (i1 > -3)
                {
                    switch (i1)
                    {
                        case -2: return new Tup7<long>(-d2 - d, -d2, -d, 0L, d, d2, d2);
                        case -1: return new Tup7<long>(-d2 - d, -d2, -d, 0L, d, d, d);
                        case 0: return new Tup7<long>(-d2 - d, -d2, -d, 0L, 0L, 0L, 0L);
                        case 1: return new Tup7<long>(-d2 - d, -d2, -d, -d, -d, -d, -d);
                        case 2: return new Tup7<long>(-d2 - d, -d2, -d2, -d2, -d2, -d2, -d2);
                        default: return new Tup7<long>(-i1 * d);
                    }
                }
                return new Tup7<long>(-d2 - d, -d2, -d, 0L, d, d2, d2 + d);
            };

    }

    /// <summary>
    /// Generic tensor of elements in r dimensons, each of arbitrary size
    /// with arbitrary strides in each dimension. All sizes are given as
    /// r-dimensional array of longs, with the dimensions ordered from
    /// inner to outer dimension.
    /// The tensor does not exclusively own its underlying data array, it
    /// can also serve as a window into other arrays, matrices, volumes, and    
    /// tensors. Operations on tensors are supported by function arguments
    /// which can be easily exploited by using lambda functions.
    /// Note: stride is called Delta (or D) within this data structure.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public struct Tensor<T> : IValidity, ITensor<T>, IArrayTensorN
    {
        private T[] m_data;
        private long m_origin;
        private int m_rank;
        private long[] m_size;
        private long[] m_delta;

        #region Constructors

        public Tensor(long[] size)
        {
            m_rank = size.Length;
            m_size = size.Copy();            
            long total;
            m_delta = Tools.DenseDelta(m_size, out total);
            m_data = new T[total];
            m_origin = 0;
        }

        public Tensor(int[] size)
            : this(size.Map(x => (long)x))
        { }

        public Tensor(long[] size, T value)
        {
            m_rank = size.Length;
            m_size = size.Copy();
            long total;
            m_delta = Tools.DenseDelta(m_size, out total);
            m_data = new T[total];
            for (long i = 0; i < total; i++) m_data[i] = value;
            m_origin = 0;
        }

        public Tensor(int[] size, T value)
            : this(size.Map(x => (long)x), value)
        { }

        public Tensor(T[] data, long origin, int[] size)
        {
            m_data = data;
            m_rank = (int)size.Length;
            m_origin = origin;
            m_size = size.Map(x => (long)x);
            long total;
            m_delta = Tools.DenseDelta(m_size, out total);
        }

        public Tensor(T[] data, long origin, long[] size)
        {
            m_data = data;
            m_rank = size.Length;
            m_origin = origin;
            m_size = size.Copy();
            long total;
            m_delta = Tools.DenseDelta(size, out total);
        }

        public Tensor(
                T[] data, long origin,
                long[] size, long[] delta
                )
        {
            m_data = data;
            m_rank = size.Length;
            m_origin = origin;
            m_size = size.Copy();
            m_delta = delta.Copy();
        }

        #endregion

        #region Properties

        public readonly bool IsValid { get { return m_data != null; } }
        public readonly bool IsInvalid { get { return m_data == null; } }

        /// <summary>
        /// Underlying data array. Not exclusively owned by the tensor.
        /// </summary>
        public readonly T[] Data { get { return m_data; } }

        /// <summary>
        /// Origin index in the unerlying data array.
        /// </summary>
        public long Origin { readonly get { return m_origin; } set { m_origin = value; }  }

        /// <summary>
        /// The total number of contravariant and covariant indices.
        /// </summary>
        public readonly int Rank { get { return m_rank; } }

        /// <summary>
        /// Length is copied out and in, emulating a value property.
        /// Setting the size property also sets the rank of the tensor.
        /// </summary>
        public long[] Size
        {
            readonly get { return m_size.Copy(); }
            set { m_size = value.Copy(); m_rank = m_size.Length; }
        }

        public readonly long Count
        {
            get
            {
                long c = 1;
                foreach (long len in m_size) c *= len;
                return c;
            }
        }

        /// <summary>
        /// Delta is copied out and in, emulating a value property.
        /// </summary>
        public long[] Delta
        {
            readonly get { return m_delta.Copy(); }
            set
            {
                if (m_delta.Length != m_rank)
                    throw new ArgumentException("delta of wrong rank");
                m_delta = value.Copy();
            }
        }

        public long OriginIndex { readonly get { return m_origin; } set { m_origin = value; } }

        public long[] SizeArray
        {
            readonly get { return Size; }
            set { Size = value; }
        }

        public long[] DeltaArray
        {
            readonly get { return Delta; }
            set { Delta = value; }
        }

        public long[] FirstArray
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Rank
        /// </summary>
        public readonly int R { get { return m_rank; } }
        /// <summary>
        /// Length
        /// </summary>
        public readonly long[] L { get { return m_size; } }
        /// <summary>
        /// Delta
        /// </summary>
        public readonly long[] D { get { return m_delta; } }

        /// <summary>
        /// Yields all elemnts ordered by index.
        /// </summary>
        public readonly IEnumerable<T> Elements
        {
            get
            {
                long i = Origin; var ve = Init();
                for (long ee = ve[R - 1]; i != ee; i = Next(i, ref ve))
                {
                    for (long e = ve[0], d = D[0]; i != e; i += d)
                        yield return m_data[i];
                }
            }
        }

        public readonly Type ArrayType
        {
            get { return typeof(T[]); }
        }

        public Array Array
        {
            readonly get { return m_data; }
            set { m_data = (T[])value; }
        }

        #endregion

        #region Indexers

        public readonly T this[Vector<int> vi]
        {
            get { return m_data[Index(vi)]; }

            set { m_data[Index(vi)] = value; }
        }

        public readonly T this[Vector<long> vi]
        {
            get { return m_data[Index(vi)]; }

            set { m_data[Index(vi)] = value; }
        }

        public readonly T this[params int[] vi]
        {
            get { return m_data[Index(vi)]; }

            set { m_data[Index(vi)] = value; }
        }

        public readonly T this[params long[] vi]
        {
            get { return m_data[Index(vi)]; }

            set { m_data[Index(vi)] = value; }
        }

        #endregion

        #region Indexing Helper Methods

        public readonly long[] Init()
        {
            var ve = new long[R];
            for (int r = 0; r < R; r++)
                ve[r] = Origin + DLR(r);
            return ve;
        }
        public readonly long Next(long i, ref long[] ve)
        {
            for (int r = 1; r < R; r++)
            {
                i += SR(r);
                if (i == ve[r]) continue;
                while (--r >= 0)
                    ve[r] = i + DLR(r);
                break;
            }
            return i;
        }

        public readonly long Next(long i, ref long[] ve, ref long[] vi)
        {
            for (int r = 1; r < R; r++)
            {
                i += SR(r); vi[r]++;
                if (i == ve[r]) continue;
                while (--r >= 0)
                {
                    ve[r] = i + DLR(r);
                    vi[r] = 0;
                }
                break;
            }
            return i;
        }

        /// <summary>
        /// Cummulative delta for all elements in dimension r.
        /// </summary>
        public readonly long DLR(int r) { return m_size[r] * m_delta[r]; }

        /// <summary>
        /// Skip this many elements in the underlying data array when
        /// stepping between subtensors of rank r (required: r > 0).
        /// </summary>
        public readonly long SR(int r) { return m_delta[r] - m_size[r - 1] * m_delta[r - 1]; }

        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        /// <param name="vi"></param>
        public readonly long Index(Vector<int> vi)
        {
            var i = Origin;
            for (long r = 0; r < R; r++) i += vi[r] * D[r];
            return i;
        }

        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        /// <param name="vi"></param>
        public readonly long Index(Vector<long> vi)
        {
            var i = Origin;
            for (long r = 0; r < R; r++) i += vi[r] * D[r];
            return i;
        }

        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        public readonly long Index(params int[] vi)
        {
            var i = Origin;
            for (long r = 0; r < R; r++) i += vi[r] * D[r];
            return i;
        }

        /// <summary>
        /// Calculate element index for underlying data array. 
        /// </summary>
        public readonly long Index(params long[] vi)
        {
            var i = Origin;
            for (long r = 0; r < R; r++) i += vi[r] * D[r];
            return i;
        }
        #endregion

        #region Reinterpretation and Parts
        /// <summary>
        /// A SubVector does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public readonly Vector<T> SubVector(long[] origin, long size, long delta)
        {
            return new Vector<T>(m_data, Index(origin), size, delta);
        }

        /// <summary>
        /// A SubMatrix does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public readonly Matrix<T> SubMatrix(long[] origin, V2l size, V2l delta)
        {
            return new Matrix<T>(m_data, Index(origin), size, delta);
        }

        /// <summary>
        /// A SubVolume does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public readonly Volume<T> SubVolume(long[] origin, V3l size, V3l delta)
        {
            return new Volume<T>(m_data, Index(origin), size, delta);
        }

        /// <summary>
        /// A SubTensor does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public readonly Tensor<T> SubTensor(long[] origin, long[] size)
        {
            return new Tensor<T>(m_data, Index(origin), size, m_delta);
        }

        /// <summary>
        /// A SubTensor does not copy any data, and thus any operations on it
        /// are reflected in the corresponding part of the parent.
        /// </summary>
        public readonly Tensor<T> SubTensor(
                long[] origin, long[] size, long[] delta
                )
        {
            return new Tensor<T>(m_data, Index(origin), size, delta);
        }

        #endregion

        #region Copying
        /// <summary>
        /// Elementwise copy.
        /// </summary>
        public readonly Tensor<T> Copy()
        {
            return new Tensor<T>(L).Set(this);
        }

        /// <summary>
        /// Elementwise copy with function application.
        /// </summary>
        public readonly Tensor<Tr> Copy<Tr>(Func<T, Tr> fun)
        {
            return new Tensor<Tr>(L).Set(this, fun);
        }
        #endregion

        #region Manipulation Methods

        public readonly Tensor<T> Set(ITensor<T> t)
        {
            var ve = Init(); var vi = new long[R];
            long i = Origin;
            for (long ee = ve[R - 1]; i != ee; i = Next(i, ref ve, ref vi))
            {
                for (long e = ve[0], d = D[0], ix = 0; i != e; i += d, ix++)
                {
                    vi[0] = ix; m_data[i] = t[vi];
                }
            }
            return this;
        }

        public readonly Tensor<T> Set<T1>(ITensor<T1> t, Func<T1,T> fun)
        {
            var ve = Init(); var vi = new long[R];
            long i = Origin;
            for (long ee = ve[R - 1]; i != ee; i = Next(i, ref ve, ref vi))
            {
                for (long e = ve[0], d = D[0], ix = 0; i != e; i += d, ix++)
                {
                    vi[0] = ix; m_data[i] = fun(t[vi]);
                }
            }
            return this;
        }

        /// <summary>
        /// Set each element to the value of a function of the element coords.
        /// </summary>
        /// <returns>this</returns>
        public readonly Tensor<T> SetByCoord(Func<long[], T> fun)
        {
            var ve = Init(); var vi = new long[R];
            long i = Origin;
            for (long ee = ve[R - 1]; i != ee; i = Next(i, ref ve, ref vi))
            {
                for (long e = ve[0], d = D[0], ix = 0; i != e; i += d, ix++)
                {
                    vi[0] = ix;  m_data[i] = fun(vi);
                }
            }
            return this;
        }

        public readonly Tensor<T> SetByIndex<T1>(Tensor<T1> t1, Func<long, T> fun)
        {
            var ve = Init(); var ve1 = t1.Init();
            long i = Origin, i1 = t1.Origin;
            for (long ee = ve[R - 1]; i != ee;
                      i = Next(i, ref ve), i1 = t1.Next(i1, ref ve1))
            {
                for (long e = ve[0], d = D[0], d1 = t1.D[0];
                          i != e; i += d, i1 += d1)
                    m_data[i] = fun(i1);
            }
            return this;
        }

        /// <summary>
        /// Copy all elements from another tensor.
        /// </summary>
        /// <returns>this</returns>
        public readonly Tensor<T> Set(Tensor<T> t1)
        {
            var ve = Init(); var ve1 = t1.Init();
            long i = Origin, i1 = t1.Origin;
            for (long ee = ve[R - 1]; i != ee;
                      i = Next(i, ref ve), i1 = t1.Next(i1, ref ve1))
            {
                for (long e = ve[0], d = D[0], d1 = t1.D[0];
                          i != e; i += d, i1 += d1)
                    m_data[i] = t1.Data[i1];
            }
            return this;
        }

        /// <summary>
        /// Set the elements of a tensor to the result of a function of
        /// the elements of the supplied tensor.
        /// </summary>
        /// <returns>this</returns>
        public readonly Tensor<T> Set<T1>(Tensor<T1> t1, Func<T1, T> fun)
        {
            var ve = Init(); var ve1 = t1.Init();
            long i = Origin, i1 = t1.Origin;
            for (long ee = ve[R - 1]; i != ee;
                      i = Next(i, ref ve), i1 = t1.Next(i1, ref ve1))
            {
                for (long e = ve[0], d = D[0], d1 = t1.D[0];
                          i != e; i += d, i1 += d1)
                    m_data[i] = fun(t1.Data[i1]);
            }
            return this;
        }

        /// <summary>
        /// Set the elements of a tensor to the result of a function of
        /// corresponding pairs of elements of the two supplied tensors.
        /// </summary>
        /// <returns>this</returns>
        public readonly Tensor<T> Set<T1, T2>(
                Tensor<T1> t1, Tensor<T2> t2, Func<T1, T2, T> fun)
        {
            var ve = Init(); var ve1 = t1.Init(); var ve2 = t2.Init();
            long i = Origin, i1 = t1.Origin, i2 = t2.Origin;
            for (long ee = ve[R - 1]; i != ee;
                      i = Next(i, ref ve), i1 = t1.Next(i1, ref ve1),
                      i2 = t2.Next(i2, ref ve2))
            {
                for (long e = ve[0], d = D[0], d1 = t1.D[0], d2 = t2.D[0];
                          i != e; i += d, i1 += d1, i2 += d2)
                    m_data[i] = fun(t1.Data[i1], t2.Data[i2]);
            }
            return this;
        }

        /// <summary>
        /// Set the elements of a tensor to the result of a function of
        /// corresponding triples of elements of the three supplied tensors.
        /// </summary>
        /// <returns>this</returns>
        public readonly Tensor<T> Set<T1, T2, T3>(
                Tensor<T1> t1, Tensor<T2> t2, Tensor<T3> t3,
                Func<T1, T2, T3, T> fun)
        {
            var ve = Init(); var ve1 = t1.Init();
            var ve2 = t2.Init(); var ve3 = t3.Init();
            long i = Origin, i1 = t1.Origin, i2 = t2.Origin, i3 = t3.Origin;
            for (long ee = ve[R - 1]; i != ee;
                      i = Next(i, ref ve), i1 = t1.Next(i1, ref ve1),
                      i2 = t2.Next(i2, ref ve2), i3 = t3.Next(i3, ref ve3))
            {
                for (long e = ve[0], d = D[0], d1 = t1.D[0],
                          d2 = t2.D[0], d3 = t3.D[0];
                          i != e; i += d, i1 += d1, i2 += d2, i3 += d3)
                    m_data[i] = fun(t1.Data[i1], t2.Data[i2], t3.Data[i3]);
            }
            return this;
        }
        #endregion

        #region Creator Functions

        public static Tensor<T> Create(ITensor<T> t)
        {
            return new Tensor<T>(t.Dim).Set(t);
        }

        public static Tensor<T> Create<T1>(ITensor<T1> t, Func<T1, T> fun)
        {
            return new Tensor<T>(t.Dim).Set(t, fun);
        }

        /// <summary>
        /// Create a new tensor by applying a function to each element of the
        /// supplied tensor.
        /// </summary>
        static Tensor<T> Create<T1>(Tensor<T1> t1, Func<T1, T> fun)
        {
            return new Tensor<T>(t1.L).Set(t1, fun);
        }

        /// <summary>
        /// Create a new tensor by applying a function to each corresponding
        /// pair of elements of the two supplied tensors.
        /// </summary>
        static Tensor<T> Create<T1, T2>(Tensor<T1> t1, Tensor<T2> t2, Func<T1, T2, T> fun)
        {
            return new Tensor<T>(t1.L).Set(t1, t2, fun);
        }

        /// <summary>
        /// Create a new tensor by applying a function to each corresponding
        /// triple of elements of the three supplied tensors.
        /// </summary>
        static Tensor<T> Create<T1, T2, T3>(Tensor<T1> t1, Tensor<T2> t2, Tensor<T3> t3, Func<T1, T2, T3, T> fun)
        {
            return new Tensor<T>(t1.L).Set(t1, t2, t3, fun);
        }
        #endregion

        #region ITensor

        public readonly long[] Dim { get { return m_size; } }

        public readonly object GetValue(params long[] v)
        {
            return (object)this[v];
        }

        public void SetValue(object value, params long[] v)
        {
            this[v] = (T)value;
        }

        #endregion
    }

    /// <summary>
    /// Symmetric operations with a scalar or non-tensor result.
    /// </summary>
    public static partial class Tensor
    {
        #region Scalar Functions

        public static Ts InnerProduct<T1, T2, Tm, Ts>(
                Tensor<T1> t1, Tensor<T2> t2, Func<T1, T2, Tm> mulFun,
                Ts bias, Func<Ts, Tm, Ts> sumFun
                )
        {
            Ts result = bias;
            var ve1 = t1.Init(); var ve2 = t2.Init();
            long i1 = t1.Origin, i2 = t2.Origin;
            for (long ee1 = ve1[t1.R - 1]; i1 != ee1;
                      i1 = t1.Next(i1, ref ve1), i2 = t2.Next(i2, ref ve2))
            {
                for (long e1 = ve1[0], d1 = t1.D[0], d2 = t2.D[0];
                          i1 != e1; i1 += d1, i2 += d2)
                    result = sumFun(result, mulFun(t1.Data[i1], t2.Data[i2]));
            }
            return result;
        }

        #endregion    
    }

    public partial struct Matrix<Td>
    {
        public readonly TRes Sample4<T1, TRes>(
                long d0, Tup4<long> d1, Tup4<T1> w,
                FuncRef1<Td, Td, Td, Td, Tup4<T1>, TRes> smp) =>
            smp(Data[d0 + d1.E0], Data[d0 + d1.E1],Data[d0 + d1.E2], Data[d0 + d1.E3], ref w);
    }

    public partial struct Matrix<Td, Tv>
    {

        public readonly TRes Sample4<T1, TRes>(
                long d0, Tup4<long> d1, Tup4<T1> w,
                FuncRef1<Tv, Tv, Tv, Tv, Tup4<T1>, TRes> smp) =>
            smp(Getter(Data, d0 + d1.E0), Getter(Data, d0 + d1.E1),
                Getter(Data, d0 + d1.E2), Getter(Data, d0 + d1.E3), ref w);


        public readonly void SetScaled16InDevelopment<T1, T2, T3>(Matrix<Td, Tv> sourceMat,
                double xScale, double yScale, double xShift, double yShift,
                Func<double, Tup4<T1>> xipl,
                Func<double, Tup4<T2>> yipl,
                FuncRef1<Tv, Tv, Tv, Tv, Tup4<T1>, T3> xsmp,
                FuncRef1<T3, T3, T3, T3, Tup4<T2>, Tv> ysmp,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_xIndexFun,
                Func<long, long, long, long, Tup4<long>> index_min_max_delta_yIndexFun)
        {
            var dxa = new Tup4<long>[SX];
            var wxa = new Tup4<T1>[SX];
            long fx = sourceMat.FX, ex = sourceMat.EX, dx1 = sourceMat.DX;
            for (long tix = 0, tsx = SX; tix < tsx; tix++, xShift += xScale)
            {
                double xid = Fun.Floor(xShift); long xi = (long)xid; double xf = xShift - xid;
                var dx = index_min_max_delta_xIndexFun(xi, fx, ex, dx1);
                var dxi = xi * dx1; dx.E0 += dxi; dx.E1 += dxi; dx.E2 += dxi; dx.E3 += dxi;
                dxa[tix] = dx; wxa[tix] = xipl(xf);
            }

            var mat = new Matrix<T3>(SX, sourceMat.SY);

            {
                var srcdy = sourceMat.DY;
                var data = mat.Data;
                long i = mat.FirstIndex;
                long ys = mat.Info.DSY, yj = mat.Info.JY;
                long xs = mat.Info.SX;
                for (long ye = i + ys, y = FY, dy = sourceMat.Origin; i != ye; i += yj, y++, dy += srcdy)
                    for (long xe = i + xs, x = FX; i != xe; i++, x++)
                        data[i] = sourceMat.Sample4(dy, dxa[x], wxa[x], xsmp);
            }

            var dya = new Tup4<long>[SY];
            var wya = new Tup4<T2>[SY];
            long fy = mat.FY, ey = mat.EY, dy1 = mat.DY;
            for (long tiy = 0, tsy = SY; tiy < tsy; tiy++, yShift += yScale)
            {
                double yid = Fun.Floor(yShift); long yi = (long)yid; double yf = yShift - yid;
                var dy = index_min_max_delta_yIndexFun(yi, fy, ey, dy1);
                var dyi = yi * dy1; dy.E0 += dyi; dy.E1 += dyi; dy.E2 += dyi; dy.E3 += dyi;
                dya[tiy] = dy; wya[tiy] = yipl(yf);
            }
            {
                var matdx = mat.DX;
                var data = Data;
                long i = FirstIndex;
                if (Info.DX == 1)
                {
                    long ys = Info.DSY, yj = Info.JY;
                    long xs = Info.SX;
                    for (long ye = i + ys, y = FY; i != ye; i += yj, y++)
                    {
                        for (long xe = i + xs, x = FX, dx = 0; i != xe; i++, x++, dx += matdx)
                        {
                            Setter(data, i, mat.Sample4(dx, dya[y], wya[y], ysmp));
                        }
                    }
                }
                else if (Info.DY == 1)
                {
                    long xs = Info.DSX, xj = Info.JXY;
                    long ys = Info.SY;
                    for (long xe = i + xs, x = FX, dx = 0; i != xe; i += xj, x++, dx += matdx)
                    {
                        for (long ye = i + ys, y = FY; i != ye; i++, y++)
                        {
                            Setter(data, i, mat.Sample4(dx, dya[y], wya[y], ysmp));
                        }
                    }
                }
                else
                {
                    long ys = Info.DSY, yj = Info.JY;
                    long xs = Info.DSX, xj = Info.JX;
                    for (long ye = i + ys, y = FY; i != ye; i += yj, y++)
                    {
                        for (long xe = i + xs, x = FX, dx = 0; i != xe; i += xj, x++, dx += matdx)
                        {
                            Setter(data, i, mat.Sample4(dx, dya[y], wya[y], ysmp));
                        }
                    }
                }
            }
        }
    }
}
