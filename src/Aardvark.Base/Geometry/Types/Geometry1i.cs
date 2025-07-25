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
namespace Aardvark.Base;

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

    public readonly Line1i Reversed { get { return new Line1i(I1, I0); } }

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

    public readonly Triangle1i Reversed { get { return new Triangle1i(I2, I1, I0); } }

    #endregion
}

#endregion

#region Quad1i

public partial struct Quad1i
{
    #region Properties

    public readonly Quad1i Reversed { get { return new Quad1i(I3, I2, I1, I0); } }

    #endregion
}

#endregion
