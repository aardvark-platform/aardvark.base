using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static partial class Numerics
    {
        public static Matrix<double> MutliplyBy(this Matrix<double> m0, Matrix<double> m1)
        {
            if (m0.SX != m1.SY)
                throw new ArgumentException("m0.SX != m1.SY");

            var result = new Matrix<double>(m1.SX, m0.SY);

            var data = result.Data; var data0 = m0.Data; var data1 = m1.Data;
            long i = result.FirstIndex, yj = result.JY, my0 = m0.DY;
            long xs = result.DSX, mf1 = m1.FirstIndex, xj = result.JX, mx1 = m1.DX;
            long ds0 = m0.DSX, d0 = m0.DX, d1 = m1.DY;
            for (long ye = i + result.DSY, f0 = m0.FirstIndex, e0 = f0 + ds0;
                 i != ye; i += yj, f0 += my0, e0 += my0)
                for (long xe = i + xs, f1 = mf1; i != xe; i += xj, f1 += mx1)
                {
                    double dot = 0.0;
                    for (long i0 = f0, i1 = f1; i0 != e0; i0 += d0, i1 += d1)
                        dot += data0[i0] * data1[i1];
                    data[i] = dot;
                }

            return result;
        }
    }
}
