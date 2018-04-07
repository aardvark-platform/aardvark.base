namespace Aardvark.Base
{
    /// <summary>
    /// Non-generic Interface for Dim.X x Dim.Y x Dim.Z, Dim.W - dimensional volumes.
    /// </summary>
    public interface ITensor4
    {
        V4l Dim { get; }
        object GetValue(long x, long y, long z, long w);
        void SetValue(object value, long x, long y, long z, long w);
        object GetValue(V4l v);
        void SetValue(object value, V4l v);
    }

    /// <summary>
    /// Non-generic Interface for arbitrarily sized tensors.
    /// </summary>
    public interface ITensor
    {
        long[] Dim { get; }
        object GetValue(params long[] v);
        void SetValue(object value, params long[] v);
    }

    /// <summary>
    /// Generic Interface for NxMxLXK-dimensional volume of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface ITensor4<T> : ITensor4
    {
        T this[long x, long y, long z, long w] { get; set; }
        T this[V4l v] { get; set; }
    }

    /// <summary>
    /// Generic Interface for arbitrarily sized tensors of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface ITensor<T> : ITensor
    {
        int Rank { get; }
        T this[params long[] v] { get; set; }
    }
}
