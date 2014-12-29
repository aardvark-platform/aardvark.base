using System;

namespace Aardvark.Base
{
    /// <summary>
    /// </summary>
    public struct Time
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
        public Time(DateTime t, double delta)
        {
            T = t;
            Delta = delta;
        }

        #endregion

        #region Creators

        /// <summary>
        /// </summary>
        public static Time Now
        {
            get { return new Time(DateTime.Now, 0.0); }
        }

        #endregion
    }
}
