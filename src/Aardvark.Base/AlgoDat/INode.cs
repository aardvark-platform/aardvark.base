using Aardvark.Base;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.VRVis
{
    /// <summary>
    /// A simple hierarchical node interface without any
    /// notion of a traversal state.
    /// </summary>
    public interface INode
    {
        IEnumerable<INode> SubNodes { get; }
    }

    /// <summary>
    /// Immediately supply some basic traversal functionality.
    /// </summary>
    public static class INodeExtensions
    {
        public static int ComputeDepth(this INode self)
        {
            var subNodes = self.SubNodes;
            if (subNodes.Count() == 0) return 0;
            return 1 + subNodes.Max(x => x.ComputeDepth());
        }

        public static IEnumerable<T> NodesAtDepth<T>(this INode self, int depth)
            where T : class
        {
            return from node in self.NodesAtDepth(depth)
                   where node is T
                   select node as T;
        }

        /// <summary>
        /// Returns all nodes of a graph that are at the same depth as a flat
        /// sequence.
        /// </summary>
        public static IEnumerable<INode> NodesAtDepth(this INode self, int depth)
        {
            if (depth < 0) return Enumerable.Empty<INode>();
            if (depth == 0) return self.IntoIEnumerable();
            var subNodes = self.SubNodes;
            if (depth == 1) return subNodes;
            return from sn in subNodes
                   from n in sn.NodesAtDepth(depth - 1)
                   select n;
        }

        /// <summary>
        /// Enumerates all nodes of this scene graph in depth first order.
        /// </summary>
        public static IEnumerable<INode> DepthFirst(this INode self)
        {
            if (self == null) return new INode[0];
            return self.DepthFirst(n => n.SubNodes);
        }

        /// <summary>
        /// Enumerates all nodes of type T of this scene graph in depth
        /// first order.
        /// </summary>
        public static IEnumerable<T> DepthFirst<T>(this INode self)
            where T : class
        {
            if (self == null) return Enumerable.Empty<T>();
            return from node in self.DepthFirst()
                   where node is T
                   select node as T;
        }

        /// <summary>
        /// Enumerates all nodes of this scene graph in breadth first order.
        /// </summary>
        public static IEnumerable<INode> BreadthFirst(this INode self)
        {
            if (self == null) return Enumerable.Empty<INode>();
            return self.BreadthFirst(n => n.SubNodes);
        }

        /// <summary>
        /// Enumerates all nodes of type T of this scene graph in breadth
        /// first order.
        /// </summary>
        public static IEnumerable<T> BreadthFirst<T>(this INode self)
            where T : class
        {
            if (self == null) return Enumerable.Empty<T>();
            return from node in self.BreadthFirst()
                   where node is T
                   select node as T;
        }

        /// <summary>
        /// Enumerates descendent nodes including the node itself (uses Depth-First traversal order)
        /// </summary>
        public static IEnumerable<T> DescendentsAndSelf<T>(this T node)
            where T : class, INode
        {
            return node.DepthFirst<T>();
        }

        /// <summary>
        /// Enumerates all descendent nodes (uses Depth-First traversal order and skips first)
        /// </summary>
        public static IEnumerable<T> Descendents<T>(this T node)
            where T : class, INode
        {
            return node.DepthFirst<T>().Skip(1);
        }
    }
}
