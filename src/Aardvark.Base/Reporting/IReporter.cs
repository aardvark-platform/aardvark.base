namespace Aardvark.Base
{
    /// <summary>
    /// The IReporter interface can be implemented for objects to catch
    /// reports on a level, where some info is passed with typed parameters
    /// (e.g. timing seconds as doubles, values as objects).
    /// As soon as a reporter object is added, all Report calls from the
    /// same thread are also delivered to that reporter object. Reporting
    /// on other threads, where the reporter object was not added, does not
    /// appear. If the same reporter object is added multiple times from
    /// different threads, all these methods need to do their own locking.
    /// If only one reporter object is added from one thread, no locking is
    /// necessary. The supplied threadIndex, is a running number for each
    /// thread assigned based on their first sending of a Report message.
    /// It can be directly used as a key in e.g. an IntDict.
    /// </summary>
    public interface IReporter
    {
        void Line(int threadIndex, LogType type, int level, ILogTarget target,
                  string leftText, int rightPos = 0, string rightText = null);
        // the following methods are regarded as LogType.Info
        void Text(int threadIndex, int level, ILogTarget target, string text);
        void Wrap(int threadIndex, int level, ILogTarget target, string text);
        void Begin(int threadIndex, int level, ILogTarget target, string text, bool timed);
        void End(int threadIndex, int level, ILogTarget target, string text, double seconds);
        void Progress(int threadIndex, int level, ILogTarget target, string text, double progress, double seconds);
        void Values(int threadIndex, int level, ILogTarget target, string name, string separator, object[] values);
    }
}
