using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Aardvark.VRVis
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# var directlyCodeableTypes = Meta.DirectlyCodeableTypes.Map(t =>t.Name);
    //# var geometryTypes = Meta.GeometryTypes.Map(t => t.Name);
    //# var specialSimpleTypes = new[] { "bool", "char", "string", "Type", "Guid", "Symbol" };
    //# {
    //# var structTypes = new List<string>();
    //# var tensorTypes = new List<string>();
    public partial class NewXmlWritingCoder
    {
        #region Vectors

        //# foreach (var t in Meta.VecTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { throw new NotImplementedException(); }
        //# }

        #endregion

        #region Matrices

        //# foreach (var t in Meta.MatTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { throw new NotImplementedException(); }
        //# }

        #endregion

        #region Ranges and Boxes

        //# foreach (var t in Meta.RangeAndBoxTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { throw new NotImplementedException(); }
        //# }

        #endregion

        #region Colors

        //# foreach (var t in Meta.ColorTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { throw new NotImplementedException(); }
        //# }

        #endregion

        #region Trafos

        //# foreach (var t in Meta.TrafoTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { throw new NotImplementedException(); }
        //# }

        #endregion

        #region Tensors

        //# foreach (var t in geometryTypes) structTypes.Add(t);
        //# var simpleTypes = directlyCodeableTypes.Concat(specialSimpleTypes).Concat(geometryTypes);
        //# var genericTensorTypes = new[] { "Vector", "Matrix", "Volume", "Tensor" };
        //# genericTensorTypes.ForEach(tt => {
        //#     simpleTypes.ForEach(t => {
        //#         var type = tt + "<" + t + ">";
        //#         tensorTypes.Add(type);
        //# var name = Meta.GetXmlTypeName(type);
        public void Code__name__(ref __type__ value)
        {
            throw new NotImplementedException();
        }

        //#     });
        //# });
        #endregion

        #region Arrays

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] v) { throw new NotImplementedException(); }
        //# });

        #endregion

        #region Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[,]");
        public void Code__name2d__(ref __t__[,] v) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[,,]");
        public void Code__name3d__(ref __t__[, ,] v) { throw new NotImplementedException(); }
        //# });

        #endregion

        #region Jagged Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[][]");
        public void Code__name2d__(ref __t__[][] v) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[][][]");
        public void Code__name3d__(ref __t__[][][] v) { throw new NotImplementedException(); }
        //# });

        #endregion

        #region Lists

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> v) { throw new NotImplementedException(); }
        //# });

        #endregion

        #region Arrays of Tensors

        //# tensorTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] v) { throw new NotImplementedException(); }
        //# });

        #endregion

        #region Lists of Tensors

        //# tensorTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> v) { throw new NotImplementedException(); }
        //# });

        #endregion
    }

    //# } {
    //# var structTypes = new List<string>();
    //# var tensorTypes = new List<string>();
    public partial class XmlWritingCoder
    {
        #region Vectors

        //# foreach (var t in Meta.VecTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { AddValue(v.ToString()); }
        //# }

        #endregion

        #region Matrices

        //# foreach (var t in Meta.MatTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { AddValue(v.ToString()); }
        //# }

        #endregion

        #region Ranges and Boxes

        //# foreach (var t in Meta.RangeAndBoxTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { AddValue(v.ToString()); }
        //# }

        #endregion

        #region Colors

        //# foreach (var t in Meta.ColorTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { AddValue(v.ToString()); }
        //# }

        #endregion

        #region Trafos

        //# foreach (var t in Meta.TrafoTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { AddValue(v.ToString()); }
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
        //#         tensorTypes.Add(type);
        //# var dname = Meta.GetXmlTypeName(t + "[]");
        //# var name = Meta.GetXmlTypeName(type);
        public void Code__name__(ref __type__ value)
        {
            var item = new XElement("__tt__");
            m_elementStack.Push(m_element);
            m_element = item;

            var element = new XElement("Data");
            m_elementStack.Push(m_element);
            m_element = element;

            var data = value.Data; Code__dname__(ref data);

            m_element = m_elementStack.Pop();
            m_element.Add(element);

            element = new XElement("Origin");
            m_elementStack.Push(m_element);
            m_element = element;

            var origin = value.Origin; CodeLong(ref origin);

            m_element = m_elementStack.Pop();
            m_element.Add(element);

            element = new XElement("Length");
            m_elementStack.Push(m_element);
            m_element = element;

            var size = value.Size; Code__tsn__(ref size);

            m_element = m_elementStack.Pop();
            m_element.Add(element);

            element = new XElement("Delta");
            m_elementStack.Push(m_element);
            m_element = element;

            var delta = value.Delta; Code__tsn__(ref delta);

            m_element = m_elementStack.Pop();
            m_element.Add(element);

            m_element = m_elementStack.Pop();
            AddValue(item);
        }

        //#     });
        //# });
        #endregion

        #region Arrays

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] v) { CodeArrayOfStruct(v); }
        //# });

        #endregion

        #region Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[,]");
        public void Code__name2d__(ref __t__[,] v) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[,,]");
        public void Code__name3d__(ref __t__[, ,] v) { throw new NotImplementedException(); }
        //# });

        #endregion

        #region Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[][]");
        public void Code__name2d__(ref __t__[][] v) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[][][]");
        public void Code__name3d__(ref __t__[][][] v) { throw new NotImplementedException(); }
        //# });

        #endregion

        #region Lists

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> v) { CodeListOfStruct(v); }
        //# });

        #endregion

        #region Arrays of Tensors

        //# tensorTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] v) { CodeArrayOf(v); }
        //# });

        #endregion

        #region Lists of Tensors

        //# tensorTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> v) { CodeListOf(v); }
        //# });

        #endregion
    }
    //# }
}