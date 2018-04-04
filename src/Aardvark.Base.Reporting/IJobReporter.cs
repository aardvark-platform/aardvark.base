namespace Aardvark.Base
{
    public interface IJobReporter
    {
        /// <summary>
        /// The number of spaces of indent caused by Begin / Report.End.
        /// This property should only be set BEFORE actual reporting.
        /// </summary>
        int Indent { get; set; }
        void Line(LogType type, int level, ILogTarget target,
                  string leftText, int rightPos = 0, string rightText = null);
        // the following methods are regarded as LogType.Info
        void Text(int level, ILogTarget target, string text);
        void Wrap(int level, ILogTarget target, string text);
        ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false);
        double End(int level, ILogTarget target, string text, bool addTimeToParent);
        void Tests(TestInfo testInfo);
        void Progress(int level, ILogTarget target, double progress, bool relative = false);
        void Values(int level, ILogTarget target, string name, string separator, object[] values);
        // internal API for managing external reporters
        void AddReporter(IReporter reporter);
        void RemoveReporter(IReporter reporter);
    }
}
