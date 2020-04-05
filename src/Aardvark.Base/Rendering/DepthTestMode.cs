using System.Runtime.CompilerServices;

namespace Aardvark.Base.Rendering
{
    public struct DepthTestMode
    {
        public readonly DepthTestComparison Comparison;
        public readonly Range1d Bounds;

        public DepthTestMode(DepthTestComparison comparison)
        {
            Comparison = comparison;
            Bounds = Range1d.Infinite;
        }

        public DepthTestMode(DepthTestComparison comparison, Range1d range)
        {
            Comparison = comparison;
            Bounds = range;
        }

        public DepthTestMode(DepthTestMode other, Range1d range)
        {
            Comparison = other.Comparison;
            Bounds = range;
        }

        public bool Clamp => !Bounds.IsInfinite;

        public bool IsEnabled => Comparison != DepthTestComparison.None || !Bounds.IsInfinite;

        public static readonly DepthTestMode None = new DepthTestMode(DepthTestComparison.None);
        public static readonly DepthTestMode Less = new DepthTestMode(DepthTestComparison.Less);
        public static readonly DepthTestMode LessOrEqual = new DepthTestMode(DepthTestComparison.LessOrEqual);
        public static readonly DepthTestMode Greater = new DepthTestMode(DepthTestComparison.Greater);
        public static readonly DepthTestMode GreaterOrEqual = new DepthTestMode(DepthTestComparison.GreaterOrEqual);
        public static readonly DepthTestMode Equal = new DepthTestMode(DepthTestComparison.Equal);
        public static readonly DepthTestMode NotEqual = new DepthTestMode(DepthTestComparison.NotEqual);
        public static readonly DepthTestMode Never = new DepthTestMode(DepthTestComparison.Never);
        public static readonly DepthTestMode Always = new DepthTestMode(DepthTestComparison.Always);

        public override int GetHashCode() => HashCode.Combine(Comparison.GetHashCode(), Bounds.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(DepthTestMode other)
            => Comparison.Equals(other.Comparison) && Bounds.Equals(other.Bounds);

        public override bool Equals(object obj)
            => (obj is DepthTestMode o) ? Equals(o) : false;

        public override string ToString()
            => Bounds.IsInfinite ? Comparison.ToString() : string.Format("{0}/{1}", Comparison, Bounds);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(DepthTestMode l, DepthTestMode r)
            => l.Comparison == r.Comparison && l.Bounds == r.Bounds;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(DepthTestMode l, DepthTestMode r)
            => l.Comparison != r.Comparison || l.Bounds != r.Bounds;
    }
}
