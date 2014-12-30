namespace Aardvark.Base
{
    /// <summary>
    /// An editable vertex.
    /// </summary>
    public interface IEditableVertex2d :
        IBehaviorPosition2d,
        IBehaviorTransform2d,
        IBehaviorDeletable,
        IEditableSequence<V2d>
    {
    }
}
