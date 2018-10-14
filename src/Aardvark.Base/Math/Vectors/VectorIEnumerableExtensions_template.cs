using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class VectorIEnumerableExtensions
    {
        #region Centroid

        //# foreach(var scalar in Meta.VecFieldTypes)
        //# {
        //# foreach(var dim in Meta.VecTypeDimensions)
        //# {
        //#     var inputVecType = Meta.VecTypeOf(dim, scalar).Name;
        //#     var outputVecType = scalar.IsReal ? inputVecType : Meta.VecTypeOf(dim, Meta.DoubleType).Name;
        //#     var scalarType = scalar.IsReal ? scalar.Name :  Meta.DoubleType.Name;
        //#     var cast = scalar.IsReal ? "" : "(" + outputVecType + ")";
        /// <summary>
        /// Calculates the centroid for a given set of __inputVecType__s.
        /// </summary>
        public static __outputVecType__ ComputeCentroid(this IEnumerable<__inputVecType__> vectors)
        { 
            __outputVecType__ sum = __outputVecType__.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += __cast__e;
                count++;
            }

            return sum / (__scalarType__)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of __inputVecType__s.
        /// </summary>
        public static __outputVecType__ ComputeCentroid(this __inputVecType__[] vectors)
        {
            __outputVecType__ sum = __outputVecType__.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += __cast__vectors[i];
            }

            return sum / (__scalarType__)vectors.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of __inputVecType__s.
        /// </summary>
        public static __outputVecType__ ComputeCentroid(this __inputVecType__[] vectors, __scalarType__[] weights)
        {
            __outputVecType__ sum = __outputVecType__.Zero;
            __scalarType__ weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * __cast__vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        //# } ///foreach(Meta.VecTypeDimensions);
        //# } ///foreach(Meta.VecTypes)

        #endregion
    }
}
