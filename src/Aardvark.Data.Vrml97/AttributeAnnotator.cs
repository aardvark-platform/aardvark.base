using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aardvark.Data.Vrml97
{
    /// <summary>
    /// The AttributeAnnotator is used to annotate all
    /// geometry nodes with material and transform
    /// attributes.
    /// 
    /// Example:
    /// 
    /// Parser parser = new Parser("myVrmlFile.wrl");
    /// SymMapBase parseTree = parser.Perform();
    /// 
    /// AttributeAnnotator resolver = new AttributeAnnotator();
    /// SymMapBase annotatedParseTree = resolver.Perform(parseTree);
    /// 
    /// </summary>
    internal class AttributeAnnotator
    {
        private Stack<SymMapBase> m_material;
        private Stack<SymMapBase> m_texture;
        private Stack<SymMapBase> m_textureTransform;
        private Stack<Trafo3d> m_transform;
        private string m_path = "";
        private Dictionary<SymMapBase, SymMapBase> m_visited = new Dictionary<SymMapBase, SymMapBase>();

        public static Vrml97Scene Annotate(Vrml97Scene vrmlParseTree)
        {
            return new AttributeAnnotator().Perform(vrmlParseTree);
        }

        /// <summary>
        /// Takes a Vrml97 parse tree (see also <seealso cref="Parser"/>)
        /// and augments all geometry nodes with material and
        /// transform attributes.
        /// </summary>
        /// <param name="root">Parse tree.</param>
        /// <returns>Augmented parse tree.</returns>
        public Vrml97Scene Perform(Vrml97Scene root)
        {
            root.ParseTree["AttributeAnnotator.Performed"] = true; // leave hint

            if (root.ParseTree.TypeName == "Vrml97")
            {
                string filename = root.ParseTree.Get<string>(Vrml97Sym.filename);
                m_path = Path.GetDirectoryName(filename);
            }

            m_material = new Stack<SymMapBase>();
            m_texture = new Stack<SymMapBase>();
            m_textureTransform = new Stack<SymMapBase>();
            m_transform = new Stack<Trafo3d>();

            m_transform.Push(Trafo3d.Identity);

            SymMapBaseTraversal trav = new SymMapBaseTraversal(SymMapBaseTraversal.Mode.Modifying, SymMapBaseTraversal.Visit.PreAndPost);

            trav.PerNameVisitors["ImageTexture"] =
            delegate(SymMapBase m, SymMapBaseTraversal.Visit visit)
            {
                if (visit == SymMapBaseTraversal.Visit.Pre)
                {
                    if (m_visited.ContainsKey(m)) return m;
                    List<string> urls = m.Get<List<string>>(Vrml97Sym.url);
                    if (urls != null)
                    {
                        for (int i = 0; i < urls.Count; i++)
                        {
                            urls[i] = Path.Combine(m_path, urls[i]);
                        }
                    }
                    m_visited[m] = m;
                }
                return m;
            };

            // geometry nodes
            SymMapBaseVisitor foo = (SymMapBase m, SymMapBaseTraversal.Visit visit) =>
            {
                if (visit == SymMapBaseTraversal.Visit.Post) return m;

                if (m_material.Count == 0
                    && m_texture.Count == 0
                    && m_textureTransform.Count == 0
                    && m_transform.Count == 0)
                    return m;

                var map = new SymMapBase(m);

                if (m_material.Count > 0)
                {
                    string key = "material";
                    // while (m.Contains(key)) key += "X";
                    map[key] = m_material.Peek();
                }

                if (m_texture.Count > 0)
                {
                    string key = "texture";
                    // while (m.Contains(key)) key += "X";
                    map[key] = m_texture.Peek();
                }

                if (m_textureTransform.Count > 0)
                {
                    string key = "textureTransform";
                    // while (m.Contains(key)) key += "X";
                    var tt = m_textureTransform.Peek();

                    map[key] = tt.ExtractVrmlTextureTrafo();
                }

                if (m_transform.Count > 1) // [0] contains initial identity
                {
                    string key = "transform";
                    // while (m.Contains(key)) key += "X";
                    if (m.Contains(key))
                    {
                        Console.WriteLine("WARNING: trying to annotate annotated node!");
                    }
                    map[key] = m_transform.Peek();
                }

                return map;
            };

            trav.PerNameVisitors["IndexedFaceSet"] = foo;
            trav.PerNameVisitors["IndexedLineSet"] = foo;
              

            // attributes
            trav.PerNameVisitors["Shape"] =
            delegate(SymMapBase m, SymMapBaseTraversal.Visit visit)
            {
                bool hasMaterial = false;
                bool hasTexture = false;
                bool hasTextureTransform = false;
                SymMapBase app = null;

                if (m.Contains("appearance"))
                {
                    app = m.Get<SymMapBase>(Vrml97Sym.appearance);
                    hasMaterial = app.Contains(Vrml97Sym.material);
                    hasTexture = app.Contains(Vrml97Sym.texture);
                    hasTextureTransform = app.Contains(Vrml97Sym.textureTransform);
                }

                if (visit == SymMapBaseTraversal.Visit.Pre)
                {
                    if (hasMaterial) m_material.Push(app.Get<SymMapBase>(Vrml97Sym.material));
                    if (hasTexture) m_texture.Push(app.Get<SymMapBase>(Vrml97Sym.texture));
                    if (hasTextureTransform) m_textureTransform.Push(app.Get<SymMapBase>(Vrml97Sym.textureTransform));
                }
                else if (visit == SymMapBaseTraversal.Visit.Post)
                {
                    if (hasMaterial) m_material.Pop();
                    if (hasTexture) m_texture.Pop();
                    if (hasTextureTransform) m_textureTransform.Pop();
                }

                return m;
            };

            trav.PerNameVisitors["Transform"] =
            delegate(SymMapBase m, SymMapBaseTraversal.Visit visit)
            {
                if (visit == SymMapBaseTraversal.Visit.Pre)
                {
                    var trafo = m.ExtractVrmlGeometryTrafo();

                    m["trafo"] = trafo;

                    m_transform.Push(trafo * m_transform.Peek());
                }
                else if (visit == SymMapBaseTraversal.Visit.Post)
                {
                    m_transform.Pop();
                }

                return m;
            };

            root.ParseTree = trav.Traverse(root.ParseTree);
            return root;
        }
    }
    
    /// <summary>
    /// Various helper methods.
    /// </summary>
    public static class VrmlHelpers
    {
        /// <summary>
        /// Extracts texture transform from given node.
        /// </summary>
        public static Trafo2d ExtractVrmlTextureTrafo(this SymMapBase m)
        {
			if (m == null) return Trafo2d.Identity;

            // get trafo parts
            var c = (V2d)m.Get<V2f>(Vrml97Sym.center, V2f.Zero);
            var r = (double)m.Get<float>(Vrml97Sym.rotation, 0.0f);
            var s = (V2d)m.Get<V2f>(Vrml97Sym.scale, new V2f(1, 1));
            var t = (V2d)m.Get<V2f>(Vrml97Sym.translation, V2f.Zero);

            M33d C = M33d.Translation(c), Ci = M33d.Translation(-c);
            M33d R = M33d.Rotation(r), Ri = M33d.Rotation(-r);
            M33d S = M33d.Scale(s), Si = M33d.Scale(1 / s);
            M33d T = M33d.Translation(t), Ti = M33d.Translation(-t);

            return new Trafo2d(
                            Ci * S * R * C * T,
                            Ti * Ci * Ri * Si * C);
        }

        /// <summary>
        /// Returns geometry transform from given node.
        /// </summary>
        public static Trafo3d ExtractVrmlGeometryTrafo(this SymMapBase m)
        {
            // get trafo parts
            var c = (V3d)m.Get<V3f>(Vrml97Sym.center, V3f.Zero);

            var r = (V4d)m.Get<V4f>(Vrml97Sym.rotation, V4f.Zero);
            if (r.X == 0 && r.Y == 0 && r.Z == 0) r.Z = 1;

            var s = (V3d)m.Get<V3f>(Vrml97Sym.scale, new V3f(1, 1, 1));

            var sr = (V4d)m.Get<V4f>(Vrml97Sym.scaleOrientation, V4f.Zero);
            if (sr.X == 0 && sr.Y == 0 && sr.Z == 0) sr.Z = 1;

            var t = (V3d)m.Get<V3f>(Vrml97Sym.translation, V3f.Zero);

            // create composite trafo (naming taken from vrml97 spec)
            M44d C = M44d.Translation(c), Ci = M44d.Translation(-c);
            M44d SR = M44d.Rotation(sr.XYZ, sr.W),
                                    SRi = M44d.Rotation(sr.XYZ, -sr.W);
            M44d T = M44d.Translation(t), Ti = M44d.Translation(-t);

            //if (m_aveCompatibilityMode) r.W = -r.W;
            M44d R = M44d.Rotation(r.XYZ, r.W),
                                        Ri = M44d.Rotation(r.XYZ, -r.W);

            // in case some axis scales by 0 the best thing for the inverse scale is also 0
            var si = new V3d(s.X.IsTiny() ? 0 : 1 / s.X, 
                             s.Y.IsTiny() ? 0 : 1 / s.Y, 
                             s.Z.IsTiny() ? 0 : 1/ s.Z);
            M44d S = M44d.Scale(s), Si = M44d.Scale(si);

            return new Trafo3d(
                            T * C * R * SR * S * SRi * Ci,
                            C * SR * Si * SRi * Ri * Ci * Ti);
        }
    }
}
