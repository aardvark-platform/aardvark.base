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
