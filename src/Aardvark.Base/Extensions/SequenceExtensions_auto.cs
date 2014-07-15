using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static partial class SequenceExtensions
    {
        #region byte Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is byte.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static byte Min(this IEnumerable<byte> sequence, byte minValue = byte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is byte.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static byte MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, byte> element_valueSelector, byte minValue = byte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is byte.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static byte MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, byte> element_valueSelector, ref TSeq minElement, byte minValue = byte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is byte.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, byte> element_valueSelector, byte minValue = byte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static byte MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, byte> element_valueSelector, byte minValue = byte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static byte MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, byte> element_valueSelector, ref TSeq minElement, byte minValue = byte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, byte> element_valueSelector, byte minValue = byte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is byte.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static byte Max(this IEnumerable<byte> sequence, byte maxValue = byte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is byte.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static byte MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, byte> element_valueSelector, byte maxValue = byte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is byte.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static byte MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, byte> element_valueSelector, ref TSeq maxElement, byte maxValue = byte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is byte.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, byte> element_valueSelector, byte maxValue = byte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static byte MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, byte> element_valueSelector, byte maxValue = byte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static byte MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, byte> element_valueSelector, ref TSeq maxElement, byte maxValue = byte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, byte> element_valueSelector, byte maxValue = byte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted byte array with another ascendingly
        /// sorted byte array resulting in a single ascendingly sorted
        /// byte array.
        /// </summary>
        public static byte[] MergeAscending(this byte[] a0, byte[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new byte[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted byte array with another ascendingly
        /// sorted byte array resulting in a single ascendingly sorted
        /// byte array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, byte> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted byte array with another descendingly
        /// sorted byte array resulting in a single descendingly sorted
        /// byte array.
        /// </summary>
        public static byte[] MergeDescending(this byte[] a0, byte[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new byte[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted byte array with another descendingly
        /// sorted byte array resulting in a single descendingly sorted
        /// byte array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, byte> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region sbyte Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is sbyte.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static sbyte Min(this IEnumerable<sbyte> sequence, sbyte minValue = sbyte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is sbyte.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static sbyte MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, sbyte> element_valueSelector, sbyte minValue = sbyte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is sbyte.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static sbyte MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, sbyte> element_valueSelector, ref TSeq minElement, sbyte minValue = sbyte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is sbyte.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, sbyte> element_valueSelector, sbyte minValue = sbyte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static sbyte MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, sbyte> element_valueSelector, sbyte minValue = sbyte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static sbyte MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, sbyte> element_valueSelector, ref TSeq minElement, sbyte minValue = sbyte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, sbyte> element_valueSelector, sbyte minValue = sbyte.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is sbyte.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static sbyte Max(this IEnumerable<sbyte> sequence, sbyte maxValue = sbyte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is sbyte.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static sbyte MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, sbyte> element_valueSelector, sbyte maxValue = sbyte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is sbyte.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static sbyte MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, sbyte> element_valueSelector, ref TSeq maxElement, sbyte maxValue = sbyte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is sbyte.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, sbyte> element_valueSelector, sbyte maxValue = sbyte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static sbyte MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, sbyte> element_valueSelector, sbyte maxValue = sbyte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static sbyte MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, sbyte> element_valueSelector, ref TSeq maxElement, sbyte maxValue = sbyte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, sbyte> element_valueSelector, sbyte maxValue = sbyte.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted sbyte array with another ascendingly
        /// sorted sbyte array resulting in a single ascendingly sorted
        /// sbyte array.
        /// </summary>
        public static sbyte[] MergeAscending(this sbyte[] a0, sbyte[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new sbyte[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted sbyte array with another ascendingly
        /// sorted sbyte array resulting in a single ascendingly sorted
        /// sbyte array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, sbyte> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted sbyte array with another descendingly
        /// sorted sbyte array resulting in a single descendingly sorted
        /// sbyte array.
        /// </summary>
        public static sbyte[] MergeDescending(this sbyte[] a0, sbyte[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new sbyte[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted sbyte array with another descendingly
        /// sorted sbyte array resulting in a single descendingly sorted
        /// sbyte array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, sbyte> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region short Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is short.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static short Min(this IEnumerable<short> sequence, short minValue = short.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is short.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static short MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, short> element_valueSelector, short minValue = short.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is short.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static short MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, short> element_valueSelector, ref TSeq minElement, short minValue = short.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is short.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, short> element_valueSelector, short minValue = short.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static short MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, short> element_valueSelector, short minValue = short.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static short MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, short> element_valueSelector, ref TSeq minElement, short minValue = short.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, short> element_valueSelector, short minValue = short.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is short.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static short Max(this IEnumerable<short> sequence, short maxValue = short.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is short.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static short MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, short> element_valueSelector, short maxValue = short.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is short.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static short MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, short> element_valueSelector, ref TSeq maxElement, short maxValue = short.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is short.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, short> element_valueSelector, short maxValue = short.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static short MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, short> element_valueSelector, short maxValue = short.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static short MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, short> element_valueSelector, ref TSeq maxElement, short maxValue = short.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, short> element_valueSelector, short maxValue = short.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted short array with another ascendingly
        /// sorted short array resulting in a single ascendingly sorted
        /// short array.
        /// </summary>
        public static short[] MergeAscending(this short[] a0, short[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new short[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted short array with another ascendingly
        /// sorted short array resulting in a single ascendingly sorted
        /// short array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, short> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted short array with another descendingly
        /// sorted short array resulting in a single descendingly sorted
        /// short array.
        /// </summary>
        public static short[] MergeDescending(this short[] a0, short[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new short[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted short array with another descendingly
        /// sorted short array resulting in a single descendingly sorted
        /// short array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, short> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region ushort Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is ushort.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static ushort Min(this IEnumerable<ushort> sequence, ushort minValue = ushort.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is ushort.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ushort MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ushort> element_valueSelector, ushort minValue = ushort.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is ushort.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ushort MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ushort> element_valueSelector, ref TSeq minElement, ushort minValue = ushort.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is ushort.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ushort> element_valueSelector, ushort minValue = ushort.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static ushort MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, ushort> element_valueSelector, ushort minValue = ushort.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static ushort MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, ushort> element_valueSelector, ref TSeq minElement, ushort minValue = ushort.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, ushort> element_valueSelector, ushort minValue = ushort.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is ushort.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static ushort Max(this IEnumerable<ushort> sequence, ushort maxValue = ushort.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is ushort.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ushort MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ushort> element_valueSelector, ushort maxValue = ushort.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is ushort.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ushort MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ushort> element_valueSelector, ref TSeq maxElement, ushort maxValue = ushort.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is ushort.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ushort> element_valueSelector, ushort maxValue = ushort.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static ushort MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, ushort> element_valueSelector, ushort maxValue = ushort.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static ushort MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, ushort> element_valueSelector, ref TSeq maxElement, ushort maxValue = ushort.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, ushort> element_valueSelector, ushort maxValue = ushort.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted ushort array with another ascendingly
        /// sorted ushort array resulting in a single ascendingly sorted
        /// ushort array.
        /// </summary>
        public static ushort[] MergeAscending(this ushort[] a0, ushort[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new ushort[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted ushort array with another ascendingly
        /// sorted ushort array resulting in a single ascendingly sorted
        /// ushort array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, ushort> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted ushort array with another descendingly
        /// sorted ushort array resulting in a single descendingly sorted
        /// ushort array.
        /// </summary>
        public static ushort[] MergeDescending(this ushort[] a0, ushort[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new ushort[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted ushort array with another descendingly
        /// sorted ushort array resulting in a single descendingly sorted
        /// ushort array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, ushort> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region int Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is int.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static int Min(this IEnumerable<int> sequence, int minValue = int.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is int.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static int MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, int> element_valueSelector, int minValue = int.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is int.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static int MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, int> element_valueSelector, ref TSeq minElement, int minValue = int.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is int.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, int> element_valueSelector, int minValue = int.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static int MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, int> element_valueSelector, int minValue = int.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static int MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, int> element_valueSelector, ref TSeq minElement, int minValue = int.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, int> element_valueSelector, int minValue = int.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is int.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static int Max(this IEnumerable<int> sequence, int maxValue = int.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is int.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static int MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, int> element_valueSelector, int maxValue = int.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is int.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static int MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, int> element_valueSelector, ref TSeq maxElement, int maxValue = int.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is int.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, int> element_valueSelector, int maxValue = int.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static int MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, int> element_valueSelector, int maxValue = int.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static int MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, int> element_valueSelector, ref TSeq maxElement, int maxValue = int.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, int> element_valueSelector, int maxValue = int.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted int array with another ascendingly
        /// sorted int array resulting in a single ascendingly sorted
        /// int array.
        /// </summary>
        public static int[] MergeAscending(this int[] a0, int[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new int[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted int array with another ascendingly
        /// sorted int array resulting in a single ascendingly sorted
        /// int array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, int> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted int array with another descendingly
        /// sorted int array resulting in a single descendingly sorted
        /// int array.
        /// </summary>
        public static int[] MergeDescending(this int[] a0, int[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new int[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted int array with another descendingly
        /// sorted int array resulting in a single descendingly sorted
        /// int array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, int> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region uint Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is uint.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static uint Min(this IEnumerable<uint> sequence, uint minValue = uint.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is uint.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static uint MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, uint> element_valueSelector, uint minValue = uint.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is uint.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static uint MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, uint> element_valueSelector, ref TSeq minElement, uint minValue = uint.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is uint.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, uint> element_valueSelector, uint minValue = uint.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static uint MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, uint> element_valueSelector, uint minValue = uint.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static uint MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, uint> element_valueSelector, ref TSeq minElement, uint minValue = uint.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, uint> element_valueSelector, uint minValue = uint.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is uint.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static uint Max(this IEnumerable<uint> sequence, uint maxValue = uint.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is uint.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static uint MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, uint> element_valueSelector, uint maxValue = uint.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is uint.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static uint MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, uint> element_valueSelector, ref TSeq maxElement, uint maxValue = uint.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is uint.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, uint> element_valueSelector, uint maxValue = uint.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static uint MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, uint> element_valueSelector, uint maxValue = uint.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static uint MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, uint> element_valueSelector, ref TSeq maxElement, uint maxValue = uint.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, uint> element_valueSelector, uint maxValue = uint.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted uint array with another ascendingly
        /// sorted uint array resulting in a single ascendingly sorted
        /// uint array.
        /// </summary>
        public static uint[] MergeAscending(this uint[] a0, uint[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new uint[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted uint array with another ascendingly
        /// sorted uint array resulting in a single ascendingly sorted
        /// uint array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, uint> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted uint array with another descendingly
        /// sorted uint array resulting in a single descendingly sorted
        /// uint array.
        /// </summary>
        public static uint[] MergeDescending(this uint[] a0, uint[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new uint[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted uint array with another descendingly
        /// sorted uint array resulting in a single descendingly sorted
        /// uint array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, uint> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region long Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is long.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static long Min(this IEnumerable<long> sequence, long minValue = long.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is long.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static long MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, long> element_valueSelector, long minValue = long.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is long.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static long MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, long> element_valueSelector, ref TSeq minElement, long minValue = long.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is long.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, long> element_valueSelector, long minValue = long.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static long MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, long> element_valueSelector, long minValue = long.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static long MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, long> element_valueSelector, ref TSeq minElement, long minValue = long.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, long> element_valueSelector, long minValue = long.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is long.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static long Max(this IEnumerable<long> sequence, long maxValue = long.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is long.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static long MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, long> element_valueSelector, long maxValue = long.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is long.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static long MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, long> element_valueSelector, ref TSeq maxElement, long maxValue = long.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is long.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, long> element_valueSelector, long maxValue = long.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static long MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, long> element_valueSelector, long maxValue = long.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static long MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, long> element_valueSelector, ref TSeq maxElement, long maxValue = long.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, long> element_valueSelector, long maxValue = long.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted long array with another ascendingly
        /// sorted long array resulting in a single ascendingly sorted
        /// long array.
        /// </summary>
        public static long[] MergeAscending(this long[] a0, long[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new long[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted long array with another ascendingly
        /// sorted long array resulting in a single ascendingly sorted
        /// long array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, long> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted long array with another descendingly
        /// sorted long array resulting in a single descendingly sorted
        /// long array.
        /// </summary>
        public static long[] MergeDescending(this long[] a0, long[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new long[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted long array with another descendingly
        /// sorted long array resulting in a single descendingly sorted
        /// long array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, long> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region ulong Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is ulong.MaxValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static ulong Min(this IEnumerable<ulong> sequence, ulong minValue = ulong.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is ulong.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ulong MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ulong> element_valueSelector, ulong minValue = ulong.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is ulong.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ulong MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ulong> element_valueSelector, ref TSeq minElement, ulong minValue = ulong.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is ulong.MaxValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ulong> element_valueSelector, ulong minValue = ulong.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static ulong MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, ulong> element_valueSelector, ulong minValue = ulong.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static ulong MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, ulong> element_valueSelector, ref TSeq minElement, ulong minValue = ulong.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, ulong> element_valueSelector, ulong minValue = ulong.MaxValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is ulong.MinValue.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static ulong Max(this IEnumerable<ulong> sequence, ulong maxValue = ulong.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is ulong.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ulong MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ulong> element_valueSelector, ulong maxValue = ulong.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is ulong.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static ulong MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ulong> element_valueSelector, ref TSeq maxElement, ulong maxValue = ulong.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is ulong.MinValue.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, ulong> element_valueSelector, ulong maxValue = ulong.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static ulong MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, ulong> element_valueSelector, ulong maxValue = ulong.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static ulong MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, ulong> element_valueSelector, ref TSeq maxElement, ulong maxValue = ulong.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, ulong> element_valueSelector, ulong maxValue = ulong.MinValue)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted ulong array with another ascendingly
        /// sorted ulong array resulting in a single ascendingly sorted
        /// ulong array.
        /// </summary>
        public static ulong[] MergeAscending(this ulong[] a0, ulong[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new ulong[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted ulong array with another ascendingly
        /// sorted ulong array resulting in a single ascendingly sorted
        /// ulong array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, ulong> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted ulong array with another descendingly
        /// sorted ulong array resulting in a single descendingly sorted
        /// ulong array.
        /// </summary>
        public static ulong[] MergeDescending(this ulong[] a0, ulong[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new ulong[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted ulong array with another descendingly
        /// sorted ulong array resulting in a single descendingly sorted
        /// ulong array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, ulong> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region float Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is float.PositiveInfinity.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static float Min(this IEnumerable<float> sequence, float minValue = float.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is float.PositiveInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static float MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, float> element_valueSelector, float minValue = float.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is float.PositiveInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static float MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, float> element_valueSelector, ref TSeq minElement, float minValue = float.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is float.PositiveInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, float> element_valueSelector, float minValue = float.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static float MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, float> element_valueSelector, float minValue = float.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static float MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, float> element_valueSelector, ref TSeq minElement, float minValue = float.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, float> element_valueSelector, float minValue = float.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is float.NegativeInfinity.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static float Max(this IEnumerable<float> sequence, float maxValue = float.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is float.NegativeInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static float MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, float> element_valueSelector, float maxValue = float.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is float.NegativeInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static float MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, float> element_valueSelector, ref TSeq maxElement, float maxValue = float.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is float.NegativeInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, float> element_valueSelector, float maxValue = float.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static float MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, float> element_valueSelector, float maxValue = float.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static float MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, float> element_valueSelector, ref TSeq maxElement, float maxValue = float.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, float> element_valueSelector, float maxValue = float.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted float array with another ascendingly
        /// sorted float array resulting in a single ascendingly sorted
        /// float array.
        /// </summary>
        public static float[] MergeAscending(this float[] a0, float[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new float[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted float array with another ascendingly
        /// sorted float array resulting in a single ascendingly sorted
        /// float array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, float> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted float array with another descendingly
        /// sorted float array resulting in a single descendingly sorted
        /// float array.
        /// </summary>
        public static float[] MergeDescending(this float[] a0, float[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new float[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted float array with another descendingly
        /// sorted float array resulting in a single descendingly sorted
        /// float array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, float> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

        #region double Sequences

        #region Extreme Values

        /// <summary>
        /// Finds the smallest element in a sequence, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or maxValue if no element is smaller.
        /// The default value of minValue is double.PositiveInfinity.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static double Min(this IEnumerable<double> sequence, double minValue = double.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value < minValue) minValue = value;
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is double.PositiveInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static double MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, double> element_valueSelector, double minValue = double.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is double.PositiveInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minElement">Is set to the element that yielded the smallest value or is not set if no element is smaller than the initial min value.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static double MinValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, double> element_valueSelector, ref TSeq minElement, double minValue = double.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the smallest value, that is smaller than the initially
        /// supplied minValue, or the first such element if there are equally small
        /// elements, or minValue if no element is smaller.
        /// The default value of minValue is double.PositiveInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the minimum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="minValue">The initially supplied minimum value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MinElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, double> element_valueSelector, double minValue = double.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and return this value.
        /// </summary>
        public static double MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, double> element_valueSelector, double minValue = double.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value < minValue) minValue = value;
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static double MinValue<TSeq>(this TSeq[] sequence, Func<TSeq, double> element_valueSelector, ref TSeq minElement, double minValue = double.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the smallest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MinElement<TSeq>(this TSeq[] sequence, Func<TSeq, double> element_valueSelector, double minValue = double.PositiveInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var minElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = element;
                }
            }
            return minElement;
        }

        /// <summary>
        /// Finds the largest element in a sequence, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or minValue if no element is larger.
        /// The default value of maxValue is double.NegativeInfinity.
        /// </summary>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence is null.</exception>
        public static double Max(this IEnumerable<double> sequence, double maxValue = double.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var value in sequence)
                if (value > maxValue) maxValue = value;
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is double.NegativeInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static double MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, double> element_valueSelector, double maxValue = double.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is double.NegativeInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxElement">Is set to the element that yielded the smallest value or is not set if no element is larger than the initial max value.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static double MaxValue<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, double> element_valueSelector, ref TSeq maxElement, double maxValue = double.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence, and
        /// finds the element with the largest value, that is larger than the initially
        /// supplied maxValue, or the first such element if there are equally large
        /// elements, or maxValue if no element is larger.
        /// The default value of maxValue is double.NegativeInfinity.
        /// </summary>
        /// <typeparam name="TSeq">The type of the elements of sequence.</typeparam>
        /// <param name="sequence">A sequence of values to determine the maximum value of.</param>
        /// <param name="element_valueSelector">A transform function to apply to each element.</param>
        /// <param name="maxValue">The initially supplied maximum value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">sequence or selector is null.</exception>
        public static TSeq MaxElement<TSeq>(this IEnumerable<TSeq> sequence, Func<TSeq, double> element_valueSelector, double maxValue = double.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            foreach (var element in sequence)
            {
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and return this value.
        /// </summary>
        public static double MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, double> element_valueSelector, double maxValue = double.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var value = element_valueSelector(sequence[i]);
                if (value > maxValue) maxValue = value;
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements, returns the value and
        /// the corresponding element as a referenced parameter.
        /// </summary>
        public static double MaxValue<TSeq>(this TSeq[] sequence, Func<TSeq, double> element_valueSelector, ref TSeq maxElement, double maxValue = double.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Invokes the value selector function on each element of a sequence,
        /// finds the largest value of all elements and returns the
        /// corresponding element.
        /// </summary>
        public static TSeq MaxElement<TSeq>(this TSeq[] sequence, Func<TSeq, double> element_valueSelector, double maxValue = double.NegativeInfinity)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (element_valueSelector == null) throw new ArgumentNullException("element_valueSelector");
            var maxElement = default(TSeq);
            for (long i = 0; i < sequence.LongLength; i++)
            {
                var element = sequence[i];
                var value = element_valueSelector(element);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = element;
                }
            }
            return maxElement;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Merge an ascendingly sorted double array with another ascendingly
        /// sorted double array resulting in a single ascendingly sorted
        /// double array.
        /// </summary>
        public static double[] MergeAscending(this double[] a0, double[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new double[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] < a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an ascendingly sorted double array with another ascendingly
        /// sorted double array resulting in a single ascendingly sorted
        /// double array.
        /// </summary>
        public static TSeq[] MergeAscending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, double> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) < element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted double array with another descendingly
        /// sorted double array resulting in a single descendingly sorted
        /// double array.
        /// </summary>
        public static double[] MergeDescending(this double[] a0, double[] a1)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new double[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0] > a1[i1])
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        /// <summary>
        /// Merge an descendingly sorted double array with another descendingly
        /// sorted double array resulting in a single descendingly sorted
        /// double array.
        /// </summary>
        public static TSeq[] MergeDescending<TSeq>(this TSeq[] a0, TSeq[] a1, Func<TSeq, double> element_valueSelector)
        {
            long count0 = a0.LongLength;
            long count1 = a1.LongLength;
            var a = new TSeq[count0 + count1];
            long i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (element_valueSelector(a0[i0]) > element_valueSelector(a1[i1]))
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        #endregion

        #endregion

    }
}
