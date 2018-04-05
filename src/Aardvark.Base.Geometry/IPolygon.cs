namespace Aardvark.Base
{
    public interface IPolygon<T>
    {
        int PointCount { get; }
        T this[int index] { get; } // counter clockwise access
    }
}
