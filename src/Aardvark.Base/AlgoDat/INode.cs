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
using Aardvark.Base;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.VRVis;

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
        if (!subNodes.Any()) return 0;
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
        if (depth < 0) return [];
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
        if (self == null) return [];
        return self.DepthFirst(n => n.SubNodes);
    }

    /// <summary>
    /// Enumerates all nodes of type T of this scene graph in depth
    /// first order.
    /// </summary>
    public static IEnumerable<T> DepthFirst<T>(this INode self)
        where T : class
    {
        if (self == null) return [];
        return from node in self.DepthFirst()
               where node is T
               select node as T;
    }

    /// <summary>
    /// Enumerates all nodes of this scene graph in breadth first order.
    /// </summary>
    public static IEnumerable<INode> BreadthFirst(this INode self)
    {
        if (self == null) return [];
        return self.BreadthFirst(n => n.SubNodes);
    }

    /// <summary>
    /// Enumerates all nodes of type T of this scene graph in breadth
    /// first order.
    /// </summary>
    public static IEnumerable<T> BreadthFirst<T>(this INode self)
        where T : class
    {
        if (self == null) return [];
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
