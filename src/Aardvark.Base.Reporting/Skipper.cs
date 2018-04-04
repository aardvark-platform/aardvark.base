namespace Aardvark.Base
{
    /// <summary>
    /// This small struct helps to optimize the number of calls to Report.Progress.
    /// If you initialize it with a value of N, each Nth call of <see cref="Skipper.Do"/>
    /// will return true.
    /// </summary>
    public struct Skipper
    {
        int m_count;
        int m_limit;

        #region Constructor

        /// <summary>
        /// Initialize a skipper that results true every Nth call to Do.
        /// </summary>
        /// <param name="limit">N</param>
        public Skipper(int limit)
        {
            m_count = limit;
            m_limit = limit;
        }

        #endregion

        #region Do

        public bool HasDone
        {
            get { return m_count == m_limit; }
        }

        public bool Do
        {
            get
            {
                if (--m_count > 0) return false;
                m_count = m_limit;
                return true;
            }
        }

        #endregion
    }
}
