using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base.Rendering
{
    public struct RasterizerState
    {
        private DepthTestMode m_depthTest;
        private CullMode m_cullMode;
        private BlendMode m_blendMode; 
        private FillMode m_fillMode ;
        private StencilMode m_stencilMode;

        public DepthTestMode DepthTest { get { return m_depthTest; } set { m_depthTest = value; } }
        public CullMode CullMode { get { return m_cullMode; } set { m_cullMode = value; } }
        public BlendMode BlendMode { get { return m_blendMode; } set { m_blendMode = value; } }
        public FillMode FillMode { get { return m_fillMode; } set { m_fillMode = value; } }
        public StencilMode StencilMode { get { return m_stencilMode; } set { m_stencilMode = value; } }

        #region Constructors

        public RasterizerState(RasterizerState old)
        {
            m_depthTest = old.DepthTest;
            m_cullMode = old.CullMode;
            m_blendMode = old.BlendMode;
            m_fillMode = old.FillMode;
            m_stencilMode = old.StencilMode;
        }

        #endregion

        #region Methods

        public RasterizerState ToPremultipliedAlpha()
        {
            return new RasterizerState(this)
            {
                BlendMode = BlendMode.ToPremultipliedAlpha()
            };
        }

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
            return DepthTest.GetHashCode() ^ CullMode.GetHashCode() ^ BlendMode.GetHashCode() ^ FillMode.GetHashCode() ^ StencilMode.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is RasterizerState)
            {
                var rs = (RasterizerState)obj;
                return rs.DepthTest == DepthTest && rs.CullMode == CullMode && rs.BlendMode.Equals(BlendMode) && rs.StencilMode.Equals(StencilMode) && rs.FillMode == FillMode;
            }
            else return false;
        }

        #endregion
    }
}
