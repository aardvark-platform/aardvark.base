using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool Clamp
        {
            get { return !Bounds.IsInfinite; }
        }

        public bool IsEnabled
        {
            get { return Comparison != DepthTestComparison.None || !Bounds.IsInfinite; }
        }

        public static readonly DepthTestMode None = new DepthTestMode(DepthTestComparison.None);
        public static readonly DepthTestMode Less = new DepthTestMode(DepthTestComparison.Less);
        public static readonly DepthTestMode LessOrEqual = new DepthTestMode(DepthTestComparison.LessOrEqual);
        public static readonly DepthTestMode Greater = new DepthTestMode(DepthTestComparison.Greater);
        public static readonly DepthTestMode GreaterOrEqual = new DepthTestMode(DepthTestComparison.GreaterOrEqual);
        public static readonly DepthTestMode Equal = new DepthTestMode(DepthTestComparison.Equal);
        public static readonly DepthTestMode NotEqual = new DepthTestMode(DepthTestComparison.NotEqual);
        public static readonly DepthTestMode Never = new DepthTestMode(DepthTestComparison.Never);
        public static readonly DepthTestMode Always = new DepthTestMode(DepthTestComparison.Always);

        public override int GetHashCode()
        {
            return HashCode.Combine(Comparison.GetHashCode(), Bounds.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj is DepthTestMode)
            {
                var o = (DepthTestMode)obj;
                return Comparison == o.Comparison && Bounds == o.Bounds;
            }
            else return false;
        }

        public override string ToString()
        {
            if (Bounds.IsInfinite) return Comparison.ToString();
            else return string.Format("{0}/{1}", Comparison, Bounds);
        }

        public static bool operator ==(DepthTestMode l, DepthTestMode r)
        {
            return l.Comparison == r.Comparison && l.Bounds == r.Bounds;
        }
        public static bool operator !=(DepthTestMode l, DepthTestMode r)
        {
            return l.Comparison != r.Comparison || l.Bounds != r.Bounds;
        }
    }
}
