using System;
using System.Diagnostics;

namespace Aardvark.Base
{
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
            TestInfo = default(TestInfo);
                        
            if (timed)
                StartTime = Stopwatch.GetTimestamp();
        }

        public void Dispose() { if (!Disposed) Report.End(Level); }
    }
}
