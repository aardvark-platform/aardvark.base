using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public partial struct Ellipse3d : IValidity
    {
        public static readonly Ellipse3d Invalid = new Ellipse3d(V3d.NaN, V3d.Zero, V3d.NaN, V3d.NaN);

        public bool IsValid { get { return Normal != V3d.Zero; } }
        public bool IsInvalid { get { return Normal == V3d.Zero; } }

        public double Area
        {
            get
            {
                return V3d.Cross(Axis0, Axis1).Length * Constant.Pi;
            }
        }
    }
}
