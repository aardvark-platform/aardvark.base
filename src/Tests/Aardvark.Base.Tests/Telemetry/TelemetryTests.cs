using Aardvark.Base;
using NUnit.Framework;
using System;
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

        [Test]
        public void CpuTimePrivileged_Works()
        {
            var t = new Telemetry.CpuTime();
            var tUser = t.UserTime;
            var tPriv = t.PrivilegedTime;
            using (t.Timer) for (var i = 0; i < 100; i++) Directory.GetFiles(".");

            Assert.IsTrue(t.Value.TotalSeconds > 0.0);
            Assert.IsTrue(tUser.Value.TotalSeconds > 0.0);
            Assert.IsTrue(tPriv.Value.TotalSeconds > 0.0);
        }

        #endregion
    }
}
