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

        #region Overrides

        public override int GetHashCode()
            => Function.GetHashCode() ^ Reference.GetHashCode() ^ Mask.GetHashCode();

        public override bool Equals(object obj) => (obj is StencilFunction f)
            ? (f.Function == Function && f.Reference == Reference && f.Mask == Mask)
            : false;

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

        #region Overrides

        public override int GetHashCode()
            => StencilFail.GetHashCode() ^ DepthFail.GetHashCode() ^ DepthPass.GetHashCode();

        public override bool Equals(object obj) => (obj is StencilOperation o)
            ? (o.StencilFail == StencilFail && o.DepthFail == DepthFail && o.DepthPass == DepthPass)
            : false;

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

        public StencilMode(StencilOperationFunction depthPass, StencilOperationFunction depthFail, StencilOperationFunction stencilFail, StencilCompareFunction compare, int reference, uint mask)
            : this (new StencilOperation(depthPass, depthFail, stencilFail), new StencilFunction(compare, reference, mask))
        {
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
            => IsEnabled.GetHashCode() ^ CompareFront.GetHashCode() ^ CompareBack.GetHashCode() ^ OperationFront.GetHashCode() ^ OperationBack.GetHashCode();
       
        public override bool Equals(object obj)
        {
            if (obj is StencilMode m)
            {
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
