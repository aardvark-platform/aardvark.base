namespace Aardvark.Base
{
    /// <summary>
    /// Editable object can be deleted.
    /// </summary>
    public interface IBehaviorDeletable : IBehavior
    {
        /// <summary>
        /// Deletes editable object.
        /// </summary>
        void Delete();
    }
}
