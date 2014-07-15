using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// An n-dimensional generic vector that can be used for numerical
    /// computations when used with a double type parameter. Note that
    /// the vector acts as a facade structure to an arbitrarily sized
    /// data array. This makes it possible to easily store multiple such
    /// vectors in the same data array. For this reason, modifying the
    /// vector will change the referenced data array. All operations
    /// that do that are prefixed with "Set...", including setting the
    /// individual data elements of the vector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Vec<T> : IVector<T>, IEnumerable<T>
    {
        internal T[] m_data;
        internal long m_origin;
        internal long m_dim;

        #region Constructors

        public Vec(T[] data, long origin, long dim)
        {
            m_data = data;
            m_origin = origin;
            m_dim = dim;
        }

        public Vec(long dim)
        {
            m_data = new T[dim];
            m_origin = 0;
            m_dim = dim;
        }

        public Vec(params T[] items)
        {
            m_data = items.Copy();
            m_origin = 0;
            m_dim = items.Length;
        }

        #endregion

        #region Constants

        public static readonly Func<Vec<T>, long, T> Getter =
                (v, i) => v.m_data[v.m_origin + i];
        public static readonly Action<Vec<T>, long, T> Setter =
                (v, i, s) => v.m_data[v.m_origin + i] = s;

        #endregion

        #region Properties

        public long Dim
        {
            get { return m_dim; }
        }

        #endregion

        #region Indexers

        public T this[long index]
        {
            get { return m_data[m_origin + index]; }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Manipulations

        public void Set(long index, T value)
        {
            m_data[m_origin + index] = value;
        }

        public Vec<T> Copy()
        {
            return new Vec<T>(m_dim).Set(this);
        }

        public Vec<T1> Copy<T1>(Func<T,T1> element_fun)
        {
            return new Vec<T1>(m_dim).Set(this, element_fun);
        }

        public Vec<T1> CopyByIndex<T1>(Func<T, long, T1> element_index_fun)
        {
            return new Vec<T1>(m_dim).SetByIndex(this, element_index_fun);
        }

        public Vec<T> Set(T s1)
        {
            for (long i0 = m_origin, e0 = m_origin + m_dim; i0 != e0; i0++)
                m_data[i0] = s1;
            return this;
        }

        public Vec<T> Set(Func<T, T> element_fun)
        {
            for (long i0 = m_origin, e0 = m_origin + m_dim; i0 != e0; i0++)
                m_data[i0] = element_fun(m_data[i0]);
            return this;
        }

        public Vec<T> Set(Vec<T> v1)
        {
            for (long i0 = m_origin, e0 = m_origin + m_dim, i1 = v1.m_origin;
                 i0 != e0; i0++, i1++)
                m_data[i0] = v1.m_data[i1];
            return this;
        }

        public Vec<T> Set<T1>(Vec<T1> v1, Func<T1, T> element1_fun)
        {
            for (long i0 = m_origin, e0 = m_origin + m_dim, i1 = v1.m_origin;
                 i0 != e0; i0++, i1++)
                m_data[i0] = element1_fun(v1.m_data[i1]);
            return this;
        }

        public Vec<T> Set<T1>(Vec<T1> v1, Func<T, T1, T> element_element1_fun)
        {
            for (long i0 = m_origin, e0 = m_origin + m_dim, i1 = v1.m_origin;
                 i0 != e0; i0++, i1++)
                m_data[i0] = element_element1_fun(m_data[i0], v1.m_data[i1]);
            return this;
        }

        public Vec<T> Set<T1, T2>(
                Vec<T1> v1, Vec<T2> v2, Func<T1, T2, T> e1_e2_fun)
        {
            for (long i0 = m_origin, e0 = m_origin + m_dim,
                 i1 = v1.m_origin, i2 = v2.m_origin;
                 i0 != e0; i0++, i1++, i2++)
                m_data[i0] = e1_e2_fun(v1.m_data[i1], v2.m_data[i2]);
            return this;
        }

        public Vec<T> Set<T1, T2, T3>(
                Vec<T1> v1, Vec<T2> v2, Vec<T3> v3, Func<T1, T2, T3, T> e1_e2_e3_fun)
        {
            for (long i0 = m_origin, e0 = m_origin + m_dim,
                 i1 = v1.m_origin, i2 = v2.m_origin, i3 = v3.m_origin;
                 i0 != e0; i0++, i1++, i2++, i3++)
                m_data[i0] = e1_e2_e3_fun(v1.m_data[i1], v2.m_data[i2], v3.m_data[i3]);
            return this;
        }

        public Vec<T> SetByIndex(
                Func<long, T> index_fun)
        {
            for (long i = 0, i0 = m_origin, e0 = m_origin + m_dim;
                 i0 != e0; i++, i0++)
                m_data[i0] = index_fun(i);
            return this;
        }

        public Vec<T> SetByIndex<T1>(
                Vec<T1> v1, Func<T1, long, T> e1_index_fun)
        {
            for (long i = 0, i0 = m_origin, e0 = m_origin + m_dim, i1 = v1.m_origin;
                 i0 != e0; i++, i0++, i1++)
                m_data[i] = e1_index_fun(v1.m_data[i1], i);
            return this;
        }

        public Vec<T> SetByIndex<T1, T2>(
                Vec<T1> v1, Vec<T2> v2, Func<T1, T2, long, T> e1_e2_index_fun)
        {
            for (long i = 0, i0 = m_origin, e0 = m_origin + m_dim,
                 i1 = v1.m_origin, i2 = v2.m_origin;
                 i0 != e0; i++, i0++, i1++, i2++)
                m_data[i0] = e1_e2_index_fun(v1.m_data[i1], v2.m_data[i2], i);
            return this;
        }

        public Vec<T> SetByIndex<T1, T2, T3>(
                Vec<T1> v1, Vec<T2> v2, Vec<T3> v3, Func<T1, T2, T3, long, T> e1_e2_e3_index_fun)
        {
            for (long i = 0, i0 = m_origin, e0 = m_origin + m_dim,
                 i1 = v1.m_origin, i2 = v2.m_origin, i3 = v3.m_origin;
                 i0 != e0; i++, i0++, i1++, i2++)
                m_data[i0] = e1_e2_e3_index_fun(v1.m_data[i1], v2.m_data[i2], v3.m_data[i3], i);
            return this;
        }

        public TSum Norm<TSum>(
                TSum bias, Func<TSum, T, TSum> sum_element_fun)
        {
            TSum sum = bias;
            for (long i0 = m_origin, e0 = m_origin + m_dim;
                 i0 != e0; i0++)
                sum = sum_element_fun(sum, m_data[i0]);
            return sum;
        }

        public TSum Norm<TSum>(
                TSum bias, Func<TSum, T, TSum> sum_element_fun,
                Func<TSum, bool> sum_breakIfTrueFun)
        {
            TSum sum = bias;
            for (long i0 = m_origin, e0 = m_origin + m_dim;
                 i0 != e0; i0++)
            {
                sum = sum_element_fun(sum, m_data[i0]);
                if (sum_breakIfTrueFun(sum)) return sum;
            }
            return sum;
        }

        public TSum InnerProduct<T1, TSum, TProd>(
                Vec<T1> v1, Func<T, T1, TProd> element_element1_productFun,
                TSum bias, Func<TSum, TProd, TSum> sum_product_fun)
        {
            TSum sum = bias;
            for (long i0 = m_origin, e0 = m_origin + m_dim, i1 = v1.m_origin;
                 i0 != e0; i0++, i1++)
                sum = sum_product_fun(sum, element_element1_productFun(m_data[i0], v1.m_data[i1]));
            return sum;
        }

        public TSum InnerProduct<T1, TSum, TProd>(
                Vec<T1> v1, Func<T, T1, TProd> element_element1_productFun,
                TSum bias, Func<TSum, TProd, TSum> sum_product_fun,
                Func<TSum, bool> sum_breakIfTrueFun)
        {
            TSum sum = bias;
            for (long i0 = m_origin, e0 = m_origin + m_dim, i1 = v1.m_origin;
                 i0 != e0; i0++, i1++)
            {
                sum = sum_product_fun(sum, element_element1_productFun(m_data[i0], v1.m_data[i1]));
                if (sum_breakIfTrueFun(sum)) return sum;
            }
            return sum;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator<T>(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator<T>(this);
        }

        #endregion

        #region Enumerator<U>

        public struct Enumerator<U> : IEnumerator<U>
        {
            U[] m_data;
            long m_index;
            long m_end;

            public Enumerator(Vec<U> collection)
            {
                m_data = collection.m_data;
                m_index = collection.m_origin - 1;
                m_end = collection.m_origin + collection.m_dim;
            }

            #region IEnumerator<U> Members

            public U Current
            {
                get { return m_data[m_index]; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (++m_index < m_end) return true;
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            #endregion
        }

        #endregion

        #region IVector Members

        public object GetValue(long index)
        {
            return m_data[m_origin + index];
        }

        public void SetValue(object value, long index)
        {
            m_data[m_origin + index] = (T)value;
        }

        #endregion
    }

    /// <summary>
    /// A companion structure to the generic vector structure that holds
    /// arrays of generic vectors. Again this acts as a facade to an
    /// arbitrarily sized data array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct VecArray<T> : IVector<Vec<T>>, IEnumerable<Vec<T>>
    {
        internal T[] m_data;
        internal long m_origin;
        internal long m_dim;
        internal long m_count;

        #region Constructors

        public VecArray(T[] data, long origin, long dim, long count)
        {
            m_data = data;
            m_origin = origin;
            m_dim = dim;
            m_count = count;
        }

        public VecArray(long dim, long count)
            : this (new T[dim * count], 0, dim, count)
        { }

        /// <summary>
        /// Create a new vec array with specified dimension, count and
        /// capacity that specifies an expected count after additional
        /// elements are added. Note that the capacity need not be exact,
        /// as the array will be resized if necessary.
        /// </summary>
        public VecArray(long dim, long count, long capacity)
            : this(new T[dim * capacity], 0, dim, count)
        { }

        #endregion

        #region Constants

        public static readonly Func<VecArray<T>, long, Vec<T>> Getter =
                (va, i) => new Vec<T>(va.m_data, va.m_origin + i * va.m_dim, va.m_dim);

        public static readonly Action<VecArray<T>, long, Vec<T>> Setter =
                (va, i, v) => new Vec<T>(va.m_data, va.m_origin + i * va.m_dim, va.m_dim).Set(v);

        #endregion

        #region Properties

        public long Dim
        {
            get { return m_dim; }
        }

        public long Count
        {
            get { return m_count; }
        }

        #endregion

        #region Indexers

        public Vec<T> this[long index]
        {
            get { return new Vec<T>(m_data, m_origin + index * m_dim, m_dim); }
            set { new Vec<T>(m_data, m_origin + index * m_dim, m_dim).Set(value); }
        }

        #endregion

        #region Operations

        public VecArray<T> Copy()
        {
            return new VecArray<T>(m_dim, m_count).Set(this);
        }

        public VecArray<T1> Copy<T1>(Func<Vec<T>, Vec<T1>> element_fun)
        {
            return new VecArray<T1>(m_dim, m_count).Set(this, element_fun);
        }

        public VecArray<T1> Copy<T1>(Func<Vec<T>, Vec<T1>> element_fun, long newDim)
        {
            return new VecArray<T1>(newDim, m_count).Set(this, element_fun);
        }

        public VecArray<T> ForEach(Action<Vec<T>> vec_act)
        {
            for (long i = 0, index = m_origin; i < m_count; i++, index += m_dim)
                vec_act(new Vec<T>(m_data, index, m_dim));
            return this;
        }

        public VecArray<T> ForeachIndex(Action<Vec<T>, long> vec_index_act)
        {
            for (long i = 0, index = m_origin; i < m_count; i++, index += m_dim)
                vec_index_act(new Vec<T>(m_data, index, m_dim), i);
            return this;
        }

        public VecArray<T> SubArray(long begin)
        {
            return new VecArray<T>(m_data, m_origin + m_dim * begin, m_dim, m_count - begin);
        }

        public VecArray<T> SubArray(long begin, long count)
        {
            return new VecArray<T>(m_data, m_origin + m_dim * begin, m_dim, count);
        }

        public VecArray<T> Set(VecArray<T> va1)
        {
            if (m_dim != va1.m_dim) throw new ArgumentException();
            if (m_count != va1.m_count) throw new ArgumentException();
            if (m_origin == va1.m_origin)
            {
                for (long index = m_origin, end = m_origin + m_dim * m_count; index != end; index++)
                    m_data[index] = va1.m_data[index];
            }
            else
            {
                for (long index = m_origin, end = m_origin + m_dim * m_count, index1 = va1.m_origin;
                     index != end; index++, index1++)
                    m_data[index] = va1.m_data[index1];
            }
            return this;
        }

        public VecArray<T> Set<T1>(VecArray<T1> va1, Func<Vec<T1>, Vec<T>> vec_fun)
        {
            if (m_count != va1.m_count) throw new ArgumentException();
            var data = m_data;
            var data1 = va1.m_data;
            if (m_dim == va1.m_dim && m_origin == va1.m_origin)
            {
                for (long index = m_origin, end = m_origin + m_dim * m_count, dim = m_dim;
                     index != end; index += dim)
                    new Vec<T>(data, index, dim).Set(vec_fun(new Vec<T1>(data1, index, dim)));
            }
            else
            {
                for (long index = m_origin, end = m_origin + m_dim * m_count,
                      index1 = va1.m_origin, dim = m_dim, dim1 = va1.m_dim;
                     index != end; index += dim, index1 += dim1)
                    new Vec<T>(data, index, dim).Set(vec_fun(new Vec<T1>(data1, index1, dim1)));
            }
            return this;
        }

        public VecArray<T> Set(IEnumerable<Vec<T>> vecs)
        {
            long index = m_origin;
            long end = m_origin + m_dim * m_count;
            foreach (var v in vecs)
            {
                new Vec<T>(m_data, index, m_dim).Set(v);
                index += m_dim;
                if (index >= end) break;
            }
            return this;
        }

        public VecArray<T> SetByIndex(Func<long, Vec<T>> index_fun)
        {
            for (long i = 0, index = m_origin; i < m_count; i++, index += m_dim)
                new Vec<T>(m_data, index, m_dim).Set(index_fun(i));
            return this;
        }

        private void Resize(long newCount)
        {
            var capacity = Fun.Max(2 * m_count, newCount);
            var data = new T[capacity * m_dim];
            var oldLen = m_count * m_dim;
            for (long i = 0, i0 = m_origin; i < oldLen; i++, i0++)
                data[i] = m_data[i0];
            m_data = data;
            m_origin = 0;
        }

        /// <summary>
        /// Standard List functionality. WARNING: Do not use this method on
        /// a SubArray, as this will affect the parent array in non-ovious
        /// ways.
        /// </summary>
        public void Add(Vec<T> v)
        {
            long end = m_origin + m_count * m_dim;
            long newCount = m_count + 1;
            if (end + m_dim > m_data.Length) Resize(newCount);
            new Vec<T>(m_data, end, m_dim).Set(v);
            m_count = newCount;
        }

        /// <summary>
        /// Standard List functionality. WARNING: Do not use this method on
        /// a SubArray, as this will affect the parent array in non-ovious
        /// ways.
        /// </summary>
        public void AddRange(IEnumerable<Vec<T>> vecs)
        {
            long newCount = m_count + vecs.Count();
            if (m_origin + newCount * m_dim > m_data.Length) Resize(newCount);
            long end = m_origin + m_count * m_dim;
            foreach (var v in vecs)
            {
                new Vec<T>(m_data, end, m_dim).Set(v);
                end += m_dim;
            }
            m_count = newCount;
        }

        /// <summary>
        /// Standard List functionality. WARNING: Do not use this method on
        /// a SubArray, as this will affect the parent array in non-ovious
        /// ways.
        /// </summary>
        public void RemoveAt(int index)
        {
            var pos = m_origin + index * m_dim;
            var end = m_origin + m_count * m_dim;
            var nextPos = pos + m_dim;
            while (nextPos < end)
                m_data[pos++] = m_data[nextPos++];
            --m_count;
        }

        #endregion

        #region IEnumerable<Vec<T>> Members

        public IEnumerator<Vec<T>> GetEnumerator()
        {
            return new Enumerator<T>(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator<T>(this);
        }

        #endregion

        #region Enumerator<U>

        public struct Enumerator<U> : IEnumerator<Vec<U>>
        {
            U[] m_data;
            long m_position;
            long m_dim;
            long m_end;

            public Enumerator(VecArray<U> collection)
            {
                m_data = collection.m_data;
                m_dim = collection.m_dim;
                m_position = collection.m_origin - m_dim;
                m_end = collection.m_origin + collection.m_count * m_dim;
            }

            #region IEnumerator<Vec<U>> Members

            public Vec<U> Current
            {
                get { return new Vec<U>(m_data, m_position, m_dim); }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                m_position += m_dim;
                if (m_position < m_end) return true;
                m_position = long.MinValue;
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            #endregion
        }

        #endregion

        #region IVector Members

        public object GetValue(long index)
        {
            return new Vec<T>(m_data, m_origin + index * m_dim, m_dim);
        }

        public void SetValue(object value, long index)
        {
            new Vec<T>(m_data, m_origin + index * m_dim, m_dim).Set((Vec<T>)value);
        }

        #endregion
    }

    /// <summary>
    /// A companion bounding box data structure to the generic vector
    /// structure that holds a minimum and a maximum vector that is
    /// computed as the componentwise minimum/maximum of a sequence
    /// of vectors. This works on all vectors of comparable components.
    /// </summary>
    public struct VecBox<T> : ISimpleRange<Vec<T>, VecBox<T>>
        where T : IComparable<T>
    {
        public Vec<T> Min;
        public Vec<T> Max;

        #region Constructors

        public VecBox(Vec<T> min, Vec<T> max)
        {
            Min = min;
            Max = max;
        }

        public VecBox(IEnumerable<Vec<T>> vecs)
        {
            Min = vecs.MinVec();
            Max = vecs.MaxVec();
        }

        #endregion

        #region Properties

        public bool IsEmpty
        {
            get { return Min.AnyGreaterOrEqual(Max); }
        }

        public bool IsNonEmpty
        {
            get { return Min.AllSmaller(Max); }
        }

        public bool IsValid
        {
            get { return Min.AllSmallerOrEqual(Max); }
        }

        public bool IsInvalid
        {
            get { return Min.AnyGreater(Max); }
        }

        #endregion

        #region Box Arithmetics

        public VecBox<T> Repair()
        {
            if (IsInvalid)
            {
                var min = Min.MinVec(Max);
                var max = Min.MaxVec(Max);
                Min = min;
                Max = max;
            }
            return this;
        }

        public VecBox<T> ExtendedBy(Vec<T> value)
        {
            return new VecBox<T>(Min.MinVec(value), Max.MaxVec(value));
        }

        public VecBox<T> ExtendedBy(VecBox<T> box)
        {
            return new VecBox<T>(Min.MinVec(box.Min), Max.MaxVec(box.Max));
        }

        public void ExtendBy(Vec<T> value)
        {
            Min.SetMin(value);
            Max.SetMax(value);
        }

        public void ExtendBy(VecBox<T> box)
        {
            Min.SetMin(box.Min);
            Max.SetMax(box.Max);
        }

        public bool Contains(Vec<T> value)
        {
            return Min.AllSmallerOrEqual(value) && Max.AllGreaterOrEqual(value);
        }

        public bool Contains(VecBox<T> box)
        {
            return Min.AllSmallerOrEqual(box.Min) && Max.AllGreaterOrEqual(box.Max);
        }

        public bool Intersects(VecBox<T> range)
        {
            return (Max.AllGreaterOrEqual(range.Min) && Min.AllSmallerOrEqual(range.Max));
        }

        /// <summary>
        /// Return the corner of the box with the given index. The corner
        /// index is constructed in such a way, that bit 0 enumerates the
        /// 0th coordinate (0 ... min, 1 ... max), bit 1 enumerates the 1st
        /// coordinate, aso.
        /// </summary>
        public Vec<T> Corner(long ci)
        {
            long dim = Min.Dim;
            if (dim >= 64) throw new ArgumentException(); // not possible otherwise
            var c = new Vec<T>(dim);
            var min = Min; var max = Max;
            c.SetByIndex(i => (ci & (1L << (int)i)) == 0 ? min[i] : max[i]);
            return c;
        }

        /// <summary>
        /// Computes the corners of the box and returns them in an array.
        /// The order of the corners is determined by their index which is
        /// constructed as in the method <see cref="Corner"/>.
        /// </summary>
        public VecArray<T> ComputeCorners()
        {
            int dim = (int)Min.Dim;
            if (dim >= 32) throw new ArgumentException(); // more does not realisically make sense
            long count = 1L << dim;
            var ca = new VecArray<T>(dim, count);
            var min = Min; var max = Max;
            ca.ForeachIndex((c, ci) =>
                c.SetByIndex(i => (ci & (1L << (int)i)) == 0 ? min[i] : max[i]));
            return ca;
        }

        #endregion
    }

    public static class VecExtensions
    {
        #region Setters

        /// <summary>
        /// Sets the vector to be the componentwise minimum of the vector
        /// and the other vector.
        /// </summary>
        public static void SetMin<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            v0.Set(v1, (x0, x1) => x0.CompareTo(x1) < 0 ? x0 : x1);
        }

        /// <summary>
        /// Sets the vector to be the componentwise minimum of the vector
        /// and the other vector.
        /// </summary>
        public static void SetMax<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            v0.Set(v1, (x0, x1) => x0.CompareTo(x1) > 0 ? x0 : x1);
        }

        #endregion

        #region Vector-Valued Binary Operations

        /// <summary>
        /// Returns the componentwise minimum vector.
        /// </summary>
        public static Vec<T> MinVec<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return new Vec<T>(v0.m_dim).Set(v0, v1, (x0, x1) => x0.CompareTo(x1) < 0 ? x0 : x1);
        }

        /// <summary>
        /// Returns the componentwise maximum vector.
        /// </summary>
        public static Vec<T> MaxVec<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return new Vec<T>(v0.m_dim).Set(v0, v1, (x0, x1) => x0.CompareTo(x1) > 0 ? x0 : x1);
        }

        #endregion

        #region Vector-Valued N-ary Operations

        /// <summary>
        /// Returns the componentwise minimum vector.
        /// </summary>
        public static Vec<T> MinVec<T>(this IEnumerable<Vec<T>> vecs)
            where T : IComparable<T>
        {
            var e = vecs.GetEnumerator();
            e.MoveNext();
            var min = e.Current.Copy();
            while (e.MoveNext()) min.SetMin(e.Current);
            return min;
        }

        /// <summary>
        /// Returns the componentwise maximum vector.
        /// </summary>
        public static Vec<T> MaxVec<T>(this IEnumerable<Vec<T>> vecs)
            where T : IComparable<T>
        {
            var e = vecs.GetEnumerator();
            e.MoveNext();
            var min = e.Current.Copy();
            while (e.MoveNext()) min.SetMax(e.Current);
            return min;
        }

        #endregion

        #region Componentwise Comparisons

        public static bool AllSmaller<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(true, (b, x0) => b && x0.CompareTo(x1) < 0);
        }

        public static bool AllSmaller<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(true, (b, x1) => b && x0.CompareTo(x1) < 0);
        }

        public static bool AllSmaller<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) < 0, true, (b, c) => b && c);
        }

        public static bool AnySmaller<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(false, (b, x0) => b || x0.CompareTo(x1) < 0);
        }

        public static bool AnySmaller<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(false, (b, x1) => b || x0.CompareTo(x1) < 0);
        }

        public static bool AnySmaller<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) < 0, false, (b, c) => b || c);
        }

        public static bool AllGreater<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(true, (b, x0) => b && x0.CompareTo(x1) > 0);
        }

        public static bool AllGreater<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(true, (b, x1) => b && x0.CompareTo(x1) > 0);
        }

        public static bool AllGreater<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) > 0, true, (b, c) => b && c);
        }

        public static bool AnyGreater<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(false, (b, x0) => b || x0.CompareTo(x1) > 0);
        }

        public static bool AnyGreater<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(false, (b, x1) => b || x0.CompareTo(x1) > 0);
        }

        public static bool AnyGreater<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) > 0, false, (b, c) => b || c);
        }

        public static bool AllSmallerOrEqual<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(true, (b, x0) => b && x0.CompareTo(x1) <= 0);
        }

        public static bool AllSmallerOrEqual<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(true, (b, x1) => b && x0.CompareTo(x1) <= 0);
        }

        public static bool AllSmallerOrEqual<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) <= 0, true, (b, c) => b && c);
        }

        public static bool AnySmallerOrEqual<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(false, (b, x0) => b || x0.CompareTo(x1) <= 0);
        }

        public static bool AnySmallerOrEqual<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(false, (b, x1) => b || x0.CompareTo(x1) <= 0);
        }

        public static bool AnySmallerOrEqual<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) <= 0, false, (b, c) => b || c);
        }

        public static bool AllGreaterOrEqual<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(true, (b, x0) => b && x0.CompareTo(x1) >= 0);
        }

        public static bool AllGreaterOrEqual<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(true, (b, x1) => b && x0.CompareTo(x1) >= 0);
        }

        public static bool AllGreaterOrEqual<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) >= 0, true, (b, c) => b && c);
        }

        public static bool AnyGreaterOrEqual<T>(this Vec<T> v0, T x1)
            where T : IComparable<T>
        {
            return v0.Norm(false, (b, x0) => b || x0.CompareTo(x1) >= 0);
        }

        public static bool AnyGreaterOrEqual<T>(this T x0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v1.Norm(false, (b, x1) => b || x0.CompareTo(x1) >= 0);
        }

        public static bool AnyGreaterOrEqual<T>(this Vec<T> v0, Vec<T> v1)
            where T : IComparable<T>
        {
            return v0.InnerProduct(v1, (x0, x1) => x0.CompareTo(x1) >= 0, false, (b, c) => b || c);
        }

        #endregion
    }

    /// <summary>
    /// This static class contains a number of extension methods on the Vec
    /// with type parameter double. These methods allow standard numerical
    /// methods in arbitrary dimensions. Note that this class does not have
    /// the usual "Extensions" as parts of its name, so that the method name
    /// looks more natural when it is uses as a function parameter.
    /// </summary>
    public static class VecDouble
    {
        #region Constants

        public static Vec<double> NaN(long dim)
        {
            return new Vec<double>(dim).Set(double.NaN);
        }

        public static Vec<double> MinValue(long dim)
        {
            return new Vec<double>(dim).Set(Constant<double>.ParseableMinValue);
        }

        public static Vec<double> MaxValue(long dim)
        {
            return new Vec<double>(dim).Set(Constant<double>.ParseableMaxValue);
        }

        public static Vec<double> Unit(long dim)
        {
            return new Vec<double>(dim).SetByIndex(i => i == dim ? 1.0 : 0.0);
        }

        #endregion

        #region Vector-Valued Unary Operations

        public static Vec<double> Negated(this Vec<double> v0)
        {
            return new Vec<double>(v0.m_dim).Set(v0, x0 => -x0);
        }

        #endregion

        #region Vector-Valued Binary Operations

        public static Vec<double> Plus(this Vec<double> v0, Vec<double> v1)
        {
            return new Vec<double>(v0.m_dim).Set(v0, v1, (a, b) => a + b);
        }

        public static Vec<double> Minus(this Vec<double> v0, Vec<double> v1)
        {
            return new Vec<double>(v0.m_dim).Set(v0, v1, (a, b) => a - b);
        }

        public static Vec<double> MultiplyBy(this Vec<double> v0, double s1)
        {
            return new Vec<double>(v0.m_dim).Set(v0, a => a * s1);
        }

        public static Vec<double> DivideBy(this Vec<double> v0, double s1)
        {
            var f1 = 1.0 / s1;
            return new Vec<double>(v0.m_dim).Set(v0, a => a * f1);
        }

        #endregion

        #region Vector-Valued N-Ary Operations

        public static Vec<double> Sum(this VecArray<double> vecArray)
        {
            Vec<double> result = new Vec<double>(vecArray.Dim).Set(0.0);
            vecArray.ForEach(v => result.SetPlus(v));
            return result;
        }

        public static Vec<double> Sum(this IEnumerable<Vec<double>> vecs)
        {
            Vec<double> result = new Vec<double>(vecs.First().Dim).Set(0.0);
            foreach (var v in vecs) result.SetPlus(v);
            return result;
        }

        public static Vec<double> ComputeCentroid(this VecArray<double> vecArray)
        {
            Vec<double> result = new Vec<double>(vecArray.Dim).Set(0.0);
            long count = vecArray.Count;
            if (count == 0) return result;
            vecArray.ForEach(v => result.SetPlus(v));
            result.SetDivideBy((double)count);
            return result;
        }

        public static Vec<double> ComputeCentroid(this IEnumerable<Vec<double>> vecs)
        {
            Vec<double> result = new Vec<double>(vecs.First().Dim).Set(0.0);
            long count = 0;
            foreach (var v in vecs) { result.SetPlus(v); ++count; }
            if (count > 0) result.SetDivideBy((double)count);
            return result;
        }

        #endregion

        #region Scalar-Valued Binary Operations

        public static double Dot(this Vec<double> v0, Vec<double> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => x0 * x1,
                                   0.0, (s, p) => s + p);
        }

        public static double Dist1(this Vec<double> v0, Vec<double> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0),
                                   0.0, (s, p) => s + p);
        }

        public static double Dist2Squared(this Vec<double> v0, Vec<double> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => Fun.Square(x1 - x0),
                                   0.0, (s, p) => s + p);
        }

        public static double Dist2(this Vec<double> v0, Vec<double> v1)
        {
            return Fun.Sqrt(v0.Dist2Squared(v1));
        }

        public static double DistMax(this Vec<double> v0, Vec<double> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0),
                                   0.0, Fun.Max);
        }

        #endregion

        #region Scalar-Valued Unary Operations

        public static double Min(this Vec<double> v0)
        {
            return v0.Norm(double.MaxValue, (m, x) => Fun.Min(m, x));
        }

        public static double Max(this Vec<double> v0)
        {
            return v0.Norm(double.MinValue, (m, x) => Fun.Max(m, x));
        }

        public static double Norm1(this Vec<double> v0)
        {
            return v0.Norm(0.0, (s, x) => s + Fun.Abs(x));
        }

        public static double Norm2Squared(this Vec<double> v0)
        {
            return v0.Norm(0.0, (s, x) => s + Fun.Square(x));
        }

        public static double Norm2(this Vec<double> v0)
        {
            return Fun.Sqrt(v0.Norm2Squared());
        }

        public static double NormMax(this Vec<double> v0)
        {
            return v0.Norm(0.0, (s, x) => Fun.Max(s, Fun.Abs(x)));
        }

        #endregion

        #region Index-Valued Unary Operations

        public static long MinDim(this Vec<double> v0)
        {
            long d = 0, dim = 0;
            v0.Norm(double.MaxValue, (m, x) => { if (x < m) { dim = d++; return x; } ++d; return m; });
            return dim;
        }

        public static long MaxDim(this Vec<double> v0)
        {
            long d = 0, dim = 0;
            v0.Norm(double.MinValue, (m, x) => { if (x > m) { dim = d++; return x; } ++d; return m; });
            return dim;
        }

        #endregion

        #region Boolean-Valued Unary Operations

        public static bool IsNaN(this Vec<double> v0)
        {
            return v0.Norm(false, (b, x) => b || double.IsNaN(x), b => b);
        }

        #endregion

        #region VecBox Extensions

        public static long GetMajorDim(this VecBox<double> box)
        {
            long dim = box.Min.Dim;
            long majorDim = 0;
            var majorSize = box.Max[0] - box.Min[0];
            for (long d = 1; d < dim; d++)
            {
                var size = box.Max[d] - box.Min[d];
                if (size > majorSize) { majorDim = d; majorSize = size; }
            }
            return majorDim;
        }

        public static long GetMinorDim(this VecBox<double> box)
        {
            long dim = box.Min.Dim;
            long minorDim = 0;
            var minorSize = box.Max[0] - box.Min[0];
            for (long d = 1; d < dim; d++)
            {
                var size = box.Max[d] - box.Min[d];
                if (size < minorSize) { minorDim = d; minorSize = size; }
            }
            return minorDim;
        }

        #endregion

        #region Conversions From Other Types To Vec<double>

        public static Vec<double> ToVec(this V2d v0)
        {
            var v1 = new Vec<double>(2);
            v1.Set(v0);
            return v1;
        }

        public static Vec<double> ToVec(this V3d v0)
        {
            var v1 = new Vec<double>(3);
            v1.Set(v0);
            return v1;
        }

        public static Vec<double> ToVec(this V4d v0)
        {
            var v1 = new Vec<double>(4);
            v1.Set(v0);
            return v1;
        }

        #endregion

        #region Conversions From Vec<double> To Other Types

        public static Vec<float> ToVecFloat(this Vec<double> v0)
        {
            return new Vec<float>(v0.Dim).Set(v0, x => (float)x);
        }

        public static V2d ToV2d(this Vec<double> v0)
        {
            return new V2d(v0.m_data[v0.m_origin],
                           v0.m_data[v0.m_origin + 1]);
        }

        public static V3d ToV3d(this Vec<double> v0)
        {
            return new V3d(v0.m_data[v0.m_origin],
                           v0.m_data[v0.m_origin + 1],
                           v0.m_data[v0.m_origin + 2]);
        }

        public static V4d ToV4d(this Vec<double> v0)
        {
            return new V4d(v0.m_data[v0.m_origin],
                           v0.m_data[v0.m_origin + 1],
                           v0.m_data[v0.m_origin + 2],
                           v0.m_data[v0.m_origin + 3]);
        }

        #endregion

        #region Conversions From Other Types To VecArray<double>

        public static VecArray<double> ToVecArray(this IEnumerable<V2d> vecs)
        {
            long count = vecs.Count();
            var va = new VecArray<double>(2, count);
            long index = 0;
            foreach (var v in vecs) va[index++].Set(v);
            return va;
        }

        public static VecArray<double> ToVecArray(this IEnumerable<V3d> vecs)
        {
            long count = vecs.Count();
            var va = new VecArray<double>(3, count);
            long index = 0;
            foreach (var v in vecs) va[index++].Set(v);
            return va;
        }

        public static VecArray<double> ToVecArray(this IEnumerable<V4d> vecs)
        {
            long count = vecs.Count();
            var va = new VecArray<double>(4, count);
            long index = 0;
            foreach (var v in vecs) va[index++].Set(v);
            return va;
        }

        #endregion

        #region Conversions From VecArray<double> To Other Types

        public static VecArray<float> ToVecArrayFloat(this VecArray<double> va)
        {
            var result = new VecArray<float>(va.Dim, va.Count);
            result.ForeachIndex((v, i) => v.Set(va[i], x => (float)x));
            return result;
        }

        public static IEnumerable<V2d> ToV2ds(this VecArray<double> vecs)
        {
            if (vecs.Dim != 2) throw new ArgumentException();
            foreach (var v in vecs) yield return v.ToV2d();
        }

        public static IEnumerable<V3d> ToV3ds(this VecArray<double> vecs)
        {
            if (vecs.Dim != 3) throw new ArgumentException();
            foreach (var v in vecs) yield return v.ToV3d();
        }

        public static IEnumerable<V4d> ToV4ds(this VecArray<double> vecs)
        {
            if (vecs.Dim != 4) throw new ArgumentException();
            foreach (var v in vecs) yield return v.ToV4d();
        }

        #endregion

        #region Internal Setters

        internal static void SetPlus(this Vec<double> v0, Vec<double> v1)
        {
            v0.Set(v1, (x0, x1) => x0 + x1);
        }

        internal static void SetMinus(this Vec<double> v0, Vec<double> v1)
        {
            v0.Set(v1, (x0, x1) => x0 - x1);
        }

        internal static void SetMultiplyBy(this Vec<double> v0, double s1)
        {
            v0.Set(x0 => x0 * s1);
        }

        internal static void SetDivideBy(this Vec<double> v0, double s1)
        {
            double f = 1.0 / s1;
            v0.Set(x0 => x0 * f);
        }

        internal static void SetPlus(this Vec<double> v0, Vec<float> v1)
        {
            v0.Set(v1, (x0, x1) => x0 + x1);
        }

        internal static void SetMinus(this Vec<double> v0, Vec<float> v1)
        {
            v0.Set(v1, (x0, x1) => x0 - x1);
        }

        internal static void Set(this Vec<double> v0, V2d v1)
        {
            v0.m_data[v0.m_origin] = v1.X;
            v0.m_data[v0.m_origin + 1] = v1.Y;
        }

        internal static void Set(this Vec<double> v0, V3d v1)
        {
            v0.m_data[v0.m_origin] = v1.X;
            v0.m_data[v0.m_origin + 1] = v1.Y;
            v0.m_data[v0.m_origin + 2] = v1.Z;
        }

        internal static void Set(this Vec<double> v0, V4d v1)
        {
            v0.m_data[v0.m_origin] = v1.X;
            v0.m_data[v0.m_origin + 1] = v1.Y;
            v0.m_data[v0.m_origin + 2] = v1.Z;
            v0.m_data[v0.m_origin + 2] = v1.W;
        }

        #endregion
    }

    /// <summary>
    /// This static class contains a number of extension methods on the Vec
    /// with type parameter float. These methods allow standard numerical
    /// methods in arbitrary dimensions. Note that this class does not have
    /// the usual "Extensions" as parts of its name, so that the method name
    /// looks more natural when it is uses as a function parameter.
    /// </summary>
    public static class VecFloat
    {
        #region Constants

        public static Vec<float> NaN(long dim)
        {
            return new Vec<float>(dim).Set(float.NaN);
        }

        public static Vec<float> MinValue(long dim)
        {
            return new Vec<float>(dim).Set(Constant<float>.ParseableMinValue);
        }

        public static Vec<float> MaxValue(long dim)
        {
            return new Vec<float>(dim).Set(Constant<float>.ParseableMaxValue);
        }

        public static Vec<float> Unit(long dim)
        {
            return new Vec<float>(dim).SetByIndex(i => i == dim ? 1.0f : 0.0f);
        }

        #endregion

        #region Vector-Valued Unary Operations

        public static Vec<float> Negated(this Vec<float> v0)
        {
            return new Vec<float>(v0.m_dim).Set(v0, x0 => -x0);
        }

        #endregion

        #region Vector-Valued Binary Operations

        public static Vec<float> Plus(this Vec<float> v0, Vec<float> v1)
        {
            return new Vec<float>(v0.m_dim).Set(v0, v1, (a, b) => a + b);
        }

        public static Vec<float> Minus(this Vec<float> v0, Vec<float> v1)
        {
            return new Vec<float>(v0.m_dim).Set(v0, v1, (a, b) => a - b);
        }

        public static Vec<float> MultiplyBy(this Vec<float> v0, float s1)
        {
            return new Vec<float>(v0.m_dim).Set(v0, a => a * s1);
        }

        public static Vec<float> DivideBy(this Vec<float> v0, float s1)
        {
            var f1 = 1.0f / s1;
            return new Vec<float>(v0.m_dim).Set(v0, a => a * f1);
        }

        #endregion

        #region Vector-Valued N-Ary Operations

        public static Vec<double> Sum(this VecArray<float> vecArray)
        {
            Vec<double> result = new Vec<double>(vecArray.Dim).Set(0.0);
            vecArray.ForEach(v => result.SetPlus(v));
            return result;
        }

        public static Vec<double> Sum(this IEnumerable<Vec<float>> vecs)
        {
            Vec<double> result = new Vec<double>(vecs.First().Dim).Set(0.0);
            foreach (var v in vecs) result.SetPlus(v);
            return result;
        }

        public static Vec<double> ComputeCentroid(this VecArray<float> vecArray)
        {
            Vec<double> result = new Vec<double>(vecArray.Dim).Set(0.0);
            long count = vecArray.Count;
            if (count == 0) return result;
            vecArray.ForEach(v => result.SetPlus(v));
            result.SetDivideBy((double)count);
            return result;
        }

        public static Vec<double> ComputeCentroid(this IEnumerable<Vec<float>> vecs)
        {
            Vec<double> result = new Vec<double>(vecs.First().Dim).Set(0.0);
            long count = 0;
            foreach (var v in vecs) { result.SetPlus(v); ++count; }
            if (count > 0) result.SetDivideBy((double)count);
            return result;
        }

        #endregion

        #region Scalar-Valued Binary Operations

        public static float Dot(this Vec<float> v0, Vec<float> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => x0 * x1,
                                   0.0f, (s, p) => s + p);
        }

        public static float Dist1(this Vec<float> v0, Vec<float> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0),
                                   0.0f, (s, p) => s + p);
        }

        public static double Dist2Squared(this Vec<float> v0, Vec<float> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => Fun.Square(x1 - x0),
                                   0.0, (s, p) => s + p);
        }

        public static float Dist2(this Vec<float> v0, Vec<float> v1)
        {
            return (float)Fun.Sqrt(v0.Dist2Squared(v1));
        }

        public static float DistMax(this Vec<float> v0, Vec<float> v1)
        {
            return v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0),
                                   0.0f, Fun.Max);
        }

        #endregion

        #region Scalar-Valued Unary Operations

        public static float Min(this Vec<float> v0)
        {
            return v0.Norm(float.MaxValue, (m, x) => Fun.Min(m, x));
        }

        public static float Max(this Vec<float> v0)
        {
            return v0.Norm(float.MinValue, (m, x) => Fun.Max(m, x));
        }

        public static float Norm1(this Vec<float> v0)
        {
            return v0.Norm(0.0f, (s, x) => s + Fun.Abs(x));
        }

        public static double Norm2Squared(this Vec<float> v0)
        {
            return v0.Norm(0.0, (s, x) => s + Fun.Square(x));
        }

        public static float Norm2(this Vec<float> v0)
        {
            return (float)Fun.Sqrt(v0.Norm2Squared());
        }

        public static float NormMax(this Vec<float> v0)
        {
            return v0.Norm(0.0f, (s, x) => Fun.Max(s, Fun.Abs(x)));
        }

        #endregion

        #region Index-Valued Unary Operations

        public static long MinDim(this Vec<float> v0)
        {
            long d = 0, dim = 0;
            v0.Norm(float.MaxValue, (m, x) => { if (x < m) { dim = d++; return x; } ++d; return m; });
            return dim;
        }

        public static long MaxDim(this Vec<float> v0)
        {
            long d = 0, dim = 0;
            v0.Norm(float.MinValue, (m, x) => { if (x > m) { dim = d++; return x; } ++d; return m; });
            return dim;
        }

        #endregion

        #region Boolean-Valued Unary Operations

        public static bool IsNaN(this Vec<float> v0)
        {
            return v0.Norm(false, (b, x) => b || float.IsNaN(x), b => b);
        }

        #endregion

        #region VecBox Extensions

        public static long GetMajorDim(this VecBox<float> box)
        {
            long dim = box.Min.Dim;
            long majorDim = 0;
            var majorSize = box.Max[0] - box.Min[0];
            for (long d = 1; d < dim; d++)
            {
                var size = box.Max[d] - box.Min[d];
                if (size > majorSize) { majorDim = d; majorSize = size; }
            }
            return majorDim;
        }

        public static long GetMinorDim(this VecBox<float> box)
        {
            long dim = box.Min.Dim;
            long minorDim = 0;
            var minorSize = box.Max[0] - box.Min[0];
            for (long d = 1; d < dim; d++)
            {
                var size = box.Max[d] - box.Min[d];
                if (size < minorSize) { minorDim = d; minorSize = size; }
            }
            return minorDim;
        }

        #endregion

        #region Conversions From Other Types To Vec<float>

        public static Vec<float> ToVec(this V2f v0)
        {
            var v1 = new Vec<float>(2);
            v1.Set(v0);
            return v1;
        }

        public static Vec<float> ToVec(this V3f v0)
        {
            var v1 = new Vec<float>(3);
            v1.Set(v0);
            return v1;
        }

        public static Vec<float> ToVec(this V4f v0)
        {
            var v1 = new Vec<float>(4);
            v1.Set(v0);
            return v1;
        }

        #endregion

        #region Conversions From Vec<float> To Other Types

        public static Vec<double> ToVecDouble(this Vec<float> v0)
        {
            return new Vec<double>(v0.Dim).Set(v0, x => (double)x);
        }

        public static V2f ToV2f(this Vec<float> v0)
        {
            return new V2f(v0.m_data[v0.m_origin],
                           v0.m_data[v0.m_origin + 1]);
        }

        public static V3f ToV3f(this Vec<float> v0)
        {
            return new V3f(v0.m_data[v0.m_origin],
                           v0.m_data[v0.m_origin + 1],
                           v0.m_data[v0.m_origin + 2]);
        }

        public static V4f ToV4f(this Vec<float> v0)
        {
            return new V4f(v0.m_data[v0.m_origin],
                           v0.m_data[v0.m_origin + 1],
                           v0.m_data[v0.m_origin + 2],
                           v0.m_data[v0.m_origin + 3]);
        }

        #endregion

        #region Conversions From Other Types To VecArray<float>

        public static VecArray<float> ToVecArray(this IEnumerable<V2f> vecs)
        {
            long count = vecs.Count();
            var va = new VecArray<float>(2, count);
            long index = 0;
            foreach (var v in vecs) va[index++].Set(v);
            return va;
        }

        public static VecArray<float> ToVecArray(this IEnumerable<V3f> vecs)
        {
            long count = vecs.Count();
            var va = new VecArray<float>(3, count);
            long index = 0;
            foreach (var v in vecs) va[index++].Set(v);
            return va;
        }

        public static VecArray<float> ToVecArray(this IEnumerable<V4f> vecs)
        {
            long count = vecs.Count();
            var va = new VecArray<float>(4, count);
            long index = 0;
            foreach (var v in vecs) va[index++].Set(v);
            return va;
        }

        #endregion

        #region Conversions From VecArray<float> To Other Types

        public static VecArray<double> ToVecArrayFloat(this VecArray<float> va)
        {
            var result = new VecArray<double>(va.Dim, va.Count);
            result.ForeachIndex((v, i) => v.Set(va[i], x => (double)x));
            return result;
        }

        public static IEnumerable<V2f> ToV2fs(this VecArray<float> vecs)
        {
            if (vecs.Dim != 2) throw new ArgumentException();
            foreach (var v in vecs) yield return v.ToV2f();
        }

        public static IEnumerable<V3f> ToV3fs(this VecArray<float> vecs)
        {
            if (vecs.Dim != 3) throw new ArgumentException();
            foreach (var v in vecs) yield return v.ToV3f();
        }

        public static IEnumerable<V4f> ToV4fs(this VecArray<float> vecs)
        {
            if (vecs.Dim != 4) throw new ArgumentException();
            foreach (var v in vecs) yield return v.ToV4f();
        }

        #endregion

        #region Internal Setters

        internal static void SetPlus(this Vec<float> v0, Vec<float> v1)
        {
            v0.Set(v1, (x0, x1) => x0 + x1);
        }

        internal static void SetMinus(this Vec<float> v0, Vec<float> v1)
        {
            v0.Set(v1, (x0, x1) => x0 - x1);
        }

        internal static void SetMultiplyBy(this Vec<float> v0, float s1)
        {
            v0.Set(x0 => x0 * s1);
        }

        internal static void SetDivideBy(this Vec<float> v0, float s1)
        {
            float f = 1.0f / s1;
            v0.Set(x0 => x0 * f);
        }

        internal static void Set(this Vec<float> v0, V2f v1)
        {
            v0.m_data[v0.m_origin] = v1.X;
            v0.m_data[v0.m_origin + 1] = v1.Y;
        }

        internal static void Set(this Vec<float> v0, V3f v1)
        {
            v0.m_data[v0.m_origin] = v1.X;
            v0.m_data[v0.m_origin + 1] = v1.Y;
            v0.m_data[v0.m_origin + 2] = v1.Z;
        }

        internal static void Set(this Vec<float> v0, V4f v1)
        {
            v0.m_data[v0.m_origin] = v1.X;
            v0.m_data[v0.m_origin + 1] = v1.Y;
            v0.m_data[v0.m_origin + 2] = v1.Z;
            v0.m_data[v0.m_origin + 2] = v1.W;
        }

        #endregion
    }
}
