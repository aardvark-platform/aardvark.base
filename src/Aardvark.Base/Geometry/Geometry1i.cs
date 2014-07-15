using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region Line1i

    public partial struct Line1i
    {
        #region Static Creator

        public static Line1i CreateSorted(int i0, int i1)
        {
            return i0 < i1 ? new Line1i(i0, i1) : new Line1i(i1, i0);
        }

        #endregion

        #region Properties

        public Line1i Reversed { get { return new Line1i(I1, I0); } }

        #endregion
    }

    #endregion

    #region Triangle1i

    public partial struct Triangle1i
    {
        #region Static Creator

        public static Triangle1i CreateSorted(int i0, int i1, int i2)
        {
            if (i0 < i1)
            {
                if (i1 < i2)
                    return new Triangle1i(i0, i1, i2);
                else
                {
                    if (i0 < i2)
                        return new Triangle1i(i0, i2, i1);
                    else
                        return new Triangle1i(i2, i0, i1);
                }
            }
            else
            {
                if (i2 < i1)
                    return new Triangle1i(i2, i1, i0);
                else
                {
                    if (i0 < i2)
                        return new Triangle1i(i1, i0, i2);
                    else
                        return new Triangle1i(i1, i2, i0);
                }
            }
        }

        #endregion

        #region Properties

        public Triangle1i Reversed { get { return new Triangle1i(I2, I1, I0); } }

        #endregion
    }

    #endregion

    #region Quad1i

    public partial struct Quad1i
    {
        #region Properties

        public Quad1i Reversed { get { return new Quad1i(I3, I2, I1, I0); } }

        #endregion
    }

    #endregion

    #region WeightedIndex

    /// <summary>
    /// A structure holding a double weight and an index.
    /// </summary>
    public struct WeightedIndex
    {
        public double Weight;
        public int Index;

        #region Constructor

        public WeightedIndex(double weight, int index)
        {
            Weight = weight;
            Index = index;
        }

        #endregion
    }

    #endregion

    #region Line1iPoint Class

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

    #endregion

}
