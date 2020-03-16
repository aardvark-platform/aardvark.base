using System.Runtime.CompilerServices;

namespace Aardvark.Base.Rendering
{
    public struct StencilFunction
    {
        public StencilCompareFunction Function;
        public int Reference;
        public uint Mask;

        #region Constructors

        public StencilFunction(StencilCompareFunction func, int reference, uint mask)
        {
            Function = func;
            Reference = reference;
            Mask = mask;
        }

        #endregion

        #region Constants

        public static readonly StencilFunction Default = new StencilFunction()
        {
            Function = StencilCompareFunction.Always,
            Reference = 0,
            Mask = uint.MaxValue
        };

        #endregion

        #region Comparison

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(StencilFunction a, StencilFunction b)
            => (a.Function == b.Function) && (a.Reference == b.Reference) && (a.Mask == b.Mask);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(StencilFunction a, StencilFunction b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
            => Function.GetHashCode() ^ Reference.GetHashCode() ^ Mask.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(StencilFunction other)
            => this == other;

        public override bool Equals(object obj)
            => (obj is StencilFunction other) ? Equals(other) : false;

        #endregion
    }

    public struct StencilOperation
    {
        public StencilOperationFunction StencilFail;
        public StencilOperationFunction DepthFail;
        public StencilOperationFunction DepthPass;

        #region Constructors

        public StencilOperation(StencilOperationFunction depthPass, StencilOperationFunction depthFail, StencilOperationFunction stencilFail)
        {
            DepthPass = depthPass;
            DepthFail = depthFail;
            StencilFail = stencilFail;
        }

        #endregion

        #region Constants

        public static readonly StencilOperation Default = new StencilOperation()
        {
            StencilFail = StencilOperationFunction.Keep,
            DepthFail = StencilOperationFunction.Keep,
            DepthPass = StencilOperationFunction.Keep
        };

        #endregion

        #region Comparison

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(StencilOperation a, StencilOperation b)
            => (a.StencilFail == b.StencilFail) && (a.DepthFail == b.DepthFail) && (a.DepthPass == b.DepthPass);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(StencilOperation a, StencilOperation b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
            => StencilFail.GetHashCode() ^ DepthFail.GetHashCode() ^ DepthPass.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(StencilOperation other)
            => this == other;

        public override bool Equals(object obj)
            => (obj is StencilOperation other) ? Equals(other) : false;

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
                OperationBack = value;
            }
        }

        public static readonly StencilMode Disabled = new StencilMode()
        {
            IsEnabled = false,
            Compare = StencilFunction.Default,
            Operation = StencilOperation.Default,
        };

        #endregion

        #region Constructors

        public StencilMode(StencilOperation operation, StencilFunction compare)
        {
            IsEnabled = true;
            CompareFront = compare;
            CompareBack = compare;
            OperationFront = operation;
            OperationBack = operation;
        }

        public StencilMode(StencilOperation frontOperation, StencilFunction frontCompare, StencilOperation backOperation, StencilFunction backCompare)
        {
            IsEnabled = true;
            CompareFront = frontCompare;
            CompareBack = backCompare;
            OperationFront = frontOperation;
            OperationBack = backOperation;
        }

        public StencilMode(StencilOperationFunction depthPass, StencilOperationFunction depthFail, StencilOperationFunction stencilFail, StencilCompareFunction compare, int reference, uint mask)
            : this (new StencilOperation(depthPass, depthFail, stencilFail), new StencilFunction(compare, reference, mask))
        {
        }

        public StencilMode(StencilOperationFunction frontDepthPass, StencilOperationFunction frontDepthFail, StencilOperationFunction frontStencilFail, StencilCompareFunction frontCompare, int frontReference, uint frontMask,
                           StencilOperationFunction backDepthPass,  StencilOperationFunction backDepthFail,  StencilOperationFunction backStencilFail,  StencilCompareFunction backCompare,  int backReference,  uint backMask)
            : this(new StencilOperation(frontDepthPass, frontDepthFail, frontStencilFail), new StencilFunction(frontCompare, frontReference, frontMask),
                  new StencilOperation(backDepthPass, backDepthFail, backStencilFail), new StencilFunction(backCompare, backReference, backMask))
        {
        }

        #endregion

        #region Comparison

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(StencilMode a, StencilMode b)
        {
            if (!a.IsEnabled && !b.IsEnabled)
                return true;
            else if (a.IsEnabled && b.IsEnabled)
            {
                return
                    a.CompareFront == b.CompareFront &&
                    a.CompareBack == b.CompareBack &&
                    a.OperationFront == b.OperationFront &&
                    a.OperationBack == b.OperationBack;
            }
            else
                return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(StencilMode a, StencilMode b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
            => IsEnabled.GetHashCode() ^ CompareFront.GetHashCode() ^ CompareBack.GetHashCode() ^ OperationFront.GetHashCode() ^ OperationBack.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(StencilMode other)
            => this == other;

        public override bool Equals(object obj)
            => (obj is StencilMode other) ? Equals(other) : false;

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
