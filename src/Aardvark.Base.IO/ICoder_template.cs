using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.VRVis
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

    }
}
