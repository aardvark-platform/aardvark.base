using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class MinimumSpanningTree
    {
        /// <summary>
        /// Creates a minimum spanning tree from a sequence of weighted undirected edges.
        /// Vertices are discovered in source order, and equal-weight candidates are
        /// selected in source edge order. Empty and single-vertex graphs return no edges;
        /// disconnected graphs throw <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="edges"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The graph is disconnected.</exception>
        public static IEnumerable<((TVertex, TVertex), TWeight)> Create<TVertex, TWeight>(
            IEnumerable<((TVertex, TVertex), TWeight)> edges
            )
            where TWeight : IComparable<TWeight>
        {
            if (edges is null) throw new ArgumentNullException(nameof(edges));

            return CreateImpl(edges);
        }

        private static IEnumerable<((TVertex, TVertex), TWeight)> CreateImpl<TVertex, TWeight>(
            IEnumerable<((TVertex, TVertex), TWeight)> edges
            )
            where TWeight : IComparable<TWeight>
        {
            int capacity = edges is ICollection<((TVertex, TVertex), TWeight)> collection
                ? collection.Count
                : edges is IReadOnlyCollection<((TVertex, TVertex), TWeight)> readOnlyCollection
                    ? readOnlyCollection.Count
                    : 0;

            var edgeList = new List<(int V0, int V1, TWeight Weight)>(capacity);
            var vertices = new List<TVertex>(capacity);
            var vertexIndices = new Dictionary<TVertex, int>(capacity);

            int GetVertexIndex(TVertex vertex)
            {
                if (!vertexIndices.TryGetValue(vertex, out int index))
                {
                    index = vertices.Count;
                    vertexIndices.Add(vertex, index);
                    vertices.Add(vertex);
                }
                return index;
            }

            foreach (var sourceEdge in edges)
            {
                var endpoints = sourceEdge.Item1;
                int v0 = GetVertexIndex(endpoints.Item1);
                int v1 = GetVertexIndex(endpoints.Item2);
                edgeList.Add((v0, v1, sourceEdge.Item2));
            }

            int vertexCount = vertices.Count;
            if (vertexCount < 2) yield break;

            // Store incident edge indices compactly. Self-loops establish vertices but
            // never enter the frontier because they cannot connect a new vertex.
            var offsets = new int[vertexCount + 1];
            for (int i = 0; i < edgeList.Count; i++)
            {
                var edge = edgeList[i];
                if (edge.V0 == edge.V1) continue;
                offsets[edge.V0 + 1]++;
                offsets[edge.V1 + 1]++;
            }
            for (int i = 1; i < offsets.Length; i++)
                offsets[i] += offsets[i - 1];

            var incidentEdges = new int[offsets[vertexCount]];
            var nextIncident = new int[vertexCount];
            Array.Copy(offsets, nextIncident, vertexCount);
            for (int i = 0; i < edgeList.Count; i++)
            {
                var edge = edgeList[i];
                if (edge.V0 == edge.V1) continue;
                incidentEdges[nextIncident[edge.V0]++] = i;
                incidentEdges[nextIncident[edge.V1]++] = i;
            }

            // A source edge is enqueued when its first endpoint becomes visited, so a
            // fixed E-slot array is sufficient for the global binary-heap frontier.
            var visited = new bool[vertexCount];
            var frontier = new int[incidentEdges.Length / 2];
            int frontierCount = 0;

            bool Precedes(int leftIndex, int rightIndex)
            {
                int order = edgeList[leftIndex].Weight.CompareTo(edgeList[rightIndex].Weight);
                return order < 0 || (order == 0 && leftIndex < rightIndex);
            }

            void Enqueue(int edgeIndex)
            {
                int index = frontierCount++;
                while (index > 0)
                {
                    int parent = (index - 1) >> 1;
                    int parentEdge = frontier[parent];
                    if (!Precedes(edgeIndex, parentEdge)) break;
                    frontier[index] = parentEdge;
                    index = parent;
                }
                frontier[index] = edgeIndex;
            }

            int Dequeue()
            {
                int result = frontier[0];
                int last = frontier[--frontierCount];
                if (frontierCount == 0) return result;

                int index = 0;
                while (true)
                {
                    int child = (index << 1) + 1;
                    if (child >= frontierCount) break;
                    int right = child + 1;
                    if (right < frontierCount && Precedes(frontier[right], frontier[child]))
                        child = right;

                    int childEdge = frontier[child];
                    if (!Precedes(childEdge, last)) break;
                    frontier[index] = childEdge;
                    index = child;
                }
                frontier[index] = last;
                return result;
            }

            void AddFrontier(int vertex)
            {
                for (int i = offsets[vertex]; i < offsets[vertex + 1]; i++)
                {
                    int edgeIndex = incidentEdges[i];
                    var edge = edgeList[edgeIndex];
                    int other = edge.V0 == vertex ? edge.V1 : edge.V0;
                    if (!visited[other]) Enqueue(edgeIndex);
                }
            }

            visited[0] = true;
            int visitedCount = 1;
            AddFrontier(0);

            while (visitedCount < vertexCount)
            {
                int edgeIndex;
                int previous;
                int next;
                while (true)
                {
                    if (frontierCount == 0)
                        throw new InvalidOperationException(
                            "The graph is disconnected and cannot produce a spanning tree.");

                    edgeIndex = Dequeue();
                    var candidate = edgeList[edgeIndex];
                    bool visited0 = visited[candidate.V0];
                    bool visited1 = visited[candidate.V1];
                    if (visited0 == visited1) continue;

                    previous = visited0 ? candidate.V0 : candidate.V1;
                    next = visited0 ? candidate.V1 : candidate.V0;
                    break;
                }

                visited[next] = true;
                visitedCount++;
                AddFrontier(next);

                var edge = edgeList[edgeIndex];
                yield return ((vertices[previous], vertices[next]), edge.Weight);
            }
        }
    }
}
