using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region Overrides

        public override int GetHashCode()
        {
            return !Enabled ? 0 : SourceFactor.GetHashCode() ^ SourceAlphaFactor.GetHashCode() ^ DestinationFactor.GetHashCode() ^ DestinationAlphaFactor.GetHashCode() ^ Operation.GetHashCode() ^ AlphaOperation.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BlendMode)
            {
                var bm = (BlendMode)obj;
                return (!Enabled && !bm.Enabled) || (bm.SourceFactor == SourceFactor && bm.SourceAlphaFactor == SourceAlphaFactor && bm.DestinationFactor == DestinationFactor && bm.DestinationAlphaFactor == DestinationAlphaFactor && bm.Operation == Operation && bm.AlphaOperation == AlphaOperation);
            }
            else return false;
        }

        #endregion
    }
}
