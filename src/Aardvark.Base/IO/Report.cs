using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.ComponentModel;
#if __ANDROID__
using Log = Android.Util.Log;
#endif

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

        public long TestCount { get { return PassedCount + FailedCount; } }

        public static TestInfo operator +(TestInfo a, TestInfo b)
        {
            return new TestInfo(a.PassedCount + b.PassedCount,
                                a.FailedCount + b.FailedCount);
        }

        public static TestInfo Empty = new TestInfo(0, 0);

        #endregion
    }

    /// <summary>
    /// This class makes it possible to report messages with different
    /// verbosity levels to the console or a stream.
    /// 
    /// In the simplest case, you can start by reporting single line messages
    /// with a level Report.Line. The level gives an
    /// indication of how important this message is: a value of 0 indicates
    /// that this is a message of utmost importance that cannot be suppressed.
    /// The higher the level, the lower the importance of the message. In the
    /// default case only messages of level 0, 1 and 2 are actually reported.
    ///
    /// The following use of levels is suggested:
    /// 
    ///     0 ... unsupressable message of utmost importance (e.g warning),
    ///     1 ... very short output,
    ///     2 ... short output (this is the default level),
    ///     3 ... normal output,
    ///     4 ... detailed output,
    ///     5-9 ... debugging levels
    /// 
    /// The static Report class wraps all methods to a concrete reporter that
    /// performs the actual reporting. The default reporting setup consists
    /// of a console reporter at verbosity level 2, and a file reporter at
    /// reporting level 9 that writes it contents to the file
    /// "Aardvark.log". Each thread gets its own reporters, which report to
    /// the logs in a synchronized manner. In order to globally change the
    /// console report level, you can just conveniently use the static
    /// Verbosity property.
    /// </summary>
    public static class Report
    {
        #region
        /// <summary>
        /// Breaks and throws an exception on error if a debugger is attached.
        /// </summary>
        public static bool ThrowOnError = false;
        #endregion

        #region Telemetry

        public static Telemetry.Counter CountCallsToBeginTimed = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToBegin = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToEnd = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToLine = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToText = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToWarn = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToDebug = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToTrace = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToError = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToFatal = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToValues = new Telemetry.Counter();
        public static Telemetry.Counter CountCallsToProgress = new Telemetry.Counter();

        static Report()
        {
            Telemetry.Register("Report: BeginTimed", CountCallsToBeginTimed);
            Telemetry.Register("Report: BeginTimed/s", CountCallsToBeginTimed.RatePerSecond());
            Telemetry.Register("Report: Begin", CountCallsToBegin);
            Telemetry.Register("Report: Begin/s", CountCallsToBegin.RatePerSecond());
            Telemetry.Register("Report: End", CountCallsToEnd);
            Telemetry.Register("Report: End/s", CountCallsToEnd.RatePerSecond());
            Telemetry.Register("Report: Line", CountCallsToLine);
            Telemetry.Register("Report: Line/s", CountCallsToLine.RatePerSecond());
            Telemetry.Register("Report: Text", CountCallsToText);
            Telemetry.Register("Report: Text/s", CountCallsToText.RatePerSecond());
            Telemetry.Register("Report: Warn", CountCallsToWarn);
            Telemetry.Register("Report: Warn/s", CountCallsToWarn.RatePerSecond());
            Telemetry.Register("Report: Debug", CountCallsToDebug);
            Telemetry.Register("Report: Debug/s", CountCallsToDebug.RatePerSecond());
            Telemetry.Register("Report: Trace", CountCallsToTrace);
            Telemetry.Register("Report: Trace/s", CountCallsToTrace.RatePerSecond());
            Telemetry.Register("Report: Error", CountCallsToError);
            Telemetry.Register("Report: Error/s", CountCallsToError.RatePerSecond());
            Telemetry.Register("Report: Fatal", CountCallsToFatal);
            Telemetry.Register("Report: Fatal/s", CountCallsToFatal.RatePerSecond());
            Telemetry.Register("Report: Values", CountCallsToValues);
            Telemetry.Register("Report: Values/s", CountCallsToValues.RatePerSecond());
            Telemetry.Register("Report: Progress", CountCallsToProgress);
            Telemetry.Register("Report: Progress/s", CountCallsToProgress.RatePerSecond());
        }

        #endregion

        #region Static Report Targets

        public static readonly PerThreadJobReporter ThreadedJobReporter = new PerThreadJobReporter { Indent = 2 };
        public static readonly JobReporter JobReporter = new JobReporter { Indent = 2 };

        /// <summary>
        /// By assigning this property multi-threaded reporting can be turned on or off.
        /// This is mainly a feature for cosmetic and performance reasons of single-
        /// threaded applciations.
        /// </summary>
        public static bool MultiThreaded
        {
            get { return s_reporter is PerThreadJobReporter; }
            set { s_reporter = value ? (IJobReporter)ThreadedJobReporter : (IJobReporter)JobReporter; }
        }

        private static IJobReporter s_reporter = ThreadedJobReporter;

        public static void ConsoleWriteAct(int threadIndex, LogType type, int level, string message)
        {
            Console.Write(message); Console.Out.Flush();
        }

		#if !__ANDROID__
        private static ConsoleColor s_defaultForeground = Console.ForegroundColor;
        private static ConsoleColor s_defaultBackground = Console.BackgroundColor;

        public static void ConsoleColoredWriteAct(int threadIndex, LogType type, int level, string message)
        {
            bool resetBackground = false;
            switch (type)
            {
                case LogType.Fatal:
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                        resetBackground = true;
                        break;
                    }
                case LogType.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.Warn:  Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogType.Info:
                    switch (level)
                    {
                        case 0: Console.ForegroundColor = s_defaultForeground; break;
                        case 1: Console.ForegroundColor = ConsoleColor.Gray; break;
                        case 2: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        default: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                    }
                    break;
                default: Console.ForegroundColor = s_defaultForeground; break;
            }
            Console.Write(message);
            Console.ForegroundColor = s_defaultForeground;
            if (resetBackground) Console.BackgroundColor = s_defaultBackground;
            Console.Out.Flush();
        }

        public static void ConsoleForegroundColoredWriteAct(
                int threadIndex, LogType type, int level, string message)
        {
            switch (type)
            {
                case LogType.Fatal: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.Warn:  Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogType.Info:
                    switch (level)
                    {
                        case 0: Console.ForegroundColor = s_defaultForeground; break;
                        case 1: Console.ForegroundColor = ConsoleColor.Gray; break;
                        case 2: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        default: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                    }
                    break;
                default: Console.ForegroundColor = s_defaultForeground; break;
            }
            Console.Write(message);
            Console.ForegroundColor = s_defaultForeground;
            Console.Out.Flush();
        }

		#else

		public static void ConsoleColoredWriteAct(int threadIndex, LogType type, int level, string message)
		{
			Log.Verbose ("Aardvark", "{0}", message);
		}

		public static void ConsoleForegroundColoredWriteAct(int threadIndex, LogType type, int level, string message)
		{
			Log.Verbose ("Aardvark", "{0}", message);
		}

		#endif

        // Note that TextLogTarget needs to be created with a writer function that
        // gets the (type, level, message) triple as parameter. The writer functions
        // flush their output in order to provide output even if incomplete lines are
        // sent.

        public static readonly TextLogTarget ConsoleTarget =
                new TextLogTarget(ConsoleColoredWriteAct)
                        { Width = 80, Verbosity = 2, AllowBackspace = true, };
  
        public static readonly TextLogTarget TraceTarget =
                new TextLogTarget((i, t, l, m) => { System.Diagnostics.Debug.Write(m);
                                                    System.Diagnostics.Debug.Flush(); })
                        { Width = 100, Verbosity = 15, /* LogCompleteLinesOnly = true, */ };

        /// <summary>
        /// The LogFileName may be modified before the first log message is written.
        /// </summary>
        public static string LogFileName = @"Aardvark";

        /// <summary>
        /// Creates a stream writer writing to LogFileName but retries, if file is
        /// already open by another instance.
        /// </summary>
        private static StreamWriter CreateLogFileWriter(string fileName, int cnt)
        {
            if (cnt > 5)
                throw new Exception("Could not create writer (many instances running?)");

            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(
                    new FileStream(fileName + ".log",
                               FileMode.Create, FileAccess.Write, FileShare.Read));
                return writer;
            }
            catch (IOException)
            {
                return CreateLogFileWriter(string.Format("{0}_{1}", fileName, cnt), cnt + 1);
            }
        }

        /// <summary>
        /// The LogTarget opens the log file on the first logging write, so that it can
        /// be deactivated before first use by calling Report.Targets.Remove(Report.LogTarget)
        /// without leaving an empty log file
        /// </summary>
        public readonly static TextLogTarget LogTarget
                 = new TextLogTarget((firstThreadIndex, firstType, firstLevel, firstMessage) =>
                        {
                            StreamWriter writer = CreateLogFileWriter(LogFileName, 0);
                            LogTarget.WriteAct = (i, t, l, m) => 
                                {
                                    try
                                    {
                                        writer.Write(m); writer.Flush();
                                    } catch(ObjectDisposedException) // in finalization, finalizers which perform logging fail due to non deterministic finalization order
                                    {}
                                };
                            LogTarget.WriteAct(firstThreadIndex, firstType, firstLevel, firstMessage);
                        })
                    { Width = 100, Verbosity = 9 };

        public static string PerThreadLogName = @"Aardvark_{0}.log";
        public readonly static PerThreadLogTarget PerThreadLogTarget =
            new PerThreadLogTarget(CreatePerThreadLogTarget);

        private static TextLogTarget CreatePerThreadLogTarget(int index)
        {
            TextLogTarget logTarget =
                new TextLogTarget(null, index) { Width = 100, Verbosity = 9, Synchronized = false };
            logTarget.WriteAct = (firstThreadIndex, firstType, firstLevel, firstMessage) =>
            {
                var writer = new StreamWriter(
                    new FileStream(String.Format(PerThreadLogName, firstThreadIndex),
                                   FileMode.Create, FileAccess.Write, FileShare.Read));
                logTarget.WriteAct = (i, t, l, m) => { writer.Write(m); writer.Flush(); };
                logTarget.WriteAct(firstThreadIndex, firstType, firstLevel, firstMessage);
            };
            return logTarget;
        }

        /// <summary>
        /// You can add and remove log targets by adding or removing them from Targets.
        /// </summary>
        public readonly static MultiLogTarget Targets = new MultiLogTarget(ConsoleTarget, LogTarget);

        /// <summary>
        /// If you only want a single log target, you can assign it to the RootTarget.
        /// </summary>
        public static ILogTarget RootTarget = Targets;

        #endregion

        #region Static Reporting Methods

        /// <summary>
        /// A shortcat for the verbosity of the ConsoleTarget.
        /// </summary>
        public static int Verbosity
        {
            get { return ConsoleTarget.Verbosity; }
            set { ConsoleTarget.Verbosity = value; }
        }

        private static string Format(string message, params object[] args)
        {
            if (args.Length == 0) return message;
            

            try
            {
                return String.Format(Localization.FormatEnUS, message, args);
            }
            catch (FormatException)
            {
                Report.Warn("Report FormatException in \"{0}\"", message);
                return message;
            }
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimedNoBeginLog(ReportJob parentJob, int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(parentJob, level, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimedNoBeginLog(ReportJob parentJob, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(parentJob, 0, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimed(ReportJob parentJob, int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(parentJob, level, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimed(ReportJob parentJob, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(parentJob, 0, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static ReportJob Job(ReportJob parentJob, int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            return s_reporter.Begin(parentJob, level, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static ReportJob Job(ReportJob parentJob, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            return s_reporter.Begin(parentJob, 0, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimedNoBeginLog(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(null, level, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimedNoBeginLog([Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(null, 0, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimed(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(null, level, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static ReportJob JobTimed([Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            return s_reporter.Begin(null, 0, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static ReportJob Job(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            return s_reporter.Begin(null, level, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static ReportJob Job([Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            return s_reporter.Begin(null, 0, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimedNoLog(ReportJob parentJob, int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(parentJob, level, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimedNoLog(ReportJob parentJob, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(parentJob, 0, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimed(ReportJob parentJob, int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(parentJob, level, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimed(ReportJob parentJob, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(parentJob, 0, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin(ReportJob parentJob, int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(parentJob, level, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin(ReportJob parentJob, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(parentJob, 0, RootTarget, Format(message, args), false);
        }


        /// <summary>
        /// Begin a block without an explicit message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin(ReportJob parentJob, int level)
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(parentJob, level, RootTarget, "", false);
        }

        /// <summary>
        /// Begin a block without an explicit message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin(ReportJob parentJob)
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(parentJob, 0, RootTarget, "", false);
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimedNoLog(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(null, level, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message that will only be logged at the end of the job.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimedNoLog([Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(null, 0, RootTarget, Format(message, args), true, true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimed(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(null, level, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a timed block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// At the <see cref="End()"/> of the block the run time is reported.
        /// </summary>
        public static void BeginTimed([Localizable(true)] string message, params object[] args)
        {
            CountCallsToBeginTimed.Increment();
            s_reporter.Begin(null, 0, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(null, level, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Begin a block with a formatted message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin([Localizable(true)] string message, params object[] args)
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(null, 0, RootTarget, Format(message, args), false);
        }


        /// <summary>
        /// Begin a block without an explicit message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin(int level)
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(null, level, RootTarget, "", false);
        }

        /// <summary>
        /// Begin a block without an explicit message.
        /// All report calls till the call to the next
        /// <see cref="End()"/> are either indented (console/stream) or
        /// hierarchical children of this block (treeview).
        /// </summary>
        public static void Begin()
        {
            CountCallsToBegin.Increment();
            s_reporter.Begin(null, 0, RootTarget, "", false);
        }

        /// <summary>
        /// Ends a block of messages. If the block was timed, its runtime is
        /// reported. The End call MUST use the same level as the
        /// corresponding Begin/BeginTimed. The runtime in seconds is also returned.
        /// </summary>
        public static double End(int level)
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(level, RootTarget, null, false);
        }

        /// <summary>
        /// Ends a block of messages. If the block was timed, its runtime is
        /// reported. The End call MUST use the same level as the
        /// corresponding Begin/BeginTimed. The runtime in seconds is also returned.
        /// </summary>
        public static double End()
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(0, RootTarget, null, false);
        }

        /// <summary>
        /// Ends a block of messages, and supply an end message. If the end
        /// message is the very same as the begin message it is suppressed.
        /// If the end message starts with a continuation character (a space,
        /// a colon or a comma), it is viewed as a continuation message of
        /// the begin message, and appended as appropriate. If it is not the
        /// same and does not start with a continuation character, it is used
        /// as a distinct end message.
        /// </summary>
        public static double End(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(level, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Ends a block of messages, and supply an end message. If the end
        /// message is the very same as the begin message it is suppressed.
        /// If the end message starts with a continuation character (a space,
        /// a colon or a comma), it is viewed as a continuation message of
        /// the begin message, and appended as appropriate. If it is not the
        /// same and does not start with a continuation character, it is used
        /// as a distinct end message.
        /// </summary>
        public static double End([Localizable(true)] string message, params object[] args)
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(0, RootTarget, Format(message, args), false);
        }

        /// <summary>
        /// Ends a block of messages. If the block was timed, its runtime is
        /// reported. The End call MUST use the same level as the
        /// corresponding Begin/BeginTimed. The runtime in seconds is also returned.
        /// </summary>
        public static double EndTimed(int level)
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(level, RootTarget, null, true);
        }

        /// <summary>
        /// Ends a block of messages. If the block was timed, its runtime is
        /// reported. The End call MUST use the same level as the
        /// corresponding Begin/BeginTimed. The runtime in seconds is also returned.
        /// </summary>
        public static double EndTimed()
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(0, RootTarget, null, true);
        }

        /// <summary>
        /// Ends a block of messages, and supply an end message. If the end
        /// message is the very same as the begin message it is suppressed.
        /// If the end message starts with a continuation character (a space,
        /// a colon or a comma), it is viewed as a continuation message of
        /// the begin message, and appended as appropriate. If it is not the
        /// same and does not start with a continuation character, it is used
        /// as a distinct end message.
        /// </summary>
        public static double EndTimed(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(level, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Ends a block of messages, and supply an end message. If the end
        /// message is the very same as the begin message it is suppressed.
        /// If the end message starts with a continuation character (a space,
        /// a colon or a comma), it is viewed as a continuation message of
        /// the begin message, and appended as appropriate. If it is not the
        /// same and does not start with a continuation character, it is used
        /// as a distinct end message.
        /// </summary>
        public static double EndTimed([Localizable(true)] string message, params object[] args)
        {
            CountCallsToEnd.Increment();
            return s_reporter.End(0, RootTarget, Format(message, args), true);
        }

        /// <summary>
        /// Report a single message line with formatted parameters.
        /// </summary>
        public static void Line(int level, [Localizable(true)] string line, params object[] args)
        {
            CountCallsToLine.Increment();
            s_reporter.Line(LogType.Info, level, RootTarget, Format(line, args));
        }

        /// <summary>
        /// Report a single message line with formatted parameters.
        /// </summary>
        public static void Line([Localizable(true)] string line, params object[] args)
        {
            CountCallsToLine.Increment();
            s_reporter.Line(LogType.Info, 0, RootTarget, Format(line, args));
        }

        /// <summary>
        /// Report an empty line or end the line after using Text.
        /// </summary>
        public static void Line(int level)
        {
            CountCallsToLine.Increment();
            s_reporter.Line(LogType.Info, level, RootTarget, "");
        }

        /// <summary>
        /// Report an empty line or end the line after using Text.
        /// </summary>
        public static void Line()
        {
            CountCallsToLine.Increment();
            s_reporter.Line(LogType.Info, 0, RootTarget, "");
        }

        /// <summary>
        /// Report a (possibly multilined) message with formatted parameters.
        /// </summary>
        public static void Text(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToText.Increment();
            s_reporter.Text(level, RootTarget, Format(message, args));
        }

        /// <summary>
        /// Report a (possibly multilined) message with formatted parameters.
        /// </summary>
        public static void Text([Localizable(true)] string message, params object[] args)
        {
            CountCallsToText.Increment();
            s_reporter.Text(0, RootTarget, Format(message, args));
        }

        /// <summary>
        /// Report a message that will be written in the current line if it fits, or in
        /// the next line (with correct indent) if it does not fit.
        /// </summary>
        public static void Wrap(int level, [Localizable(true)] string message, params object[] args)
        {
            CountCallsToText.Increment();
            s_reporter.Wrap(level, RootTarget, Format(message, args));
        }

        /// <summary>
        /// Report a message that will be written in the current line if it fits, or in
        /// the next line (with correct indent) if it does not fit.
        /// </summary>
        public static void Wrap([Localizable(true)] string message, params object[] args)
        {
            CountCallsToText.Increment();
            s_reporter.Wrap(0, RootTarget, Format(message, args));
        }

        /// <summary>
        /// Write a warning. Warnings are at level 0 and cannot be
        /// suppressed.
        /// </summary>
        public static void Warn([Localizable(true)] string line, params object[] args)
        {
            CountCallsToWarn.Increment();
            s_reporter.Line(LogType.Warn, 0, RootTarget, "WARNING:", 0, Format(line, args));
        }

        /// <summary>
        /// Write a debug message.
        /// </summary>
        public static void Debug([Localizable(true)] string line, params object[] args)
        {
            CountCallsToDebug.Increment();
            s_reporter.Line(LogType.Debug, 0, RootTarget, "Debug:", 0, Format(line, args));
        }

        /// <summary>
        /// Write a debug message.
        /// </summary>
        public static void Debug(int level, [Localizable(true)] string line, params object[] args)
        {
            CountCallsToDebug.Increment();
            s_reporter.Line(LogType.Debug, level, RootTarget, "Debug:", 0, Format(line, args));
        }

        /// <summary>
        /// Write a debug message.
        /// </summary>
        public static void Trace([Localizable(true)] string line, params object[] args)
        {
            CountCallsToTrace.Increment();
            s_reporter.Line(LogType.Trace, 0, RootTarget, "Trace:", 0, Format(line, args));
        }

        /// <summary>
        /// Write a debug message.
        /// </summary>
        public static void Trace(int level, [Localizable(true)] string line, params object[] args)
        {
            CountCallsToTrace.Increment();
            s_reporter.Line(LogType.Trace, level, RootTarget, "Trace:", 0, Format(line, args));
        }

        /// <summary>
        /// Write an error. Erors are at level 0 and cannot be
        /// suppressed. More serious than a Warning, the program commences.
        /// </summary>
        public static void Error([Localizable(true)] string line, params object[] args)
        {
            var output = Format(line, args);
            CountCallsToError.Increment();
            s_reporter.Line(LogType.Error, 0, RootTarget, "ERROR:", 0, output);
            if (ThrowOnError)
            {
                System.Diagnostics.Debugger.Break();
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    throw new Exception(output);
                }
            }
        }

        /// <summary>
        /// Report a critical failure and break into the debugger, if
        /// one is attached.
        /// </summary>
        public static void Fatal([Localizable(true)] string line, params object[] args)
        {
            CountCallsToFatal.Increment();

            s_reporter.Line(LogType.Fatal, 0, RootTarget, "CRITICAL ERROR:", 0, Format(line, args));
            /* System.Diagnostics.StackTrace trace = */ new System.Diagnostics.StackTrace(1);

            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
            throw new NotImplementedException();
        }

        public static void Progress(int level, double progress)
        {
            CountCallsToProgress.Increment();
            s_reporter.Progress(level, RootTarget, progress, false);
        }

        public static void Progress(double progress)
        {
            CountCallsToProgress.Increment();
            s_reporter.Progress(0, RootTarget, progress, false);
        }

        public static void ProgressDelta(int level, double progressDelta)
        {
            CountCallsToProgress.Increment();
            s_reporter.Progress(level, RootTarget, progressDelta, true);
        }

        public static void ProgressDelta(double progressDelta)
        {
            CountCallsToProgress.Increment();
            s_reporter.Progress(0, RootTarget, progressDelta, true);
        }

        /// <summary>
        /// Report a single, named, reportable value.
        /// </summary>
        public static void Value(int level, [Localizable(true)] string name, IReportable reportable)
        {
            CountCallsToValues.Increment();
            reportable.ReportValue(level, name);
        }

        /// <summary>
        /// Report a single, named, reportable value.
        /// </summary>
        public static void Value([Localizable(true)] string name, IReportable reportable)
        {
            CountCallsToValues.Increment();
            reportable.ReportValue(0, name);
        }

        /// <summary>
        /// Report a single, named, formattable value.
        /// </summary>
        public static void Value(int level, [Localizable(true)] string name, object value)
        {
            CountCallsToValues.Increment();
            s_reporter.Values(level, RootTarget, name, null, value.IntoArray());
        }

        /// <summary>
        /// Report a single, named, formattable value.
        /// </summary>
        public static void Value([Localizable(true)] string name, object value)
        {
            CountCallsToValues.Increment();
            s_reporter.Values(0, RootTarget, name, null, value.IntoArray());
        }

        /// <summary>
        /// Report a sequence of formattable values.
        /// </summary>
        public static void Values(int level, [Localizable(true)] string name, string separator, params object[] values)
        {
            CountCallsToValues.Increment();
            s_reporter.Values(level, RootTarget, name, separator, values);
        }

        /// <summary>
        /// Report a sequence of formattable values.
        /// </summary>
        public static void Values([Localizable(true)] string name, string separator, params object[] values)
        {
            CountCallsToValues.Increment();
            s_reporter.Values(0, RootTarget, name, separator, values);
        }

        public static void Tests(TestInfo testInfo)
        {
            s_reporter.Tests(testInfo);
        }

        public static void Tests()
        {
            s_reporter.Tests(TestInfo.Empty);
        }

        /// <summary>
        /// Adds the reporter to report for the current active thread.
        /// </summary>
        public static void AddReporter(IReporter reporter)
        {
            s_reporter.AddReporter(reporter);
        }

        /// <summary>
        /// Removes the reporter to report for the current active thread.
        /// </summary>
        public static void RemoveReporter(IReporter reporter)
        {
            s_reporter.RemoveReporter(reporter);
        }

        #endregion

    }

    [Flags]
    public enum LogType
    {
        Unknown = 0,
        Info = 1,
        Begin = 2,
        End = 3,
        Progress = 4,
        Warn = 5,
        Trace = 6,
        Debug = 7,
        Error = 8,
        Fatal = 9,
    }

    [Flags]
    public enum LogOpt
    {
        None =      0x00,
        EndLine =   0x01,
        Timed =     0x02,
        Join =      0x04,
        Wrap =      0x08,
        NewText =   0x10, // new text on Report.End
    };

    public interface IReportable
    {
        void ReportValue(int verbosity, string name);
    }

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

    public struct LogMsg
    {
        public LogType Type;
        public LogOpt Opt;
        public int Level;
        public int LeftPos; public string LeftText;
        public int RightPos; public string RightText;

        public LogMsg(LogType type, LogOpt opt, int level,
                      int leftPos, string leftText, int rightPos = 0, string rightText = null)
        {
            Type = type; Opt = opt; Level = level;
            LeftPos = leftPos; LeftText = leftText; RightPos = rightPos; RightText = rightText;
        }
    }

    public interface ILogTarget : IDisposable
    {
        void NewThreadIndex(int threadIndex);
        void Log(int threadIndex, LogMsg msg);
    }

    public class ReportJob : IDisposable
    {
        internal string Message;
        public readonly int HierarchyLevel;
        public readonly ReportJob ParentJob;
        public double ChildrenTime;
        internal int Level;
        internal Stopwatch Timer;
        internal double Progress;
        internal bool ReportTests;
        internal TestInfo TestInfo;
        internal bool Disposed;

        public ReportJob(string message, int level, bool timed, ReportJob parentJob = null)
        {
            Disposed = false;
            Message = message;
            ParentJob = parentJob;
            ChildrenTime = 0.0;
            HierarchyLevel = parentJob == null ? 0 : parentJob.HierarchyLevel + 1;
            Level = level;
            Progress = -1.0;
            ReportTests = false;
            TestInfo = default(TestInfo);
            if (timed)
                { Timer = new Stopwatch(); Timer.Start(); }
            else
                Timer = null;
        }

        public void Dispose() { if (!Disposed)  Report.End(Level); }
    }

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


    public class JobReporter : IJobReporter
    {
        private int m_ti;
        public int m_indent;
        private ReportJob m_job;
        private Stack<ReportJob> m_jobStack;
        private volatile IReporter[] m_reporterArray;
        private object m_reporterArrayLock;

        #region Constructor

        public JobReporter(int threadIndex = 0)
        {
            m_ti = threadIndex; m_indent = 2;
            m_job = new ReportJob("THREAD", 0, true);
            m_jobStack = new Stack<ReportJob>();
            m_reporterArray = null;
            m_reporterArrayLock = new object();
        }

        #endregion

        #region Properties

        public int Indent
        {
            get { return m_indent; } set { m_indent = value; }
        }

        #endregion

        #region IJobReporter

        public void Line(LogType type, int level, ILogTarget target, string t0, int p1 = 0, string t1 = null)
        {
            target.Log(m_ti, new LogMsg(type, LogOpt.EndLine, level, m_job.HierarchyLevel * m_indent, t0, p1, t1));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Line(m_ti, type, level, target, t0, p1, t1);
        }

        public void Wrap(int level, ILogTarget target, string text)
        {
            target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.Join | LogOpt.Wrap, level,
                                            m_job.HierarchyLevel * m_indent, text, -2));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Wrap(m_ti, level, target, text);
        }

        public void Text(int level, ILogTarget target, string text)
        {
            string[] lines = text.Split('\n');
            var last = lines.Length - 1;
            if (last == 0)
            {
                target.Log(m_ti,  new LogMsg(LogType.Info, LogOpt.Join, level,
                                             m_job.HierarchyLevel * m_indent, lines[0], -2));
            }
            else
            {
                for (int i = 0; i < last; i++)
                    target.Log(m_ti,  new LogMsg(LogType.Info, LogOpt.Join | LogOpt.EndLine, level,
                                                 m_job.HierarchyLevel * m_indent, lines[i]));
                if (lines[last].Length > 0)
                    target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.Join, level,
                                                m_job.HierarchyLevel * m_indent, lines[last]));
            }
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Text(m_ti, level, target, text);
       }

        public ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false)
        {
            if (parentJob == null) parentJob = m_job;
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Begin(m_ti, level, target, text, timed);
            var opt = timed ? LogOpt.Timed : LogOpt.None;
            if (!noLog)
                target.Log(m_ti, new LogMsg(LogType.Begin, opt, level, parentJob.HierarchyLevel * m_indent, text, -2));
            m_jobStack.Push(m_job);
            m_job = new ReportJob(text, level, timed, parentJob);
            return m_job;
        }

        public void Tests(TestInfo testInfo)
        {
            m_job.TestInfo += testInfo;
            m_job.ReportTests = true;
        }

        public double End(int level, ILogTarget target, string text, bool addTimeToParent)
        {
            var parentJob = m_job.ParentJob;
            double seconds; string time; LogOpt opt;
            if (m_job.Timer != null)
            {
                seconds = m_job.Timer.ElapsedMilliseconds * 0.001; opt = LogOpt.EndLine | LogOpt.Timed;
                time =  String.Format(Localization.FormatEnUS, "{0:F3} s", seconds);
            }
            else
            {
                seconds = 0.0; time = null; opt = LogOpt.EndLine;
            }
            m_job.Disposed = true;
            if (level != m_job.Level)
            {
                Report.Warn("Report.Begin({0}) level different from Report.End({1}),"
                            + " using Report.End({0})", m_job.Level, level);
                level = m_job.Level;
            }
            var beginText = m_job.Message;
            if (text == null) text = beginText;
            else if (text != beginText) { text = beginText + text; opt |= LogOpt.NewText; }
            if (m_job.ReportTests == true)
            {
                var testInfo = m_job.TestInfo;
                if (m_jobStack.Count > 0)
                {
                    m_job = m_jobStack.Pop(); m_job.TestInfo += testInfo;
                }
                else
                    Report.Warn("superfluous Report.End() encountered");
                var passed = String.Format(Localization.FormatEnUS,
                             "[{0}/{1} OK]", testInfo.PassedCount, testInfo.TestCount);
                time = time != null ? passed + ' ' + time : passed;
                target.Log(m_ti, new LogMsg(LogType.End, opt, level, parentJob.HierarchyLevel * m_indent,
                                            text, -2, time));
                if (testInfo.FailedCount > 0)
                {
                    var failed = String.Format(Localization.FormatEnUS,
                                               " {0}/{1} FAILED", testInfo.FailedCount, testInfo.TestCount);
                    target.Log(m_ti, new LogMsg(LogType.Warn, opt, level, parentJob.HierarchyLevel * m_indent,
                                                    "WARNING: " + text + failed));
                }
            }
            else
            {
                var childrenTime = m_job.ChildrenTime;
                if (seconds > 0.0 && childrenTime > 0.0)
                    time = String.Format(Localization.FormatEnUS, "[{0:F3}x] ", childrenTime / seconds) + time;
                if (addTimeToParent)
                {
                    lock (parentJob) parentJob.ChildrenTime += seconds;
                }
                if (m_jobStack.Count > 0)
                    m_job = m_jobStack.Pop();
                else
                    Report.Warn("superfluous Report.End() encountered");
                target.Log(m_ti, new LogMsg(LogType.End, opt, level, parentJob.HierarchyLevel * m_indent,
                                            text, -2, time));
            }
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 1; i < reporterArray.Length; i++)
                    reporterArray[i].End(m_ti, level, target, text, seconds);
            return seconds;
        }

        public void Progress(int level, ILogTarget target, double progress, bool relative = false)
        {
            if (relative) progress += m_job.Progress;
            if (progress > 1.0) progress = 1.0;
            m_job.Progress = progress;
            double seconds; string text; LogOpt opt;
            if (m_job.Timer == null)
            {
                seconds = -1.0; opt = LogOpt.None;
                text = String.Format(Localization.FormatEnUS, "{0,6:F2}%", 100.0 * progress);
            }
            else
            {
                seconds = m_job.Timer.ElapsedMilliseconds * 0.001; opt = LogOpt.Timed;
                text = String.Format(Localization.FormatEnUS, "{0,6:F2}% {1,10:F3} s", 100.0 * progress, seconds);
            }
            target.Log(m_ti, new LogMsg(LogType.Progress, opt, level,
                                        (m_job.HierarchyLevel - 1) * m_indent, m_job.Message, -2, text));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Progress(m_ti, level, target, m_job.Message, progress, seconds);
        }

        public void Values(int level, ILogTarget target, string name, string separator, object[] values)
        {
            var text = values.Length == 1 ? values[0].ToString() : values.Map(o => o.ToString()).Join(separator);
            target.Log(m_ti, new LogMsg(LogType.Info, LogOpt.EndLine, level,
                                        m_job.HierarchyLevel * m_indent, name, 40, text));
            var reporterArray = m_reporterArray;
            if (reporterArray != null)
                for (int i = 0; i < reporterArray.Length; i++)
                    reporterArray[i].Values(m_ti, level, target, name, separator, values);
        }

        #endregion

        #region Adding and Removing IReporters

        public void AddReporter(IReporter reporter)
        {
            lock (m_reporterArrayLock)
            {
                m_reporterArray = m_reporterArray.WithAppended(reporter);
            }
        }

        public void RemoveReporter(IReporter reporter)
        {
            lock (m_reporterArrayLock)
            {
                m_reporterArray = m_reporterArray.WithRemoved(reporter);
            }
        }

        #endregion

    }

    public class PerThreadJobReporter : IJobReporter
    {
        private SpinLock m_lock;
        private volatile IntDict<IJobReporter> m_reporterMap;
        private volatile int m_threadCount;
        private int m_indent;

        #region Constructor

        public PerThreadJobReporter()
        {
            m_reporterMap = new IntDict<IJobReporter>();
            m_threadCount = 0; m_indent = 2;
        }

        #endregion

        #region Thread Handling

        private IJobReporter CurrentReporter(ILogTarget target = null)
        {
            IJobReporter reporter;
            var threadId = Thread.CurrentThread.ManagedThreadId;
            bool lockTaken = false;
            try
            {
                m_lock.Enter(ref lockTaken);
                if (!m_reporterMap.TryGetValue(threadId, out reporter))
                {
                    var threadIndex = m_threadCount++;
                    reporter = new JobReporter(threadIndex) { Indent = m_indent };
                    m_reporterMap[threadId] = reporter;
                    if (target != null) target.NewThreadIndex(threadIndex);
                }
            }
            finally
            {
                if (lockTaken) m_lock.Exit(true);
            }
            return reporter;
        }

        #endregion

        #region IJobReporter

        public int Indent
        {
            get { return m_indent; } set { m_indent = value; }
        }

        public void Line(LogType type, int level, ILogTarget target, string text0, int pos1 = 0, string text1 = null)
        {
            CurrentReporter(target).Line(type, level, target, text0, pos1, text1);
        }

        public void Text(int level, ILogTarget target, string text)
        {
            CurrentReporter(target).Text(level, target, text);
        }

        public void Wrap(int level, ILogTarget target, string text)
        {
            CurrentReporter(target).Wrap(level, target, text);
        }

        public ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false)
        {
            return CurrentReporter(target).Begin(parentJob, level, target, text, timed, noLog);
        }

        public double End(int level, ILogTarget target, string text, bool addTimeToParent)
        {
            return CurrentReporter(target).End(level, target, text, addTimeToParent);
        }

        public void Tests(TestInfo testInfo)
        {
            CurrentReporter().Tests(testInfo);
        }

        public void Progress(int level, ILogTarget target, double progress, bool relative = false)
        {
            CurrentReporter(target).Progress(level, target, progress, relative);
        }

        public void Values(int level, ILogTarget target, string name, string separator, object[] values)
        {
            CurrentReporter(target).Values(level, target, name, separator, values);
        }

        public void AddReporter(IReporter reporter)
        {
            CurrentReporter().AddReporter(reporter);
        }

        public void RemoveReporter(IReporter reporter)
        {
            CurrentReporter().RemoveReporter(reporter);
        }

        #endregion

    }

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

    public class TextLogTarget : ILogTarget
    {
        private object m_lock;
        private ReportState m_state;
        private IntDict<ReportState> m_stateTable;
        public Action<int, LogType, int, string> WriteAct;
        private int m_width = 80;
        private int m_maxIndent = 40;

        public int Verbosity = 0;
        public bool LogCompleteLinesOnly = false;
        public bool AllowBackspace = false;
        public bool Synchronized = true;
        public Func<int, string> m_prefixFun = threadIndex => String.Format("{0,2:x}: ", threadIndex);

        #region Constructor

        public TextLogTarget(Action<int, LogType, int, string> write, int threadIndex = 0)
        {
            m_lock = new object();
            m_state = new ReportState(threadIndex, m_prefixFun(threadIndex));
            m_stateTable = new IntDict<ReportState>();
            WriteAct = write;
        }

        #endregion

        #region Properties

        public int Width
        {
            get { return m_width; }
            set { m_width = value; m_maxIndent = value / 2; }
        }

        public Func<int, string> PrefixFun
        {
            set
            {
                m_prefixFun = value; m_state.Prefix = value(m_state.TIdx);
                m_state.PrefixLength = m_state.Prefix.Length;
            }
        }

        #endregion

        #region Constants

        const int c_maxWidth = 160;
        const string c_spaces160 =
            "                                                                                "
            + "                                                                                ";
        const string c_dots160 =
            "................................................................................"
            + "................................................................................";
        const string c_back160 =
            "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08";

        #endregion

        #region ILogTarget

        public void NewThreadIndex(int threadIndex)
        {
        }

        public void Log(int threadIndex, LogMsg msg)
        {
            if (msg.Level > Verbosity) return;
            if (!Synchronized) { Log(msg); return; }
            lock (m_lock)
            {
                if (threadIndex != m_state.TIdx)
                {
                    if (m_state.Level <= Verbosity && m_state.Buffer.Length > 0 && !LogCompleteLinesOnly)
                    {
                        WriteAct(m_state.TIdx, m_state.Type, m_state.Level, "\n"); // crlf for clean start
                        m_state.DoneCount = 0; // trigger the line to be printed again by the thread later
                    }
                    if (!m_stateTable.TryGetValue(threadIndex, out m_state))
                        m_stateTable[threadIndex] = m_state = new ReportState(threadIndex,
                                                                              m_prefixFun(threadIndex));
                }
                Log(msg);
            }
        }

        public void Dispose()
        {
            lock (m_lock)
            {
                if (m_state.Level <= Verbosity && m_state.Buffer.Length > 0)
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
            }
        }

        #endregion

        #region ReportState

        private class ReportState
        {
            public int TIdx;
            public string Prefix;
            public int PrefixLength;
            public LogType Type;
            public int Level = 0;
            public StringBuilder Buffer;
            public bool Timed;
            public int LeftPos;
            public int RightPos;
            public int DoneCount;

            public ReportState(int threadIndex, string prefix)
            {
                TIdx = threadIndex; Type = LogType.Unknown; Level = 0;
                Prefix = prefix;
                PrefixLength = Prefix.Length;
                Buffer = new StringBuilder(80); DoneCount = 0;
            }

            public void AddSpaceText(int pos, string text)
            {
                int len = Buffer.Length - PrefixLength;
                var fillCount = pos < len ? 0 : pos - len;
                if (fillCount > 0) Buffer.Append(c_spaces160, 0, fillCount);
                Buffer.Append(text);
            }

            public void AddDotsText(int pos, string text, int width)
            {
                int len = Buffer.Length;
                if (pos < 0) pos = width + pos - text.Length;
                if (len > PrefixLength && Buffer[len-1] != ' ') { Buffer.Append(' '); len += 1; }
                var empty = text == "";
                var fillCount = pos < len ? -1 : pos - len - 1 + (empty ? 1 : 0);
                if (fillCount > 0) Buffer.Append(c_dots160, 0, fillCount);
                if (fillCount >= 0 && !empty) Buffer.Append(' ');
                Buffer.Append(text);
            }

            public string Backspace(int count)
            {
                if (count > Buffer.Length) count = Buffer.Length;
                if (count > DoneCount) count = DoneCount;
                Buffer.Length -= count; DoneCount -= count; return c_back160.Substring(160 - count);
            }

            public string GetBufferLineAndClear()
            {
                int pos = DoneCount; DoneCount = 0; Buffer.Append('\n');
                var text = Buffer.ToString(pos, Buffer.Length - pos);
                Buffer.Clear();
                return text;
            }
        }

        #endregion

        #region Log Message

        public void Log(LogMsg msg)
        {
            if (msg.Type == LogType.End)
            {
                if (m_state.Buffer.Length > 0 &&
                    (m_state.Type == LogType.Info
                     || (m_state.Type != LogType.Unknown && m_state.LeftPos != msg.LeftPos))) // different indent == other job
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
                if ((msg.Opt & (LogOpt.Timed | LogOpt.NewText)) == 0) return;// if End is not timed, suppress repeated start message
            }
            if (msg.Type == LogType.Progress && !AllowBackspace) msg.Opt |= LogOpt.EndLine;
            if (LogCompleteLinesOnly)
            {
                if (msg.Type == LogType.Begin)
                {
                    msg.Opt = msg.Opt | LogOpt.EndLine; if ((msg.Opt & LogOpt.Timed) != 0) msg.RightText = "";
                }
                else if (msg.Type == LogType.Progress)
                {
                    msg.Opt = msg.Opt | LogOpt.EndLine;
                }
            }
            else if (m_state.Type == LogType.Begin)
            {
                if (msg.Type == LogType.End)
                {
                    var pos = m_state.LeftPos + m_state.PrefixLength; var len = m_state.Buffer.Length - pos;
                    if (len > 0 // len < 0 if we are from a different indent == other job
                        && msg.LeftText.StartsWith(m_state.Buffer.ToString(pos, len)))
                    {
                        msg.LeftText = msg.LeftText.Substring(len); msg.LeftPos = 0;
                    }
                }
                else if (msg.Type == LogType.Progress)
                {
                    msg.LeftText = null;
                }
                else if ((msg.Opt & LogOpt.Wrap) == 0) 
                {
                    if (m_state.Timed) m_state.AddDotsText(m_state.RightPos, "", m_width); // dots to line end
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
                }
            }
            else if (m_state.Type == LogType.Progress)
            {
                if (msg.Type == LogType.Progress)
                {
                    if (AllowBackspace)
                    {
                        if (m_state.DoneCount > 0)
                        {
                            WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.Backspace(msg.RightText.Length));
                            msg.LeftText = null;
                        }
                        else
                            m_state.Buffer.Clear();
                    }
                }
                else if (msg.Type == LogType.End)
                {
                    if (AllowBackspace)
                    {
                        if (m_state.DoneCount > 0)
                        {
                            var len = m_state.Buffer.Length - m_state.PrefixLength - m_state.LeftPos;
                            WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.Backspace(len));
                        }
                        else
                            m_state.Buffer.Clear();
                    }
                }
                else
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
            }
            if ((msg.Opt & LogOpt.Wrap) != 0)
            {
                if (m_state.Buffer.Length + msg.LeftText.Length + 1 > Width + msg.RightPos)
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
                else if (m_state.Buffer.Length > 0 && m_state.Buffer[m_state.Buffer.Length-1] != ' ')
                    m_state.Buffer.Append(' ');
            }
            if (m_state.Buffer.Length == 0) m_state.Buffer.Append(m_state.Prefix);
            if (msg.LeftText != null) m_state.AddSpaceText(Fun.Min(msg.LeftPos, m_maxIndent), msg.LeftText);
            if (msg.RightText != null) m_state.AddDotsText(Fun.Min(msg.RightPos, m_width), msg.RightText, m_width);
            if ((msg.Opt & LogOpt.EndLine) != 0)
            {
                WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.GetBufferLineAndClear());
                m_state.Type = LogType.Unknown;
            }
            else
            {
                if (!LogCompleteLinesOnly)
                {
                    int pos = m_state.DoneCount; int len = m_state.Buffer.Length - pos;
                    WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.Buffer.ToString(pos, len));
                    m_state.DoneCount = pos + len;
                }
                m_state.Type = msg.Type;
                m_state.Timed = (msg.Opt & LogOpt.Timed) != 0;
                m_state.LeftPos = msg.LeftPos;
                m_state.RightPos = msg.RightPos; // remember EndPos of timed begins for dotting to lineEnd
            }
            m_state.Level = msg.Level;
        }

        #endregion
    }

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
