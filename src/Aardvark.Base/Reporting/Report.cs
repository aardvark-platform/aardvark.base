using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
#if __ANDROID__
using Log = Android.Util.Log;
#endif

namespace Aardvark.Base
{
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

            try
            {
                s_defaultForeground = Console.ForegroundColor;
                s_defaultBackground = Console.BackgroundColor;
                s_coloredConsole = true;
            }
            catch
            {
                s_coloredConsole = false;
                s_defaultForeground = ConsoleColor.White;
                s_defaultBackground = ConsoleColor.Black;
            }

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

        private static readonly bool s_coloredConsole;
        private static readonly ConsoleColor s_defaultForeground;
        private static readonly ConsoleColor s_defaultBackground;


#if !__ANDROID__

        public static void ConsoleColoredWriteAct(int threadIndex, LogType type, int level, string message)
        {
            if(!s_coloredConsole)
            {
                ConsoleWriteAct(threadIndex, type, level, message);
                return;
            }

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
                case LogType.Warn: Console.ForegroundColor = ConsoleColor.Yellow; break;
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
            if (!s_coloredConsole)
            {
                ConsoleWriteAct(threadIndex, type, level, message);
                return;
            }
            switch (type)
            {
                case LogType.Fatal: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.Warn: Console.ForegroundColor = ConsoleColor.Yellow; break;
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
                new TextLogTarget((i, t, l, m) => {
                    System.Diagnostics.Debug.Write(m);
                    System.Diagnostics.Debug.Flush();
                })
                { Width = 100, Verbosity = 15, /* LogCompleteLinesOnly = true, */ };

        /// <summary>
        /// The LogFileName may be modified before the first log message is written.
        /// </summary>
        public static string LogFileName = @"Aardvark.log";

        /// <summary>
        /// Creates a stream writer writing to LogFileName but retries, if file is
        /// already open by another instance.
        /// </summary>
        private static StreamWriter CreateLogFileWriter(string fileName, int cnt)
        {
            if (cnt > 5)
                throw new Exception("Could not create writer (many instances running?)");

            try
            {
                var dir = Path.GetDirectoryName(fileName);

                if (!dir.IsNullOrEmpty() && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                return new StreamWriter(stream);
            }

            catch (IOException)
            {
                return CreateLogFileWriter(string.Format("{0}_{1}", fileName, cnt), cnt + 1);
            }
            catch (UnauthorizedAccessException)
            {
                var dir = Path.Combine(Path.GetTempPath(), "Aardvark", "logs");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffffff");
                var name = Path.Combine(dir, now);
                return CreateLogFileWriter(name, cnt + 1);
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
                         }
                         catch (ObjectDisposedException) // in finalization, finalizers which perform logging fail due to non deterministic finalization order
                         { }
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

        public static readonly ILogTarget NoTarget = new MultiLogTarget();

        /// <summary>
        /// If you only want a single log target, you can assign it to the RootTarget.
        /// </summary>
        public static ILogTarget RootTarget = Targets;

#endregion

        #region Static Reporting Methods

        /// <summary>
        /// A shortcut for the verbosity of the ConsoleTarget.
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
                return String.Format(CultureInfo.InvariantCulture, message, args);
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
        /// Write a warning. Warnings are at level 0 and cannot be
        /// suppressed.
        /// </summary>
        public static void WarnNoPrefix([Localizable(true)] string line, params object[] args)
        {
            CountCallsToWarn.Increment();
            s_reporter.Line(LogType.Warn, 0, RootTarget, "", 0, Format(line, args));
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
        public static void DebugNoPrefix([Localizable(true)] string line, params object[] args)
        {
            CountCallsToDebug.Increment();
            s_reporter.Line(LogType.Debug, 0, RootTarget, "", 0, Format(line, args));
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
        /// Write an error. Errors are at level 0 and cannot be
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
        /// Write an error. Errors are at level 0 and cannot be
        /// suppressed. More serious than a Warning, the program commences.
        /// </summary>
        public static void ErrorNoPrefix([Localizable(true)] string line, params object[] args)
        {
            var output = Format(line, args);
            CountCallsToError.Increment();
            s_reporter.Line(LogType.Error, 0, RootTarget, "", 0, output);
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
            /* System.Diagnostics.StackTrace trace = */
            new System.Diagnostics.StackTrace(1);

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
}
