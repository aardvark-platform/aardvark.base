using System;

namespace Aardvark.Base
{
    /// <summary>
    /// </summary>
    public struct TimeValue
    {
        /// <summary>
        /// </summary>
        public readonly DateTime T;

        /// <summary>
        /// </summary>
        public readonly double Delta;

        #region Constructors

        /// <summary>
        /// </summary>
        public TimeValue(DateTime t, double delta)
        {
            T = t;
            Delta = delta;
        }

        #endregion

        #region Creators

        /// <summary>
        /// </summary>
        public static TimeValue Now
        {
            get { return new TimeValue(DateTime.Now, 0.0); }
        }

        #endregion
    }
}
