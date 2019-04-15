namespace Aardvark.Base.Rendering
{
    public struct RasterizerState
    {
        public DepthTestMode DepthTest { get; set; }
        public DepthBiasValues DepthBias { get; set; }
        public CullMode CullMode { get; set; }
        public Face FrontFace { get; set; }
        public BlendMode BlendMode { get; set; }
        public FillMode FillMode { get; set; }
        public StencilMode StencilMode { get; set; }

        #region Constructors

        public RasterizerState(RasterizerState old)
        {
            DepthTest = old.DepthTest;
            CullMode = old.CullMode;
            FrontFace = old.FrontFace;
            BlendMode = old.BlendMode;
            FillMode = old.FillMode;
            StencilMode = old.StencilMode;
            DepthBias = old.DepthBias;
        }

        #endregion

        #region Methods

        public RasterizerState ToPremultipliedAlpha()
            => new RasterizerState(this) { BlendMode = BlendMode.ToPremultipliedAlpha() };
        
        #endregion

        #region Constants

        public static readonly RasterizerState Default = new RasterizerState()
        {
            DepthTest = DepthTestMode.LessOrEqual,
            CullMode = CullMode.None,
            BlendMode = BlendMode.None,
            FillMode = FillMode.Fill,
            StencilMode = StencilMode.Disabled
        };

        public static readonly RasterizerState NoDepthTest = new RasterizerState()
        {
            DepthTest = DepthTestMode.None,
            CullMode = CullMode.None,
            BlendMode = BlendMode.None,
            FillMode = FillMode.Fill,
            StencilMode = StencilMode.Disabled
        };

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return DepthTest.GetHashCode() ^ DepthBias.GetHashCode() ^ FrontFace.GetHashCode() ^ CullMode.GetHashCode() ^ BlendMode.GetHashCode() ^ FillMode.GetHashCode() ^ StencilMode.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is RasterizerState rs)
            {
                return rs.DepthTest == DepthTest 
                    && rs.DepthBias == DepthBias
                    && rs.FrontFace == FrontFace 
                    && rs.CullMode == CullMode
                    && rs.BlendMode.Equals(BlendMode) 
                    && rs.StencilMode.Equals(StencilMode)
                    && rs.FillMode == FillMode;
            }
            else return false;
        }

        #endregion
    }
}
