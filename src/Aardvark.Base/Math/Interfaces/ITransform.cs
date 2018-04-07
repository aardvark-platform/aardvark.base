using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
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
}
