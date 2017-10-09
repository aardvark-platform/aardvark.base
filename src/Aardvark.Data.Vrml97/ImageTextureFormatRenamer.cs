using Aardvark.Base;
using System.Collections.Generic;

namespace Aardvark.Data.Vrml97
{

    /// <summary>
    /// </summary>
    public class ImageTextureFormatRenamer
    {

        /// <summary>
        /// Takes a Vrml97 parse tree (see also <seealso cref="Parser"/>)
        /// and replaces the file extension of all image texture URLs to
        ///  the specified extension.
        /// </summary>
        /// <param name="root">Parse tree.</param>
        /// <param name="newExtension"></param>
        internal Vrml97Scene Perform(Vrml97Scene root, string newExtension)
        {
            SymMapBaseTraversal trav = new SymMapBaseTraversal(SymMapBaseTraversal.Visit.Post);

            trav.PerNameVisitors["ImageTexture"] =
            delegate(SymMapBase m, SymMapBaseTraversal.Visit visit)
            {
                List<string> urls = m.Get<List<string>>(Vrml97Sym.url);
                if (urls == null) return m;
                for (int i = 0; i < urls.Count; i++)
                {
                    int p = urls[i].LastIndexOf('.');
                    urls[i] = urls[i].Substring(0, p) + "." + newExtension;
                }

                return m;
            };

            trav.Traverse(root.ParseTree);
            return root;
        }
    }

}
