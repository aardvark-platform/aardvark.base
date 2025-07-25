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

namespace Aardvark.Base;

public readonly struct SymmetricMatrix<T>(long size)
{
    private readonly T[] m_data = new T[size * (size + 1) / 2];
    private readonly long m_size = size;

    #region Constructors

    #endregion

    #region Properties

    private long GetIndex(long x, long y)
    {
        if (y > x) return x * (m_size + 1) - (x * (x + 1)) / 2 + y - x;
        else return y * (m_size + 1) - (y * (y + 1)) / 2 + x - y;
    }

    private V2l GetCoord(long index)
    {
        long r, c;
        r = (int)(0.5 * (-System.Math.Sqrt(-8 * index + 4 * m_size * m_size + 4 * m_size + 1) + 2 * m_size + 1));
        if (r == 0) c = index;
        else c = r + index % (r * (m_size + 1) - (r * (r + 1)) / 2);

        return new V2l(c, r);
    }

    public T this[long index]
    {
        get { return m_data[index]; }
        set { m_data[index] = value; }
    }

    public T this[long row, long col]
    {
        get { return m_data[GetIndex(col, row)]; }
        set { m_data[GetIndex(col, row)] = value; }
    }

    public T this[V2l vec]
    {
        get { return m_data[GetIndex(vec.X, vec.Y)]; }
        set { m_data[GetIndex(vec.X, vec.Y)] = value; }
    }

    public V2l Size => new(m_size, m_size);

    #endregion

    #region Methods

    public void ForeachCoord(Action<V2l> action)
    {
        V2l current = V2l.Zero;
        for (long i = 0; i < m_data.Length; i++)
        {
            action(current);
            current.X++;
            if (current.X >= m_size)
            {
                current.Y++;
                current.X = current.Y;
            }
        }
    }

    public void ForeachCoord(Action<long, long> action)
    {
        V2l current = V2l.Zero;
        for (long i = 0; i < m_data.Length; i++)
        {
            action(current.X, current.Y);
            current.X++;
            if (current.X >= m_size)
            {
                current.Y++;
                current.X = current.Y;
            }
        }
    }
    
    public void ForeachIndex(Action<long> action)
    {
        for (long i = 0; i < m_data.Length; i++) action(i);
    }

    public void ForeachXYIndex(Action<long, long, long> action)
    {
        var current = V2l.Zero;
        for (long i = 0; i < m_data.Length; i++)
        {
            action(current.X, current.Y, i);
            current.X++;
            if (current.X >= m_size)
            {
                current.Y++;
                current.X = current.Y;
            }
        }
    }
    
    public void ForeachY(int x, Action<long, V2l> action)
    {
        var index = GetIndex(x, 0);
        var vec = new V2l(x, 0);
        for (vec.Y = 0; vec.Y <= x; vec.Y++)
        {
            action(index, vec);
            index += (m_size - vec.Y - 1);
        }
    }

    public void ForeachX(int y, Action<long, V2l> action)
    {
        var index = GetIndex(y, y);
        var vec = new V2l(y, y);
        for (vec.X = y; vec.X < m_size; vec.X++)
        {
            action(index, vec);
            index++;
        }
    }


    public void SetByIndex(Func<long, T> setter)
    {
        for (long i = 0; i < m_data.Length; i++) m_data[i] = setter(i);
    }

    public void SetByCoord(Func<V2l, T> setter)
    {
        V2l current = V2l.Zero;
        for (long i = 0; i < m_data.Length; i++)
        {
            m_data[i] = setter(current);
            current.X++;
            if (current.X >= m_size)
            {
                current.Y++;
                current.X = current.Y;
            }
        }
    }

    public void SetByCoord(Func<long, long, T> setter)
    {
        V2l current = V2l.Zero;
        for (long i = 0; i < m_data.Length; i++)
        {
            m_data[i] = setter(current.X, current.Y);
            current.X++;
            if (current.X >= m_size)
            {
                current.Y++;
                current.X = current.Y;
            }
        }
    }

    #endregion
}

public abstract class AbstractGraph<TVertex, TCost> where TCost : struct, IComparable<TCost>
{
    protected List<TVertex> m_nodes;
    protected Tree m_minimumSpanningTree;

    #region Reference-Structures/Classes

