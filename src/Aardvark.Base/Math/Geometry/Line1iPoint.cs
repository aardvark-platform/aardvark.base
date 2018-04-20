namespace Aardvark.Base
{
    /// <summary>
    /// Represents a value at an interpolated point between two indexed
    /// values of an indexable set of values. This is implemented as a
    /// class in order to avoid duplicating interpolated points in some
    /// algorithms.
    /// </summary>
    public class Line1iPoint
    {
        public readonly Line1i Line;
        public readonly double T;

        #region Constructor

        public Line1iPoint(
            int i0, int i1, double t)
        {
            Line.I0 = i0; Line.I1 = i1; T = t;
        }

        #endregion
    }
}
