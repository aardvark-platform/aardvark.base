namespace Aardvark.Base
{
    /// <summary>
    /// Non-generic Interface for Dim.X x Dim.Y x Dim.Z - dimensional volumes.
    /// </summary>
    public interface IVolume
    {
        V3l Dim { get; }
        object GetValue(long x, long y, long z);
        void SetValue(object value, long x, long y, long z);
        object GetValue(V3l v);
        void SetValue(object value, V3l v);
    }

    /// <summary>
    /// Generic Interface for NxMxL-dimensional volume of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface IVolume<T> : IVolume
    {
        T this[long x, long y, long z] { get; set; }
        T this[V3l v] { get; set; }
    }
}
