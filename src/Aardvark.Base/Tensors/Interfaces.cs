using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region ITensorLong

    /// <summary>
    /// This interface defines properties of Tensors and TensorInfos that are
    /// independent of their dimension (rank).
    /// </summary>
    public interface ITensorLong
    {
        /// <summary>
        /// The rank of the tensor, in our case this is equal to the tensor's
        /// dimension.
        /// </summary>
        int Rank { get; }

        /// <summary>
        /// The index of the element at the origin within the tensor's data
        /// array. Note that this index may actually lie outside the tensor's
        /// data array if the lowest valid coordinate of the tensor (First,
        /// FirstArray) has positive values.
        /// </summary>
        long OriginIndex { get; set; }

        /// <summary>
        /// The deltas or strides of the tensor indices in each dimension. 
        /// </summary>
        long[] DeltaArray { get; set; }

        /// <summary>
        /// The size of the tensor in each dimension.
        /// </summary>
        long[] SizeArray { get; set; }

        /// <summary>
        /// The lowest valid coordinate of the tensor. 
        /// </summary>
        long[] FirstArray { get; set; }
    }

    #endregion

    #region ITensorInfo

    /// <summary>
    /// Dimension independent properties of tensor infos, and actions
    /// that can be performed on tensors infos regardless of their
    /// dimension.
    /// </summary>
    public interface ITensorInfo : ITensorLong
    {
        /// <summary>
        /// Perform an action for each element of the tensor. The index
        /// of each element within the tensor's data array is supplied
        /// as a parameter to the action.
        /// </summary>
        /// <param name="i_action"></param>
        void ForeachIndex(Action<long> i_action);    
    }

    #endregion

    #region IArrayTensorInfo

    /// <summary>
    /// All tensors built on top of arrays implement this interface.
    /// </summary>
    public interface IArrayTensor : ITensorLong
    {
        Type ArrayType { get; }
        Array Array { get; set; }
        long Origin { get; set; }
    }

    #endregion

    #region IArrayVector

    public interface IArrayVector : IArrayTensor
    {
        long Size { get; set; }
        long Delta { get; set; }
        long First { get; set; }
    }

    #endregion

    #region IArrayMatrix

    public interface IArrayMatrix : IArrayTensor
    {
        V2l Size { get; set; }
        V2l Delta { get; set; }
        V2l First { get; set; }
    }

    #endregion

    #region IArrayVolume

    public interface IArrayVolume : IArrayTensor
    {
        V3l Size { get; set; }
        V3l Delta { get; set; }
        V3l First { get; set; }
    }

    #endregion

    #region IArrayTensor4

    public interface IArrayTensor4 : IArrayTensor
    {
        V4l Size { get; set; }
        V4l Delta { get; set; }
        V4l First { get; set; }
    }

    #endregion

    #region IArrayTensorN

    public interface IArrayTensorN : IArrayTensor
    {
        long[] Size { get; set; }
        long[] Delta { get; set; }
    }

    #endregion

}
