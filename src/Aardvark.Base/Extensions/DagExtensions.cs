using System;
using System.Collections.Generic;

namespace Aardvark.VRVis
{
    public static class TreeExtensions
    {
        /// <summary>
        /// Enumerates a tree iteratively in depth-first preorder.
        /// </summary>
        /// <remarks>
        /// The traversal owns every child enumerator obtained from <paramref name="subNodes"/>
        /// and disposes it after exhaustion or when enumeration stops early or fails.
        /// </remarks>
        public static IEnumerable<T> DepthFirst<T>(
            this T self,
            Func<T, IEnumerable<T>> subNodes
            )
        {
            yield return self;

            var stack = new Stack<IEnumerator<T>>();
            try
            {
                stack.Push(subNodes(self).GetEnumerator());

                while (stack.Count > 0)
                {
                    var enumerator = stack.Peek();
                    if (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        yield return current;
                        stack.Push(subNodes(current).GetEnumerator());
                    }
                    else
                    {
                        stack.Pop().Dispose();
                    }
                }
            }
            finally
            {
                while (stack.Count > 0)
                    stack.Pop().Dispose();
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
