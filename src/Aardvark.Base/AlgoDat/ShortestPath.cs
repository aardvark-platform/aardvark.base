using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public interface IShortestPath<T>
    {
        void Cancel();
        void CalculateShortestPaths(T pos);
        void CalculateShortestPathsByIndex(int posIdx);
        List<T> GetMinimalPath(T pos);
        List<T> GetMinimalPathByIndex(int posIdx);
    }

    /// <summary>
    /// Asynchronously computes shortest paths from a seed node.
    /// </summary>
    /// <remarks>
    /// Starting a calculation cancels and replaces the current calculation. Path queries are
    /// thread-safe and use the last fully completed result until the replacement completes.
    /// Costs must be finite and non-negative, with finite accumulated path costs. The frontier
    /// expands a node of minimum tentative cost, including after decreasing an active node's cost.
    /// </remarks>
    public class ShortestPath<T> : IShortestPath<T>
    {
        private sealed class CalculationRun : IDisposable
        {
            private readonly object m_resourceLock = new object();
            private readonly CancellationTokenSource m_cancellation = new CancellationTokenSource();
            private bool m_disposed;

            public CalculationRun(int seedIndex, int nodeCount)
            {
                SeedIndex = seedIndex;
                Expanded = new bool[nodeCount];
                Predecessors = new int[nodeCount].Set(seedIndex);
                Token = m_cancellation.Token;
            }

            public int SeedIndex { get; }
            public bool[] Expanded { get; }
            public int[] Predecessors { get; }
            public CancellationToken Token { get; }
            public Task Task { get; set; }

            public void Cancel()
            {
                lock (m_resourceLock)
                {
                    if (!m_disposed)
                        m_cancellation.Cancel();
                }
            }

            public void Dispose()
            {
                lock (m_resourceLock)
                {
                    if (m_disposed)
                        return;

                    m_disposed = true;
                    m_cancellation.Dispose();
                }
            }
        }

        private sealed class ResultSnapshot
        {
            public ResultSnapshot(int seedIndex, bool[] expanded, int[] predecessors)
            {
                SeedIndex = seedIndex;
                Expanded = expanded;
                Predecessors = predecessors;
            }

            public int SeedIndex { get; }
            public bool[] Expanded { get; }
            public int[] Predecessors { get; }
        }

        private readonly List<T> m_nodes;
        private readonly List<int>[] m_neighbors;
        private readonly Func<T, T, float> m_getCostFunc;
        private readonly object m_runLock = new object();
        private CalculationRun m_currentRun;
        private ResultSnapshot m_result;

        public ShortestPath(List<T> nodes, List<(int, int)> edges, Func<T, T, float> getCostFunc)
        {
            m_nodes = nodes;
            m_neighbors = new List<int>[nodes.Count];
            for(int i = 0; i<m_nodes.Count; i++) m_neighbors[i] = new List<int>();
            foreach (var e in edges)
            {
                m_neighbors[e.Item1].Add(e.Item2);
                m_neighbors[e.Item2].Add(e.Item1);
            }
            m_getCostFunc = getCostFunc;
            m_result = CreateInitialResult(nodes.Count);
        }

        public ShortestPath(T[] nodes, List<int>[] neighbors, Func<T, T, float> getCostFunc)
        {
            m_nodes = nodes.ToList();
            m_neighbors = neighbors;
            m_getCostFunc = getCostFunc;
            m_result = CreateInitialResult(nodes.Length);
        }

        /// <summary>
        /// Cancels and waits for the current calculation, if any.
        /// </summary>
        /// <remarks>The last successfully completed result remains available to path queries.</remarks>
        public void Cancel()
        {
            CalculationRun run;
            lock (m_runLock)
            {
                run = m_currentRun;
                m_currentRun = null;
            }

            if (run == null)
                return;

            run.Cancel();
            try
            {
                run.Task.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException e) when (
                run.Token.IsCancellationRequested && e.CancellationToken == run.Token)
            {
            }
            finally
            {
                run.Dispose();
            }
        }

        /// <summary>
        /// Starts a shortest-path calculation from <paramref name="seed"/>.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="seed"/> is not present in the node collection.
        /// </exception>
        public void CalculateShortestPaths(T seed)
        {
            var index = m_nodes.IndexOf(seed);
            if (index < 0)
                throw new ArgumentException("The seed node is not present in the graph.", nameof(seed));

            StartCalculation(index);
        }

        /// <summary>
        /// Starts a shortest-path calculation from the node at <paramref name="index"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is outside the node collection.
        /// </exception>
        public void CalculateShortestPathsByIndex(int index)
        {
            if (index < 0 || index >= m_nodes.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            StartCalculation(index);
        }

        private static ResultSnapshot CreateInitialResult(int nodeCount)
        {
            return new ResultSnapshot(0, new bool[nodeCount], new int[nodeCount]);
        }

        private void StartCalculation(int seedIndex)
        {
            var run = new CalculationRun(seedIndex, m_nodes.Count);
            CalculationRun previous;

            try
            {
                lock (m_runLock)
                {
                    previous = m_currentRun;
                    run.Task = Task.Run(() => Calculate(run));
                    m_currentRun = run;
                }
            }
            catch
            {
                run.Dispose();
                throw;
            }

            previous?.Cancel();
        }

        private void Calculate(CalculationRun run)
        {
            var reportStarted = false;
            try
            {
                Report.BeginTimed("Shortest paths calculation");
                reportStarted = true;

                var token = run.Token;
                token.ThrowIfCancellationRequested();

                var activePixels = new FibonacciHeap<int>();
                var inActiveList = new Dictionary<int, FibonacciHeap<int>.Node>();
                inActiveList[run.SeedIndex] = activePixels.Insert(0, run.SeedIndex);

                var totalCost = new float[m_nodes.Count].Set(float.MaxValue);
                totalCost[run.SeedIndex] = 0;

                while (!activePixels.IsEmpty())
                {
                    token.ThrowIfCancellationRequested();

                    var q = activePixels.DeleteMin();
                    token.ThrowIfCancellationRequested();
                    inActiveList.Remove(q);

                    run.Expanded[q] = true;
                    var totalCostQ = totalCost[q];

                    foreach (var r in m_neighbors[q])
                    {
                        token.ThrowIfCancellationRequested();
                        if (run.Expanded[r])
                            continue;

                        var edgeCost = m_getCostFunc(m_nodes[q], m_nodes[r]);
                        token.ThrowIfCancellationRequested();
                        var newCost = totalCostQ + edgeCost;
                        var isActive = inActiveList.TryGetValue(r, out var activeNode);

                        if (!isActive || newCost < totalCost[r])
                        {
                            if (isActive)
                                activePixels.DecreaseKey(activeNode, newCost);
                            else
                                inActiveList[r] = activePixels.Insert(newCost, r);

                            totalCost[r] = newCost;
                            run.Predecessors[r] = q;
                        }
                    }
                }

                token.ThrowIfCancellationRequested();
                lock (m_runLock)
                {
                    token.ThrowIfCancellationRequested();
                    if (ReferenceEquals(m_currentRun, run))
                        Volatile.Write(
                            ref m_result,
                            new ResultSnapshot(run.SeedIndex, run.Expanded, run.Predecessors));
                }
            }
            finally
            {
                try
                {
                    if (reportStarted)
                        Report.End();
                }
                finally
                {
                    run.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the path from the indexed target toward the seed using one completed result snapshot.
        /// Reachable paths exclude the seed; the seed path is empty. Unreachable targets return [target, seed].
        /// </summary>
        public List<T> GetMinimalPathByIndex(int endIndex)
        {
            var result = Volatile.Read(ref m_result);

            var contour = new List<T>();
            var id = endIndex;
            var end = m_nodes[id];
            var seed = m_nodes[result.SeedIndex];

            if (!result.Expanded[id])
                return new List<T>() { end, seed };

            while (id != result.SeedIndex)
            {
                contour.Add(m_nodes[id]);
                id = result.Predecessors[id];
            }
            return contour;
        }

        /// <summary>
        /// Gets the path from <paramref name="end"/> toward the seed using one completed result snapshot.
        /// </summary>
        public List<T> GetMinimalPath(T end)
        {
            var id = m_nodes.IndexOf(end);
            return GetMinimalPathByIndex(id);
        }
    }

    /// <summary>
    /// Internal minimum-priority frontier. Insert and decrease-key take O(1) amortized time;
    /// extraction takes O(log n) amortized time. Keys must be finite and decreases must not increase a key.
    /// </summary>
    class FibonacciHeap<T>
    {
        public sealed class Node
        {
            // Intrusive links use registry indices so consolidation does not pay a GC
            // write barrier for every sibling and parent update.
            private FibonacciHeap<T> _heap;
            internal readonly int _index;
            private int _parent = -1;
            private int _left;
            private int _right;
            private int _child = -1;
            private int _degree;
            private bool _released;

            internal Node(FibonacciHeap<T> heap, int index, float key, T item)
            {
                _heap = heap;
                _index = index;
                Key = key;
                Value = item;
                _left = _right = index;
            }

            public T Value { get; }
            public Node Parent => _parent < 0 ? null : _heap._nodes[_parent];
            public Node Left => _released ? this : _heap._nodes[_left];
            public Node Right => _released ? this : _heap._nodes[_right];
            public Node Child => _child < 0 ? null : _heap._nodes[_child];
            public float Key { get; set; }
            public int Degree => _degree;
            public bool Marked { get; set; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Node CreateAfter(Node anchor, float key, T item)
            {
                var heap = anchor._heap;
                var node = heap.CreateNode(key, item);
                int right = anchor._right;
                node._left = anchor._index;
                node._right = right;
                heap._nodes[right]._left = node._index;
                anchor._right = node._index;
                return node;
            }

            // Move directly between rings, avoiding redundant self-links before insertion.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddChild(Node node, int degree)
            {
                node.Unlink();
                AddDetachedChild(node, degree);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddDetachedChild(Node node, int degree)
            {
                _degree = degree;
                node._parent = _index;
                if (degree == 1)
                {
                    node._left = node._right = node._index;
                    _child = node._index;
                }
                else
                    _heap._nodes[_child].InsertAfter(node);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void CutChild(Node node, Node root)
            {
                if (_child == node._index)
                    _child = _degree == 1 ? -1 : node._right;
                _degree--;
                node.Unlink();
                node._parent = -1;
                node.Marked = false;
                root.InsertBefore(node);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Node RemoveAllChildren()
            {
                int first = _child;
                if (first >= 0)
                {
                    _child = -1;
                    _degree = 0;
                }
                return first < 0 ? null : _heap._nodes[first];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void MakeRoot()
            {
                if (_parent >= 0)
                {
                    _parent = -1;
                    Marked = false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void InsertAfter(Node node)
            {
                node._left = _index;
                node._right = _right;
                _heap._nodes[_right]._left = node._index;
                _right = node._index;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void InsertBefore(Node node)
            {
                node._right = _index;
                node._left = _left;
                _heap._nodes[_left]._right = node._index;
                _left = node._index;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void LinkNext(Node node)
            {
                _right = node._index;
                node._left = _index;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void ReplaceWithRing(Node first)
            {
                int last = first._left;
                first._left = _left;
                _heap._nodes[_left]._right = first._index;
                _heap._nodes[last]._right = _right;
                _heap._nodes[_right]._left = last;
                _left = _right = _index;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Unlink()
            {
                _heap._nodes[_left]._right = _right;
                _heap._nodes[_right]._left = _left;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Isolate()
            {
                Unlink();
                _left = _right = _index;
            }

            public void Release()
            {
                _parent = _child = -1;
                _left = _right = _index;
                _degree = 0;
                Marked = false;
                _released = true;
                _heap = null;
            }
        }

        private Node[] _nodes = Array.Empty<Node>();
        private int[] _freeIndices = Array.Empty<int>();
        private int _nextIndex;
        private int _freeCount;
        private Node _min;
        private Node[] _degreeTable = Array.Empty<Node>();
        private bool _allRootsAreLeaves = true;
        private bool _mayHaveMarkedNodes;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node Insert(float key, T item)
        {
            Node node;
            var min = _min;
            if (min == null)
            {
                node = CreateNode(key, item);
                _min = node;
            }
            else
            {
                node = Node.CreateAfter(min, key, item);
                if (key < min.Key)
                    _min = node;
            }
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Node CreateNode(float key, T item)
        {
            int index;
            if (_freeCount > 0)
                index = _freeIndices[--_freeCount];
            else
            {
                index = _nextIndex++;
                if (index == _nodes.Length)
                    Array.Resize(ref _nodes, Math.Max(8, index * 2));
            }

            var node = new Node(this, index, key, item);
            _nodes[index] = node;
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReleaseNode(Node node)
        {
            int index = node._index;
            _nodes[index] = null;
            if (_freeCount == _freeIndices.Length)
                Array.Resize(ref _freeIndices, Math.Max(8, _freeCount * 2));
            _freeIndices[_freeCount++] = index;
            node.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T DeleteMin()
        {
            var min = _min;
            var children = min.RemoveAllChildren();
            if (_mayHaveMarkedNodes && children != null)
                ClearMarks(children);
            Node next;
            if (min.Right == min)
                next = children;
            else if (children != null)
            {
                next = children;
                min.ReplaceWithRing(children);
            }
            else
            {
                next = min.Right;
                min.Isolate();
            }
            if (next == null)
            {
                _min = null;
                _allRootsAreLeaves = true;
                _mayHaveMarkedNodes = false;
            }
            else if (next.Right == next)
            {
                next.MakeRoot();
                _min = next;
            }
            else
                _min = Consolidate(next);
            var value = min.Value;
            ReleaseNode(min);
            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ClearMarks(Node first)
        {
            var node = first;
            do
            {
                node.Marked = false;
                node = node.Right;
            } while (node != first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DecreaseKey(Node node, float newKey)
        {
            node.Key = newKey;
            var parent = node.Parent;
            if (parent != null && newKey < parent.Key)
            {
                parent.CutChild(node, _min);
                while (parent.Parent != null)
                {
                    if (!parent.Marked)
                    {
                        // A non-root may lose one child; losing another cuts it as well.
                        parent.Marked = true;
                        _mayHaveMarkedNodes = true;
                        break;
                    }
                    var ancestor = parent.Parent;
                    ancestor.CutChild(parent, _min);
                    parent = ancestor;
                }
            }
            // A decreased child can become the global minimum just like a decreased root.
            if (newKey < _min.Key)
                _min = node;
        }

        public bool IsEmpty() => _min == null;

        private Node Consolidate(Node current)
        {
            var last = current.Left;
            // A two-root frontier can be consolidated without scratch storage.
            if (current.Right == last)
                return ConsolidateTwo(current, last);

            if (_allRootsAreLeaves)
            {
                _allRootsAreLeaves = false;
                return ConsolidateInitial(current, last);
            }

            var table = _degreeTable;
            if (table.Length == 0)
                table = GrowDegreeTable(0);

            Node root;
            bool isLast;
            do
            {
                // Only previously processed roots enter the table. Until the final root,
                // neither last nor next can be linked away. Test the boundary before linking.
                isLast = current == last;
                var next = current.Right;
                root = current;
                int degree = root.Degree;
                while (table[degree] != null)
                {
                    var other = table[degree];
                    table[degree] = null;
                    float otherKey = other.Key;
                    if (otherKey < root.Key)
                    {
                        var swap = root;
                        root = other;
                        other = swap;
                    }
                    degree++;
                    root.AddChild(other, degree);
                }

                // Grow from actual degrees, not a rounded logarithmic estimate of the node count.
                if (degree >= table.Length - 1)
                    table = GrowDegreeTable(degree);
                table[degree] = root;
                current = next;
            } while (!isLast);

            // Inspect every surviving root, including those never linked, without
            // retaining a minimum that may have become a child.
            var minimum = root;
            float minimumKey = root.Key;
            root.MakeRoot();
            table[root.Degree] = null;
            for (var node = root.Right; node != root; node = node.Right)
            {
                node.MakeRoot();
                table[node.Degree] = null;
                if (node.Key < minimumKey)
                {
                    minimum = node;
                    minimumKey = node.Key;
                }
            }
            return minimum;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private Node ConsolidateTwo(Node current, Node last)
        {
            _allRootsAreLeaves = false;
            if (last.Key < current.Key)
            {
                var swap = current;
                current = last;
                last = swap;
            }
            if (current.Degree == last.Degree)
            {
                int degree = current.Degree + 1;
                if (degree >= _degreeTable.Length - 1)
                    GrowDegreeTable(degree);
                current.AddChild(last, degree);
                current.MakeRoot();
            }
            else
            {
                current.MakeRoot();
                last.MakeRoot();
            }
            return current;
        }

        private Node ConsolidateInitial(Node current, Node last)
        {
            var table = _degreeTable;
            if (table.Length == 0)
                table = GrowDegreeTable(0);

            Node root;
            bool isLast;
            do
            {
                isLast = current == last;
                var next = current.Right;
                root = current;
                int degree = 0;
                while (table[degree] != null)
                {
                    var other = table[degree];
                    table[degree] = null;
                    float otherKey = other.Key;
                    if (otherKey < root.Key)
                    {
                        var swap = root;
                        root = other;
                        other = swap;
                    }
                    degree++;
                    root.AddDetachedChild(other, degree);
                }

                if (degree >= table.Length - 1)
                    table = GrowDegreeTable(degree);
                table[degree] = root;
                current = next;
            } while (!isLast);

            Node first = null;
            Node previous = null;
            Node minimum = null;
            float minimumKey = float.PositiveInfinity;
            for (int degree = table.Length - 1; degree >= 0; degree--)
            {
                root = table[degree];
                if (root == null)
                    continue;
                table[degree] = null;
                if (first == null)
                    first = root;
                else
                    previous.LinkNext(root);
                previous = root;
                if (root.Key < minimumKey)
                {
                    minimum = root;
                    minimumKey = root.Key;
                }
            }
            previous.LinkNext(first);
            return minimum;
        }

        private Node[] GrowDegreeTable(int degree)
        {
            Array.Resize(ref _degreeTable, Math.Max(degree + 2, Math.Max(8, _degreeTable.Length * 2)));
            return _degreeTable;
        }
    }
}