    public readonly struct Edge : IComparable<Edge>
    {
        private readonly AbstractGraph<TVertex, TCost> m_graph;
        public readonly int Index0;
        public readonly int Index1;

        #region Constructors

        internal Edge(AbstractGraph<TVertex, TCost> graph, int i0, int i1)
        {
            if (i0 == i1) throw new ArgumentException("Degenerated Edges are not posible");
            m_graph = graph;

            if (i0 < i1) { Index0 = i0; Index1 = i1; }
            else { Index0 = i1; Index1 = i0; }
        }

        #endregion

        #region Properties

        public TVertex Node0 => m_graph.m_nodes[Index0];

        public TVertex Node1 => m_graph.m_nodes[Index1]; 

        public TCost Cost => m_graph.GetCost(Index0, Index1);

        #endregion

        #region Overrides

        public override int GetHashCode()
            => m_graph.GetHashCode() ^ ((Index0.GetHashCode() << 12) ^ Index1.GetHashCode());

        public override bool Equals(object obj)
        {
            if (obj is Edge e)
            {
                if (!e.m_graph.Equals(m_graph)) return false;
                else return Index0 == e.Index0 && Index1 == e.Index1;
            }
            else return false;
        }

        public override string ToString() => string.Format("[{0},{1}]", Index0, Index1);

        #endregion

        #region IComparable<Edge> Members

        public int CompareTo(Edge other) => Cost.CompareTo(other.Cost);
        public static bool operator ==(Edge left, Edge right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Edge left, Edge right)
        {
            return !(left == right);
        }

        public static bool operator <(Edge left, Edge right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Edge left, Edge right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Edge left, Edge right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Edge left, Edge right)
        {
            return left.CompareTo(right) >= 0;
        }

        #endregion
    }

    protected class UnionFind
    {
        private readonly List<TVertex> m_nodes;
        private readonly int[] m_parent;

        #region Constructors

        internal UnionFind(List<TVertex> nodes)
        {
            m_nodes = nodes;
            m_parent = new int[m_nodes.Count].SetByIndex(i => i);
        }

        #endregion

        #region Methods

        private int FindRoot(int n)
        {
            int p = m_parent[n];

            if (p != n)
            {
                p = FindRoot(p);
                m_parent[n] = p;
                return p;
            }
            else
            {
                return n;
            }
        }

        public bool Add(int n0, int n1)
        {
            int p0 = FindRoot(n0);
            int p1 = FindRoot(n1);

            if (p0 != p1)
            {
                m_parent[p0] = p1;
                return true;
            }
            else return false;
        }

        #endregion
    }

    public class Tree
    {
        private readonly AbstractGraph<TVertex, TCost> m_graph;

        private readonly List<Tup<int, int>> m_edges;

        private int m_edgeCount;
        private readonly bool[] m_visited;

        #region Constructors

        internal Tree(AbstractGraph<TVertex, TCost> graph)
        {
            m_graph = graph;
            m_edges = new List<Tup<int, int>>(m_graph.m_nodes.Count);

            for (int i = 0; i < m_graph.m_nodes.Count; i++)
            {
                m_edges.Add(new Tup<int, int>(-1, -1));
            }
            
            m_visited = new bool[m_graph.m_nodes.Count].SetByIndex(i => false);
            m_edgeCount = 0;
        }

        #endregion

        #region Edges

        private void AddHalfEdge(int l, int r)
        {
            int index = l;

            var tup = m_edges[index];
            if (tup.E0 < 0)
            {
                tup.E0 = r;
                m_edges[index] = tup;
            }
            else
            {
                while (tup.E1 >= 0)
                {
                    index = tup.E1;
                    tup = m_edges[index];
                }
                tup.E1 = m_edges.Count;
                m_edges[index] = tup;
                m_edges.Add(new Tup<int, int>(r, -1));
            }
        }

        internal void AddEdge(Edge edge)
        {
            AddHalfEdge(edge.Index0, edge.Index1);
            AddHalfEdge(edge.Index1, edge.Index0);
            m_edgeCount++;
        }

        #endregion

        #region Traversals

        private void TraverseAux(int n, Action<TVertex> nodeAction, Action<Edge> edgeAction)
        {
            if (m_visited[n]) return;
            m_visited[n] = true;

            nodeAction(m_graph.m_nodes[n]);

            var tup = m_edges[n];
            if (tup.E0 < 0) return;

            int edgeIndex = n;
            edgeAction(new Edge(m_graph, n, tup.E0));
            TraverseAux(tup.E0, nodeAction, edgeAction);
            
            while (tup.E1 >= 0)
            {
                edgeIndex = tup.E1;
                tup = m_edges[edgeIndex];

                edgeAction(new Edge(m_graph, n, tup.E0));
                TraverseAux(tup.E0, nodeAction, edgeAction);
            }
        }

