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
    /// extraction takes O(log n) amortized time. Keys must not be NaN and decreases must not increase a key.
    /// </summary>
    class FibonacciHeap<T>
    {
        public sealed class Node
        {
            private Node _parent;
            private Node _left;
            private Node _right;
            private Node _child;
            private int _degree;

            public Node(float key, T item)
            {
                Key = key;
                Value = item;
                _left = _right = this;
            }

            public T Value { get; }
            public Node Parent => _parent;
            public Node Left => _left;
            public Node Right => _right;
            public Node Child => _child;
            public float Key { get; set; }
            public int Degree => _degree;
            public bool Marked { get; set; }

            // Move directly between rings, avoiding redundant self-links before insertion.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddChild(Node node, int degree)
            {
                node.Unlink();
                _degree = degree;
                node._parent = this; // Both incoming roots are already unmarked.
                if (degree == 1)
                {
                    node._left = node._right = node;
                    _child = node;
                }
                else
                    _child.InsertAfter(node);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void CutChild(Node node, Node root)
            {
                if (_child == node)
                    _child = _degree == 1 ? null : node._right;
                _degree--;
                node.Unlink();
                node._parent = null;
                node.Marked = false;
                root.InsertBefore(node);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Node RemoveAllChildren()
            {
                var first = _child;
                if (first != null)
                {
                    var child = first;
                    do
                    {
                        child._parent = null;
                        child.Marked = false;
                        child = child._right;
                    } while (child != first);
                    _child = null;
                    _degree = 0;
                }
                return first;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void InsertAfter(Node node)
            {
                node._left = this;
                node._right = _right;
                _right._left = node;
                _right = node;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void InsertBefore(Node node)
            {
                node._right = this;
                node._left = _left;
                _left._right = node;
                _left = node;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void ReplaceWithRing(Node first)
            {
                var last = first._left;
                first._left = _left;
                _left._right = first;
                last._right = _right;
                _right._left = last;
                _left = _right = this;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Unlink()
            {
                _left._right = _right;
                _right._left = _left;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Isolate()
            {
                Unlink();
                _left = _right = this;
            }
        }

        private Node _min;
        private Node[] _degreeTable = Array.Empty<Node>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node Insert(float key, T item)
        {
            var node = new Node(key, item);
            if (_min == null)
                _min = node;
            else
            {
                _min.InsertAfter(node);
                if (key < _min.Key)
                    _min = node;
            }
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T DeleteMin()
        {
            var min = _min;
            var children = min.RemoveAllChildren();
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
            _min = next == null || next.Right == next ? next : Consolidate(next);
            return min.Value;
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
            {
                if (last.Key < current.Key)
                    Fun.Swap(ref current, ref last);
                if (current.Degree == last.Degree)
                    current.AddChild(last, current.Degree + 1);
                return current;
            }

            var table = _degreeTable;
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
                while (degree < table.Length && table[degree] != null)
                {
                    var other = table[degree];
                    table[degree] = null;
                    if (other.Key < root.Key)
                        Fun.Swap(ref root, ref other);
                    degree++;
                    root.AddChild(other, degree);
                }

                // Grow from actual degrees, not a rounded logarithmic estimate of the node count.
                if (degree >= table.Length)
                    table = GrowDegreeTable(degree);
                table[degree] = root;
                current = next;
            } while (!isLast);

            // The final winner is still a root. Inspect every surviving root, including
            // those never linked, without retaining a minimum that may have become a child.
            var minimum = root;
            float minimumKey = root.Key;
            table[root.Degree] = null;
            for (var node = root.Right; node != root; node = node.Right)
            {
                table[node.Degree] = null; // Consumed slots were cleared when linking; clear the survivors too.
                if (node.Key < minimumKey)
                {
                    minimum = node;
                    minimumKey = node.Key;
                }
            }
            return minimum;
        }

        private Node[] GrowDegreeTable(int degree)
        {
            Array.Resize(ref _degreeTable, Math.Max(degree + 1, Math.Max(8, _degreeTable.Length * 2)));
            return _degreeTable;
        }
    }
}
