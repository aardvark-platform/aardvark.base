using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public struct StencilFunction
    {
        public StencilCompareFunction Function;
        public int Reference;
        public uint Mask;

        #region Constants

        public static readonly StencilFunction Default = new StencilFunction()
        {
            Function = StencilCompareFunction.Always,
            Reference = 0,
            Mask = uint.MaxValue
        };

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return Function.GetHashCode() ^ Reference.GetHashCode() ^ Mask.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is StencilFunction)
            {
                var f = (StencilFunction)obj;
                return f.Function == Function && f.Reference == Reference && f.Mask == Mask;
            }
            else return false;
        }

        #endregion
    }

    public struct StencilOperation
    {
        public StencilOperationFunction StencilFail;
        public StencilOperationFunction DepthFail;
        public StencilOperationFunction DepthPass;

        #region Constants

        public static readonly StencilOperation Default = new StencilOperation()
        {
            StencilFail = StencilOperationFunction.Keep,
            DepthFail = StencilOperationFunction.Keep,
            DepthPass = StencilOperationFunction.Keep
        };

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return StencilFail.GetHashCode() ^ DepthFail.GetHashCode() ^ DepthPass.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is StencilOperation)
            {
                var o = (StencilOperation)obj;
                return o.StencilFail == StencilFail && o.DepthFail == DepthFail && o.DepthPass == DepthPass;
            }
            else return false;
        }

        #endregion
    }

    public struct StencilMode
    {
        public bool IsEnabled;
        public StencilFunction CompareFront;
        public StencilFunction CompareBack;
        public StencilOperation OperationFront;
        public StencilOperation OperationBack;

        #region Properties

        public StencilFunction Compare
        {
            get { return CompareFront; }
            set
            {
                CompareFront = value;
                CompareBack = value;
            }
        }

        public StencilOperation Operation
        {
            get { return OperationFront; }
            set
            {
                OperationFront = value;
                OperationFront = value;
            }
        }

        public static readonly StencilMode Disabled = new StencilMode()
        {
            IsEnabled = false,
            Compare = StencilFunction.Default,
            Operation = StencilOperation.Default,
        };

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return IsEnabled.GetHashCode() ^ CompareFront.GetHashCode() ^ CompareBack.GetHashCode() ^ OperationFront.GetHashCode() ^ OperationBack.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is StencilMode)
            {
                var m = (StencilMode)obj;
                if (IsEnabled != m.IsEnabled) return false;

                return m.CompareFront.Equals(CompareFront) && m.CompareBack.Equals(CompareBack) &&
                       m.OperationFront.Equals(OperationFront) && m.OperationBack.Equals(OperationBack);
            }
            else return false;
        }

        #endregion
    }

    public enum StencilCompareFunction
    {
        Always,
        Never,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
        Equal,
        NotEqual,
    }

    public enum StencilOperationFunction
    {
        Increment,
        Keep,
        Zero,
        Replace,
        IncrementWrap,
        Decrement,
        DecrementWrap,
        Invert
    }
}