        public void Traverse(Action<TVertex> nodeAction, Action<Edge> edgeAction)
        {
            m_visited.SetByIndex(i => false);
            TraverseAux(0, nodeAction, edgeAction);
        }

        private void TraverseEulerAux(int n, Action<int> action)
        {
            if (m_visited[n]) return;
            m_visited[n] = true;

            int nodeIndex = n;
            action(nodeIndex);//m_graph.m_nodes[n]);

            var tup = m_edges[n];
            if (tup.E0 < 0) return;

            TraverseEulerAux(tup.E0, action);
            action(nodeIndex);//m_graph.m_nodes[n]);

            while (tup.E1 >= 0)
            {
                n = tup.E1;
                tup = m_edges[n];

                TraverseEulerAux(tup.E0, action);
                action(nodeIndex);//m_graph.m_nodes[n]);
            }
        }

        public void TraverseEuler(Action<int> action)
        {
            m_visited.SetByIndex(i => false);
            TraverseEulerAux(0, action);
        }

        #endregion

        #region Properties

        public int VertexCount => m_graph.VertexCount;

        public int EdgeCount => m_edgeCount;

        public TCost Cost => GetCost<TCost>(GraphHelpers.Add<TCost>, default);

        public TAccumulate GetCost<TAccumulate>(Func<TAccumulate, TCost, TAccumulate> addition, TAccumulate seed)
        {
            var sum = seed;
            Traverse(node => { }, edge => sum = addition(sum, edge.Cost));

            return sum;
        }

        #endregion

    }

    public class Tour
    {
        private readonly AbstractGraph<TVertex, TCost> m_graph;
        private readonly int[] m_permutation;
        private bool m_2optImproved;
        private readonly bool m_circular;

        #region Constructors

        internal Tour(AbstractGraph<TVertex, TCost> graph, int[] permutation, bool circular = true)
        {
            m_circular = circular;
            m_graph = graph;
            m_permutation = permutation;
            m_2optImproved = !circular;
        }

        #endregion

        #region Methods

        public bool Improve(int iterations = 1)
        {
            if (iterations < 0) iterations = int.MaxValue;

            int steps = 0;
            if (!m_2optImproved)
            {
                do
                {
                    if (m_graph.Improve2Opt(m_permutation, GraphHelpers.Add<TCost>) == 0) m_2optImproved = true;
                    else steps++;
                }
                while (steps < iterations && !m_2optImproved);
            }

            return steps != 0;
        }

        public void Improve3Opt(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                m_graph.Improve3Opt(m_permutation, GraphHelpers.Add<TCost>);
            }
        }

        #endregion

        #region Properties

        public IEnumerable<TVertex> Vertices
        {
            get
            {
                foreach (var i in m_permutation) yield return m_graph.m_nodes[i];
            }
        }

        public IEnumerable<Edge> Edges
        {
            get
            {
                int count = m_permutation.Length;

                for (int i = 0; i < (m_circular ? count : count - 1); i++)
                {
                    yield return new Edge(m_graph, m_permutation[i], m_permutation[(i + 1) % count]);
                }
            }
        }

        public TAccumulate GetCost<TAccumulate>(Func<TAccumulate, TCost, TAccumulate> addition, TAccumulate seed)
        {
            var sum = seed;

            foreach (var e in Edges)
            {
                sum = addition(sum, e.Cost);
            }

            return sum;
        }

        public TCost Cost => GetCost<TCost>(GraphHelpers.Add, default);

        public int Count => m_permutation.Length;

        public int[] Permutation => m_permutation;

        public TVertex this[int index] => m_graph.m_nodes[m_permutation[index]];

        #endregion

        #region Overrides

        public override string ToString()
        {
            var e = Vertices.GetEnumerator();
            if (!e.MoveNext()) return "[]";

            string res = e.Current.ToString();
            while (e.MoveNext())
            {
                res += string.Format(", {0}", e.Current.ToString());
            }

            return string.Format("[{0}]", res);
        }

