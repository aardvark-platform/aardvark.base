/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;

namespace Aardvark.Base;

/// <summary>
/// IRange enforces a uniform design of all range types.
/// This non-generic interface contains all the methods and properties
/// that are independent of type. It serves as a base interface to
/// the generic IRange interface.
/// </summary>
public interface IRange : IValidity
{
    bool IsEmpty { get; }
    bool IsNonEmpty { get; }
}

/// <summary>
/// ISimpleRange enforces a uniform interface of range operations that
/// ony require comparison of boundaries.
/// </summary>
public interface ISimpleRange<TValue, TRange> : IRange
{
    /// <summary>
    /// Checks if the range is still valid and repairs if not.
    /// </summary>
    TRange Repair();

    /// <summary>
    /// Returns whether range contains given value.
    /// </summary>
    bool Contains(TValue value);

    /// <summary>
    /// Returns whether range completely contains given range.
    /// </summary>
    bool Contains(TRange range);

    /// <summary>
    /// Checks if 2 ranges intersect each other.
    /// </summary>
    bool Intersects(TRange range);

    /// <summary>
    /// Returns the range extended to contain the supplied value.
    /// </summary>
    TRange ExtendedBy(TValue value);

    /// <summary>
    /// Returns the range extended to contain the supplied range.
    /// Returns this.
    /// </summary>
    TRange ExtendedBy(TRange range);

    /// <summary>
    /// Extends the range to contain the supplied value.
    /// </summary>
    void ExtendBy(TValue value);

    /// <summary>
    /// Extends the range to contain the supplied range.
    /// </summary>
    void ExtendBy(TRange range);

}

/// <summary>
/// IRange enforces a uniform design of all range types.
/// </summary>
/// <typeparam name="TValue">Type of range boundaries.</typeparam>
/// <typeparam name="TDiff">Type of differences of TValue.
/// Given values a and b of type TValue: typeof(TDiff) == typeof(a-b).</typeparam>
/// <typeparam name="TRange">Type of the concrete (implementing) range type.</typeparam>
public interface IRange<TValue, TDiff, TRange> : IRange, IFormattable, ISimpleRange<TValue, TRange>
{
    // TValue Minimum { get; set; }
    // TValue Maximum { get; set; }
    TValue Center { get; }
    TDiff Size { get; }
    TRange SplitRight(TValue splitValue);
    TRange SplitLeft(TValue splitValue);

    /// <summary>
    /// Checks if 2 ranges intersect each other with tolerance parameter.
    /// </summary>
    bool Intersects(TRange range, TDiff eps);

    /// <summary>
    /// Returns range enlarged by specified vector in BOTH directions.
    /// </summary>
    TRange EnlargedBy(TDiff v);

    /// <summary>
    /// Returns range by shrunk specified vector in BOTH directions.
    /// The returned range may be invalid if vector is too large.
    /// </summary>
    TRange ShrunkBy(TDiff v);

    /// <summary>
    /// Returns range enlarged by given values (paddings).
    /// </summary>
    TRange EnlargedBy(TDiff left, TDiff right);

    /// <summary>
    /// Returns range shrunk by given values (paddings).
    /// The returned range may be invalid if the paddings are too large.
    /// </summary>
    TRange ShrunkBy(TDiff left, TDiff right);

    /// <summary>
    /// Enlarges range by specified vector in BOTH directions.
    /// </summary>
    void EnlargeBy(TDiff v);

    /// <summary>
    /// Shrinks range by specified vector in BOTH directions.
    /// Range may become invalid if vector is too large.
    /// </summary>
    void ShrinkBy(TDiff v);

    /// <summary>
    /// Enlarges range by given values (paddings).
    /// </summary>
    void EnlargeBy(TDiff left, TDiff right);

    /// <summary>
    /// Shrinks range by given values (paddings).
    /// </summary>
    void ShrinkBy(TDiff left, TDiff right);

}

/// <summary>
/// IRange enforces a uniform design of all range types.
/// </summary>
/// <typeparam name="TValue">Type of min and max elements of the range.</typeparam>
/// <typeparam name="TRange">Type of the concrete (implementing) range type.</typeparam>
public interface IRange<TValue, TRange> : IRange<TValue, TValue, TRange>
{
}
