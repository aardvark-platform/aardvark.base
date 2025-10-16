using System;

namespace Aardvark.Base
{
    /// <summary>
    /// Enumerates the six faces of a cube map.
    /// </summary>
    /// <remarks>
    /// The integer value of each member matches the index of the corresponding face
    /// in a texture array that represents a cube map. This is the conventional order
    /// used throughout the library.
    /// </remarks>
    public enum CubeSide
    {
        PositiveX = 0,
        NegativeX = 1,
        PositiveY = 2,
        NegativeY = 3,
        PositiveZ = 4,
        NegativeZ = 5,
    }

    /// <summary>
    /// Bitmask flags for selecting cube faces.
    /// </summary>
    /// <remarks>
    /// The bit position of each flag corresponds to the <see cref="CubeSide"/> index.
    /// </remarks>
    [Flags]
    public enum CubeSideFlags
    {
        PositiveX = 0x1,
        NegativeX = 0x2,
        PositiveY = 0x4,
        NegativeY = 0x8,
        PositiveZ = 0x10,
        NegativeZ = 0x20,
        All = 0x3f,
    }

    /// <summary>
    /// Represents a cube map consisting of six image mipmap chains (one per face).
    /// </summary>
    /// <seealso cref="PixImageMipMap"/>
    /// <seealso cref="CubeSide"/>
    public class PixCube : IPix
    {
        /// <summary>
        /// The six mipmap chains that make up the cube map, ordered by <see cref="CubeSide"/>.
        /// </summary>
        /// <remarks>
        /// May have more than six elements, excess ones are ignored.
        /// </remarks>
        public PixImageMipMap[] MipMapArray;

        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="PixCube"/> from an array of mipmap chains.
        /// The order of the array must follow the <see cref="CubeSide"/> enumeration.
        /// </summary>
        /// <param name="sides">The six mipmap chains representing the cube faces.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="sides"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">if <paramref name="sides"/> has fewer than six elements.</exception>
        public PixCube(PixImageMipMap[] sides)
        {
            if (sides == null) throw new ArgumentNullException(nameof(sides));
            if (sides.Length < 6) throw new ArgumentException($"Image array must have at least 6 elements, but only has {sides.Length}.", nameof(sides));
            MipMapArray = sides;
        }


        /// <summary>
        /// Initializes a new <see cref="PixCube"/> from an array of base-level images.
        /// The order of the array must follow the <see cref="CubeSide"/> enumeration.
        /// </summary>
        /// <param name="sides">The six images representing the cube faces.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="sides"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">if <paramref name="sides"/> has fewer than six elements.</exception>
        public PixCube(PixImage[] sides)
            => MipMapArray = sides?.Map(image => new PixImageMipMap(image));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the mipmap chain for the specified cube <paramref name="side"/>.
        /// </summary>
        public PixImageMipMap this[CubeSide side]
        {
            get => MipMapArray[(int)side];
            set => MipMapArray[(int)side] = value;
        }

        /// <summary>
        /// Gets the pixel format of the cube map (taken from the first non-empty face).
        /// </summary>
        /// <exception cref="InvalidOperationException">if all faces are empty.</exception>
        public PixFormat PixFormat
        {
            get
            {
                for (int side = 0; side < 6; side++) { if (!MipMapArray[side].IsEmpty) return MipMapArray[side].PixFormat; }
                throw new InvalidOperationException("Cannot determine format of empty PixCube.");
            }
        }

        #endregion
    }
}