        #endregion
    }

    public readonly struct Vertex
    {
        private readonly AbstractGraph<TVertex, TCost> m_graph;
        private readonly int m_index;

        #region Constructors

        internal Vertex(AbstractGraph<TVertex, TCost> graph, int index)
        {
            m_graph = graph;
            m_index = index;
        }

        #endregion

        #region Properties

        public TVertex Value => m_graph.m_nodes[m_index]; 

        public int Index => m_index;

        #endregion

        #region Overrides

        public override string ToString() => string.Format("[{0}: {1}]", m_index, Value);

        public override int GetHashCode() => m_graph.GetHashCode() ^ m_index.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is Vertex v)
            {
                return v.m_graph.Equals(m_graph) && v.m_index == m_index;
            }
            else return false;
        }
        public static bool operator ==(Vertex left, Vertex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vertex left, Vertex right)
        {
            return !(left == right);
        }

        #endregion
    }

    #endregion

    #region Abstract Members

    public abstract int EdgeCount { get; }
    public abstract TCost GetCost(int n0, int n1);
    public abstract IEnumerable<Edge> Edges { get; }
    public abstract Tree MinimumSpanningTree { get; }

    #endregion

    #region Properties

    public int VertexCount => m_nodes.Count;

    #endregion

    public Vertex GetVertex(int index) => new(this, index);

    public IEnumerable<Vertex> GetVertices(TVertex value)
    {
        int index = 0;
        foreach (var v in m_nodes)
        {
            if (v.Equals(value)) yield return new Vertex(this, index);
            index++;
        }
    }

    #region TSP-Solver

    protected int Improve2Opt(int[] tour, Func<TCost, TCost, TCost> add)
    {
        int steps = 0;
        int count = tour.Length;
        for (int i = 0; i < count; i++)
        {
            for (int j = i + 2; j < count; j++)
            {
                int l0 = i % count;
                int l1 = (i + 1) % count;
                int r0 = j % count;
                int r1 = (j + 1) % count;

                var wOld = add(GetCost(tour[l0], tour[l1]), GetCost(tour[r0], tour[r1]));
                var wNew = add(GetCost(tour[l0], tour[r0]), GetCost(tour[r1], tour[l1]));

                if (wNew.CompareTo(wOld) < 0)
                {
                    int temp;
                    //reverse l1 to r0
                    while (r0 > l1)
                    {
                        temp = tour[r0];
                        tour[r0] = tour[l1];
                        tour[l1] = temp;

                        l1++;
                        r0--;
                    }

                    steps++;
                }

            }
        }

        return steps;
    }

    protected int Improve3Opt(int[] tour, Func<TCost, TCost, TCost> add)
    {
        int improvements = 0;
        int count = tour.Length;

        for (int i = 0; i < count; i++)
        {
            int i0 = tour[i];
            int i1 = tour[(i + 1) % count];

            for (int j = i + 2; j < count; j++)
            {
                int i2 = tour[j];
                int i3 = tour[(j + 1) % count];

                for (int k = j + 2; k < count; k++)
                {
                    int i4 = tour[k];
                    int i5 = tour[(k + 1) % count];

                    //Initial:
                    //01 23 45

                    //New:
                    //0: 02 13 45      (already tested with 2-opt)
                    //1: 02 14 35      Reverse: [2:1], [4:3]
                    //2: 03 41 25      Reverse: nothing
                    //3: 03 42 15      Reverse: [2:1]
                    //4: 04 31 25      Reverse: [4:3]
                    //5: 04 32 15      Reverse: [4:3], [2:1]
                    int alternativeIndex = -1;
                    var min = add(add(GetCost(i0, i1), GetCost(i2, i3)), GetCost(i4, i5));

                    //02 14 35      Reverse: [2:1], [4:3]
                    var alternativeCost = add(add(GetCost(i0, i2), GetCost(i1, i4)), GetCost(i3, i5));
                    if (alternativeCost.CompareTo(min) < 0)
                    {
                        min = alternativeCost;
                        alternativeIndex = 1;
                    }

                    //03 41 25      Reverse: nothing
                    alternativeCost = add(add(GetCost(i0, i3), GetCost(i4, i1)), GetCost(i2, i5));
                    if (alternativeCost.CompareTo(min) < 0)
                    {
                        min = alternativeCost;
                        alternativeIndex = 2;
                    }

                    //03 42 15      Reverse: [2:1]
                    alternativeCost = add(add(GetCost(i0, i3), GetCost(i4, i2)), GetCost(i1, i5));
                    if (alternativeCost.CompareTo(min) < 0)
                    {
                        min = alternativeCost;
                        alternativeIndex = 3;
                    }

                    //04 31 25      Reverse: [4:3]
                    alternativeCost = add(add(GetCost(i0, i4), GetCost(i3, i1)), GetCost(i2, i5));
                    if (alternativeCost.CompareTo(min) < 0)
                    {
                        min = alternativeCost;
                        alternativeIndex = 4;
                    }

                    //04 32 15      Reverse: [4:3], [2:1]
                    alternativeCost = add(add(GetCost(i0, i4), GetCost(i3, i2)), GetCost(i1, i5));
                    if (alternativeCost.CompareTo(min) < 0)
                    {
                        /*min = alternativeCost;*/
                        alternativeIndex = 5;
                    }

                    if (alternativeIndex > 0)
                    {
                        if (alternativeIndex == 1)
                        {
                            //1: 02 14 35      Reverse: [2:1], [4:3]

                        }
                    }
                }
            }
        }

        return improvements;
    }

    #endregion

    #region Shortest Path

    private static TR NearestNode<TI, TR>(HashSet<int> nodes, Func<int, TI> comparable, Func<int, TR> result) where TI : IComparable<TI>
    {
        var e = nodes.GetEnumerator();
        if (!e.MoveNext()) throw new IndexOutOfRangeException();
        var min = e.Current;
        var comparableMin = comparable(min);

        while (e.MoveNext())
        {
            var comparableTemp = comparable(e.Current);
            if (comparableTemp.CompareTo(comparableMin) < 0)
            {
                min = e.Current;
                comparableMin = comparableTemp;
            }
        }

        return result(min);
    }

    public Tour ShortestPath(int start, int end)
    {
        var parent = new int[m_nodes.Count].Set(-1);
        var nodes = new HashSet<int>();
        var distances = new TCost[m_nodes.Count];

        for (int i = 0; i < m_nodes.Count; i++)
        {
            if (i != start)
            {
                distances[i] = GetCost(start, i);
                parent[i] = start;
            }
            else distances[i] = default;
        }
        
        for (int i = 0; i < m_nodes.Count; i++)
        {
            nodes.Add(i);
        }

        while (nodes.Count > 0)
        {
            int u = NearestNode(nodes, n => distances[n], n => n);
            nodes.Remove(u);

            foreach (var v in nodes)
            {
                //u,v
                var alternative = GraphHelpers.Add(distances[u], GetCost(u, v));
                if (alternative.CompareTo(distances[v]) < 0)
                {
                    distances[v] = alternative;
                    parent[v] = u;
                }
            }
        }

        var path = new List<int> { end };
        var current = end;

        while (parent[current] >= 0)
        {
            current = parent[current];
            path.Insert(0, current);
        }

        return new Tour(this, [.. path], false);
    }

    #endregion
}

