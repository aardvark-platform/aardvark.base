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

public class MultiLogTarget(params ILogTarget[] targetArray) : ILogTarget
{
    private volatile ILogTarget[] m_targetArray = targetArray;
    private readonly object m_targetArrayLock = new();

    #region ILogTarget

    public void NewThreadIndex(int threadIndex)
    {
        var targetArray = m_targetArray;
        for (int i = 0; i < targetArray.Length; i++)
            targetArray[i].NewThreadIndex(threadIndex);
    }

    public void Log(int threadIndex, LogMsg msg)
    {
        var targetArray = m_targetArray;
        for (int i = 0; i < targetArray.Length; i++)
            targetArray[i].Log(threadIndex, msg);
    }

    public void Dispose()
    {
        var targetArray = m_targetArray;
        for (int i = 0; i < targetArray.Length; i++)
            targetArray[i].Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Adding and Removing Targets

    public void Add(ILogTarget target)
    {
        lock (m_targetArrayLock)
        {
            m_targetArray = m_targetArray.WithAppended(target);
        }
    }

    public void Remove(ILogTarget target)
    {
        lock (m_targetArrayLock)
        {
            m_targetArray = m_targetArray.WithRemoved(target);
        }
    }

    #endregion
}
