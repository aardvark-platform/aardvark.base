using Aardvark.Base;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Data.Vrml97
{
    /// <summary>
    /// IndexedFaceSet.
    /// </summary>
    public class Vrml97Ifs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Vrml97Ifs(SymMapBase parseTree) => m_parseTree = parseTree;

        /// <summary>
        /// Raw parse tree.
        /// </summary>
        public SymMapBase ParseTree => m_parseTree;

        /// <summary>
        /// Single texture.
        /// </summary>
        public string TextureFileName
        {
            get
            {
                var texnode = m_parseTree.Get<SymMapBase>(Vrml97Sym.texture);
                if (texnode == null) return null;
                return texnode.Get<List<string>>(Vrml97Sym.url).FirstOrDefault();
            }
        }

        /// <summary>
        /// Multiple textures.
        /// </summary>
        public List<string> TextureFileNames
        {
            get
            {
                var texnode = m_parseTree.Get<SymMapBase>(Vrml97Sym.texture);
                if (texnode == null) return null;
                return texnode.Get<List<string>>(Vrml97Sym.url);
            }
        }

        private SymMapBase m_parseTree;
    }
}