public class DenseGraph<TVertex, TCost> : AbstractGraph<TVertex, TCost> where TCost : struct, IComparable<TCost>
{
    private SymmetricMatrix<TCost> m_costs;

    #region Constructors

    public DenseGraph(IEnumerable<TVertex> nodes, Func<TVertex, TVertex, TCost> weightFun)
    {
        m_nodes = [.. nodes];
        BuildWeights(weightFun);
    }

    public DenseGraph(IEnumerable<TVertex> nodes, Func<int, TVertex, int, TVertex, TCost> weightFun)
    {
        m_nodes = [.. nodes];
        BuildWeights(weightFun);
    }

    #endregion

    #region Methods

    public void BuildWeights(Func<TVertex, TVertex, TCost> weightFun)
    {
        m_costs = new SymmetricMatrix<TCost>(m_nodes.Count);

        m_costs.SetByCoord((x, y) =>
        {
            return weightFun(m_nodes[(int)x], m_nodes[(int)y]);
        });

        m_minimumSpanningTree = null;
    }

    public void BuildWeights(Func<int, TVertex, int, TVertex, TCost> weightFun)
    {
        m_costs = new SymmetricMatrix<TCost>(m_nodes.Count);

        m_costs.SetByCoord((x, y) =>
        {
            return weightFun((int)x, m_nodes[(int)x], (int)y, m_nodes[(int)y]);
        });

        m_minimumSpanningTree = null;
    }

    #endregion

    #region Properties

    public override IEnumerable<Edge> Edges
    {
        get
        {
            for (int x = 1; x < m_nodes.Count; x++)
            {
                for (int y = 0; y < x; y++)
                {
                    yield return new Edge(this, y, x);
                }
            }
        }
    }

    public override int EdgeCount => m_nodes.Count * (m_nodes.Count - 1) / 2;

