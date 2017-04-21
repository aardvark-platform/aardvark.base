using System.Threading;

namespace Aardvark.Base
{
    public class PerThreadJobReporter : IJobReporter
    {
        private SpinLock m_lock;
        private volatile IntDict<IJobReporter> m_reporterMap;
        private volatile int m_threadCount;
        private int m_indent;

        #region Constructor

        public PerThreadJobReporter()
        {
            m_reporterMap = new IntDict<IJobReporter>();
            m_threadCount = 0; m_indent = 2;
        }

        #endregion

        #region Thread Handling

        private IJobReporter CurrentReporter(ILogTarget target = null)
        {
            IJobReporter reporter;
            var threadId = Thread.CurrentThread.ManagedThreadId;
            bool lockTaken = false;
            try
            {
                m_lock.Enter(ref lockTaken);
                if (!m_reporterMap.TryGetValue(threadId, out reporter))
                {
                    var threadIndex = m_threadCount++;
                    reporter = new JobReporter(threadIndex) { Indent = m_indent };
                    m_reporterMap[threadId] = reporter;
                    if (target != null) target.NewThreadIndex(threadIndex);
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
}
