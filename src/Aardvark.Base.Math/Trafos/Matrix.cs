
namespace Aardvark.Base
{
    #region M22i

    public partial struct M22i
    {
    }

    #endregion

    #region M22l

    public partial struct M22l
    {
    }

    #endregion

    #region M22f

    public partial struct M22f
    {
    }

    #endregion

    #region M22d

    public partial struct M22d
    {
    }

    #endregion

    #region M23i

    public partial struct M23i
    {
    }

    #endregion

    #region M23l

    public partial struct M23l
    {
    }

    #endregion

    #region M23f

    public partial struct M23f
    {
    }

    #endregion

    #region M23d

    public partial struct M23d
    {
    }

    #endregion

    #region M33i

    public partial struct M33i
    {
    }

    #endregion

    #region M33l

    public partial struct M33l
    {
    }

    #endregion

    #region M33f

    public partial struct M33f
    {
    }

    #endregion

    #region M33d

    public partial struct M33d
    {
    }

    #endregion

    #region M34i

    public partial struct M34i
    {
    }

    #endregion

    #region M34l

    public partial struct M34l
    {
    }

    #endregion

    #region M34f

    public partial struct M34f
    {
    }

    #endregion

    #region M34d

    public partial struct M34d
    {
        public M34d(M33d r, V3d t)
        {
            M00 = r.M00; M01 = r.M01; M02 = r.M02; M03 = t.X;
            M10 = r.M10; M11 = r.M11; M12 = r.M12; M13 = t.Y;
            M20 = r.M20; M21 = r.M21; M22 = r.M22; M23 = t.Z;
        }
    }

    #endregion

    #region M44i

    public partial struct M44i
    {
    }

    #endregion

    #region M44l

    public partial struct M44l
    {
    }

    #endregion

    #region M44f

    public partial struct M44f
    {
    }

    #endregion

    #region M44d

    public partial struct M44d
    {
    }

    #endregion
}