using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Rendering
{
    public struct DepthBiasValues
    {
        public readonly double Bias;
        public readonly double SlopeScale;
        public readonly double Clamp;

        public bool BiasEnabled => Bias != 0 && SlopeScale != 0;
        public bool UseClamp => Clamp == 0;

        public DepthBiasValues(double constantBias, double slopeScaleBias, double depthBiasClamp = 0)
        {
            Bias = constantBias;
            SlopeScale = slopeScaleBias;
            Clamp = depthBiasClamp;
        }

        public override int GetHashCode() 
            => HashCode.Combine(Bias.GetHashCode(), SlopeScale.GetHashCode(), Clamp.GetHashCode());

        public override bool Equals(object obj)
            => (obj is DepthBiasValues o) ? Bias == o.Bias && SlopeScale == o.SlopeScale && Clamp == o.Clamp : false;

        public static bool operator ==(DepthBiasValues l, DepthBiasValues r)
            => l.Bias == r.Bias && l.SlopeScale == r.SlopeScale && l.Clamp == r.Clamp;

        public static bool operator !=(DepthBiasValues l, DepthBiasValues r)
            => l.Bias != r.Bias || l.SlopeScale != r.SlopeScale || l.Clamp != r.Clamp;
    }
}
