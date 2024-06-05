using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region IndexedValue

    public readonly struct IndexedValue<T>
    {
        public readonly int Index;
        public readonly T Value;

        #region Constructor

        public IndexedValue(int index, T value)
        {
            Index = index; Value = value;
        }

        #endregion
    }

    #endregion

    #region ComparableIndexedValue

    public readonly struct ComparableIndexedValue<T> : IComparable<ComparableIndexedValue<T>>
        where T : IComparable<T>
    {
        public readonly int Index;
        public readonly T Value;

        #region Constructor

        public ComparableIndexedValue(int index, T value)
        {
            Index = index; Value = value;
        }

        #endregion

        #region IComparable<IndexedValue<T>> Members

        public int CompareTo(ComparableIndexedValue<T> other)
        {
            return Value.CompareTo(other.Value);
        }

        #endregion
    }

    #endregion

    public static class StructsExtensions
    {
        #region ComparableIndexedValue

        public static IEnumerable<ComparableIndexedValue<T>> ComparableIndexedValues<T>(
                this IEnumerable<T> self)
            where T : IComparable<T>
        {
            return self.Select((item, i) => new ComparableIndexedValue<T>(i, item));
        }
        
        public static ComparableIndexedValue<T> ComparableIndexedValue<T>(
                this T self, int index)
            where T : IComparable<T>
        {
            return new ComparableIndexedValue<T>(index, self);
        }

        #endregion
    }
}