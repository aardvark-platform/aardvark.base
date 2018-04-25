using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public partial struct Ellipse2d : IValidity
    {
        public bool IsValid => true;
        public bool IsInvalid => false;

        public double Area => Fun.Abs(Axis0.X * Axis1.Y - Axis0.Y * Axis1.X) * Constant.Pi;

        static double GetArea(V2d axis0, V2d axis1) => Fun.Abs(axis0.X * axis1.Y - axis0.Y * axis1.X) * Constant.Pi;
    }
}
