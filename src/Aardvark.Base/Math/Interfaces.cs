using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region IValidity

    public interface IValidity
    {
        bool IsValid { get; }
        bool IsInvalid { get; }
    }

    #endregion

    #region IRange

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

    #endregion

    #region ISimpleRange<TValue, TRange>

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

    #endregion

    #region IRange<TValue, TDiff, TRange>

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

    #endregion

    #region IRange<TValue, TRange>

    /// <summary>
    /// IRange enforces a uniform design of all range types.
    /// </summary>
    /// <typeparam name="TValue">Type of min and max elements of the range.</typeparam>
    /// <typeparam name="TRange">Type of the concrete (implementing) range type.</typeparam>
    public interface IRange<TValue, TRange> : IRange<TValue, TValue, TRange>
    {
    }

    #endregion

    #region IOpacity

    public interface IOpacity
    {
        /// <summary>
        /// In range [0,1], where 0 means fully transparent, and 1 is fully opaque.
        /// </summary>
        double Opacity { get; set; }
    }

    #endregion

    #region IRGB

    public interface IRGB
    {
        double Red { get; set; }
        double Green { get; set; }
        double Blue { get; set; }
    }

    #endregion

    #region IVector

    /// <summary>
    /// Non-generic Interface for Dim - dimensional vectors.
    /// 
    /// </summary>
    public interface IVector
    {
        long Dim { get; }
        object GetValue(long index);
        void SetValue(object value, long index);
    }

    #endregion

    #region IMatrix

    /// <summary>
    /// Non-generic Interface for Dim.X x Dim.Y - dimensional matrices.
    /// </summary>
    public interface IMatrix
    {
        V2l Dim { get; }
        object GetValue(long x, long y);
        void SetValue(object value, long x, long y);
        object GetValue(V2l v);
        void SetValue(object value, V2l v);
    }

    #endregion

    #region IVolume

    /// <summary>
    /// Non-generic Interface for Dim.X x Dim.Y x Dim.Z - dimensional volumes.
    /// </summary>
    public interface IVolume
    {
        V3l Dim { get; }
        object GetValue(long x, long y, long z);
        void SetValue(object value, long x, long y, long z);
        object GetValue(V3l v);
        void SetValue(object value, V3l v);
    }

    #endregion

    #region ITensor4

    /// <summary>
    /// Non-generic Interface for Dim.X x Dim.Y x Dim.Z, Dim.W - dimensional volumes.
    /// </summary>
    public interface ITensor4
    {
        V4l Dim { get; }
        object GetValue(long x, long y, long z, long w);
        void SetValue(object value, long x, long y, long z, long w);
        object GetValue(V4l v);
        void SetValue(object value, V4l v);
    }

    #endregion

    #region ITensor

    /// <summary>
    /// Non-generic Interface for arbitrarily sized tensors.
    /// </summary>
    public interface ITensor
    {
        long[] Dim { get; }
        object GetValue(params long[] v);
        void SetValue(object value, params long[] v);
    }

    #endregion

    #region IVector<T>

    /// <summary>
    /// Generic Interface for N-dimensional vector of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface IVector<T> : IVector
    {
        T this[long i] { get; set; }
    }

    #endregion

    #region IMatrix<T>

    /// <summary>
    /// Generic Interface for NxM-dimensional matrix of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface IMatrix<T> : IMatrix
    {
        T this[long x, long y] { get; set; }
        T this[V2l v] { get; set; }
    }

    #endregion

    #region IVolume<T>

    /// <summary>
    /// Generic Interface for NxMxL-dimensional volume of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface IVolume<T> : IVolume
    {
        T this[long x, long y, long z] { get; set; }
        T this[V3l v] { get; set; }
    }

    #endregion

    #region ITensor4<T>

    /// <summary>
    /// Generic Interface for NxMxLXK-dimensional volume of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface ITensor4<T> : ITensor4
    {
        T this[long x, long y, long z, long w] { get; set; }
        T this[V4l v] { get; set; }
    }

    #endregion

    #region ITensor<T>

    /// <summary>
    /// Generic Interface for arbitrarily sized tensors of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface ITensor<T> : ITensor
    {
        int Rank { get; }
        T this[params long[] v] { get; set; }
    }

    #endregion

    #region IBoundingRange1d

    public interface IBoundingRange1d
    {
        Range1d BoundingRange1d { get; }
    }

    #endregion

    #region IBoundingBox2d

    /// <summary>
    /// Return an axis aligned two-dimensional box that contains the complete
    /// geometry.
    /// </summary>
    public interface IBoundingBox2d
    {
        Box2d BoundingBox2d { get; }
    }

    #endregion

    #region IBoundingBox3d

    /// <summary>
    /// Return an axis aligned three-dimensional box that contains the complete
    /// geometry.
    /// </summary>
    public interface IBoundingBox3d
    {
        Box3d BoundingBox3d { get; }
    }

    #endregion

    #region IBoundingCircle2d

    public interface IBoundingCircle2d
    {
        Circle2d BoundingCircle2d { get; }
    }

    #endregion

    #region IBoundingSphere3d

    public interface IBoundingSphere3d
    {
        Sphere3d BoundingSphere3d { get; }
    }

    #endregion

    #region ISize2d

    /// <summary>
    /// A Size2d reprents the size of an object in two dimensions.
    /// </summary>
    public interface ISize2d
    {
        V2d Size2d { get; }
    }

    #endregion

    #region ISize3d

    /// <summary>
    /// A Size3d reprents the size of an object in three dimensions.
    /// </summary>
    public interface ISize3d
    {
        V3d Size3d { get; }
    }

    #endregion

    #region ISize4d

    /// <summary>
    /// A Size4d reprents the size of an object in four dimensions.
    /// </summary>
    public interface ISize4d
    {
        V4d Size4d { get; }
    }

    #endregion

    #region ITransform<TTrafo, TVec, TVecSub1>

    /// <summary>
    /// A transform can be inverted, and it can be used to transform vectors.
    /// </summary>
    /// <typeparam name="TTrafo">Transform type.</typeparam>
    /// <typeparam name="TVec">Vector type of same dimension as T.</typeparam>
    /// <typeparam name="TVecSub1">Vector type with one dimension less than V.</typeparam>
    public interface ITransform<TTrafo, TVec, TVecSub1>
    {
        /// <summary>
        /// Inverts this transform.
        /// </summary>
        bool Invert();

        /// <summary>
        /// Returns inverse of this transform.
        /// </summary>
        TTrafo Inverse { get; }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        TVec Transform(TVec v);

        /// <summary>
        /// Transforms direction vector v (p.w is presumed 0).
        /// </summary>
        TVecSub1 TransformDir(TVecSub1 v);

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0).
        /// No projective transform is performed.
        /// </summary>
        TVecSub1 TransformPos(TVecSub1 p);

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0).
        /// Projective transform is performed.
        /// </summary>
        TVecSub1 TransformPosProj(TVecSub1 p);

        /* Sketch
        /// <summary>
        /// Transforms direction vector v (p.w is presumed 0)
        /// with the inverse of this transform.
        /// </summary>
        VMINOR InvTransformDir(VMINOR v);

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) with the inverse of this transform.
        /// No projective transform is performed.
        /// </summary>
        VMINOR InvTransformPos(VMINOR p);

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) with the inverse of this transform.
        /// Projective transform is performed.
        /// </summary>
        VMINOR InvTransformPosProj(VMINOR p);
        */

    }

    #endregion

    #region IMatrix<TMatrix, TVec, TVecSub1, TScalar>

    /// <summary>
    /// A matrix is a transform and elements can be accessed by [row, column].
    /// </summary>
    public interface IMatrix<TMatrix, TVec, TVecSub1, TScalar>
            : ITransform<TMatrix, TVec, TVecSub1>
    {
        /// <summary>
        /// Gets or sets the specified element of this matrix.
        /// </summary>
        TScalar this[int row, int column] { get; set; }
    }

    #endregion

    #region ISquareMatrix<TMat, TVec, TVecSub1, TScalar>

    /// <summary>
    /// A square matrix is a matrix is a transform.
    /// </summary>
    /// <typeparam name="TMat">Matrix type.</typeparam>
    /// <typeparam name="TVec">Vector type of TMat.</typeparam>
    /// <typeparam name="TVecSub1">Vector type of 1 dimension less than V.</typeparam>
    /// <typeparam name="TScalar">Scalar type of TMat.</typeparam>
    public interface ISquareMatrix<TMat, TVec, TVecSub1, TScalar>
            : IMatrix<TMat, TVec, TVecSub1, TScalar>
    {
        /// <summary>
        /// Returns index-th row of this matrix.
        /// </summary>
        TVec Row(int index);

        /// <summary>
        /// Returns index-th column of this matrix.
        /// </summary>
        TVec Column(int index);

        /// <summary>
        /// Returns the trace of this matrix.
        /// The trace is defined as the sum of the diagonal elements,
        /// and is only defined for square matrices.
        /// </summary>
        TScalar Trace { get; }

        /// <summary>
        /// Returns the determinant of this matrix.
        /// The determinant is only defined for square matrices.
        /// </summary>
        TScalar Det { get; }

        /// <summary>
        /// Returns whether this matrix is invertible.
        /// A square matrix is invertible if its determinant is not zero.
        /// </summary>
        bool Invertible { get; }

        /// <summary>
        /// Returns whether this matrix is singular.
        /// </summary>
        bool Singular { get; }

        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        /// <returns>This.</returns>
        void Transpose();

        /// <summary>
        /// Returns transpose of this matrix.
        /// </summary>
        TMat Transposed { get; }

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        TMat Adjoin();

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        TMat Adjoint { get; }
    }

    #endregion

    #region IPolygon<T>

    public interface IPolygon<T>
    {
        int PointCount { get; }
        T this[int index] { get; } // counter clockwise access
    }

    #endregion

    #region IMutablePolygon<T>

    public interface IMutablePolygon<T> : IPolygon<T>
    {
        void Set(int index, T value);
    }

    #endregion

}
