namespace Aardvark.Base
{
    public interface IOpacity
    {
        /// <summary>
        /// In range [0,1], where 0 means fully transparent, and 1 is fully opaque.
        /// </summary>
        double Opacity { get; set; }
    }
}
