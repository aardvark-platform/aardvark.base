using System;
using System.Collections.Generic;
using System.Globalization;

namespace Aardvark.Base
{
    public class JobReporter : IJobReporter
    {
        private readonly int m_ti;
        public int m_indent;
        private ReportJob m_job;
        private readonly Stack<ReportJob> m_jobStack;
        private volatile IReporter[] m_reporterArray;
        private readonly object m_reporterArrayLock;

        #region Constructor

        public JobReporter(int threadIndex = 0)
        {
            m_ti = threadIndex; m_indent = 2;
            m_job = new ReportJob("THREAD", 0, true);
            m_jobStack = new Stack<ReportJob>();
            m_reporterArray = null;
            m_reporterArrayLock = new object();
        }

        #endregion

        #region Properties

        public int Indent
        {
            get { return m_indent; }
            set { m_indent = value; }
        }

        #endregion

        #region IJobReporter

        public void Line(LogType type, int level, ILogTarget target, string t0, int p1 = 0, string t1 = null)
        {
            target.Log(m_ti, new LogMsg(type, LogOpt.EndLine, level, m_job.HierarchyLevel * m_indent, t0, p1, t1));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Line(m_ti, type, level, target, t0, p1, t1);
        }

        public void Wrap(int level, ILogTarget target, string text)
        {
            target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.Join | LogOpt.Wrap, level,
                                            m_job.HierarchyLevel * m_indent, text, -2));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Wrap(m_ti, level, target, text);
        }

        public void Text(int level, ILogTarget target, string text)
        {
            string[] lines = text.Split('\n');
            var last = lines.Length - 1;
            if (last == 0)
            {
                target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.Join, level,
                                             m_job.HierarchyLevel * m_indent, lines[0], -2));
            }
            else
            {
                for (int i = 0; i < last; i++)
                    target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.Join | LogOpt.EndLine, level,
                                                 m_job.HierarchyLevel * m_indent, lines[i]));
                if (lines[last].Length > 0)
                    target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.Join, level,
                                                m_job.HierarchyLevel * m_indent, lines[last]));
            }
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Text(m_ti, level, target, text);
        }

        public ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false)
        {
            if (parentJob == null) parentJob = m_job;
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Begin(m_ti, level, target, text, timed);
            var opt = timed ? LogOpt.Timed : LogOpt.None;
            if (!noLog)
                target.Log(m_ti, new LogMsg(LogType.Begin, opt, level, parentJob.HierarchyLevel * m_indent, text, -2));
            m_jobStack.Push(m_job);
            m_job = new ReportJob(text, level, timed, parentJob);
            return m_job;
        }

        public void Tests(TestInfo testInfo)
        {
            m_job.TestInfo += testInfo;
            m_job.ReportTests = true;
        }

        public double End(int level, ILogTarget target, string text, bool addTimeToParent)
        {
            var parentJob = m_job.ParentJob;
            if (m_jobStack.Count == 0)
            {
                Report.Warn("superfluous Report.End() encountered");
                if (text != null) // still report message if some
                    target.Log(m_ti, new LogMsg(LogType.End, LogOpt.NewText, level, 0,
                                                text, -2, null));
                return 0.0;
            }
            double seconds; string time; LogOpt opt;
            if (m_job.IsTimed)
            {
                seconds = m_job.ElapsedSeconds; opt = LogOpt.EndLine | LogOpt.Timed;
                time = String.Format(CultureInfo.InvariantCulture, "{0:F3} s", seconds);
            }
            else
            {
                seconds = 0.0; time = null; opt = LogOpt.EndLine;
            }
            m_job.Disposed = true;
            if (level != m_job.Level)
            {
                Report.Warn("Report.Begin({0}) level different from Report.End({1}),"
                            + " using Report.End({0})", m_job.Level, level);
                level = m_job.Level;
            }
            var beginText = m_job.Message;
            if (text == null) text = beginText;
            else if (text != beginText) { text = beginText + text; opt |= LogOpt.NewText; }
            if (m_job.ReportTests == true)
            {
                var testInfo = m_job.TestInfo;
                m_job = m_jobStack.Pop(); m_job.TestInfo += testInfo;
                var passed = String.Format(CultureInfo.InvariantCulture,
                             "[{0}/{1} OK]", testInfo.PassedCount, testInfo.TestCount);
                time = time != null ? passed + ' ' + time : passed;
                target.Log(m_ti, new LogMsg(LogType.End, opt, level, parentJob.HierarchyLevel * m_indent,
                                            text, -2, time));
                if (testInfo.FailedCount > 0)
                {
                    var failed = String.Format(CultureInfo.InvariantCulture,
                                               " {0}/{1} FAILED", testInfo.FailedCount, testInfo.TestCount);
                    target.Log(m_ti, new LogMsg(LogType.Warn, opt, level, parentJob.HierarchyLevel * m_indent,
                                                    "WARNING: " + text + failed));
                }
            }
            else
            {
                var childrenTime = m_job.ChildrenTime;
                if (seconds > 0.0 && childrenTime > 0.0)
                    time = String.Format(CultureInfo.InvariantCulture, "[{0:F2}x] ", childrenTime / seconds) + time;
                if (addTimeToParent)
                {
                    lock (parentJob) parentJob.ChildrenTime += seconds;
                }
                m_job = m_jobStack.Pop();
                target.Log(m_ti, new LogMsg(LogType.End, opt, level, parentJob.HierarchyLevel * m_indent,
                                            text, -2, time));
            }
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 1; i < reporterArray.Length; i++)
                    reporterArray[i].End(m_ti, level, target, text, seconds);
            return seconds;
        }

        public void Progress(int level, ILogTarget target, double progress, bool relative = false)
        {
            if (relative) progress += m_job.Progress;
            if (progress > 1.0) progress = 1.0;
            m_job.Progress = progress;
            double seconds; string text; LogOpt opt;
            if (!m_job.IsTimed)
            {
                seconds = -1.0; opt = LogOpt.None;
                text = String.Format(CultureInfo.InvariantCulture, "{0,6:F2}%", 100.0 * progress);
            }
            else
            {
                seconds = m_job.ElapsedSeconds; opt = LogOpt.Timed;
                text = String.Format(CultureInfo.InvariantCulture, "{0,6:F2}% {1,10:F3} s", 100.0 * progress, seconds);
            }
            target.Log(m_ti, new LogMsg(LogType.Progress, opt, level,
                                        (m_job.HierarchyLevel - 1) * m_indent, m_job.Message, -2, text));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Progress(m_ti, level, target, m_job.Message, progress, seconds);
        }

        public void Values(int level, ILogTarget target, string name, string separator, object[] values)
        {
            var text = values.Length == 1 ? values[0].ToString() : values.Map(o => o.ToString()).Join(separator);
            target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.EndLine, level,
                                        m_job.HierarchyLevel * m_indent, name, 40, text));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Values(m_ti, level, target, name, separator, values);
        }

        #endregion

        #region Adding and Removing IReporters

        public void AddReporter(IReporter reporter)
        {
            lock (m_reporterArrayLock)
            {
                m_reporterArray = m_reporterArray.WithAppended(reporter);
            }
        }

        public void RemoveReporter(IReporter reporter)
        {
            lock (m_reporterArrayLock)
            {
                m_reporterArray = m_reporterArray.WithRemoved(reporter);
            }
        }

        #endregion

    }
}
