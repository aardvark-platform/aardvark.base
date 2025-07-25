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
using System.Collections.Generic;

namespace Aardvark.Base;

public static class VectorIEnumerableExtensions
{
    //# foreach(var scalar in Meta.VecFieldTypes)
    //# {
    //# foreach(var dim in Meta.VecTypeDimensions)
    //# {
    //#     var inputVecType = Meta.VecTypeOf(dim, scalar).Name;
    //#     var outputVecType = scalar.IsReal ? inputVecType : Meta.VecTypeOf(dim, Meta.DoubleType).Name;
    //#     var scalarType = scalar.IsReal ? scalar.Name :  Meta.DoubleType.Name;
    //#     var cast = scalar.IsReal ? "" : "(" + outputVecType + ")";
    #region __inputVecType__

    #region Sum
    /// <summary>
    /// Calculates the sum for a given set of __inputVecType__s.
    /// </summary>
    public static __inputVecType__ Sum(this IEnumerable<__inputVecType__> vectors)
    {
        __inputVecType__ sum = __inputVecType__.Zero;

        foreach (var e in vectors)
        {
            sum += e;
        }

        return sum;
    }

    /// <summary>
    /// Calculates the sum for a given set of __inputVecType__s.
    /// </summary>
    public static __inputVecType__ Sum(this __inputVecType__[] vectors)
    {
        __inputVecType__ sum = __inputVecType__.Zero;

        for (var i = 0; i < vectors.Length; i++)
        {
            sum += vectors[i];
        }

        return sum;
    }

    #endregion

    #region Centroid
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
    /// Calculates the centroid for a given set of __inputVecType__s.
    /// </summary>
    public static __outputVecType__ ComputeCentroid(this __inputVecType__[] vectors, int[] indices)
    {
        __outputVecType__ sum = __outputVecType__.Zero;

        for (var i = 0; i < indices.Length; i++)
        {
            sum += __cast__vectors[indices[i]];
        }

        return sum / (__scalarType__)indices.Length;
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

    /// <summary>
    /// Calculates a weighted centroid for vectors and weights given by indices.
    /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
    /// </summary>
    public static __outputVecType__ ComputeCentroid(this __inputVecType__[] vectors, __scalarType__[] weights, int[] indices)
    {
        __outputVecType__ sum = __outputVecType__.Zero;
        __scalarType__ weightSum = 0;

        for (int i = 0; i < indices.Length; i++)
        {
            var w = weights[indices[i]];
            sum += w * __cast__vectors[indices[i]];
            weightSum += w;
        }

        return sum / weightSum;
    }

    #endregion

    #endregion

    //# } ///foreach(Meta.VecTypeDimensions);
    //# } ///foreach(Meta.VecTypes)
}
