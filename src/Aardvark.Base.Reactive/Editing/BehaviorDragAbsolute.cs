using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    /// <summary>
    /// Behavior extension DragAbsolute.
    /// </summary>
    public static class BehaviorDragAbsolute
    {
        /// <summary>
        /// Updates position until stopDragging completes.
        /// </summary>
        public static async Task<IBehavior> DragAbsolute(this IBehavior self,
            IEvent<V2d> positions, IAwaitable stopDragging, CancellationToken ct
            )
        {
            var obj = self as IBehaviorPosition2d;
            if (obj == null) throw new InvalidOperationException("DragAbsolute requires IBehaviorPosition2d.");

            await stopDragging.RepeatUntilCompleted(
                async delegate { var p = await positions.Next.WithCancellation(ct); obj.Position = p; }
                );

            return self;
        }
    }
}
