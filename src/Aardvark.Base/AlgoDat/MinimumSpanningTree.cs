using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;

namespace Aardvark.Base
{
    public static class MinimumSpanningTree
    {
        /// <summary>
        /// Creates a minimum spanning tree from a set of weighted edges.
        /// The input are edges ((TVertex, TVertex), TWeight) of a graph.
        /// The output are a subset of these edges which form a minimum spanning tree.
        /// The type of the weight (TWeight) needs to be IComparable.
        /// </summary>
        public static IEnumerable<Tup<Pair<TVertex>, TWeight>> Create<TVertex, TWeight>(
            IEnumerable<Tup<Pair<TVertex>, TWeight>> edges
            )
            where TWeight : IComparable<TWeight>
        {
            Requires.NotNull(edges);

            Report.BeginTimed("create vertex set");
            var vertexSet = new HashSet<TVertex>(edges.SelectMany(e => e.E0));
            Report.End();
            if (vertexSet.Count < 2) yield break;

            //compare function
            Func<KeyValuePair<TWeight, TVertex>, KeyValuePair<TWeight, TVertex>, int> compare =
                (kvp0, kvp1) => kvp0.Key.CompareTo(kvp1.Key);

            // init per-vertex edge priority queues
            Report.BeginTimed("init per-vertex edge priority queues");
            //var v2es = new Dictionary<TVertex, PriorityQueue<TWeight, TVertex>>();
            //vertexSet.ForEach(v => v2es[v] = new PriorityQueue<TWeight, TVertex>());
            var v2es = new Dictionary<TVertex, List<KeyValuePair<TWeight, TVertex>>>();
            vertexSet.ForEach(v => v2es[v] = new List<KeyValuePair<TWeight, TVertex>>());
            foreach (var e in edges)
            {
                //v2es[e.E0.E0].Enqueue(e.E1, e.E0.E1);
                //v2es[e.E0.E1].Enqueue(e.E1, e.E0.E0);
                v2es[e.E0.E0].HeapEnqueue(compare, new KeyValuePair<TWeight, TVertex>(e.E1, e.E0.E1));
                v2es[e.E0.E1].HeapEnqueue(compare, new KeyValuePair<TWeight, TVertex>(e.E1, e.E0.E0));
            }
            Report.End();

            // mst
            var mst = new HashSet<TVertex>();
            Action<TVertex> move = v => { vertexSet.Remove(v); mst.Add(v); };

            // build minimum spanning tree using Prim's algorithm
            Report.BeginTimed("build mst");
            move(vertexSet.First());
            while (vertexSet.Count > 0)
            {
                var candidateQueues = mst
                    .Where(v => v2es.ContainsKey(v))
                    .Select(v => Tup.Create(v, v2es[v])).Where(q => !q.E1.IsEmptyOrNull())
                    ;
                foreach (var q in candidateQueues)
                {
                    //while (!q.E1.IsEmptyOrNull() && mst.Contains(q.E1.Peek()))
                    while (!q.E1.IsEmptyOrNull() && mst.Contains(q.E1[0].Value))
                        //q.E1.Dequeue();
                        q.E1.HeapDequeue(compare);
                    if (q.E1.IsEmptyOrNull()) v2es.Remove(q.E0);
                }
                var best = candidateQueues
                    //.Select(q => Tup.Create(q.E0, q.E1.PeekKeyAndValue()))
                    .Select(q => Tup.Create(q.E0, q.E1[0]))
                    .Min((a, b) => a.E1.Key.CompareTo(b.E1.Key) < 0)
                    ;
                //v2es[best.E0].Dequeue();
                v2es[best.E0].HeapDequeue(compare);
                move(best.E1.Value);
                yield return Tup.Create(Pair.Create(best.E0, best.E1.Value), best.E1.Key);
            }
            Report.End();
        }
    }

    public static class MinimumSpanningTreeTest
    {
        public static void Test()
        {
            var r = new Random();
            var es = from a in Enumerable.Range(1, 1000)
                     from b in Enumerable.Range(a + 1, 1000 - a - 1)
                     let w = r.NextDouble() + 1
                     select Tup.Create(Pair.Create(a, b), w)
                     ;
            Report.Line("number of edges: {0}", es.Count());

            var edges = new Tup<Pair<string>, int>[]
            {
                Tup.Create(Pair.Create("A", "B"), 7),
                Tup.Create(Pair.Create("A", "D"), 5),
                Tup.Create(Pair.Create("D", "B"), 9),
                Tup.Create(Pair.Create("B", "C"), 8),
                Tup.Create(Pair.Create("B", "E"), 7),
                Tup.Create(Pair.Create("C", "E"), 5),
                Tup.Create(Pair.Create("E", "D"), 15),
                Tup.Create(Pair.Create("F", "D"), 6),
                Tup.Create(Pair.Create("E", "F"), 8),
                Tup.Create(Pair.Create("F", "G"), 11),
                Tup.Create(Pair.Create("E", "G"), 9),
            };

            Report.BeginTimed("building mst");
            var mst = MinimumSpanningTree.Create(es).ToArray();
            Report.End();

            Report.Line("#edges in mst: {0}", mst.Length);
            //foreach (var e in mst) Console.WriteLine(e);
        }
    }
}
