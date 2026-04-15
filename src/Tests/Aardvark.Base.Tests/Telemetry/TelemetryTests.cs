using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Tests
{
    [TestFixture]
    public class TelemetryTests : TestSuite
    {
        public TelemetryTests() : base() { }
        public TelemetryTests(Options options) : base(options) { }

        public void NUnitRun()
        {
            Test.Begin("Telemetry tests");
            SnapshotProbeLong_CanBeCreated();
            SnapshotProbeLong_Works();
            SnapshotProbeDouble_CanBeCreated();
            SnapshotProbeDouble_Works();
            SnapshotProbeTimeSpan_CanBeCreated();
            SnapshotProbeTimeSpan_Works();
            Test.End();
        }

        #region Snapshot probes

        [Test]
        public void SnapshotProbeLong_CanBeCreated()
        {
            Test.Begin("SnapshotProbeLong can be created");
            var a = new Telemetry.Counter();
            a.Set(100);

            var s = new Telemetry.SnapshotProbeLong(a);

            Test.IsTrue(s.Value == 0);
            Test.End();
        }

        [Test]
        public void SnapshotProbeLong_Works()
        {
            Test.Begin("SnapshotProbeLong works");
            var a = new Telemetry.Counter();
            a.Set(100);
            
            var s = new Telemetry.SnapshotProbeLong(a);
            
            a.Increment();
            Test.IsTrue(s.Value == 1);

            a.Increment();
            Test.IsTrue(s.Value == 2);

            a.Decrement();
            Test.IsTrue(s.Value == 1);

            a.Decrement(2);
            Test.IsTrue(s.Value == -1);
            Test.End();
        }

        [Test]
        public void SnapshotProbeDouble_CanBeCreated()
        {
            Test.Begin("SnapshotProbeDouble can be created");
            double x = 5.1;
            var a = new Telemetry.CustomProbeDouble(() => x);

            var s = new Telemetry.SnapshotProbeDouble(a);

            Test.IsTrue(s.Value == 0.0);
            Test.End();
        }

        [Test]
        public void SnapshotProbeDouble_Works()
        {
            Test.Begin("SnapshotProbeDouble works");
            double x = 5.1;
            var a = new Telemetry.CustomProbeDouble(() => x);

            var s = new Telemetry.SnapshotProbeDouble(a);

            x = 6.1;
            Test.IsTrue(s.Value == 1.0);

            x = 10.1;
            Test.IsTrue(s.Value == 5.0);

            x = 0.0;
            Test.IsTrue(s.Value == -5.1);
            Test.End();
        }

        [Test]
        public void SnapshotProbeTimeSpan_CanBeCreated()
        {
            Test.Begin("SnapshotProbeTimeSpan can be created");
            var x = TimeSpan.FromSeconds(60);
            var a = new Telemetry.CustomProbeTimeSpan(() => x);

            var s = new Telemetry.SnapshotProbeTimeSpan(a);

            Test.IsTrue(s.Value == TimeSpan.FromSeconds(0));
            Test.End();
        }

        [Test]
        public void SnapshotProbeTimeSpan_Works()
        {
            Test.Begin("SnapshotProbeTimeSpan works");
            var x = TimeSpan.FromSeconds(60);
            var a = new Telemetry.CustomProbeTimeSpan(() => x);

            var s = new Telemetry.SnapshotProbeTimeSpan(a);

            x = TimeSpan.FromSeconds(70);
            Test.IsTrue(s.Value == TimeSpan.FromSeconds(10.0));

            x = TimeSpan.FromSeconds(120);
            Test.IsTrue(s.Value == TimeSpan.FromSeconds(60.0));

            x = TimeSpan.FromSeconds(0);
            Test.IsTrue(s.Value == TimeSpan.FromSeconds(-60.0));
            Test.End();
        }

        #endregion

        #region Timing probes

        [Test]
        public void StopwatchTime_Works()
        {
            var t = new Telemetry.StopWatchTime();
            using (t.Timer)
            {
                Thread.Sleep(1000);
            }

            Assert.IsTrue(t.Value.TotalSeconds >= 1.0);
        }

        [Test]
        public void WallclockTime_Works()
        {
            var t = new Telemetry.WallClockTime();

            Task.WaitAll(Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
                {
                    using (t.Timer)
                    {
                        Thread.Sleep(1000);
                    }
                })
            ).ToArray());

            Assert.IsTrue(t.Value.TotalSeconds >= 1.0 && t.Value.TotalSeconds < 1.1);
        }

        [Test]
        public void WallclockTime_DoubleDisposeDoesNotThrowAndProbeRemainsUsable()
        {
            var t = new Telemetry.WallClockTime();
            var timer = t.Timer;

            Thread.Sleep(50);

            Assert.DoesNotThrow(() => timer.Dispose());
            Assert.DoesNotThrow(() => timer.Dispose());

            var elapsedAfterDoubleDispose = t.Value;

            using (t.Timer)
            {
                Thread.Sleep(50);
            }

            Assert.That(t.Value, Is.GreaterThan(elapsedAfterDoubleDispose));
        }

        [Test]
        public void CpuTime_Works()
        {
            var t0 = new Telemetry.CpuTime();
            using (t0.Timer) for (var i = 0; i < 100000000; i++) ;

            var t = new Telemetry.CpuTime();
            Task.WaitAll(Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
            {
                using (t.Timer)
                {
                    for (var i = 0; i < 100000000; i++) ;
                    Thread.Sleep(100);
                }
            })
            ).ToArray());

            Assert.IsTrue(t.Value.TotalSeconds >= t0.Value.TotalSeconds * 2);
        }

        private static void RunCpuAndSyscallHeavyWorkload()
        {
            for (var i = 0; i < 100000000; i++) ;
            for (var i = 0; i < 500; i++) Directory.GetFiles(".");
        }

        private static bool RuntimeSupportsPrivilegedProcessorTime()
        {
            var process = Process.GetCurrentProcess();
            var before = process.PrivilegedProcessorTime;

            RunCpuAndSyscallHeavyWorkload();
            process.Refresh();

            return process.PrivilegedProcessorTime > before;
        }

        private static void MeasureOnDedicatedThread(Action action)
        {
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    exception = e;
                }
            });

            thread.Start();
            thread.Join();

            if (exception != null) throw exception;
        }

        [Test]
        public void CpuTimePrivileged_Works()
        {
            var supportsPrivilegedProcessorTime = RuntimeSupportsPrivilegedProcessorTime();
            var t = new Telemetry.CpuTime();
            var tUser = t.UserTime;
            var tPriv = t.PrivilegedTime;

            MeasureOnDedicatedThread(() =>
            {
                using (t.Timer) RunCpuAndSyscallHeavyWorkload();
            });

            if (t.Value <= TimeSpan.Zero || tUser.Value <= TimeSpan.Zero)
                Assert.Ignore("Telemetry.CpuTime did not report positive total/user CPU time on this runtime.");

            Assert.IsTrue(t.Value.TotalSeconds > 0.0);
            Assert.IsTrue(tUser.Value.TotalSeconds > 0.0);

            if (supportsPrivilegedProcessorTime)
            {
                Assert.IsTrue(tPriv.Value > TimeSpan.Zero);
            }
            else
            {
                Assert.IsTrue(tPriv.Value >= TimeSpan.Zero);
                Assert.DoesNotThrow(() =>
                {
                    MeasureOnDedicatedThread(() =>
                    {
                        using (t.Timer) RunCpuAndSyscallHeavyWorkload();
                    });
                });
                Assert.IsTrue(tPriv.Value >= TimeSpan.Zero);
            }
        }

        #endregion
    }
}
