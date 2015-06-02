using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{

    /// <summary>
    /// The TestSuite is configured to use NUnit by default, but can
    /// be constructed to be used without NUnit.
    /// </summary>
    public class TestSuite
    {
        public Options Opt;

        [Flags]
        public enum Options
        {
            None = 0x0000,
            UseLogTarget = 0x0001,
            UseNUnit = 0x0002,

            Default = UseNUnit,
        }

        /// <summary>
        /// This member provides the API for running tests,
        /// e.g. Test.Begin(...); Test.IsTrue(...); Test.End();
        /// </summary>
        public Info Test;

        /// <summary>
        /// The default constructor uses NUnit, and turns off the log
        /// file to make it work with TeamCity/NUnit.
        /// </summary>
        public TestSuite() : this(Options.Default)  { }


        public TestSuite(Options options)
        {
            Opt = options;
            if ((options & Opt) == 0) Report.Targets.Remove(Report.LogTarget);
            Test = new Info(TestInfo.Empty, (options & Options.UseNUnit) != 0);
        }

        public class Info
        {
            internal TestInfo m_ti;
            internal bool m_useNUnit;

            public Info(TestInfo ti, bool useNUnit) { m_ti = ti; m_useNUnit = useNUnit; }

            /// <summary>
            /// Begin a named, timed test.
            /// </summary>
            public void Begin(string testName, params object[] args)
            {
                Report.BeginTimed(testName, args);
                m_ti = TestInfo.Empty;
            }

            /// <summary>
            /// Begin a named, timed test.
            /// </summary>
            public void Begin(int level, string testName, params object[] args)
            {
                Report.BeginTimed(level, testName, args);
                m_ti = TestInfo.Empty;
            }

            /// <summary>
            /// End the test, and report results.
            /// </summary>
            public void End()
            {
                Report.Tests(m_ti); Report.End();
                m_ti = TestInfo.Empty;
            }

            /// <summary>
            /// End the test, and report results.
            /// </summary>
            public void End(string message, params object[] args)
            {
                Report.Tests(m_ti); Report.End(message, args);
                m_ti = TestInfo.Empty;
            }

            /// <summary>
            /// End the test, and report results.
            /// </summary>
            public void End(int level)
            {
                Report.Tests(m_ti); Report.End(level);
                m_ti = TestInfo.Empty;
            }

            /// <summary>
            /// End the test, and report results.
            /// </summary>
            public void End(int level, string message, params object[] args)
            {
                Report.Tests(m_ti); Report.End(level, message, args);
                m_ti = TestInfo.Empty;
            }

            /// <summary>
            /// Test if the supplied condition is true.
            /// </summary>
            public bool IsTrue(bool condition)
            {
                if (m_useNUnit) Assert.IsTrue(condition);
                if (condition)
                {
                    m_ti.PassedCount++; return true;
                }
                else
                {
                    m_ti.FailedCount++; return false;
                }
            }

            /// <summary>
            /// Test if the supplied condition is false.
            /// </summary>
            public bool IsFalse(bool condition)
            {
                if (m_useNUnit) Assert.IsFalse(condition);
                if (condition)
                {
                    m_ti.FailedCount++; return false;
                }
                else
                {
                    m_ti.PassedCount++; return true;
                }
            }

            /// <summary>
            /// Test if the supplied condition is true. Warn if this is not the case,
            /// using the supplied massage.
            /// </summary>
            public bool IsTrue(bool condition, string messageIfFalse, params object[] args)
            {
                if (m_useNUnit) Assert.IsTrue(condition);
                if (condition)
                {
                    m_ti.PassedCount++; return true;
                }
                else
                {
                    m_ti.FailedCount++;
                    Report.Warn(messageIfFalse, args);
                    return false;
                }
            }

            /// <summary>
            /// Test if the supplied condition is false. Warn if this is not the case,
            /// using the supplied massage.
            /// </summary>
            public bool IsFalse(bool condition, string messageIfTrue, params object[] args)
            {
                if (m_useNUnit) Assert.IsFalse(condition);
                if (condition)
                {
                    m_ti.FailedCount++;
                    Report.Warn(messageIfTrue, args);
                    return true;
                }
                else
                {
                    m_ti.PassedCount++;
                    return false;
                }
            }
        }
    }
}
