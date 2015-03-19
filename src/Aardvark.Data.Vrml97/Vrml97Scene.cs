using Aardvark.Base;
using System.Collections.Generic;
using System.IO;

namespace Aardvark.Data.Vrml97
{
    /// <summary>
    /// A complete VRML97 scene.
    /// </summary>
    public class Vrml97Scene
    {
        private SymMapBase m_parseTree;
        private Dictionary<string, SymMapBase> m_namedNodes;

        /// <summary>
        /// Creates a Vrml97Scene from given VRML97 file.
        /// </summary>
        public static Vrml97Scene FromFile(string fileName)
        {
            return FromFile(fileName, true, true);
        }

        /// <summary>
        /// Creates a Vrml97Scene from given VRML97 file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="resolveDefUse"></param>
        /// <param name="annotate"></param>
        /// <param name="duplicateDefUseMaps"></param>
        /// <returns></returns>
        public static Vrml97Scene FromFile(string fileName, bool resolveDefUse, bool annotate, bool duplicateDefUseMaps = true)
        {
            if (fileName == null) return null;

            var result = Parse(new Parser(fileName), resolveDefUse, annotate, duplicateDefUseMaps);
            return result;
        }

        /// <summary>
        /// Creates a Vrml97Scene from given stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Vrml97Scene FromStream(Stream stream, string fileName)
        {
            return Parse(new Parser(stream, fileName), true, true);
        }

        /// <summary>
        ///  Constructor.
        /// </summary>
        public Vrml97Scene(SymMapBase parseTree)
        {
            m_parseTree = parseTree;
        }

        /// <summary>
        /// Raw parse tree.
        /// </summary>
        public SymMapBase ParseTree
        {
            get { return m_parseTree; }
            internal set { m_parseTree = value; }
        
        }

        /// <summary>
        /// Enumerates all IndexedFaceSets in scene.
        /// </summary>
        public IEnumerable<Vrml97Ifs> IndexedFaceSets
        {
            get
            {
                foreach (var x
                    in SymMapBaseCollectionTraversal.Collect(ParseTree, "IndexedFaceSet"))
                    yield return new Vrml97Ifs(x);
            }
        }

        /// <summary>
        /// Enumerates all IndexedLineSets in scene.
        /// </summary>
        public IEnumerable<Vrml97Ils> IndexedLineSets
        {
            get
            {
                foreach (var x
                    in SymMapBaseCollectionTraversal.Collect(ParseTree, "IndexedLineSet"))
                    yield return new Vrml97Ils(x);
            }
        }

        /// <summary>
        /// Enumerates all PositionInterpolators in scene.
        /// </summary>
        public IEnumerable<SymMapBase> PositionInterpolators
        {
            get
            {
                foreach (var x
                    in SymMapBaseCollectionTraversal.Collect(ParseTree, "PositionInterpolator"))
                    yield return x;
            }
        }

        /// <summary>
        /// Enumerates all OrientationInterpolators in scene.
        /// </summary>
        public IEnumerable<SymMapBase> OrientationInterpolators
        {
            get
            {
                foreach (var x
                    in SymMapBaseCollectionTraversal.Collect(ParseTree, "OrientationInterpolator"))
                    yield return x;
            }
        }

        /// <summary>
        /// Enumerates all PointSets in scene.
        /// </summary>
        public IEnumerable<SymMapBase> PointSets
        {
            get
            {
                foreach (var x
                    in SymMapBaseCollectionTraversal.Collect(ParseTree, "PointSet"))
                    yield return x;
            }
        }

        /// <summary>
        /// Enumerates all TimeSensors in scene.
        /// </summary>
        public IEnumerable<SymMapBase> TimeSensor
        {
            get
            {
                foreach (var x
                    in SymMapBaseCollectionTraversal.Collect(ParseTree, "TimeSensor"))
                    yield return x;
            }
        }

        /// <summary>
        /// Returns dictionary containing all named nodes in the scene.
        /// </summary>
        public Dictionary<string, SymMapBase> NamedNodes
        {
            get
            {
                return m_namedNodes;
            }
        }

        private static Vrml97Scene Parse(Parser parser, bool resolveDefUse, bool annotate, bool duplicateMaps = true)
        {
            var root = parser.Perform();

            if (resolveDefUse)
                root = DefUseResolver.Resolve(root, out root.m_namedNodes, duplicateMaps);

            if (annotate)
                root = AttributeAnnotator.Annotate(root);

            return root;
        }
    }
}
