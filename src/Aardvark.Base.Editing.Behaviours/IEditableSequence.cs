namespace Aardvark.Base
{
    /// <summary>
    /// Exposes sequence of editing steps.
    /// </summary>
    public interface IEditableSequence<T>
    {
        /// <summary>
        /// Sets or gets current version of editable object.
        /// </summary>
        T Current { get; set; }

        /// <summary>
        /// Sequence of editing steps. 
        /// </summary>
        IEvent<T> EditingSteps { get; }
    }
}
