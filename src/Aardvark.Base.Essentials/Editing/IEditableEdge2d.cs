namespace Aardvark.Base
{
    /// <summary>
    /// An editable edge.
    /// </summary>
    public interface IEditableEdge2d :
        IBehaviorPosition2d,
        IBehaviorTransform2d,
        IBehaviorSplittableEdge2d,
        IBehaviorDeletable,
        IEditableSequence<Line2d>
    {
    }
}
