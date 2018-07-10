using System;
using System.Collections.Generic;

namespace Aardvark.VRVis
{
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
}
