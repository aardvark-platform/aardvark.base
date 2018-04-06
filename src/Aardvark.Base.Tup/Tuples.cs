using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region Pair

    /// <summary>
    /// This static class allows the creation of pairs without specifying the
    /// type in angle brackets.
    /// </summary>
    public static class Pair
    {
        #region Static Creator Functions

        public static Pair<T> Create<T>(T e0, T e1)
        {
            return new Pair<T>(e0, e1);
        }

        public static Pair<double> CreateAscending(double d0, double d1)
        {
            return d0 < d1
                    ? new Pair<double>(d0, d1)
                    : new Pair<double>(d1, d0);
        }

        #endregion

        #region Extension Functions

        public static int CountNonNaNs(this Pair<double> p)
        {
            int count = 2;
            if (double.IsNaN(p.E0)) --count;
            if (double.IsNaN(p.E1)) --count;
            return count;
        }

        #endregion
    }

    /// <summary>
    /// A pair is a structure containing two elements of the same type,
    /// that can be accessed at index 0 and index 1, or using the fields
    /// E0 and E1.
    /// </summary>
    public struct Pair<T> : IEquatable<Pair<T>>, IEnumerable<T>
    {
        public T E0;
        public T E1;

        #region Constructor

        public Pair(T e) { E0 = e; E1 = e; }
        public Pair(T e0, T e1) { E0 = e0; E1 = e1; }

        #endregion

        #region Indexer

        public T this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return E0;
                    case 1: return E1;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0: E0 = value; return;
                    case 1: E1 = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region Properties

        public Pair<T> Swapped { get { return new Pair<T>(E1, E0); } }

        public IEnumerable<T> Elements
        {
            get { yield return E0; yield return E1; }
        }

        #endregion

        #region Copying

        public Pair<TR> Copy<TR>(Func<T, TR> fun)
        {
            return new Pair<TR>(fun(E0), fun(E1));
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("({0}, {1})", E0, E1);
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombinedWithDefaultZero(E0, E1);
        }

        public override bool Equals(object other)
        {
            return other is Pair<T> ? Equals((Pair<T>)other) : false;
        }

        #endregion

        #region IEquatable<Pair<T>> Members

        public bool Equals(Pair<T> other)
        {
            return (E0.Equals(other.E0) && E1.Equals(other.E1));
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        #endregion
    }

    #endregion

    #region Triple

    /// <summary>
    /// This static class allows the creation of triples without specifying
    /// the type in angle brackets.
    /// </summary>
    public static class Triple
    {
        #region Static Creator Functions

        public static Triple<T> Create<T>(T e0, T e1, T e2)
        {
            return new Triple<T>(e0, e1, e2);
        }

        public static Triple<T> Create<T>(T e0, Pair<T> p)
        {
            return new Triple<T>(e0, p);
        }

        public static Triple<T> Create<T>(Pair<T> p, T e2)
        {
            return new Triple<T>(p, e2);
        }

        public static (double, double, double) CreateAscending(double d0, double d1, double d2)
        {
            if (d0 < d1)
            {
                if (d1 < d2)
                    return (d0, d1, d2);
                else
                    return (d0 < d2) ? (d0, d2, d1) : (d2, d0, d1);
            }
            else
            {
                if (d2 < d1)
                    return (d2, d1, d0);
                else
                    return (d0 < d2) ? (d1, d0, d2) : (d1, d2, d0);
            }
        }

        #endregion

        #region Extension Functions

        public static int CountNonNaNs(this Triple<double> t)
        {
            int count = 3;
            if (double.IsNaN(t.E0)) --count;
            if (double.IsNaN(t.E1)) --count;
            if (double.IsNaN(t.E2)) --count;
            return count;
        }

        public static Triple<T> ToTriple<T>(this (T, T, T) t) => Create(t.Item1, t.Item2, t.Item3);

        #endregion
    }

    /// <summary>
    /// A triple is a structure containing three elements of the same type,
    /// that can be accessed at index 0, 1 and 2, or using the fields E0,
    /// E1, and E2.
    /// </summary>
    public struct Triple<T> : IEquatable<Triple<T>>, IEnumerable<T>
    {
        public T E0;
        public T E1;
        public T E2;

        #region Constructors

        public Triple(T e) { E0 = e; E1 = e; E2 = e; }
        public Triple(T e0, T e1, T e2) { E0 = e0; E1 = e1; E2 = e2; }
        public Triple(T e0, Pair<T> p) { E0 = e0; E1 = p.E0; E2 = p.E1; }
        public Triple(Pair<T> p, T e2) { E0 = p.E0; E1 = p.E1; E2 = e2; }

        #endregion

        #region Properties

        public IEnumerable<T> Elements
        {
            get { yield return E0; yield return E1; yield return E2; }
        }

        #endregion

        #region Indexer

        public T this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return E0;
                    case 1: return E1;
                    case 2: return E2;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0: E0 = value; return;
                    case 1: E1 = value; return;
                    case 2: E2 = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region Copying

        public Triple<TR> Copy<TR>(Func<T, TR> fun)
        {
            return new Triple<TR>(fun(E0), fun(E1), fun(E2));
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", E0, E1, E2);
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombinedWithDefaultZero(E0, E1, E2);
        }

        public override bool Equals(object other)
        {
            return other is Triple<T> ? Equals((Triple<T>)other) : false;
        }

        #endregion

        #region IEquatable<Triple<T>> Members

        public bool Equals(Triple<T> other)
        {
            return (E0.Equals(other.E0) 
                    && E1.Equals(other.E1)
                    && E2.Equals(other.E2));
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        #endregion
    }

    #endregion

    #region Quad (ruple)

    /// <summary>
    /// This static class allows the creation of quadruple without specifying
    /// the type in angle brackets.
    /// </summary>
    public static class Quad
    {
        #region Static Creation Functions

        public static Quad<T> Create<T>(T e0, T e1, T e2, T e3)
        {
            return new Quad<T>(e0, e1, e2, e3);
        }

        public static Quad<T> Create<T>(Pair<T> p0, Pair<T> p1)
        {
            return new Quad<T>(p0, p1);
        }

        public static Quad<T> Create<T>(T e0, Triple<T> t)
        {
            return new Quad<T>(e0, t);
        }

        public static Quad<T> Create<T>(Triple<T> t, T e3)
        {
            return new Quad<T>(t, e3);
        }

        #endregion

        #region Extension Functions

        public static int CountNonNaNs(this Quad<double> t)
        {
            int count = 4;
            if (double.IsNaN(t.E0)) --count;
            if (double.IsNaN(t.E1)) --count;
            if (double.IsNaN(t.E2)) --count;
            if (double.IsNaN(t.E3)) --count;
            return count;
        }

        #endregion
    }

    /// <summary>
    /// A quadruple is a structure containing three elements of the same type,
    /// that can be accessed at index 0, 1, 2, and 3, or using the fields E0,
    /// E1, E2, and E3.
    /// </summary>
    public struct Quad<T> : IEquatable<Quad<T>>, IEnumerable<T>
    {
        public T E0;
        public T E1;
        public T E2;
        public T E3;

        #region Constructors

        public Quad(T e) { E0 = e; E1 = e; E2 = e; E3 = e; }
        public Quad(T e0, T e1, T e2, T e3) { E0 = e0; E1 = e1; E2 = e2; E3 = e3; }
        public Quad(Pair<T> p0, Pair<T> p1) { E0 = p0.E0; E1 = p0.E1; E2 = p1.E0; E3 = p1.E1; }
        public Quad(T e0, Triple<T> t) { E0 = e0; E1 = t.E0; E2 = t.E1; E3 = t.E2; }
        public Quad(Triple<T> t, T e3) { E0 = t.E0; E1 = t.E1; E2 = t.E2; E3 = e3; }

        #endregion

        #region Properties

        public IEnumerable<T> Elements
        {
            get { yield return E0; yield return E1; yield return E2; yield return E3; }
        }

        #endregion

        #region Indexer

        public T this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return E0;
                    case 1: return E1;
                    case 2: return E2;
                    case 3: return E3;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0: E0 = value; return;
                    case 1: E1 = value; return;
                    case 2: E2 = value; return;
                    case 3: E3 = value; return;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region Copying

        public Quad<TR> Copy<TR>(Func<T, TR> fun)
        {
            return new Quad<TR>(fun(E0), fun(E1), fun(E2), fun(E3));
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", E0, E1, E2, E3);
        }

        public override int GetHashCode()
        {
            return HashCode.GetCombinedWithDefaultZero(E0, E1, E2, E3);
        }

        public override bool Equals(object other)
        {
            return other is Quad<T> ? Equals((Quad<T>)other) : false;
        }

        #endregion

        #region IEquatable<Triple<T>> Members

        public bool Equals(Quad<T> other)
        {
            return E0.Equals(other.E0)
                   && E1.Equals(other.E1)
                   && E2.Equals(other.E2)
                   && E3.Equals(other.E3);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        #endregion
    }

    #endregion

}
