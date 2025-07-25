/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Base;

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
        if (self is not IBehaviorTransform2d obj) throw new InvalidOperationException("DragRelative requires IBehaviorTransform2d.");

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
