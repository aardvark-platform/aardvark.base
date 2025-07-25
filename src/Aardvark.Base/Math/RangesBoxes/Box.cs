/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System.Runtime.Serialization;

namespace Aardvark.Base;

#region OctoBox2d

[DataContract]
public struct OctoBox2d(
    double px, double py, double nx, double ny,
    double pxpy, double pxny, double nxpy, double nxny
    )
{
    [DataMember]
    public double PX = px, PY = py, NX = nx, NY = ny, PXPY = pxpy, PXNY = pxny, NXPY = nxpy, NXNY = nxny;

    #region Constants

    public static readonly OctoBox2d Invalid =
            new(double.MinValue, double.MinValue, double.MinValue, double.MinValue,
                double.MinValue, double.MinValue, double.MinValue, double.MinValue);

    public static readonly OctoBox2d Zero =
            new(0, 0, 0, 0, 0, 0, 0, 0);

    #endregion

    #region Properties

    public readonly double Area
    {
        get
        {
            return (PX + NX) * (PY + NY)
                    - 0.5 * (Fun.Square(PX + PY - PXPY) + Fun.Square(PX + NY - PXNY)
                             + Fun.Square(NX + PY - NXPY) + Fun.Square(NX + NY - NXNY));
        }
    }

    #endregion

    #region Manipulation

    public void ExtendBy(V2d p)
    {
        ExtendBy(p.X, p.Y);
    }

    public void ExtendBy(double x, double y)
    {
        if (x > PX) PX = x;
        if (y > PY) PY = y;
        var nx = -x; if (nx > NX) NX = nx;
        var ny = -y; if (ny > NY) NY = ny;
        var pxpy = x + y; if (pxpy > PXPY) PXPY = pxpy;
        var pxny = x - y; if (pxny > PXNY) PXNY = pxny;
        var nxpy = -x + y; if (nxpy > NXPY) NXPY = nxpy;
        var nxny = -x - y; if (nxny > NXNY) NXNY = nxny;
    }

    #endregion
}

#endregion
