using System.Runtime.CompilerServices;

namespace Aardvark.Base.Rendering
{
    public struct RasterizerState
    {
        public DepthTestMode DepthTest { get; set; }
        public DepthBiasState DepthBias { get; set; }
        public CullMode CullMode { get; set; }
        public WindingOrder FrontFace { get; set; }
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

        #region Comparison

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RasterizerState a, RasterizerState b)
        {
            return (a.DepthTest == b.DepthTest) &&
                (a.DepthBias == b.DepthBias) &&
                (a.FrontFace == b.FrontFace) &&
                (a.CullMode == b.CullMode) &&
                (a.BlendMode == b.BlendMode) &&
                (a.StencilMode == b.StencilMode) &&
                (a.FillMode == b.FillMode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RasterizerState a, RasterizerState b)
            => !(a == b);

        #endregion

            #region Overrides

        public override int GetHashCode()
        {
            return DepthTest.GetHashCode() ^ DepthBias.GetHashCode() ^ FrontFace.GetHashCode() ^ CullMode.GetHashCode() ^ BlendMode.GetHashCode() ^ FillMode.GetHashCode() ^ StencilMode.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RasterizerState other)
        {
            return other.DepthTest.Equals(DepthTest)
                && other.DepthBias.Equals(DepthBias)
                && other.FrontFace.Equals(FrontFace)
                && other.CullMode.Equals(CullMode)
                && other.BlendMode.Equals(BlendMode)
                && other.StencilMode.Equals(StencilMode)
                && other.FillMode.Equals(FillMode);
        }

        public override bool Equals(object other)
            => (other is RasterizerState o) ? Equals(o) : false;

        #endregion
    }
}