    /// <summary>
    /// Returns the minimum-spanning-tree for the Graph
    /// If no Tree has been built before builds it using Prim's algorithm (which is faster than Kruskal's algorithm on dense Graphs)
    /// </summary>
    public override Tree MinimumSpanningTree
    {
        get
        {
            m_minimumSpanningTree ??= BuildMinimumSpanningTreePrim();
            return m_minimumSpanningTree;
        }
    }

    #endregion

    #region Minimum-Spanning-Tree

    /// <summary>
    /// Builds the minimum-spanning tree using Kruskal's algorithm in O(|E|*log|E|)
    /// where |E| is the number of Edges 
    /// and |V| the number of vertices (nodes) in the Graph
    /// </summary>
    /// <returns></returns>
    private Tree BuildMinimumSpanningTreeKruskal()
    {
        int N = m_nodes.Count;
        var unionFind = new UnionFind(m_nodes); //O(|V|)

        static int cmp(Edge a, Edge b) => a.CompareTo(b);
        var heap = new List<Edge>(EdgeCount);
        foreach (var e in Edges) heap.HeapEnqueue(cmp, e);

        var tree = new Tree(this);

        long i = 0;
        //O(|E|*log|E|)
        while (tree.EdgeCount < N - 1)
        {
            var edge = heap.HeapDequeue(cmp);
            i++;

            //O(1) amortized
            if (unionFind.Add(edge.Index0, edge.Index1))
            {
                //O(1)
                tree.AddEdge(edge);
            }
        }

        //O(|V| + |E|*log|E|)
        return tree;
    }
    
    /// <summary>
    /// Builds the minimum-spanning-tree using Prim's algorithm in O(|V|^2)
    /// where |V| is the number of vertices (nodes) in the Graph
    /// </summary>
    /// <returns></returns>
    public Tree BuildMinimumSpanningTreePrim()
    {
        var node = 0;
        var visited = new bool[m_nodes.Count].Set(false);
        var tree = new Tree(this);

        while (tree.EdgeCount < m_nodes.Count - 1)
        {
            visited[node] = true;
            int next = FindClosestUnvisited(node, visited);

            tree.AddEdge(new Edge(this, node, next));
            node = next;
        }

        return tree;
    }

    private int FindClosestUnvisited(int node, bool[] visited)
    {
        int N = m_nodes.Count;

        long nearest = -1;
        TCost min = default;
        m_costs.ForeachX(node, (i, p) =>
        {
            if (node != p.X && !visited[p.X])
            {
                var w = m_costs[i];
                if (nearest < 0 || w.CompareTo(min) < 0) { min = w; nearest = p.X; }
            }
        });

        m_costs.ForeachY(node, (i, p) =>
        {
            if (node != p.Y && !visited[p.Y])
            {
                var w = m_costs[i];
                if (nearest < 0 || w.CompareTo(min) < 0) { min = w; nearest = p.Y; }
            }
        });

        return (int)nearest;
    }

    #endregion

    #region TSP-Solver

    public Tour SolveTSP()
    {
        int index = 0;
        var perm = new int[m_nodes.Count];
        var visited = new bool[m_nodes.Count].SetByIndex(i => false);

        MinimumSpanningTree.TraverseEuler(ni =>
        {
            if (!visited[ni])
            {
                visited[ni] = true;
                perm[index] = ni;
                index++;
            }
        });

        return new Tour(this, perm);
    }

    public override TCost GetCost(int n0, int n1)
    {
        if (n0 < n1) return m_costs[n0, n1];
        else return m_costs[n1, n0];
    }

    #endregion
}

internal static class GraphHelpers
{
    private static readonly Dictionary<Type, Func<object, object, object>> m_additions = new()
    {
        {typeof(int), (a,b) => (int)a + (int)b},
        {typeof(uint), (a,b) => (uint)a + (uint)b},
        {typeof(long), (a,b) => (long)a + (long)b},
        {typeof(float), (a,b) => (float)a + (float)b},
        {typeof(double), (a,b) => (double)a + (double)b},
        {typeof(byte), (a,b) => (byte)a + (byte)b},
        {typeof(short), (a,b) => (short)a + (short)b},
        {typeof(ushort), (a,b) => (ushort)a + (ushort)b},
    };
    
    public static TWeight Add<TWeight>(TWeight v0, TWeight v1)
    {
        return (TWeight)m_additions[typeof(TWeight)](v0, v1);
    }
}
