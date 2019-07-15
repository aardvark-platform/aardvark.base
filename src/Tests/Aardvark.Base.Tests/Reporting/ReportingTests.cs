using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ReportingTests : TestSuite
    {
        public ReportingTests() : base() { }

        [Test]
        public void DoubleEnd()
        {
            Report.Begin("begin");
            Report.End();
            Report.End(); // bad end -> should report warning, but not throw exception

            Report.Begin("begin");
            Report.End();
            Report.End("end without begin"); // bad end -> text should be reported

            Report.Line("passed");
        }

        [Test]
        public void MultiThread()
        {
            new Thread(() =>
            {
                for (int i = 0; i < 1000; i ++)
                {
                    Report.BeginTimed("Thread 1 time");
                    Report.Line("This is thread 1 reporting {0}", i);
                    Report.End(" - end 1");
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Report.BeginTimed("Thread 2 time");
                    Report.Line("This is thread 2 reporting {0}", i);
                    Report.End();
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < 2000; i++)
                {
                    Report.Line("This is thread 3 reporting {0}", i);
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < 2000; i++)
                {
                    Report.Warn("WARNING");
                }
            }).Start();
        }
    }
}
