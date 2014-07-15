using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static partial class SequenceExtensions
    {
        //# Action<Action<int>, Func<string, string>> F = (o, f) => { Filter = f; o(0); Filter = null; };
        //# foreach (var t in Meta.StandardNumericTypes) {
        //#     var type = t.Name;
        #region __type__ Sequences

        #region Extreme Values

        //# foreach (var isMin in new[] { true, false }) {
        //#     Func<string, string> f = null; if (!isMin) f = s => s == " < " ? " > " : s;
        //#     string Min, min, max, MaxValue, small, smalle; //string-tokens to be replaced in Max-Version
        //#     if (isMin) {
        //#         Min = "Min";
        //#         min = "min"; max = "max";
        //#         MaxValue = t.IsReal ? "PositiveInfinity" : "MaxValue";
        //#         small = "small"; smalle = "smalle";
        //#     } else {
        //#         Min = "Max";
        //#         min = "max"; max = "min";
        //#         MaxValue = t.IsReal ? "NegativeInfinity" : "MinValue";
        //#         small = "large"; smalle = "large";
        //#     }
        /// <summary>
        /// Finds the __smalle__st element in a sequence, that is __smalle__r than the initially
        /// supplied __min__Value, or the first such element if there are equally __small__
        /// elements, or __max__Value if no element is __smalle__r.
        /// The default value of __min__Value is __type__.__MaxValue__.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the __min__imum value of.</param>
        /// <param name="__min__Value">The initially supplied __min__imum value.</param>
        /// <returns>The __min__imum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static __type__ __Min__(this IEnumerable<__type__> sequence, __type__ __min__Value = __type__.__MaxValue__)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value/*#F(o=>{*/ < /*#},f);*/__min__Value) __min__Value = value;
            return __min__Value;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the __smalle__st value, that is __smalle__r than the initially
        /// supplied __min__Value, or the first such element if there are equally __small__
        /// elements, or __min__Value if no element is __smalle__r.
        /// The default value of __min__Value is __type__.__MaxValue__.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the __min__imum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="__min__Value">The initially supplied __min__imum value.</param>
        /// <returns>The __min__imum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static __type__ __Min__Value<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, __type__> element_valueSelector, __type__ __min__Value = __type__.__MaxValue__)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value/*#F(o=>{*/ < /*#},f);*/__min__Value) __min__Value = value;
            }
            return __min__Value;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the __smalle__st value, that is __smalle__r than the initially
        /// supplied __min__Value, or the first such element if there are equally __small__
        /// elements, or __min__Value if no element is __smalle__r.
        /// The default value of __min__Value is __type__.__MaxValue__.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the __min__imum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="__min__Element">Is set to the element that yielded the smallest value or is not set if no element is __smalle__r than the initial __min__ value.</param>
        /// <param name="__min__Value">The initially supplied __min__imum value.</param>
        /// <returns>The element that yielded the __min__imum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static __type__ __Min__Value<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, __type__> element_valueSelector, ref TSeq __min__Element, __type__ __min__Value = __type__.__MaxValue__)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value/*#F(o=>{*/ < /*#},f);*/__min__Value)
                {
                    __min__Value = value;
                    __min__Element = element;
                }
            }
            return __min__Value;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the __smalle__st value, that is __smalle__r than the initially
        /// supplied __min__Value, or the first such element if there are equally __small__
        /// elements, or __min__Value if no element is __smalle__r.
        /// The default value of __min__Value is __type__.__MaxValue__.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the __min__imum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="__min__Value">The initially supplied __min__imum value.</param>
        /// <returns>The element that yielded the __min__imum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq __Min__Element<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, __type__> element_valueSelector, __type__ __min__Value = __type__.__MaxValue__)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var __min__Element = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value/*#F(o=>{*/ < /*#},f);*/__min__Value)
                {
                    __min__Value = value;
                    __min__Element = element;
                }
            }
            return __min__Element;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the __smalle__st value of all elements and return this value.
        /// </summary>
        public static __type__ __Min__Value<TSeq>(this TSeq[] sequence, Func<TSeq, __type__> element_valueSelector, __type__ __min__Value = __type__.__MaxValue__)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value/*#F(o=>{*/ < /*#},f);*/__min__Value) __min__Value = value;
            }
            return __min__Value;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the __smalle__st value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static __type__ __Min__Value<TSeq>(this TSeq[] sequence, Func<TSeq, __type__> element_valueSelector, ref TSeq __min__Element, __type__ __min__Value = __type__.__MaxValue__)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value/*#F(o=>{*/ < /*#},f);*/__min__Value)
                {
                    __min__Value = value;
                    __min__Element = element;
                }
            }
            return __min__Value;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the __smalle__st value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq __Min__Element<TSeq>(this TSeq[] sequence, Func<TSeq, __type__> element_valueSelector, __type__ __min__Value = __type__.__MaxValue__)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var __min__Element = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value/*#F(o=>{*/ < /*#},f);*/__min__Value)
                {
                    __min__Value = value;
                    __min__Element = element;
                }
            }
            return __min__Element;
        }

        //# } // foreach isMin
        #endregion

        #region Sorting

        //# foreach (var isAsc in new[] { true, false }) {
        //#     var Ascending = isAsc ? "Ascending" : "Descending";
        //#     var ascending = isAsc ? "ascending" : "descending";
        //#     Func<string, string> f = null; if (!isAsc) f = s => s == " < " ? " > " : s;
        /// <summary>
        /// Merge an __ascending__ly sorted __type__ array with another __ascending__ly
        /// sorted __type__ array resulting in a single __ascending__ly sorted
        /// __type__ array.
        /// </summary>
        public static __type__[] Merge__Ascending__(this __type__[] a0, __type__[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new __type__[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0]/*#F(o=>{*/ < /*#},f);*/a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an __ascending__ly sorted __type__ array with another __ascending__ly
        /// sorted __type__ array resulting in a single __ascending__ly sorted
        /// __type__ array.
        /// </summary>
        public static TSeq[] Merge__Ascending__<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, __type__> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0])/*#F(o=>{*/ < /*#},f);*/element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        //# } // isAsc
        #endregion

        #endregion

        //# } // foreach t
    }
}
