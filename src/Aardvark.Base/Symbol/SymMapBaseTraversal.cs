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
using System.IO;

namespace Aardvark.Base;

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

    /// <summary>
    /// This event fires once for each node, .
    /// </summary>
    public event SymMapBaseVisitor OnDefaultVisit;

    #region Constructors

    public SymMapBaseTraversal()
    {
        ModifyMode = Mode.Modifying;
        VisitMode = Visit.Pre;
    }

    public SymMapBaseTraversal(Mode mode)
    {
        ModifyMode = mode;
        VisitMode = Visit.Pre;
    }

    public SymMapBaseTraversal(Visit visit)
    {
        ModifyMode = Mode.NonModifying;
        VisitMode = visit;
    }

    public SymMapBaseTraversal(Mode mode, Visit visit)
    {
        ModifyMode = mode;
        VisitMode = visit;
    }

    #endregion

    #region Properties

    public Mode ModifyMode { get; set; }

    public Visit VisitMode { get; set; }

    public SymbolDict<SymMapBaseVisitor> PerNameVisitors { get; } = [];

    #endregion

    public SymMapBase Traverse(SymMapBase map)
    {
        // choose visitor
        SymMapBaseVisitor visitor = OnDefaultVisit;
        if (map.TypeName != null && PerNameVisitors.ContainsKey(map.TypeName))
        {
            visitor = PerNameVisitors[map.TypeName];
        }

        // pre visit
        if ((VisitMode & Visit.Pre) != 0)
        {
            if (visitor != null)
            {
                if (ModifyMode == Mode.Modifying)
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
        if (ModifyMode == Mode.Modifying)
        {
            SymbolDict<object> tmp = [];
            foreach (var e in map.MapItems)
            {
                tmp[e.Key] = e.Value;
            }

            foreach (var e in tmp)
            {
                object o = e.Value;
                if (o is SymMapBase oBase)
                    map[e.Key] = Traverse(oBase);
                else if (o is SymMapBase[] array)
                {
                    for (int i = 0; i < array.Length; i++)
                        array[i] = Traverse(array[i]);

                }
                else if (o is List<SymMapBase> oList)
                {
                    for (int i = 0; i < oList.Count; i++)
                        oList[i] = Traverse(oList[i]);
                }
                else if (o is Dict<string, SymMapBase> oDict)
                {
                    var dict = new Dict<string, SymMapBase>();
                    foreach (var kvp in oDict)
                    {
                        dict[kvp.Key] = Traverse(kvp.Value);
                    }
                    map[e.Key] = dict;
                }
                else if (o is SymbolDict<SymMapBase> oSymbolDict)
                {
                    var dict = new SymbolDict<SymMapBase>();
                    foreach (var kvp in oSymbolDict)
                    {
                        dict[kvp.Key] = Traverse(kvp.Value);
                    }
                    map[e.Key] = dict;
                }
                else if (o is DictSet<SymMapBase> oDictSet)
                {
                    var set = new DictSet<SymMapBase>();
                    foreach (var m in oDictSet)
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
                if (o is SymMapBase oBase)
                {
                    Traverse(oBase);
                }
                else if (o is SymMapBase[] oBases)
                {
                    foreach (var m in oBases)
                        Traverse(m);
                }
                else if (o is List<SymMapBase> oList)
                {
                    foreach (var m in oList)
                        Traverse(m);
                }
                else if (o is Dict<string, SymMapBase> oDict)
                {
                    foreach (var m in oDict.Values)
                        Traverse(m);
                }
                else if (o is SymbolDict<SymMapBase> oSymbolDict)
                {
                    foreach (var m in oSymbolDict.Values)
                        Traverse(m);
                }
                else if (o is DictSet<SymMapBase> oDictSet)
                {
                    foreach (var m in oDictSet)
                        Traverse(m);
                }
            }
        }

        // post visit
        if ((VisitMode & Visit.Post) != 0)
        {
            if (visitor != null)
            {
                if (ModifyMode == Mode.Modifying)
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
    [Obsolete("Use Symbol overload")]
    public static List<SymMapBase> Collect(SymMapBase root, string typenameToCollect)
         => new SymMapBaseCollectionTraversal(root).Collect((Symbol)typenameToCollect);

    public static List<SymMapBase> Collect(SymMapBase root, Symbol typenameToCollect)
         => new SymMapBaseCollectionTraversal(root).Collect(typenameToCollect);

    public SymMapBaseCollectionTraversal(SymMapBase root) => m_root = root;

    public SymMapBaseCollectionTraversal(SymMapBase root, TextWriter debugOutput)
    {
        m_root = root;
        m_out = debugOutput;
    }

    private readonly TextWriter m_out = null;

    [Obsolete("Use Symbol overload")]
    public List<SymMapBase> Collect(string typenameToCollect)
    {
        return Collect((Symbol)typenameToCollect);
    }

    public List<SymMapBase> Collect(Symbol typenameToCollect)
    {
        m_out?.WriteLine("SymMapBaseCollectionTraversal START");

        m_visited = [];
        m_name = typenameToCollect;
        m_result = [];
        SymMapBaseTraversal trav = new(
            SymMapBaseTraversal.Mode.NonModifying,
            SymMapBaseTraversal.Visit.Pre
            );
        trav.OnDefaultVisit += new SymMapBaseVisitor(Visit);
        trav.Traverse(m_root);

        m_out?.WriteLine("SymMapBaseCollectionTraversal END");

        return m_result;
    }

    private SymMapBase Visit(SymMapBase m, SymMapBaseTraversal.Visit visit)
    {
        m_out?.WriteLine("visiting {0}", m.TypeName);

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

    private readonly SymMapBase m_root;
    private Symbol m_name;
    private List<SymMapBase> m_result;
    private Dictionary<SymMapBase, int> m_visited;
}
