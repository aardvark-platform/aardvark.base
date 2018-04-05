namespace Aardvark.Base
{
    public interface IBoundingRange1d
    {
        Range1d BoundingRange1d { get; }
    }

    /// <summary>
    /// Return an axis aligned two-dimensional box that contains the complete
    /// geometry.
    /// </summary>
    public interface IBoundingBox2d
    {
        Box2d BoundingBox2d { get; }
    }

    /// <summary>
    /// Return an axis aligned three-dimensional box that contains the complete
    /// geometry.
    /// </summary>
    public interface IBoundingBox3d
    {
        Box3d BoundingBox3d { get; }
    }
}
