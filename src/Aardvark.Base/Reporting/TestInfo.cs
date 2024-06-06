namespace Aardvark.Base
{
    /// <summary>
    /// A container for passed and failed counts.
    /// </summary>
    public struct TestInfo
    {
        public long PassedCount;
        public long FailedCount;

        #region Constructor

        public TestInfo(long passed, long failed)
        {
            PassedCount = passed; FailedCount = failed;
        }

        #endregion

        #region Operations

        public readonly long TestCount { get { return PassedCount + FailedCount; } }

        public static TestInfo operator +(TestInfo a, TestInfo b)
        {
            return new TestInfo(a.PassedCount + b.PassedCount,
                                a.FailedCount + b.FailedCount);
        }

        public static readonly TestInfo Empty = new TestInfo(0, 0);

        #endregion
    }
}
