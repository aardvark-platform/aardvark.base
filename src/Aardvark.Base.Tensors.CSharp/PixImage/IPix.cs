namespace Aardvark.Base
{
    /// <summary>
    /// Common interface for image classes such as <see cref="PixImage"/> and <see cref="PixVolume"/>.
    /// </summary>
    public interface IPix
    {
        PixFormat PixFormat { get; }
    }
}
