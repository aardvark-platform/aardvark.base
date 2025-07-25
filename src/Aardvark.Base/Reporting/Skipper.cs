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
/// This small struct helps to optimize the number of calls to Report.Progress.
/// If you initialize it with a value of N, each Nth call of <see cref="Skipper.Do"/>
/// will return true.
/// </summary>
/// <remarks>
/// Initialize a skipper that results true every Nth call to Do.
/// </remarks>
/// <param name="limit">N</param>
public struct Skipper(int limit)
{
    int m_count = limit;
    readonly int m_limit = limit;

    #region Constructor

    #endregion

    #region Do

    public readonly bool HasDone
    {
        get { return m_count == m_limit; }
    }

    public bool Do
    {
        get
        {
            if (--m_count > 0) return false;
            m_count = m_limit;
            return true;
        }
    }

    #endregion
}
