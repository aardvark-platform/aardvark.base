using Aardvark.Base;

namespace Aardvark.Data.Vrml97
{
    /// <summary>
    /// IndexedLineSet.
    /// </summary>
    public class Vrml97Ils
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Vrml97Ils(SymMapBase parseTree)
        {
            m_parseTree = parseTree;
        }

        /// <summary>
        /// Raw parse tree.
        /// </summary>
        public SymMapBase ParseTree { get { return m_parseTree; } }

        private SymMapBase m_parseTree;
    }
}
