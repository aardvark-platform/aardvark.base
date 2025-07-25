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

/// <summary>
/// A filtering log target that only records messages for which the
/// filter function supplied in the constructor returns true.
/// </summary>
public class FilterLogTarget(ILogTarget target, Func<LogMsg, bool> filterFun) : ILogTarget
{
    private readonly ILogTarget m_target = target;
    private readonly Func<LogMsg, bool> m_filterFun = filterFun;

    public void NewThreadIndex(int threadIndex)
    {
        m_target.NewThreadIndex(threadIndex);
    }

    public void Log(int threadIndex, LogMsg msg)
    {
        if (m_filterFun(msg))
            m_target.Log(threadIndex, msg);
    }

    public void Dispose()
    {
        m_target.Dispose();
        GC.SuppressFinalize(this);
    }
}
