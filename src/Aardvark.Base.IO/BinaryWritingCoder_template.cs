using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.VRVis
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# var directlyCodeableTypes = Meta.DirectlyCodeableTypes.Map(t =>t.Name);
    //# var geometryTypes = Meta.GeometryTypes.Map(t => t.Name);
    //# var specialSimpleTypes = new[] { "bool", "char", "string", "Type", "Guid", "Symbol" };
    //# var structTypes = new List<string>();
    //# var fastStructTypes = new List<string>();
    public partial class BinaryWritingCoder
    {
        #region Vectors

        //# foreach (var t in Meta.VecTypes) { fastStructTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ value)
        {
            //# t.Fields.ForEach(f => {
            m_writer.Write(value.__f__);
            //# });
        }

        //# }
        #endregion

        #region Matrices

        //# foreach (var t in Meta.MatTypes) { fastStructTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ value) { m_writer.Write(value); }
        //# }

        #endregion

        #region Ranges and Boxes

        //# foreach (var t in Meta.RangeAndBoxTypes) { fastStructTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        //# }
        #endregion

        #region Colors

        //# foreach (var t in Meta.ColorTypes) { fastStructTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ value)
        {
            //# t.Fields.ForEach(f => {
            m_writer.Write(value.__f__);
            //# });
        }

        //# }
        #endregion

        #region Trafos

        //# foreach (var t in Meta.TrafoTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ value) { m_writer.Write(value); }
        //# }

        #endregion

        #region Tensors

        //# foreach (var t in geometryTypes) structTypes.Add(t);
        //# var simpleTypes = directlyCodeableTypes.Concat(specialSimpleTypes).Concat(geometryTypes);
        //# var genericTensorTypes = new[] { "Vector", "Matrix", "Volume", "Tensor" };
        //# var genericTensorSizes = new[] { "long", "V2l", "V3l", "long[]" };
        //# genericTensorTypes.ForEach((tt, ti) => {
        //#     var ts = genericTensorSizes[ti]; var tsn = Meta.GetXmlTypeName(ts);
        //#     simpleTypes.ForEach(t => {
        //#         var type = tt + "<" + t + ">";
        //#         structTypes.Add(type);
        //# var dname = Meta.GetXmlTypeName(t + "[]");
        //# var name = Meta.GetXmlTypeName(type);
        public void Code__name__(ref __type__ value)
        {
            var data = value.Data; Code__dname__(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; Code__tsn__(ref size);
            var delta = value.Delta; Code__tsn__(ref delta);
        }

        //#     });
        //# });
        #endregion

        #region Arrays

        //# fastStructTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        //# });
        //# structTypes.ForEach(t => {
        //# var ename = Meta.GetXmlTypeName(t);
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) Code__ename__(ref value[i]);
        }

        //# });
        #endregion

        #region Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[,]");
        public void Code__name2d__(ref __t__[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        //# var name3d = Meta.GetXmlTypeName(t + "[,,]");
        public void Code__name3d__(ref __t__[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        //# });
        #endregion

        #region Jagged Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var ename2d = Meta.GetXmlTypeName(t + "[]");
        //# var name2d = Meta.GetXmlTypeName(t + "[][]");
        public void Code__name2d__(ref __t__[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) Code__ename2d__(ref value[i]);
        }

        //# var ename3d = Meta.GetXmlTypeName(t + "[][]");
        //# var name3d = Meta.GetXmlTypeName(t + "[][][]");
        public void Code__name3d__(ref __t__[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) Code__ename3d__(ref value[i]);
        }

        //# });
        #endregion

        #region Lists

        //# fastStructTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        //# });
        //# structTypes.ForEach(t => {
        //# var ename = Meta.GetXmlTypeName(t);
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; Code__ename__(ref x); }
        }

        //# });
        #endregion
    }
}
