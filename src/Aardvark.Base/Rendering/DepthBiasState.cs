using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aardvark.Base.Rendering
{
    public struct DepthBiasState
    {
        public readonly double Constant;
        public readonly double SlopeScale;
        public readonly double Clamp;

        public bool BiasEnabled => Constant != 0 || SlopeScale != 0;
        public bool UseClamp => Clamp != 0;

        public DepthBiasState(double constantBias, double slopeScaleBias, double depthBiasClamp = 0)
        {
            Constant = constantBias;
            SlopeScale = slopeScaleBias;
            Clamp = depthBiasClamp;
        }

        public override int GetHashCode() 
            => HashCode.Combine(Constant.GetHashCode(), SlopeScale.GetHashCode(), Clamp.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(DepthBiasState other)
            => Constant.Equals(other.Constant) && SlopeScale.Equals(other.SlopeScale) && Clamp.Equals(other.Clamp);

        public override bool Equals(object other)
            => (other is DepthBiasState o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(DepthBiasState l, DepthBiasState r)
            => l.Constant == r.Constant && l.SlopeScale == r.SlopeScale && l.Clamp == r.Clamp;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(DepthBiasState l, DepthBiasState r)
            => l.Constant != r.Constant || l.SlopeScale != r.SlopeScale || l.Clamp != r.Clamp;
    }
}
