namespace Aardvark.Base
{
    /// <summary>
    /// Editable edge that can be splitted.
    /// </summary>
    public interface IBehaviorSplittableEdge2d : IBehavior
    {
        /// <summary>
        /// Splits edge at position nearest to given split position.
        /// </summary>
        void Split(V2d splitPosition);
    }
}
