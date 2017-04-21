namespace Aardvark.Base
{
    public class NullReporter : IJobReporter
    {
        public int Indent
        {
            get { return 0; }
            set { }
        }

        public void AddReporter(IReporter reporter)
        {
        }

        public ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false)
        {
            return null;
        }

        public double End(int level, ILogTarget target, string text, bool addTimeToParent)
        {
            return 0.0;
        }

        public void Line(LogType type, int level, ILogTarget target, string leftText, int rightPos = 0, string rightText = null)
        {
        }

        public void Progress(int level, ILogTarget target, double progress, bool relative = false)
        {
        }

        public void RemoveReporter(IReporter reporter)
        {
        }

        public void Tests(TestInfo testInfo)
        {
        }

        public void Text(int level, ILogTarget target, string text)
        {
        }

        public void Values(int level, ILogTarget target, string name, string separator, object[] values)
        {
        }

        public void Wrap(int level, ILogTarget target, string text)
        {
        }
    }
}
