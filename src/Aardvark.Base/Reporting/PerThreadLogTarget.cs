using System;

namespace Aardvark.Base
{
    public class PerThreadLogTarget : ILogTarget
    {
        private volatile ILogTarget[] m_targetArray;
        Func<int, ILogTarget> m_targetCreator;

        #region Constructor

        public PerThreadLogTarget(Func<int, ILogTarget> targetCreator)
        {
            m_targetArray = null;
            m_targetCreator = targetCreator;
        }

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
                if (m_targetArray != null)
                    m_targetArray[i].Dispose();
        }

        #endregion
    }
}
