using System;
using System.Collections.Generic;
using System.Linq;
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
                        m_result = new ResultSnapshot(run.SeedIndex, run.Expanded, run.Predecessors);
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
        /// </summary>
        public List<T> GetMinimalPathByIndex(int endIndex)
        {
            ResultSnapshot result;
            lock (m_runLock)
                result = m_result;

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

    class FibonacciHeap<T>
    {
        public class Node
        {
            private readonly T _item;
            private Node _parent;
            private Node _left;
            private Node _right;
            private Node _child;
            private float _key = 0;
            private int _degree = 0;
            private bool _marked = false;

            public Node(float key, T item)
            {
                _key = key;
                _item = item;
                _left = this;
                _right = this;
            }

            public T Value => _item;

            public Node Right
            {
                get { return _right; }
                set
                {
                    _right = value;
                    value._left = this;
                }
            }

            public Node Left
            {
                get { return _left; }
                set
                {
                    _left = value;
                    value._right = this;
                }
            }

            public Node Parent =>_parent; 

            public Node Child => _child; 

            public void AddChild(Node node)
            {
                _degree++;
                node._parent = this;

                if (_child == null)
                    _child = node;
                else
                    _child.InsertOneBefore(node);
            }

            public void RemoveChild(Node node)
            {
                if (_child != null)
                {
                    if (_degree == 1)
                    {
                        _child = null;
                        _degree = 0;
                        node._parent = null;
                    }
                    else
                    {
                        if (_child == node)
                            _child = _child.Left;
                        node.Isolate();
                        _degree--;
                        node._parent = null;
                    }
                }
            }

            public void RemoveAllChildren()
            {
                if (_child != null)
                {
                    foreach (var child in _child.AllSiblings)
                        child._parent = null;
                    _degree = 0;
                    _child = null;
                }
            }

            public void InsertOneBefore(Node node)
            {
                var right = Right;
                Right = node;
                right.Left = node;
            }

            public void InsertGroupBefore(Node node)
            {
                var start = node;
                var end = start.Left;

                var right = Right;
                Right = start;
                right.Left = end;
            }

            public void Isolate()
            {
                var left = Left;
                var right = Right;
                left.Right = right;
                Left = this;
            }

            public float Key
            {
                get { return _key; }
                set { _key = value; }
            }

            public int Degree => _degree;

            public bool Marked
            {
                get { return _marked; }
                set { _marked = value; }
            }

            public IEnumerable<Node> GetAllChildren()
            {
                if (_child == null)
                    return Enumerable.Empty<Node>();
                return _child.AllSiblings;
            }

            public IEnumerable<Node> AllSiblings
            {
                get
                {
                    var node = this;
                    do
                    {
                        yield return node;
                        node = node.Right;
                    } while (node != this);
                }
            }

            public bool HasNoSiblings() => _left == this;

            public bool HasMaxOneSibling() => _left._left == this;
        }

        private Node _min;
        private int _n = 0;

        public void Insert(Node node)
        {
            if (_min == null)
            {
                _min = node;
            }
            else
            {
                _min.InsertOneBefore(node);
                if (node.Key < _min.Key)
                    _min = node;
            }
            _n++;
        }

        public Node Insert(float key, T item)
        {
            var node = new Node(key, item);
            Insert(node);
            return node;
        }

        public T GetMin() => _min.Value;

        public T DeleteMin()
        {
            var min = _min;
            if (_min != null)
            {
                if (_min.HasNoSiblings() && _min.Degree == 0)
                {
                    _min = null;
                }
                else
                {
                    if (_min.Degree > 0)
                    {
                        var child = _min.Child;
                        _min.RemoveAllChildren();

                        if (!_min.HasNoSiblings())
                        {
                            _min.InsertGroupBefore(child);
                            _min.Isolate();
                        }
                        _min = child;
                    }
                    else
                    {
                        var left = _min.Left;
                        _min.Isolate();
                        _min = left;
                    }
                    Consolidate();
                }
                _n--;
            }
            return min.Value;
        }

        /*private static Node FindMinSibling(Node node)
        {
            var minNode = node;
            foreach (var n in node.AllSiblings)
            {
                if (n.Key < minNode.Key)
                {
                    minNode = n;
                }
            }
            return minNode;
        }*/

        public void Delete(Node node)
        {
            DecreaseKey(node, int.MinValue);
            DeleteMin();
        }

        public void DecreaseKey(Node node, float newKey)
        {
            node.Key = newKey;

            if (node.Parent == null)
            {
                if (newKey < _min.Key)
                    _min = node;
            }
            else if (node.Key < node.Parent.Key)
            {
                var parent = node.Parent;
                parent.RemoveChild(node);
                _min.InsertOneBefore(node);
                if (!parent.Marked)
                    parent.Marked = true;
                else
                {
                    while (parent.Parent != null && parent.Marked)
                    {
                        var pparent = parent.Parent;
                        pparent.RemoveChild(parent);
                        _min.InsertOneBefore(parent);
                        parent.Marked = false;
                        parent = pparent;
                    }
                }
            }
        }

        public bool IsEmpty() => _min == null;

        private void Consolidate()
        {
            if (_min == null)
                return;

            var lookup = new Node[2 * (int)(_n.Log() + 1)];
            var last = _min;
            var current = _min;
            var newMin = _min;
            bool stop = false;
            do
            {
                var next = current.Right;
                while (lookup[current.Degree] != null)
                {
                    var right = current.Right;
                    var first = current;
                    var second = lookup[current.Degree];

                    if (second == right && right.Right != first)
                        right = right.Right;

                    lookup[current.Degree] = null;

                    if (second.Key < first.Key)
                        Fun.Swap(ref first, ref second);

                    second.Isolate();
                    first.AddChild(second);

                    current = first;
                    if (newMin.Key > first.Key || second == newMin)
                        newMin = first;

                    if (second == last)
                        stop = true;
                }
                lookup[current.Degree] = current;
                current = next;
            } while (current != last && !stop);
            _min = newMin;
        }
    }
}
