using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Aardvark.Base.Coder
{

    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# var directlyCodeableTypes = Meta.DirectlyCodeableTypes.Map(t => t.Name);
    //# var specialSimpleTypes = new[] { "bool", "char", "string", "Type", "Guid", "Symbol" };
    //# var geometryTypes = Meta.GeometryTypes.Map(t => t.Name);
    //# var allTypes = directlyCodeableTypes.Concat(specialSimpleTypes).Concat(geometryTypes);

    public static partial class TypeCoder
    {
        internal static Dictionary<Type, Action<IWritingCoder, object>> TypeWriterMap =
                                    new Dictionary<Type, Action<IWritingCoder, object>>
        {
            //# allTypes.ForEach((t, i) => {
            #region __t__

            //# var n = Meta.GetXmlTypeName(t);
            { typeof(__t__), (c,o) => { var v = (__t__)o; c.Code__n__(ref v); } },
            //# var na = Meta.GetXmlTypeName(t + "[]");
            { typeof(__t__[]), (c,o) => { var v = (__t__[])o; c.Code__na__(ref v); } },
            //# var ln = Meta.GetXmlTypeName("List<" + t + ">");
            { typeof(List<__t__>), (c,o) => { var v = (List<__t__>)o; c.Code__ln__(ref v); } },

            //# var vet = "Vector<" + t + ">";
            //# var ven = Meta.GetXmlTypeName(vet);
            { typeof(Vector<__t__>), (c,o) => { var v = (Vector<__t__>)o; c.Code__ven__(ref v); } },
            //# var vena = Meta.GetXmlTypeName(vet + "[]");
            { typeof(Vector<__t__>[]), (c,o) => { var v = (Vector<__t__>[])o; c.Code__vena__(ref v); } },
            //# var lven = Meta.GetXmlTypeName("List<" + vet + ">");
            { typeof(List<Vector<__t__>>), (c,o) => { var v = (List<Vector<__t__>>)o; c.Code__lven__(ref v); } },

            //# var mat = "Matrix<" + t + ">";
            //# var man = Meta.GetXmlTypeName(mat);
            { typeof(Matrix<__t__>), (c,o) => { var v = (Matrix<__t__>)o; c.Code__man__(ref v); } },
            //# var mana = Meta.GetXmlTypeName(mat + "[]");
            { typeof(Matrix<__t__>[]), (c,o) => { var v = (Matrix<__t__>[])o; c.Code__mana__(ref v); } },
            //# var lman = Meta.GetXmlTypeName("List<" + mat + ">");
            { typeof(List<Matrix<__t__>>), (c,o) => { var v = (List<Matrix<__t__>>)o; c.Code__lman__(ref v); } },

            //# var vot = "Volume<" + t + ">";
            //# var von = Meta.GetXmlTypeName(vot);
            { typeof(Volume<__t__>), (c,o) => { var v = (Volume<__t__>)o; c.Code__von__(ref v); } },
            //# var vona = Meta.GetXmlTypeName(vot + "[]");
            { typeof(Volume<__t__>[]), (c,o) => { var v = (Volume<__t__>[])o; c.Code__vona__(ref v); } },
            //# var lvon = Meta.GetXmlTypeName("List<" + vot + ">");
            { typeof(List<Volume<__t__>>), (c,o) => { var v = (List<Volume<__t__>>)o; c.Code__lvon__(ref v); } },

            //# var tet = "Tensor<" + t + ">";
            //# var ten = Meta.GetXmlTypeName(tet);
            { typeof(Tensor<__t__>), (c,o) => { var v = (Tensor<__t__>)o; c.Code__ten__(ref v); } },
            //# var tena = Meta.GetXmlTypeName(tet + "[]");
            { typeof(Tensor<__t__>[]), (c,o) => { var v = (Tensor<__t__>[])o; c.Code__tena__(ref v); } },
            //# var lten = Meta.GetXmlTypeName("List<" + tet + ">");
            { typeof(List<Tensor<__t__>>), (c,o) => { var v = (List<Tensor<__t__>>)o; c.Code__lten__(ref v); } },

            #endregion

            //# });
            #region Multi-Dimensional Arrays

            //# directlyCodeableTypes.ForEach((t, i) => {
            //# var n2d = Meta.GetXmlTypeName(t + "[,]");
            { typeof(__t__[,]), (c,o) => { var v = (__t__[,])o; c.Code__n2d__(ref v); } },
            //# var n3d = Meta.GetXmlTypeName(t + "[,,]");
            { typeof(__t__[, ,]), (c,o) => { var v = (__t__[, ,])o; c.Code__n3d__(ref v); } },
            //# });

            #endregion

            #region Jagged Multi-Dimensional Arrays

            //# directlyCodeableTypes.ForEach((t, i) => {
            //# var n2d = Meta.GetXmlTypeName(t + "[][]");
            { typeof(__t__[][]), (c,o) => { var v = (__t__[][])o; c.Code__n2d__(ref v); } },
            //# var n3d = Meta.GetXmlTypeName(t + "[][][]");
            { typeof(__t__[][][]), (c,o) => { var v = (__t__[][][])o; c.Code__n3d__(ref v); } },
            //# });

            #endregion

            #region Other Types

            { typeof(IntSet), (c,o) => { var v = (IntSet)o; c.CodeIntSet(ref v); } },
            { typeof(SymbolSet), (c,o) => { var v = (SymbolSet)o; c.CodeSymbolSet(ref v); } },


            { typeof(HashSet<string>), (c, o) => { var v = (HashSet<string>)o; c.CodeHashSet_of_T_(ref v); } },

            #endregion
        };

        internal static Dictionary<Type, Func<IReadingCoder, object>> TypeReaderMap =
                                new Dictionary<Type, Func<IReadingCoder, object>>
        {
            //# allTypes.ForEach((t, i) => {
            #region __t__

            //# var n = Meta.GetXmlTypeName(t);
            { typeof(__t__), c => { var v = default(__t__); c.Code__n__(ref v); return v; } },
            //# var na = Meta.GetXmlTypeName(t + "[]");
            { typeof(__t__[]), c => { var v = default(__t__[]); c.Code__na__(ref v); return v; } },
            //# var ln = Meta.GetXmlTypeName("List<" + t + ">");
            { typeof(List<__t__>), c => { var v = default(List<__t__>); c.Code__ln__(ref v); return v; } },

            //# var vet = "Vector<" + t + ">";
            //# var ven = Meta.GetXmlTypeName(vet);
            { typeof(Vector<__t__>), c => { var v = default(Vector<__t__>); c.Code__ven__(ref v); return v; } },
            //# var vena = Meta.GetXmlTypeName(vet + "[]");
            { typeof(Vector<__t__>[]), c => { var v = default(Vector<__t__>[]); c.Code__vena__(ref v); return v; } },
            //# var lven = Meta.GetXmlTypeName("List<" + vet + ">");
            { typeof(List<Vector<__t__>>), c => { var v = default(List<Vector<__t__>>); c.Code__lven__(ref v); return v; } },

            //# var mat = "Matrix<" + t + ">";
            //# var man = Meta.GetXmlTypeName(mat);
            { typeof(Matrix<__t__>), c => { var v = default(Matrix<__t__>); c.Code__man__(ref v); return v; } },
            //# var mana = Meta.GetXmlTypeName(mat + "[]");
            { typeof(Matrix<__t__>[]), c => { var v = default(Matrix<__t__>[]); c.Code__mana__(ref v); return v; } },
            //# var lman = Meta.GetXmlTypeName("List<" + mat + ">");
            { typeof(List<Matrix<__t__>>), c => { var v = default(List<Matrix<__t__>>); c.Code__lman__(ref v); return v; } },

            //# var vot = "Volume<" + t + ">";
            //# var von = Meta.GetXmlTypeName(vot);
            { typeof(Volume<__t__>), c => { var v = default(Volume<__t__>); c.Code__von__(ref v); return v; } },
            //# var vona = Meta.GetXmlTypeName(vot + "[]");
            { typeof(Volume<__t__>[]), c => { var v = default(Volume<__t__>[]); c.Code__vona__(ref v); return v; } },
            //# var lvon = Meta.GetXmlTypeName("List<" + vot + ">");
            { typeof(List<Volume<__t__>>), c => { var v = default(List<Volume<__t__>>); c.Code__lvon__(ref v); return v; } },

            //# var tet = "Tensor<" + t + ">";
            //# var ten = Meta.GetXmlTypeName(tet);
            { typeof(Tensor<__t__>), c => { var v = default(Tensor<__t__>); c.Code__ten__(ref v); return v; } },
            //# var tena = Meta.GetXmlTypeName(tet + "[]");
            { typeof(Tensor<__t__>[]), c => { var v = default(Tensor<__t__>[]); c.Code__tena__(ref v); return v; } },
            //# var lten = Meta.GetXmlTypeName("List<" + tet + ">");
            { typeof(List<Tensor<__t__>>), c => { var v = default(List<Tensor<__t__>>); c.Code__lten__(ref v); return v; } },

            #endregion

            //# });
            #region Multi-Dimensional Arrays

            //# directlyCodeableTypes.ForEach((t, i) => {
            //# var n2d = Meta.GetXmlTypeName(t + "[,]");
            { typeof(__t__[,]), c => { var v = default(__t__[,]); c.Code__n2d__(ref v); return v; } },
            //# var n3d = Meta.GetXmlTypeName(t + "[,,]");
            { typeof(__t__[, ,]), c => { var v = default(__t__[, ,]); c.Code__n3d__(ref v); return v; } },
            //# });

            #endregion

            #region Jagged Multi-Dimensional Arrays

            //# directlyCodeableTypes.ForEach((t, i) => {
            //# var n2d = Meta.GetXmlTypeName(t + "[][]");
            { typeof(__t__[][]), c => { var v = default(__t__[][]); c.Code__n2d__(ref v); return v; } },
            //# var n3d = Meta.GetXmlTypeName(t + "[][][]");
            { typeof(__t__[][][]), c => { var v = default(__t__[][][]); c.Code__n3d__(ref v); return v; } },
            //# });

            #endregion

            #region Other Types

            { typeof(IntSet), c => { var v = default(IntSet); c.CodeIntSet(ref v); return v; } },
            { typeof(SymbolSet), c => { var v = default(SymbolSet); c.CodeSymbolSet(ref v); return v; } },

            { typeof(HashSet<string>), c => { var v = default(HashSet<string>); c.CodeHashSet_of_T_(ref v); return v; } },

            #endregion
        };

        public static partial class Default
        {
            /// <summary>
            /// The default way of encoding various basic types.
            /// </summary>
            public static TypeInfo[] Basic = new[]
            {
                //# allTypes.ForEach((t, i) => {
                #region __t__

                new TypeInfo("__t__", typeof(__t__), TypeInfo.Option.None),
                new TypeInfo(typeof(__t__[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<__t__>), TypeInfo.Option.None),

                new TypeInfo("Vector<__t__>", "Vector_of_" + "__t__", typeof(Vector<__t__>), TypeInfo.Option.None),
                new TypeInfo("Vector<__t__>[]", "Array_of_Vector_of_" + "__t__", typeof(Vector<__t__>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<__t__>>", "List_of_Vector_of_" + "__t__", typeof(List<Vector<__t__>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<__t__>", "Matrix_of_" + "__t__", typeof(Matrix<__t__>), TypeInfo.Option.None),
                new TypeInfo("Matrix<__t__>[]", "Array_of_Matrix_of_" + "__t__", typeof(Matrix<__t__>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<__t__>>", "List_of_Matrix_of_" + "__t__", typeof(List<Matrix<__t__>>), TypeInfo.Option.None),

                new TypeInfo("Volume<__t__>", "Volume_of_" + "__t__", typeof(Volume<__t__>), TypeInfo.Option.None),
                new TypeInfo("Volume<__t__>[]", "Array_of_Volume_of_" + "__t__", typeof(Volume<__t__>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<__t__>>", "List_of_Volume_of_" + "__t__", typeof(List<Volume<__t__>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<__t__>", "Tensor_of_" + "__t__", typeof(Tensor<__t__>), TypeInfo.Option.None),
                new TypeInfo("Tensor<__t__>[]", "Array_of_Tensor_of_" + "__t__", typeof(Tensor<__t__>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<__t__>>", "List_of_Tensor_of_" + "__t__", typeof(List<Tensor<__t__>>), TypeInfo.Option.None),

                #endregion

                //# });

                new TypeInfo("HashSet<string>", "HashSet_of_string", typeof(HashSet<string>), TypeInfo.Option.None),
            };
        }
    }
}
