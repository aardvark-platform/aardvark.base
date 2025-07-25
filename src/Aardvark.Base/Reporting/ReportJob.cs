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
using System.Diagnostics;

namespace Aardvark.Base;

public class ReportJob : IDisposable
{
    internal string Message;
    public readonly int HierarchyLevel;
    public readonly ReportJob ParentJob;
    public double ChildrenTime;
    internal int Level;
    internal long StartTime = -1;
    internal double Progress;
    internal bool ReportTests;
    internal TestInfo TestInfo;
    internal bool Disposed;

    internal bool IsTimed => StartTime >= 0;

#if NET8_0_OR_GREATER
    internal double ElapsedSeconds => Stopwatch.GetElapsedTime(StartTime).TotalSeconds;
#else
    internal double ElapsedSeconds => new TimeSpan((Stopwatch.GetTimestamp() - StartTime) * TimeSpan.TicksPerSecond / Stopwatch.Frequency).TotalSeconds;
#endif

    public ReportJob(string message, int level, bool timed, ReportJob parentJob = null)
    {
        Disposed = false;
        Message = message;
        ParentJob = parentJob;
        ChildrenTime = 0.0;
        HierarchyLevel = parentJob == null ? 0 : parentJob.HierarchyLevel + 1;
        Level = level;
        Progress = -1.0;
        ReportTests = false;
        TestInfo = default;
                    
        if (timed)
            StartTime = Stopwatch.GetTimestamp();
    }

    public void Dispose() 
    {
        if (!Disposed)
        {
            Report.End(Level);
            GC.SuppressFinalize(this);
        }
    }
}
