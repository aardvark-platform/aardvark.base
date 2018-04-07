using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
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

    /// <summary>
    /// Generic Interface for NxM-dimensional matrix of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface IMatrix<T> : IMatrix
    {
        T this[long x, long y] { get; set; }
        T this[V2l v] { get; set; }
    }

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
}
