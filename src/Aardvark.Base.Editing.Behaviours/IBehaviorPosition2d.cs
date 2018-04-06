namespace Aardvark.Base
{
    /// <summary>
    /// Editable object has a position in 2d.
    /// </summary>
    public interface IBehaviorPosition2d : IBehavior
    {
        /// <summary>
        /// Gets or sets 2d position.
        /// </summary>
        V2d Position { get; set; }
    }
}