using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aardvark.Base
{

    public delegate SymMapBase SymMapBaseVisitor(SymMapBase map, SymMapBaseTraversal.Visit visit);

    public class SymMapBaseTraversal
    {
        public enum Mode
        {
            NonModifying = 0x0000,
            Modifying = 0x0001,
        }

        [Flags]
        public enum Visit
        {
            Pre = 0x0001,
            Post = 0x0002,
            PreAndPost = Pre | Post,
        }

        private SymbolDict<SymMapBaseVisitor> m_perTypenameVisitors =
            new SymbolDict<SymMapBaseVisitor>();
        private Mode m_mode;
        private Visit m_visit;

        /// <summary>
        /// This event fires once for each node, .
        /// </summary>
        public event SymMapBaseVisitor OnDefaultVisit;

        #region Constructors

        public SymMapBaseTraversal()
        {
            m_mode = Mode.Modifying;
            m_visit = Visit.Pre;
        }

        public SymMapBaseTraversal(Mode mode)
        {
            m_mode = mode;
            m_visit = Visit.Pre;
        }

        public SymMapBaseTraversal(Visit visit)
        {
            m_mode = Mode.NonModifying;
            m_visit = visit;
        }

        public SymMapBaseTraversal(Mode mode, Visit visit)
        {
            m_mode = mode;
            m_visit = visit;
        }

        #endregion

        #region Properties

        public Mode ModifyMode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public Visit VisitMode
        {
            get { return m_visit; }
            set { m_visit = value; }
        }

        public SymbolDict<SymMapBaseVisitor> PerNameVisitors
        {
            get { return m_perTypenameVisitors; }
        }

        #endregion

        public SymMapBase Traverse(SymMapBase map)
        {
            // choose visitor
            SymMapBaseVisitor visitor = OnDefaultVisit;
            if (map.TypeName != null && m_perTypenameVisitors.ContainsKey(map.TypeName))
            {
                visitor = m_perTypenameVisitors[map.TypeName];
            }

            // pre visit
            if ((m_visit & Visit.Pre) != 0)
            {
                if (visitor != null)
                {
                    if (m_mode == Mode.Modifying)
                    {
                        map = visitor(map, Visit.Pre);
                    }
                    else
                    {
                        SymMapBase tmp = visitor(map, Visit.Pre);
                        if (tmp != map)
                        {
                            throw new ArgumentException(
                                "The node returned by a non-modifying pre-visit is " +
                                "different from the node that has been visited. " +
                                "This makes no sense!"
                                );
                        }
                    }
                }
            }

            // traverse children
            if (m_mode == Mode.Modifying)
            {
                SymbolDict<object> tmp = new SymbolDict<object>();
                foreach (var e in map.MapItems)
                {
                    tmp[e.Key] = e.Value;
                }

                foreach (var e in tmp)
                {
                    object o = e.Value;
                    if (o is SymMapBase)
                        map[e.Key] = Traverse((SymMapBase)o);
                    else if (o is SymMapBase[])
                    {
                        SymMapBase[] array = (SymMapBase[])o;
                        for (int i = 0; i < array.Length; i++)
                            array[i] = Traverse(array[i]);

                    }
                    else if (o is List<SymMapBase>)
                    {
                        List<SymMapBase> list = (List<SymMapBase>)o;
                        for (int i = 0; i < list.Count; i++)
                            list[i] = Traverse(list[i]);
                    }
                    else if (o is Dict<string, SymMapBase>)
                    {
                        var dict = new Dict<string, SymMapBase>();
                        foreach (var kvp in (Dict<string, SymMapBase>)o)
                        {
                            dict[kvp.Key] = Traverse(kvp.Value);
                        }
                        map[e.Key] = dict;
                    }
                    else if (o is SymbolDict<SymMapBase>)
                    {
                        var dict = new SymbolDict<SymMapBase>();
                        foreach (var kvp in (SymbolDict<SymMapBase>)o)
                        {
                            dict[kvp.Key] = Traverse(kvp.Value);
                        }
                        map[e.Key] = dict;
                    }
                    else if (o is DictSet<SymMapBase>)
                    {
                        var set = new DictSet<SymMapBase>();
                        foreach (var m in (DictSet<SymMapBase>)o)
                            set.Add(Traverse(m));
                        map[e.Key] = set;
                    }
                }
            }
            else
            {
                foreach (var e in map.MapItems)
                {
                    object o = e.Value;
                    if (o is SymMapBase)
                    {
                        Traverse((SymMapBase)o);
                    }
                    else if (o is SymMapBase[])
                    {
                        foreach (var m in (SymMapBase[])o)
                            Traverse(m);
                    }
                    else if (o is List<SymMapBase>)
                    {
                        foreach (var m in (List<SymMapBase>)o)
                            Traverse(m);
                    }
                    else if (o is Dict<string, SymMapBase>)
                    {
                        foreach (var m in ((Dict<string, SymMapBase>)o).Values)
                            Traverse(m);
                    }
                    else if (o is SymbolDict<SymMapBase>)
                    {
                        foreach (var m in ((SymbolDict<SymMapBase>)o).Values)
                            Traverse(m);
                    }
                    else if (o is DictSet<SymMapBase>)
                    {
                        foreach (var m in (DictSet<SymMapBase>)o)
                            Traverse(m);
                    }
                }
            }

            // post visit
            if ((m_visit & Visit.Post) != 0)
            {
                if (visitor != null)
                {
                    if (m_mode == Mode.Modifying)
                    {
                        map = visitor(map, Visit.Post);
                    }
                    else
                    {
                        var tmp = visitor(map, Visit.Post);
                        if (tmp != map)
                        {
                            throw new ArgumentException(
                                "The node returned by a non-modifying post-visit is " +
                                "different from the node that has been visited. " +
                                "This makes no sense!"
                                );
                        }
                    }
                }
            }

            return map;
        }
    }

    public class SymMapBaseCollectionTraversal
    {

        public static List<SymMapBase> Collect(SymMapBase root, string typenameToCollect)
        {
            return new SymMapBaseCollectionTraversal(root).Collect(typenameToCollect);
        }

        public SymMapBaseCollectionTraversal(SymMapBase root)
        {
            m_root = root;
        }

        public SymMapBaseCollectionTraversal(SymMapBase root, TextWriter debugOutput)
        {
            m_root = root;
            m_out = debugOutput;
        }

        private TextWriter m_out = null;

        public List<SymMapBase> Collect(string typenameToCollect)
        {
            if (m_out != null)
            {
                m_out.WriteLine("SymMapBaseCollectionTraversal START");
            }

            m_visited = new Dictionary<SymMapBase, int>();
            m_name = typenameToCollect;
            m_result = new List<SymMapBase>();
            SymMapBaseTraversal trav = new SymMapBaseTraversal(
                SymMapBaseTraversal.Mode.NonModifying,
                SymMapBaseTraversal.Visit.Pre
                );
            trav.OnDefaultVisit += new SymMapBaseVisitor(Visit);
            trav.Traverse(m_root);

            if (m_out != null)
            {
                m_out.WriteLine("SymMapBaseCollectionTraversal END");
            }

            return m_result;
        }

        private SymMapBase Visit(SymMapBase m, SymMapBaseTraversal.Visit visit)
        {
            if (m_out != null)
            {
                m_out.WriteLine("visiting {0}", m.TypeName);
            }

            if (m_visited.ContainsKey(m))
            {
                //throw new InvalidOperationException(m.TypeName);
                m_visited[m] += 1;
                //Console.WriteLine(
                //    "cycle detected: map {0}, {1} visits", m.TypeName, m_visited[m]
                //    );
            }
            else
            {
                m_visited[m] = 1;
            }

            if (m.TypeName == m_name)
            {
                m_result.Add(m);
            }
            return m;
        }

        private SymMapBase m_root;
        private string m_name;
        private List<SymMapBase> m_result;
        private Dictionary<SymMapBase, int> m_visited;

    }
}
