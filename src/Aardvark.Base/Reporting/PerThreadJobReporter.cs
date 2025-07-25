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
using System.Collections.Generic;
using System.Threading;

namespace Aardvark.Base;

public class PerThreadJobReporter : IJobReporter
{
    private SpinLock m_lock;
    private readonly Dictionary<int, IJobReporter> m_reporterMap;
    private volatile int m_threadCount;
    private int m_indent;

    #region Constructor

    public PerThreadJobReporter()
    {
        m_reporterMap = [];
        m_threadCount = 0; m_indent = 2;
    }

    #endregion

    #region Thread Handling

    private IJobReporter CurrentReporter(ILogTarget target = null)
    {
        IJobReporter reporter;
        var threadId = System.Environment.CurrentManagedThreadId;
        bool lockTaken = false;
        try
        {
            m_lock.Enter(ref lockTaken);
            if (!m_reporterMap.TryGetValue(threadId, out reporter))
            {
                var threadIndex = m_threadCount++;
                reporter = new JobReporter(threadIndex) { Indent = m_indent };
                m_reporterMap[threadId] = reporter;
                target?.NewThreadIndex(threadIndex);
            }
        }
        finally
        {
            if (lockTaken) m_lock.Exit(true);
        }
        return reporter;
    }

    #endregion

    #region IJobReporter

    public int Indent
    {
        get { return m_indent; }
        set { m_indent = value; }
    }

    public void Line(LogType type, int level, ILogTarget target, string text0, int pos1 = 0, string text1 = null)
    {
        CurrentReporter(target).Line(type, level, target, text0, pos1, text1);
    }

    public void Text(int level, ILogTarget target, string text)
    {
        CurrentReporter(target).Text(level, target, text);
    }

    public void Wrap(int level, ILogTarget target, string text)
    {
        CurrentReporter(target).Wrap(level, target, text);
    }

    public ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false)
    {
        return CurrentReporter(target).Begin(parentJob, level, target, text, timed, noLog);
    }

    public double End(int level, ILogTarget target, string text, bool addTimeToParent)
    {
        return CurrentReporter(target).End(level, target, text, addTimeToParent);
    }

    public void Tests(TestInfo testInfo)
    {
        CurrentReporter().Tests(testInfo);
    }

    public void Progress(int level, ILogTarget target, double progress, bool relative = false)
    {
        CurrentReporter(target).Progress(level, target, progress, relative);
    }

    public void Values(int level, ILogTarget target, string name, string separator, object[] values)
    {
        CurrentReporter(target).Values(level, target, name, separator, values);
    }

    public void AddReporter(IReporter reporter)
    {
        CurrentReporter().AddReporter(reporter);
    }

    public void RemoveReporter(IReporter reporter)
    {
        CurrentReporter().RemoveReporter(reporter);
    }

    #endregion
}
