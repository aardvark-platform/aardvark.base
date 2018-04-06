namespace Aardvark.Base
{
    /// <summary>
    /// An editable face.
    /// </summary>
    public interface IEditableFace2d :
        IBehaviorPosition2d,
        IBehaviorTransform2d,
        IBehaviorDeletable
    {
        /// <summary>
        /// Returns index-th vertex as editable vertex.
        /// </summary>
        IEditableVertex2d GetEditableVertex(int index);

        /// <summary>
        /// Returns index-th edge as editable edge.
        /// </summary>
        IEditableEdge2d GetEditableEdge(int index);
    }
}
