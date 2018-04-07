
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

    public class ShortestPath<T> : IShortestPath<T>
    {
        private List<T> m_nodes;
        private List<int>[] m_neighbors;
        private Func<T, T, float> m_getCostFunc;
        private bool[] m_expanded;
        private int[] m_pointers;
        private bool m_cancelTask = false;
        private int m_seedId;
        private Task m_task = null;
        private CancellationTokenSource m_cancellationToken;

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
            m_expanded = new bool[nodes.Count];
        }
        public ShortestPath(T[] nodes, List<int>[] neighbors, Func<T, T, float> getCostFunc)
        {
            m_nodes = nodes.ToList();
            m_neighbors = neighbors;
            m_getCostFunc = getCostFunc;
            m_expanded = new bool[nodes.Length];
        }

        public void Cancel()
        {
            if (m_task != null && !(m_task.IsCanceled || m_task.IsCompleted))
            {
                m_cancelTask = true;
                if (m_cancellationToken != null) m_cancellationToken.Cancel();
                m_task.Wait();
            }
            m_task = null;
            m_cancelTask = false;
        }

        public void CalculateShortestPaths(T seed)
        {
            //Cancel();
            if (m_cancellationToken != null) m_cancellationToken.Cancel();
            m_cancellationToken = new CancellationTokenSource();
            m_task = Task.Factory.StartNew(delegate { Calculate(seed); }, m_cancellationToken);
        }
        public void CalculateShortestPathsByIndex(int index)
        {
            //Cancel();
            if (m_cancellationToken != null) m_cancellationToken.Cancel();
            m_cancellationToken = new CancellationTokenSource();
            m_task = Task.Factory.StartNew(delegate { CalculateByIndex(index); }, m_cancellationToken);
        }

        private void Calculate(T seed)
        {
            CalculateByIndex(m_nodes.IndexOf(seed));
        }
        
        private void CalculateByIndex(int index)
        {
            Report.BeginTimed("Shortest paths calculation");
            m_seedId = index;

            var activePixels = new FibonacciHeap<int>();
            var inActiveList = new Dictionary<int, FibonacciHeap<int>.Node>();
            inActiveList[m_seedId] = activePixels.Insert(0, m_seedId);

            var totalCost = new float[m_nodes.Count].Set(float.MaxValue);
            m_expanded = new bool[m_nodes.Count];
            m_pointers = new int[m_nodes.Count].Set(m_seedId);

            totalCost[m_seedId] = 0;

            while (!activePixels.IsEmpty())
            {
                if (m_cancelTask)
                {
                    Report.End("Canceled Dijkstra");
                    return;
                }
                var q = activePixels.DeleteMin();
                inActiveList.Remove(q);

                m_expanded[q] = true;
                var totalCostQ = totalCost[q];

                foreach (var r in m_neighbors[q])
                {
                    if (m_expanded[r]) continue;

                    var gtemp = totalCostQ + m_getCostFunc(m_nodes[q], m_nodes[r]);
                    var isActive = inActiveList.ContainsKey(r);

                    if (!isActive || gtemp < totalCost[r])
                    {
                        if (isActive)
                            activePixels.DecreaseKey(inActiveList[r], gtemp);
                        else
                            inActiveList[r] = activePixels.Insert(gtemp, r);

                        totalCost[r] = gtemp;
                        m_pointers[r] = q;
                    }
                }
            }
            Report.End();
        }

        public List<T> GetMinimalPathByIndex(int endIndex)
        {
            var contour = new List<T>();
            var id = endIndex;
            var end = m_nodes[id];
            var seed = m_nodes[m_seedId];

            if (!m_expanded[id])
                return new List<T>() { end, seed };

            while (id != m_seedId)
            {
                contour.Add(m_nodes[id]);
                id = m_pointers[id];
            }
            return contour;
        }
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
            private T _item;
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

            public T Value
            {
                get { return _item; }
            }

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
            public Node Parent
            {
                get { return _parent; }
            }
            public Node Child
            {
                get { return _child; }
            }
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
                var right = this.Right;
                this.Right = node;
                right.Left = node;
            }
            public void InsertGroupBefore(Node node)
            {
                var start = node;
                var end = start.Left;

                var right = this.Right;
                this.Right = start;
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
            public int Degree
            {
                get { return _degree; }
            }
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

            public bool HasNoSiblings()
            {
                return _left == this;
            }
            public bool HasMaxOneSibling()
            {
                return _left._left == this;
            }
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
        public T GetMin()
        {
            return _min.Value;
        }

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
        public bool IsEmpty()
        {
            return _min == null;
        }

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
