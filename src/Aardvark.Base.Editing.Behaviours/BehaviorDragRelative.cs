using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    /// <summary>
    /// Behavior extension DragRelative.
    /// </summary>
    public static class BehaviorDragRelative
    {
        /// <summary>
        /// Updates position until stopDragging completes.
        /// </summary>
        public static async Task<IBehavior> DragRelative(this IBehavior self,
            V2d initialPosition, IEvent<V2d> moves, IAwaitable stopDragging, CancellationToken ct
            )
        {
            var obj = self as IBehaviorTransform2d;
            if (obj == null) throw new InvalidOperationException("DragRelative requires IBehaviorTransform2d.");

            var lastPos = initialPosition;
            await stopDragging.RepeatUntilCompleted(
                async delegate
                {
                    var p = await moves.Next.WithCancellation(ct);
                    obj.Transform(M33d.Translation(p - lastPos));
                    lastPos = p;
                });

            return self;
        }
    }
}
