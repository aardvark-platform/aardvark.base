namespace Aardvark.Base
{
    /// <summary>
    /// Editable object can be transformed in 2d.
    /// </summary>
    public interface IBehaviorTransform2d : IBehavior
    {
        /// <summary>
        /// Transform in 2d.
        /// </summary>
        void Transform(M33d trafo);
    }
}
