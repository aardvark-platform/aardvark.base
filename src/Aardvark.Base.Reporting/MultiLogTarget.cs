namespace Aardvark.Base
{
    public class MultiLogTarget : ILogTarget
    {
        private volatile ILogTarget[] m_targetArray;
        private object m_targetArrayLock;

        #region Constructor

        public MultiLogTarget(params ILogTarget[] targetArray)
        {
            m_targetArray = targetArray;
            m_targetArrayLock = new object();
        }

        #endregion

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
}
