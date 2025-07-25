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
using System;

namespace Aardvark.Base;

public class PerThreadLogTarget(Func<int, ILogTarget> targetCreator) : ILogTarget
{
    private volatile ILogTarget[] m_targetArray = null;
    readonly Func<int, ILogTarget> m_targetCreator = targetCreator;

    #region Constructor

    #endregion

    #region ILogTarget

    public void NewThreadIndex(int threadIndex)
    {
        m_targetArray = m_targetArray.With(threadIndex, m_targetCreator(threadIndex));
    }

    public void Log(int threadIndex, LogMsg msg)
    {
        m_targetArray[threadIndex].Log(threadIndex, msg);
    }

    public void Dispose()
    {
        for (int i = 0; i < m_targetArray.Length; i++)
            m_targetArray?[i].Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion
}
