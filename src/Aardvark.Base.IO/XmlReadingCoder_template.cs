using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Coder
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# var directlyCodeableTypes = Meta.DirectlyCodeableTypes.Map(t =>t.Name);
    //# var geometryTypes = Meta.GeometryTypes.Map(t => t.Name);
    //# var specialSimpleTypes = new[] { "bool", "char", "string", "Type", "Guid", "Symbol" };
    //# {
    //# var structTypes = new List<string>();
    //# var tensorTypes = new List<string>();
    public partial class NewXmlReadingCoder
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

        #region Geometry Types

        //# foreach (var isDouble in new[] { false, true }) {
        //#   var ftype = isDouble ? "Double" : "Float";
        //#   var tc = isDouble ? "d" : "f";
        public void CodeCircle2__tc__(ref Circle2__tc__ v) { throw new NotImplementedException(); }
        public void CodeLine2__tc__(ref Line2__tc__ v) { throw new NotImplementedException(); }
        public void CodeLine3__tc__(ref Line3__tc__ v) { throw new NotImplementedException(); }
        public void CodePlane2__tc__(ref Plane2__tc__ v) { throw new NotImplementedException(); }
        public void CodePlane3__tc__(ref Plane3__tc__ v) { throw new NotImplementedException(); }
        public void CodePlaneWithPoint3__tc__(ref PlaneWithPoint3__tc__ v) { throw new NotImplementedException(); }
        public void CodeQuad2__tc__(ref Quad2__tc__ v) { throw new NotImplementedException(); }
        public void CodeQuad3__tc__(ref Quad3__tc__ v) { throw new NotImplementedException(); }
        public void CodeRay2__tc__(ref Ray2__tc__ v) { throw new NotImplementedException(); }
        public void CodeRay3__tc__(ref Ray3__tc__ v) { throw new NotImplementedException(); }
        public void CodeSphere3__tc__(ref Sphere3__tc__ v) { throw new NotImplementedException(); }
        public void CodeTriangle2__tc__(ref Triangle2__tc__ v) { throw new NotImplementedException(); }
        public void CodeTriangle3__tc__(ref Triangle3__tc__ v) { throw new NotImplementedException(); }

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
        //# var genericTensorSizes = new[] { "long", "V2l", "V3l", "long[]" };
        //# genericTensorTypes.ForEach((tt, ti) => {
        //#     var ts = genericTensorSizes[ti];
        //#     simpleTypes.ForEach((t, i) => {
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
        public void Code__name__(ref __t__[] value)
        {
            throw new NotImplementedException();
        }

        //# });
        #endregion

        #region Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[,]");
        public void Code__name2d__(ref __t__[,] value) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[,,]");
        public void Code__name3d__(ref __t__[, ,] value) { throw new NotImplementedException(); }

        //# });
        #endregion

        #region Jagged Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[][]");
        public void Code__name2d__(ref __t__[][] value) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[][][]");
        public void Code__name3d__(ref __t__[][][] value) { throw new NotImplementedException(); }

        //# });
        #endregion

        #region Lists

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> value)
        {
            throw new NotImplementedException();
        }

        //# });
        #endregion

        #region Arrays of Tensors

        //# tensorTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] value)
        {
            throw new NotImplementedException();
        }

        //# });
        #endregion

        #region Lists of Tensors

        //# tensorTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> value)
        {
            throw new NotImplementedException();
        }

        //# });
        #endregion
    }

    //# } {
    //# var structTypes = new List<string>();
    //# var tensorTypes = new List<string>();
    public partial class XmlReadingCoder
    {
        #region Vectors

        //# foreach (var t in Meta.VecTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { v = __t.Name__.Parse(CurrentValue()); }
        //# }

        #endregion

        #region Matrices

        //# foreach (var t in Meta.MatTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { v = __t.Name__.Parse(CurrentValue()); }
        //# }

        #endregion

        #region Ranges and Boxes

        //# foreach (var t in Meta.RangeAndBoxTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { v = __t.Name__.Parse(CurrentValue()); }
        //# }

        #endregion

        #region Geometry Types

        //# foreach (var isDouble in new[] { false, true }) {
        //#   var ftype = isDouble ? "Double" : "Float";
        //#   var tc = isDouble ? "d" : "f";
        public void CodeCircle2__tc__(ref Circle2__tc__ v) { v = Circle2__tc__.Parse(CurrentValue()); }
        public void CodeLine2__tc__(ref Line2__tc__ v) { v = Line2__tc__.Parse(CurrentValue()); }
        public void CodeLine3__tc__(ref Line3__tc__ v) { v = Line3__tc__.Parse(CurrentValue()); }
        public void CodePlane2__tc__(ref Plane2__tc__ v) { v = Plane2__tc__.Parse(CurrentValue()); }
        public void CodePlane3__tc__(ref Plane3__tc__ v) { v = Plane3__tc__.Parse(CurrentValue()); }
        public void CodePlaneWithPoint3__tc__(ref PlaneWithPoint3__tc__ v) { v = PlaneWithPoint3__tc__.Parse(CurrentValue()); }
        public void CodeQuad2__tc__(ref Quad2__tc__ v) { v = Quad2__tc__.Parse(CurrentValue()); }
        public void CodeQuad3__tc__(ref Quad3__tc__ v) { v = Quad3__tc__.Parse(CurrentValue()); }
        public void CodeRay2__tc__(ref Ray2__tc__ v) { v = Ray2__tc__.Parse(CurrentValue()); }
        public void CodeRay3__tc__(ref Ray3__tc__ v) { v = Ray3__tc__.Parse(CurrentValue()); }
        public void CodeSphere3__tc__(ref Sphere3__tc__ v) { v = Sphere3__tc__.Parse(CurrentValue()); }
        public void CodeTriangle2__tc__(ref Triangle2__tc__ v) { v = Triangle2__tc__.Parse(CurrentValue()); }
        public void CodeTriangle3__tc__(ref Triangle3__tc__ v) { v = Triangle3__tc__.Parse(CurrentValue()); }

        //# }
        #endregion

        #region Colors

        //# foreach (var t in Meta.ColorTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { v = __t.Name__.Parse(CurrentValue()); }
        //# }

        #endregion

        #region Trafos

        //# foreach (var t in Meta.TrafoTypes) { structTypes.Add(t.Name);
        //# var name = Meta.GetXmlTypeName(t.Name);
        public void Code__name__(ref __t.Name__ v) { v = __t.Name__.Parse(CurrentValue()); }
        //# }

        #endregion

        #region Tensors

        //# foreach (var t in geometryTypes) structTypes.Add(t);
        //# var simpleTypes = directlyCodeableTypes.Concat(specialSimpleTypes).Concat(geometryTypes);
        //# var genericTensorTypes = new[] { "Vector", "Matrix", "Volume", "Tensor" };
        //# var genericTensorSizes = new[] { "long", "V2l", "V3l", "long[]" };
        //# genericTensorTypes.ForEach((tt, ti) => {
        //#     var ts = genericTensorSizes[ti]; var tsn = Meta.GetXmlTypeName(ts);
        //#     simpleTypes.ForEach((t, i) => {
        //#         var type = tt + "<" + t + ">";
        //#         tensorTypes.Add(type);
        //# var dname = Meta.GetXmlTypeName(t + "[]");
        //# var name = Meta.GetXmlTypeName(type);
        public void Code__name__(ref __type__ value)
        {
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            __t__[] data = null; Code__dname__(ref data);

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            long origin = 0L; CodeLong(ref origin);

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            __ts__ size = default(__ts__); Code__tsn__(ref size);

            m_state = m_stateStack.Pop();
            m_state.Advance();
            m_stateStack.Push(m_state);
            m_state.Enumerator = m_state.Element.Elements().GetEnumerator();
            m_state.Element = m_state.Enumerator.Current;

            __ts__ delta = default(__ts__); Code__tsn__(ref delta);

            m_state = m_stateStack.Pop();
            m_state = m_stateStack.Pop();

            value = new __type__(data, origin, size, delta);
        }

        //#     });
        //# });
        #endregion

        #region Arrays

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] value)
        {
            CodeArray(ref value, s_nestedBracketSplitter, s => __t__.Parse(s));
        }

        //# });
        #endregion

        #region Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[,]");
        public void Code__name2d__(ref __t__[,] value) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[,,]");
        public void Code__name3d__(ref __t__[, ,] value) { throw new NotImplementedException(); }

        //# });
        #endregion

        #region Jagged Multi-Dimensional Arrays

        //# directlyCodeableTypes.ForEach(t => {
        //# var name2d = Meta.GetXmlTypeName(t + "[][]");
        public void Code__name2d__(ref __t__[][] value) { throw new NotImplementedException(); }
        //# var name3d = Meta.GetXmlTypeName(t + "[][][]");
        public void Code__name3d__(ref __t__[][][] value) { throw new NotImplementedException(); }

        //# });
        #endregion

        #region Lists

        //# structTypes.ForEach(t => {
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> value)
        {
            CodeList(ref value, s_nestedBracketSplitter, s => __t__.Parse(s));
        }

        //# });
        #endregion

        #region Arrays of Tensors

        //# tensorTypes.ForEach(t => {
        //# var ename = Meta.GetXmlTypeName(t);
        //# var name = Meta.GetXmlTypeName(t + "[]");
        public void Code__name__(ref __t__[] value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                __t__ item = default(__t__); Code__ename__(ref item); value[i] = item;
                m_state.Advance();
            }
        }

        //# });
        #endregion

        #region Lists of Tensors

        //# tensorTypes.ForEach(t => {
        //# var ename = Meta.GetXmlTypeName(t);
        //# var name = Meta.GetXmlTypeName("List<" + t + ">");
        public void Code__name__(ref List<__t__> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                __t__ item = default(__t__); Code__ename__(ref item); value[i] = item;
                m_state.Advance();
            }
        }

        //# });
        #endregion
    }
    //# }
}
