using System.Runtime.CompilerServices;

namespace Aardvark.Base.Rendering
{
    public enum BlendOperation
    {
        Add = 1,
        Subtract = 2,
        ReverseSubtract = 3,

        /// <summary>
        /// NOTE: No pre-blend with this operation
        /// </summary>
        Minimum = 4,

        /// <summary>
        /// NOTE: No pre-blend with this operation
        /// </summary>
        Maximum = 5,
    }

    public enum BlendFactor
    {
        Zero,
        One,
        SourceColor,
        InvSourceColor,
        SourceAlpha,
        InvSourceAlpha,
        DestinationAlpha,
        InvDestinationAlpha,
        DestinationColor,
        InvDestinationColor,
        SourceAlphaSat,
        BlendFactor,
        InvBlendFactor,
        SecondarySourceColor,
        SecondarySourceAlpha,
        InvSecondarySourceColor,
        InvSecondarySourceAlpha,
    }

    public struct BlendMode
    {
        public bool Enabled;
        public BlendFactor SourceFactor;
        public BlendFactor SourceAlphaFactor;
        public BlendFactor DestinationFactor;
        public BlendFactor DestinationAlphaFactor;
        public BlendOperation Operation;
        public BlendOperation AlphaOperation;

        public BlendMode(bool enabled)
        {
            Enabled = enabled;
            SourceFactor = BlendFactor.SourceAlpha;
            DestinationFactor = BlendFactor.InvSourceAlpha;
            Operation = BlendOperation.Add;

            SourceAlphaFactor = BlendFactor.SourceAlpha;
            DestinationAlphaFactor = BlendFactor.InvSourceAlpha;
            AlphaOperation = BlendOperation.Add;
        }

        public BlendMode ToPremultipliedAlpha()
        {
            //TODO: think about this conversion
            return new BlendMode()
            {
                Enabled = Enabled,
                SourceFactor = SourceFactor,
                DestinationFactor = DestinationFactor,
                Operation = Operation,

                AlphaOperation = Operation,
                SourceAlphaFactor = BlendFactor.InvDestinationAlpha,
                DestinationAlphaFactor = BlendFactor.One
            };
        }

        #region Constants

        public static readonly BlendMode Blend = new BlendMode(true);
        public static readonly BlendMode None = new BlendMode(false);

        #endregion

        #region Comparison

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BlendMode a, BlendMode b)
        {
            if (!a.Enabled && !b.Enabled)
                return true;
            else if (a.Enabled && b.Enabled)
            {
                return 
                    a.SourceFactor == b.SourceFactor &&
                    a.SourceAlphaFactor == b.SourceAlphaFactor &&
                    a.DestinationFactor == b.DestinationFactor &&
                    a.DestinationAlphaFactor == b.DestinationAlphaFactor &&
                    a.Operation == b.Operation &&
                    a.AlphaOperation == b.AlphaOperation;
            }
            else
                return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BlendMode a, BlendMode b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return !Enabled ? 0 : SourceFactor.GetHashCode() ^ SourceAlphaFactor.GetHashCode() ^ DestinationFactor.GetHashCode() ^ DestinationAlphaFactor.GetHashCode() ^ Operation.GetHashCode() ^ AlphaOperation.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BlendMode other)
            => this == other;

        public override bool Equals(object obj)
            => (obj is BlendMode other) ? Equals(other) : false;

        #endregion
    }
}
