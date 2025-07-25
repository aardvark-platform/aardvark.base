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
using System.Collections.Generic;

namespace Aardvark.VRVis;

public static class TreeExtensions
{
    public static IEnumerable<T> DepthFirst<T>(
        this T self,
        Func<T, IEnumerable<T>> subNodes
        )
    {
        yield return self;

        var stack = new Stack<IEnumerator<T>>();
        stack.Push(subNodes(self).GetEnumerator());

        while (stack.Count > 0)
        {
            var e = stack.Peek();
            if (e.MoveNext())
            {
                yield return e.Current;
                stack.Push(subNodes(e.Current).GetEnumerator());
            }
            else
            {
                stack.Pop();
            }
        }
    }

    public static IEnumerable<T> BreadthFirst<T>(
        this T self,
        Func<T, IEnumerable<T>> subNodes
        )
    {
        yield return self;

        var queue = new Queue<IEnumerable<T>>();
        queue.Enqueue(subNodes(self));

        while (queue.Count > 0)
        {
            var nodes = queue.Dequeue();
            if (nodes == null) continue;

            foreach (var n in nodes)
            {
                yield return n;
                queue.Enqueue(subNodes(n));
            }
        }
    }
}
