using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Coder
{

    // AUTO GENERATED CODE - DO NOT CHANGE

    //# var directlyCodeableTypes = Meta.DirectlyCodeableTypes.Map(t =>t.Name);
    //# var geometryTypes = Meta.GeometryTypes.Map(t => t.Name);
    //# var specialSimpleTypes = new[] { "bool", "char", "string", "Type", "Guid", "Symbol" };
    //# var structTypes = new List<string>();
    public partial interface ICoder
    {
        #region Struct Types

        //# foreach (var type in specialSimpleTypes.Concat(directlyCodeableTypes)) { structTypes.Add(type);
        //# var name = Meta.GetXmlTypeName(type);
        void Code__name__(ref __type__ value);
        //# }

        #endregion

        #region Tensors

        //# foreach (var t in geometryTypes) structTypes.Add(t);
        //# var simpleTypes = directlyCodeableTypes.Concat(specialSimpleTypes).Concat(geometryTypes);
        //# var genericTensorTypes = new[] { "Vector", "Matrix", "Volume", "Tensor" };
        //# var genericTensorSizes = new[] { "long", "V2l", "V3l", "long[]" };
        //# genericTensorTypes.ForEach((tt, ti) => {
        //#     var ts = genericTensorSizes[ti];
        //#     simpleTypes.ForEach((t, i) => {
        //#         var type = tt + "<" + t + ">";
        //#         structTypes.Add(type);
        //# var name = Meta.GetXmlTypeName(type);
        void Code__name__(ref __type__ value);
        //#     });
        //# });

        #endregion

        #region Arrays

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        void Code__name__(ref __t__[] value);
        //# });

        #endregion

        #region Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[,]");
        void Code__name2d__(ref __t__[,] value);
        //# var name3d = Meta.GetXmlTypeName(t + "[,,]");
        void Code__name3d__(ref __t__[, ,] value);
        //# });

        #endregion

        #region Jagged Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[][]");
        void Code__name2d__(ref __t__[][] value);
        //# var name3d = Meta.GetXmlTypeName(t + "[][][]");
        void Code__name3d__(ref __t__[][][] value);
        //# });

        #endregion

        #region Lists

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        void Code__name__(ref List<__t__> value);
        //# });

        #endregion

        #region Geometry Types

        //# foreach (var isDouble in new[] { false, true }) {
        //#   var ftype = isDouble ? "Double" : "Float";
        //#   var tc = isDouble ? "d" : "f";
        void CodeCircle2__tc__(ref Circle2__tc__ v);
        void CodeLine2__tc__(ref Line2__tc__ v);
        void CodeLine3__tc__(ref Line3__tc__ v);
        void CodePlane2__tc__(ref Plane2__tc__ v);
        void CodePlane3__tc__(ref Plane3__tc__ v);
        void CodePlaneWithPoint3__tc__(ref PlaneWithPoint3__tc__ v);
        void CodeQuad2__tc__(ref Quad2__tc__ v);
        void CodeQuad3__tc__(ref Quad3__tc__ v);
        void CodeRay2__tc__(ref Ray2__tc__ v);
        void CodeRay3__tc__(ref Ray3__tc__ v);
        void CodeSphere3__tc__(ref Sphere3__tc__ v);
        void CodeTriangle2__tc__(ref Triangle2__tc__ v);
        void CodeTriangle3__tc__(ref Triangle3__tc__ v);

        //# }
        #endregion
    }
}
