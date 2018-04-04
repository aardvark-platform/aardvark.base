using System;

namespace Aardvark.Base
{
    /// <summary>
    /// A filtering log target that only records messages for which the
    /// filter function supplied in the constructor returns true.
    /// </summary>
    public class FilterLogTarget : ILogTarget
    {
        private ILogTarget m_target;
        private Func<LogMsg, bool> m_filterFun;

        public FilterLogTarget(ILogTarget target, Func<LogMsg, bool> filterFun)
        {
            m_target = target;
            m_filterFun = filterFun;
        }

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
        }
    }
}
