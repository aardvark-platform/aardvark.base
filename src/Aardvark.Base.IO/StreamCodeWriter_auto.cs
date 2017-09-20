namespace Aardvark.Base.Coder
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    public partial class StreamCodeWriter
    {

        #region Vectors

        public void Write(V2i x)
        {
            Write(x.X); Write(x.Y); 
        }

        public void Write(V2l x)
        {
            Write(x.X); Write(x.Y); 
        }

        public void Write(V2f x)
        {
            Write(x.X); Write(x.Y); 
        }

        public void Write(V2d x)
        {
            Write(x.X); Write(x.Y); 
        }

        public void Write(V3i x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); 
        }

        public void Write(V3l x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); 
        }

        public void Write(V3f x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); 
        }

        public void Write(V3d x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); 
        }

        public void Write(V4i x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); Write(x.W); 
        }

        public void Write(V4l x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); Write(x.W); 
        }

        public void Write(V4f x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); Write(x.W); 
        }

        public void Write(V4d x)
        {
            Write(x.X); Write(x.Y); Write(x.Z); Write(x.W); 
        }

        #endregion

        #region Matrices

        public void Write(M22i x)
        {
            Write(x.M00); Write(x.M01); 
            Write(x.M10); Write(x.M11); 
        }

        public void Write(M22l x)
        {
            Write(x.M00); Write(x.M01); 
            Write(x.M10); Write(x.M11); 
        }

        public void Write(M22f x)
        {
            Write(x.M00); Write(x.M01); 
            Write(x.M10); Write(x.M11); 
        }

        public void Write(M22d x)
        {
            Write(x.M00); Write(x.M01); 
            Write(x.M10); Write(x.M11); 
        }

        public void Write(M23i x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
        }

        public void Write(M23l x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
        }

        public void Write(M23f x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
        }

        public void Write(M23d x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
        }

        public void Write(M33i x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
            Write(x.M20); Write(x.M21); Write(x.M22); 
        }

        public void Write(M33l x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
            Write(x.M20); Write(x.M21); Write(x.M22); 
        }

        public void Write(M33f x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
            Write(x.M20); Write(x.M21); Write(x.M22); 
        }

        public void Write(M33d x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); 
            Write(x.M10); Write(x.M11); Write(x.M12); 
            Write(x.M20); Write(x.M21); Write(x.M22); 
        }

        public void Write(M34i x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
        }

        public void Write(M34l x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
        }

        public void Write(M34f x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
        }

        public void Write(M34d x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
        }

        public void Write(M44i x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
            Write(x.M30); Write(x.M31); Write(x.M32); Write(x.M33); 
        }

        public void Write(M44l x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
            Write(x.M30); Write(x.M31); Write(x.M32); Write(x.M33); 
        }

        public void Write(M44f x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
            Write(x.M30); Write(x.M31); Write(x.M32); Write(x.M33); 
        }

        public void Write(M44d x)
        {
            Write(x.M00); Write(x.M01); Write(x.M02); Write(x.M03); 
            Write(x.M10); Write(x.M11); Write(x.M12); Write(x.M13); 
            Write(x.M20); Write(x.M21); Write(x.M22); Write(x.M23); 
            Write(x.M30); Write(x.M31); Write(x.M32); Write(x.M33); 
        }

        #endregion

        #region Ranges and Boxes

        public void Write(Range1b x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1sb x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1s x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1us x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1i x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1ui x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1l x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1ul x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1f x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Range1d x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box2i x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box2l x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box2f x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box2d x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box3i x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box3l x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box3f x)
        {
            Write(x.Min); Write(x.Max);
        }

        public void Write(Box3d x)
        {
            Write(x.Min); Write(x.Max);
        }

        #endregion

        #region Colors

        public void Write(C3b c)
        {
            Write(c.R); Write(c.G); Write(c.B); 
        }

        public void Write(C3us c)
        {
            Write(c.R); Write(c.G); Write(c.B); 
        }

        public void Write(C3ui c)
        {
            Write(c.R); Write(c.G); Write(c.B); 
        }

        public void Write(C3f c)
        {
            Write(c.R); Write(c.G); Write(c.B); 
        }

        public void Write(C3d c)
        {
            Write(c.R); Write(c.G); Write(c.B); 
        }

        public void Write(C4b c)
        {
            Write(c.R); Write(c.G); Write(c.B); Write(c.A); 
        }

        public void Write(C4us c)
        {
            Write(c.R); Write(c.G); Write(c.B); Write(c.A); 
        }

        public void Write(C4ui c)
        {
            Write(c.R); Write(c.G); Write(c.B); Write(c.A); 
        }

        public void Write(C4f c)
        {
            Write(c.R); Write(c.G); Write(c.B); Write(c.A); 
        }

        public void Write(C4d c)
        {
            Write(c.R); Write(c.G); Write(c.B); Write(c.A); 
        }

        #endregion

    }
}
